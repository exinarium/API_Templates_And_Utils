using Full_Application_Blazor.Common.Requests;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.Common.Responses
{
    public class ReviewResponseType : ObjectGraphType<ReviewResponse>
    {
        [ExcludeFromCodeCoverage]
        public ReviewResponseType() 
        {
            Field(x => x.Id);
            Field(x => x.ReviewerProfileID);
            Field(x => x.RevieweeProfileID);
            Field(x => x.Comment);
            Field(x => x.Rating);
        }
    }
}
