using System;
using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum LogType
    {
        [Description("Information")]
        INFORMATION = 1,

        [Description("Warning")]
        WARNING = 2,

        [Description("Error")]
        ERROR = 3
    }
}

