using System;
using System.ComponentModel;
using System.Linq;

namespace Full_Application_Blazor.Utils.Helpers.Utilities
{
    public static class Enums
    {
        public static string GetEnumDescription(Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var fi = value.GetType().GetField(value.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), true) as DescriptionAttribute[];

            if (attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}

