using System;
namespace Full_Application_Blazor.Utils.Configuration
{
    public class PaymentConfig
    {
        public string PayfastURL { get; set; }
        public string PayfastSubscriptionURL { get; set; }
        public string PayfastMerchantKey { get; set; }
        public string PayfastMerchantID { get; set; }
        public string PayfastReturnURL { get; set; }
        public string PayfastCancelURL { get; set; }
        public string PayfastNotifyURL { get; set; }
        public string PayfastItemName { get; set; }
        public string PayfastPassphrase { get; set; }
        public bool TestMode { get; set; }
    }
}

