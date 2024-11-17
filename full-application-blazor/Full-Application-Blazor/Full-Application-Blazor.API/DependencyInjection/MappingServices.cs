using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Domain.Mappers;
using Full_Application_Blazor.Utils.Models;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public class MappingServices
    {
        public static void InjectMappingServices(IServiceCollection services)
        {
            services.AddTransient<IMapping<ReviewRequest, Review, ReviewResponse>, Mapping<ReviewRequest, Review, ReviewResponse>>();
            services.AddTransient<IFilterMapper, FilterMapper>();
        }
    }
}
