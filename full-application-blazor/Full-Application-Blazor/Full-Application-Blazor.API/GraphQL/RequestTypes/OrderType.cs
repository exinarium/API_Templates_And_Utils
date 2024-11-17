using GraphQL.Types;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.GraphQL.RequestTypes
{
    [ExcludeFromCodeCoverage]
    public class OrderType : InputObjectGraphType
    {
        public OrderType() 
        {
            Field<StringGraphType>("propertyName");
            Field<EnumerationGraphType<SortDirection>>("sortDirection");
        }
    }
}
