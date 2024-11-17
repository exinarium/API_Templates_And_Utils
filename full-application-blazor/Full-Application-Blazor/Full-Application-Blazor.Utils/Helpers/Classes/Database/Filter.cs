using System;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Helpers.Enums;

namespace Full_Application_Blazor.Utils.Helpers.Classes
{
    public class Filter<T> : IFilter
    {
        public TypeCode TypeCode { get; } = Type.GetTypeCode(typeof(T));
        public Operator Operator { get; set; }
        public string Property { get; set; }
        public T Value { get; set; }

        public override string ToString()
        {
            return Value?.ToString();
        }
    }
}
