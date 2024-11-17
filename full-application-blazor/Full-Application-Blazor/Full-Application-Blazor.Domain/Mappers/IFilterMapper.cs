using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using System.Collections.Generic;

namespace Full_Application_Blazor.Domain.Mappers
{
    public interface IFilterMapper
    {
        List<IFilter> MapFilters(List<Filter<string>> filters);
    }
}
