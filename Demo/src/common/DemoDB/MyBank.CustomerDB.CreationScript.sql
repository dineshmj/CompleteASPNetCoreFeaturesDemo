USE [MyBank.CustomerDB]
GO

IF OBJECT_ID('[dbo].[EmployeesAndRoles]', 'U') IS NOT NULL BEGIN
   DROP TABLE [dbo].[EmployeesAndRoles];
END

IF OBJECT_ID('[dbo].[Role]', 'U') IS NOT NULL BEGIN
   DROP TABLE [dbo].[Role];
END

IF OBJECT_ID('[dbo].[EmployeeLoginInfo]', 'U') IS NOT NULL BEGIN
   DROP TABLE [dbo].EmployeeLoginInfo;
END

IF OBJECT_ID('[dbo].[Employee]', 'U') IS NOT NULL BEGIN
   DROP TABLE [dbo].[Employee];
END

IF OBJECT_ID('[dbo].[Customer]', 'U') IS NOT NULL BEGIN
   DROP TABLE [dbo].[Customer];
END

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* ------------------------------- */
/*   Table "Roles"                 */
/* ------------------------------- */
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT DF_Role_IsActive DEFAULT (1), -- Defaults to Active

	CONSTRAINT PK_Role PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

/* ------------------------------- */
/*   Table "Employee"              */
/* ------------------------------- */
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[SocialSecurityNumber] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO

/* ------------------------------- */
/*   Table "Employee Login Info"   */
/* ------------------------------- */
CREATE TABLE [dbo].[EmployeeLoginInfo](
	[UserName] [nvarchar](50) NOT NULL PRIMARY KEY,
	[PasswordHash] [nvarchar](250) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	CONSTRAINT [FK_EmployeeLoginInfo_Employee]
		FOREIGN KEY (EmployeeID) REFERENCES [dbo].[Employee](Id)
) ON [PRIMARY]
GO

/* ------------------------------- */
/*   Table "Employees & Roles"     */
/* ------------------------------- */
CREATE TABLE [dbo].[EmployeesAndRoles](
	[EmployeeID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,

	CONSTRAINT FK_EmployeesAndRoles_Employee 
        FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee](Id),

	CONSTRAINT FK_EmployeesAndRoles_Role 
        FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role](Id),

	PRIMARY KEY CLUSTERED ([EmployeeID], [RoleID])
) ON [PRIMARY]
GO

/* ------------------------------- */
/*   Table "Customer"              */
/* ------------------------------- */
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[SocialSecurityNumber] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO

/* ------------------------------- */
/*   Create roles for employees    */
/* ------------------------------- */

INSERT INTO [dbo].[Role] ([RoleName])
VALUES 
    ('view_customer'),
    ('create_customer'),
    ('update_customer'),
    ('delete_customer')
GO

/* ------------------------------- */
/*   Create employees              */
/* ------------------------------- */

INSERT INTO [dbo].[Employee] (FirstName, LastName, DateOfBirth, EmailAddress, SocialSecurityNumber)
VALUES 
('Bob', 'Barker', '1985-05-12', 'bob.barker@mybank.com', '999-00-1111'),
('Dave', 'Miller', '1990-08-23', 'dave.miller@mybank.com', '999-00-2222'),
('Alice', 'Smith', '1988-11-30', 'alice.smith@mybank.com', '999-00-3333');
GO

/* ------------------------------- */
/*   Add employee login info       */
/* ------------------------------- */
-- Insert for Bob
INSERT INTO [dbo].[EmployeeLoginInfo] ([UserName], [PasswordHash], [EmployeeID])
SELECT 
    LOWER(FirstName), 
    CONVERT(NVARCHAR(100), '8d059c3640b97180dd2ee453e20d34ab0cb0f2eccbe87d01915a8e578a202b11', 2), 
    Id
FROM [dbo].[Employee] WHERE FirstName = 'Bob';

-- Insert for Dave
INSERT INTO [dbo].[EmployeeLoginInfo] ([UserName], [PasswordHash], [EmployeeID])
SELECT 
    LOWER(FirstName), 
    CONVERT(NVARCHAR(100), 'd84201a2ccf0a519d5da6c003e8a24a73fae5982eac25b69ef0c994d9e34b08c', 2), 
    Id
FROM [dbo].[Employee] WHERE FirstName = 'Dave';

-- Insert for Alice
INSERT INTO [dbo].[EmployeeLoginInfo] ([UserName], [PasswordHash], [EmployeeID])
SELECT 
    LOWER(FirstName), 
    CONVERT(NVARCHAR(100), '4e40e8ffe0ee32fa53e139147ed559229a5930f89c2204706fc174beb36210b3', 2), 
    Id
FROM [dbo].[Employee] WHERE FirstName = 'Alice';
GO

/* ------------------------------- */
/*   Add roles for employees       */
/* ------------------------------- */

-- Bob: View Only
INSERT INTO [dbo].[EmployeesAndRoles] (EmployeeID, RoleID)
SELECT e.Id, r.Id 
FROM [dbo].[Employee] e, [dbo].[Role] r
WHERE e.FirstName = 'Bob' AND r.RoleName = 'view_customer';

-- Dave: View AND Create
INSERT INTO [dbo].[EmployeesAndRoles] (EmployeeID, RoleID)
SELECT e.Id, r.Id 
FROM [dbo].[Employee] e, [dbo].[Role] r
WHERE e.FirstName = 'Dave' AND r.RoleName IN ('view_customer', 'create_customer');

-- Alice: All CRUD
INSERT INTO [dbo].[EmployeesAndRoles] (EmployeeID, RoleID)
SELECT e.Id, r.Id 
FROM [dbo].[Employee] e, [dbo].[Role] r
WHERE e.FirstName = 'Alice' AND r.RoleName IN ('view_customer', 'create_customer', 'update_customer', 'delete_customer');
GO

-- Verify Employee Roles
SELECT 
    e.FirstName, 
    e.LastName, 
    r.RoleName
FROM
	[dbo].[Employee] e
		INNER JOIN [dbo].[EmployeesAndRoles] er ON e.Id = er.EmployeeID
		INNER JOIN [dbo].[Role] r ON er.RoleID = r.Id
ORDER BY
	e.FirstName;

-- Verify Employee Login Info
SELECT
	e.FirstName,
	e.LastName,
	eli.UserName,
	eli.PasswordHash
FROM
	[dbo].[Employee] e
		INNER JOIN [dbo].[EmployeeLoginInfo] eli
			ON e.Id = eli.EmployeeID