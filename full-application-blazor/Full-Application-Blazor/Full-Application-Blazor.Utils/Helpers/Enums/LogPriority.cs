using System;
using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum LogPriority
    {
        [Description("Low")]
        LOW = 1,

        [Description("Medium")]
        MEDIUM = 2,

        [Description("High")]
        HIGH = 3
    }
}

