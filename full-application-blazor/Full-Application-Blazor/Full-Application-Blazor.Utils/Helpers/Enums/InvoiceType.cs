using System;
using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum InvoiceType
    {
        [Description("Once Off Payment")]
        ONCE_OFF_PAYMENT = 1,

        [Description("Recurring Payment")]
        RECURRING_PAYMENT = 2
    }
}

