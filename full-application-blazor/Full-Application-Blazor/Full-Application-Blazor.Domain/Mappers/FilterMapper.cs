using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using System.Collections.Generic;

namespace Full_Application_Blazor.Domain.Mappers
{
    public class FilterMapper : IFilterMapper
    {
        public List<IFilter> MapFilters(List<Filter<string>> filters)
        {
            if (filters == null) 
            {
                return null;
            }

            var mappedFilters = new List<IFilter>();
            foreach (var filter in filters) 
            {
                int intValue;
                bool boolValue;
                double doubleValue;

                if (int.TryParse(filter.Value, out intValue))
                {
                    mappedFilters.Add(new Filter<int> {
                        Property = filter.Property,
                        Operator = filter.Operator,
                        Value = intValue
                    });
                }
                else if (bool.TryParse(filter.Value, out boolValue))
                {
                    mappedFilters.Add(new Filter<bool>
                    {
                        Property = filter.Property,
                        Operator = filter.Operator,
                        Value = boolValue
                    });
                }
                else if (double.TryParse(filter.Value, out doubleValue))
                {
                    mappedFilters.Add(new Filter<double>
                    {
                        Property = filter.Property,
                        Operator = filter.Operator,
                        Value = doubleValue
                    });
                }
                else
                {
                    mappedFilters.Add(filter);
                }
            }
            return mappedFilters;
        }
    }
}
