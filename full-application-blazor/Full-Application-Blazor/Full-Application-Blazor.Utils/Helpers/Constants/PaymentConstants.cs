using System;
namespace Full_Application_Blazor.Utils.Helpers.Contants
{
    public static class PaymentConstants
    {
        public const string INVOICE_DOES_NOT_EXIST = "The invoice cannot be found";
        public const string NO_RESPONSE_FROM_PROVIDER = "The payment provider could not be reached";
        public const string PROCESS_COMPLETE = "COMPLETE";
        public const string PROCESS_ERROR = "ERROR";
        public const string NO_CARD_DETAILS_ERROR = "No card details have been saved and the payment could not be processed";
        public const string UNKNOWN_ERROR = "An unknown error occurred while processing the payment";
    }
}

