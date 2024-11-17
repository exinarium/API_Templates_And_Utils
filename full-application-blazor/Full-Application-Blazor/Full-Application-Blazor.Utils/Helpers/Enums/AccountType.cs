using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum AccountType
    {
        [Description("Cheque")]
        CHEQUE = 1,

        [Description("Savings")]
        SAVINGS = 2,

        [Description("Credit")]
        CREDIT = 3,

        [Description("Investment")]
        INVESTMENT = 4,
    }
}
