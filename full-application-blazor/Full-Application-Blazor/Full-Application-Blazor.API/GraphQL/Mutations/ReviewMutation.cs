using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Domain.Aggregates.GraphQL;
using GraphQL;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.GraphQL.Mutations
{
    [ExcludeFromCodeCoverage]
    public class ReviewMutation : ObjectGraphType
    {
        public ReviewMutation(IReviewAggregate service)
        {
            Field<ReviewResponseType>("createReview").Arguments(new QueryArguments(new QueryArgument<ReviewRequestType> { Name = "request" })).Resolve(context =>
            {
                ReviewRequest request = context.GetArgument<ReviewRequest>("request");
                return service.CreateAsync(request).GetAwaiter().GetResult();
            });

            Field<ReviewResponseType>("editReview").Arguments(new QueryArguments(new QueryArgument<ReviewRequestType> { Name = "request" }, new QueryArgument<StringGraphType>{Name = "id"})).Resolve(context =>
            {
                ReviewRequest request = context.GetArgument<ReviewRequest>("request");
                var id = context.GetArgument<string>("id");
                return service.UpdateAsync(request, id).GetAwaiter().GetResult();
            });

            Field<ReviewResponseType>("deleteReview").Arguments(new QueryArguments(new QueryArgument<StringGraphType> { Name = "request" })).Resolve(context =>
            {
                string id = context.GetArgument<string>("request");
                return service.DeleteAsync(id).GetAwaiter().GetResult();
            });
        }
    }
}
