using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum EmailType
    {
        [Description("Document")]
        DOCUMENT = 1,

        [Description("Correspondence")]
        CORRESPONDENCE = 2,

        [Description("Application")]
        APPLICATION = 3,

        [Description("Contact Us")]
        CONTACT_US = 4,

        [Description("Support")]
        SUPPORT = 5,

        [Description("Quote")]
        QUOTE = 6,

        [Description("Invoice")]
        INVOICE = 7,

        [Description("Statement")]
        STATEMENT = 8,

        [Description("Other")]
        OTHER = 9,

        [Description("Verify Email")]
        VERIFY_EMAIL = 10,

        [Description("Forgot Password")]
        FORGOT_PASSWORD = 11,
    }
}

