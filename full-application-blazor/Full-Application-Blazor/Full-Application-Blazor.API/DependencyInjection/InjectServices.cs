using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.DependencyInjection
{
	public static class InjectServices
	{
        [ExcludeFromCodeCoverage]
        public static void InjectAllServices(IServiceCollection services)
		{
            MappingServices.InjectMappingServices(services);
            StatupServices.InjectStartupServices(services);
            AuthServices.InjectAuthServices(services);
            GraphQLServices.InjectGraphQLServices(services);
        }
	}
}

