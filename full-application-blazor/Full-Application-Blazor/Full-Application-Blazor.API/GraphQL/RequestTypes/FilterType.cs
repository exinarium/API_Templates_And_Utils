using Full_Application_Blazor.Utils.Helpers.Enums;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.GraphQL.RequestTypes
{
    public class FilterType : InputObjectGraphType
    {
        [ExcludeFromCodeCoverage]
        public FilterType() 
        {
            Field<EnumerationGraphType<Operator>>("operator");
            Field<StringGraphType>("property");
            Field<StringGraphType>("value");
        }
    }
}
