using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Domain.Aggregates.GraphQL;
using Full_Application_Blazor.GraphQL.RequestTypes;
using Full_Application_Blazor.GraphQL.ResponceTypes;
using GraphQL;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.GraphQL.Queries
{
    [ExcludeFromCodeCoverage]
    public class ReviewQuery : ObjectGraphType
    {
        public ReviewQuery(IReviewAggregate service)
        {
            Field<ListResponseType>("getAllAsync").Arguments(new QueryArguments(new QueryArgument<ListRequestType> { Name = "searchParameters" })).Resolve(context =>
            {
                ListRequest request = context.GetArgument<ListRequest>("searchParameters");
                return service.GetAllAsync(request).GetAwaiter().GetResult();
            });
             

            Field<ReviewResponseType>("getAsync").Arguments(new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" })).Resolve(context =>
            {
                string id = context.GetArgument<string>("getAsync");
                return service.GetAsync(id).GetAwaiter().GetResult();
            });
        }
    }
}