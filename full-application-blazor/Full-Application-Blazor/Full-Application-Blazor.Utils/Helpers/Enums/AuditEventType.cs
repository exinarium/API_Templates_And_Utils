using System;
using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum AuditEventType
    {
        [Description("Create")]
        CREATE = 1,

        [Description("Update")]
        UPDATE = 2,

        [Description("Delete")]
        DELETE = 3,

        [Description("Login")]
        LOGIN = 4,

        [Description("Reset Password")]
        RESET_PASSWORD = 5
    }
}

