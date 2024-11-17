using System;
using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum InvoiceStatus
    {
        [Description("Pending")]
        PENDING = 1,

        [Description("Approved")]
        APPROVED = 2,

        [Description("Rejected")]
        REJECTED = 3,

        [Description("Paid")]
        PAID = 4,

        [Description("Unpaid")]
        UNPAID = 5
    }
}

