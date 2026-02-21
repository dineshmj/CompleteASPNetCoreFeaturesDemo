namespace MyBank.Common
{
	public static class CookieNames
	{
		// Cookie names must start with __Host- so that OIDC logout can work correctly.
		public const string MICROSERVICE_CUSTOMERS_HOST_BFF = "__Host-Microservice-Customers-bff";
	}
}