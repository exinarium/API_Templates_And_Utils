using System;
using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum EntityType
    {
        [Description("Organization")]
        ORGANIZATION = 1,

        [Description("Individual")]
        INDIVIDUAL = 2
    }
}

