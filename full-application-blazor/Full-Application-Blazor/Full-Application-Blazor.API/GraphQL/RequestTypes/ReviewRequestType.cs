using Full_Application_Blazor.Utils.Models;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.Common.Requests
{
    public class ReviewRequestType : InputObjectGraphType
    {
        [ExcludeFromCodeCoverage]
        public ReviewRequestType() 
        {
            Field<StringGraphType>("id");
            Field<StringGraphType>("reviewerProfileID");
            Field<StringGraphType>("revieweeProfileID");
            Field<StringGraphType>("comment");
            Field<IntGraphType>("rating");
        }
    }
}
