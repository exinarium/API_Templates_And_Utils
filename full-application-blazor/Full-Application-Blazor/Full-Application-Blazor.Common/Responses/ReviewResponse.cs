using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Full_Application_Blazor.Common.Objects.Base;

namespace Full_Application_Blazor.Common.Responses
{
    [ExcludeFromCodeCoverage]
    public class ReviewResponse : BaseResponseModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("reviewerProfileID")]
        public string ReviewerProfileID { get; set; }

        [JsonPropertyName("revieweeProfileID")]
        public string RevieweeProfileID { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }
    }
}
