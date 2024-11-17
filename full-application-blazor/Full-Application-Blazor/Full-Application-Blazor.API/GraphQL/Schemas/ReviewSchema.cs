using Full_Application_Blazor.GraphQL.Mutations;
using Full_Application_Blazor.GraphQL.Queries;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.GraphQL.Schemas
{
    public class ReviewSchema : Schema, IReviewSchema
    {
        [ExcludeFromCodeCoverage]
        public ReviewSchema(IServiceProvider provider, ReviewQuery reviewQuery, ReviewMutation reviewMutation) : base(provider)
        {
            Query = reviewQuery;
            Mutation = reviewMutation;
        }
    }
}
