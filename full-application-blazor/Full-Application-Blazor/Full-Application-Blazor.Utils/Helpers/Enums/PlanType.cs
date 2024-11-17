using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum PlanType
    {
        [Description("Free")]
        FREE = 1,

        [Description("Professional")]
        PROFESSIONAL = 2,

        [Description("Team")]
        TEAM = 3,

        [Description("Enterprise")]
        ENTERPRISE = 4,

        [Description("Unlimited")]
        UNLIMITED = 5,
    }
}
