using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Full_Application_Blazor.Utils.Models
{
    public class SendgridTemplate : BaseModel
    {
        [BsonElement("dynamic_template_data")]
        public Dictionary<string, string> DynamicTemplateData { get; set; }
    }
}

