using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.GraphQL.Mutations;
using Full_Application_Blazor.GraphQL.Queries;
using Full_Application_Blazor.GraphQL.Schemas;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Services;
using System.Diagnostics.CodeAnalysis;
using GraphQL;
using GraphQL.Instrumentation;
using Full_Application_Blazor.Domain.Aggregates.GraphQL;
using Full_Application_Blazor.GraphQL.ResponceTypes;
using Full_Application_Blazor.GraphQL.RequestTypes;

namespace Full_Application_Blazor.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class GraphQLServices
    {
        public static void InjectGraphQLServices(IServiceCollection services)
        {
            services.AddTransient<IRepository<Review>, Repository<Review>>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IReviewAggregate, ReviewAggregate>();
            services.AddTransient<IReviewSchema, ReviewSchema>();
            services.AddSingleton<ReviewQuery>();
            services.AddSingleton<ReviewMutation>();
            services.AddSingleton<ReviewRequestType>();
            services.AddSingleton<ReviewResponseType>();
            services.AddSingleton<ReviewRequest>();
            services.AddSingleton<ListResponseType>();
            services.AddSingleton<ListRequestType>();

            services.AddGraphQL(builder => builder
                        .AddSystemTextJson()
                        .AddSchema<ReviewSchema>()
                        .AddGraphTypes(typeof(ReviewQuery).Assembly)
                        .UseMiddleware<InstrumentFieldsMiddleware>(false)
                        .ConfigureSchema((schema, serviceProvider) => {}));
        }
    }
}
