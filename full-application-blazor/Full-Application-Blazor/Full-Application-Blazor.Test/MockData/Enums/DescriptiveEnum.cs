using System;
using System.ComponentModel;

namespace Full_Application_Blazor.Test.MockData.Enums
{
    public enum DescriptiveEnum
    {
        [Description("One")]
        A = 1,

        [Description("Two")]
        B = 2,

        NO_DESCRIPTION = 3
    }
}

