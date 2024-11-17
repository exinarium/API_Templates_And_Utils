using System;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Enums;

namespace Full_Application_Blazor.Utils.Helpers.Interfaces
{
    public interface IFilter
    {
        TypeCode TypeCode { get; }
        Operator Operator { get; set; }
        string Property { get; set; }
    }
}

