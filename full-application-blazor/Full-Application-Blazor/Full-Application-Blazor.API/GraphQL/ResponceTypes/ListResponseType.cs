using Full_Application_Blazor.Common.Responses;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.GraphQL.ResponceTypes
{
    public class ListResponseType : ObjectGraphType<ListResponse<ReviewResponse>>
    {
        [ExcludeFromCodeCoverage]
        public ListResponseType()
        {
            Field<ListGraphType<ReviewResponseType>>("Results");
            Field(x => x.pageNumber);
            Field(x => x.hasMoreResults);
        }
    }
}
