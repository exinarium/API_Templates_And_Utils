using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Full_Application_Blazor.Utils.Models
{
    public class SMS : BaseModel
    {
        [BsonElement("receivingNumber")]
        public string? ReceivingNumber { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; } = null;

        [BsonElement("templateId")]
        public string? TemplateId { get; set; }

        [BsonElement("keyValuePairs")]
        public Dictionary<string, string>? KeyValuePairs { get; set; }

        [BsonElement("isSent")]
        public bool IsSent { get; set; } = false;

        [BsonElement("sendDate")]
        public DateTime? SendDate { get; set; }

        [BsonElement("errorCodes")]
        public string? ErrorCodes { get; set; }
    }
}
