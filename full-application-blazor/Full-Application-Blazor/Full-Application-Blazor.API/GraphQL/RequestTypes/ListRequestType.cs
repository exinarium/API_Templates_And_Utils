using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.GraphQL.RequestTypes
{
    public class ListRequestType : InputObjectGraphType
    {
        [ExcludeFromCodeCoverage]
        public ListRequestType()
        {
            Field<OrderType>("order");
            Field<IntGraphType>("pageNumber");
            Field<IntGraphType>("itemsPerPage");
            Field<SearchType>("search");
            Field<ListGraphType<FilterType>>("filters");
        }
    }
}