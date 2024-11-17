using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.GraphQL.RequestTypes
{
    public class SearchType : InputObjectGraphType
    {
        [ExcludeFromCodeCoverage]
        public SearchType() 
        {
            Field<StringGraphType>("searchString");
            Field<ListGraphType<StringGraphType>>("properties");
        }
    }
}
