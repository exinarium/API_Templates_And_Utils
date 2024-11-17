using System.Text;

namespace Full_Application_Blazor.Utils.Helpers.Constants
{
    public static class RequestsConstants
    {
        public const string EMAIL_REQUIRED_ERROR = "The email field is required";
        public const string VALID_EMAIL_ERROR = "This is not a valid email address";

        public const string CURRENT_PASSWORD_REQUIRED_ERROR = "The current Password field is required";
        public const string CURRENT_PASSWORD_LENGTH_ERROR = "The current password should be at least 8 characters";
        public const string CURRENT_PASSWORD_REGEX_ERROR = "The current password should contain at least one uppercase letter, one lowercase letter and one number";

        public const string NEW_PASSWORD_REQUIRED_ERROR = "The new Password field is required";
        public const string NEW_PASSWORD_LENGTH_ERROR = "The new password should be at least 8 characters";
        public const string NEW_PASSWORD_REGEX_ERROR = "The new password should contain at least one uppercase letter, one lowercase letter and one number";

        public const string PASSWORD_REQUIRED_ERROR = "The Password field is required";
        public const string PASSWORD_LENGTH_ERROR = "The password should be at least 8 characters";
        public const string PASSWORD_REGEX_ERROR = "The password should contain at least one uppercase letter, one lowercase letter and one number";

        public const string TOKEN_REQUIRED_ERROR = "The token field is required";

        public const string PHONE_NUMBER_REQUIRED_ERROR = "The phone number field is required";
        public const string PHONE_NUMBER_INVALID_ERROR = "This is not a valid phone number";
        public const string PHONE_NUMBER_REGEX_ERROR = "The phone number must be in international standard format, eg. +27";

        public const string FIRSTNAME_REQUIRED_ERROR = "The first name field is required";
        public const string FIRSTNAME_LENGTH_ERROR = "The first name must be at least 3 characters";
        public const string FIRSTNAME_REGEX_ERROR = "The first name should start and contain one capital letter and the rest lower case letters";

        public const string LASTNAME_REQUIRED_ERROR = "The last name field is required";
        public const string LASTNAME_LENGTH_ERROR = "The last name must be at least 3 characters";
        public const string LASTNAME_REGEX_ERROR = "The last name should start and contain one capital letter and the rest lower case letters";
    }
}
