using System.Collections.Generic;

namespace Full_Application_Blazor.Common.Responses
{
    public class ListResponse<T>
    {
        public List<T> Results { get; set; } = new List<T>();
        public int pageNumber { get; set; } = 1;
        public bool hasMoreResults { get; set; }
    }
}
