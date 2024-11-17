using Full_Application_Blazor.Common.Objects.Base;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Full_Application_Blazor.Common.Requests
{
    [ExcludeFromCodeCoverage]
    public class ReviewRequest : BaseRequestModel
    {
        [JsonPropertyName("reviewerProfileID")]
        [Required(AllowEmptyStrings = false)]
        public string ReviewerProfileID { get; set; }

        [JsonPropertyName("revieweeProfileID")]
        [Required(AllowEmptyStrings = false)]
        public string RevieweeProfileID { get; set; }

        [JsonPropertyName("comment")]
        [Required(AllowEmptyStrings = true)]
        public string Comment { get; set; }

        [JsonPropertyName("rating")]
        [Required(AllowEmptyStrings = false)]
        public int Rating { get; set; }
    }
}
