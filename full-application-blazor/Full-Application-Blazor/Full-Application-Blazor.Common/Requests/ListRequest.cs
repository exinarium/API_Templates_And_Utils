using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using System.Collections.Generic;

namespace Full_Application_Blazor.Common.Requests
{
    public class ListRequest
    {
        public Order? Order { get; set; } = null;
        public int PageNumber { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
        public Search? Search { get; set; } = null;
        public List<Filter<string>>? Filters { get; set; } = null;
    }
}
