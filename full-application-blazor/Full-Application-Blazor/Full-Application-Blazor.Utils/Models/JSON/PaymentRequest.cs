using System;
using System.Text.Json.Serialization;

namespace Full_Application_Blazor.Utils.Helpers.Classes
{
    public class PaymentRequest
    {
        [JsonPropertyName("merchant-id")]
        public int MerchantId { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("item_name")]
        public string ItemName { get; set; }

        [JsonPropertyName("itn")]
        public bool ITN { get; set; }
    }
}