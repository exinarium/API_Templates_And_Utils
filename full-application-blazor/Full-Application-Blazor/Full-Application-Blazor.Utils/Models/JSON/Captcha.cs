using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class Captcha : BaseModel
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
