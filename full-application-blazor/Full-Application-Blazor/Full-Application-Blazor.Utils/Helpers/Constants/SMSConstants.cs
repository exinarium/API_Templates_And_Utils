using System;

namespace Full_Application_Blazor.Utils.Helpers.Constants
{
    public static class SMSConstants
    {
        public const string SMS_TEMPLATE_NOT_FOUND = "The SMS Template could not be found";
        public const string HANGFIRE_ERROR = "Failed to send sms through hangfire: ";
        public const string MOBILE_NUMBER_EMPTY = "Mobile Numebr is empty.";
        public const string MESSAGE_EMPTY = "Message text is empty.";
        public const string MESSAGE_TO_LONG = "Message is to long.";
        public const string RECIEVING_NUMBER_INCORECT = "Receiving number is incorrect.";
    }
}
