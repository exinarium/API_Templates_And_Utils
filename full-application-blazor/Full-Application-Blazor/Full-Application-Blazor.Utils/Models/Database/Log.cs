using System;
using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class Log : BaseModel
    {
        [BsonElement("customMessage")]
        public string CustomMessage { get; set; }

        [BsonElement("systemMessage")]
        public string SystemMessage { get; set; }

        [BsonElement("className")]
        public string ClassName { get; set; }

        [BsonElement("requestId")]
        public string RequestId { get; set; }

        [BsonElement("logPriority")]
        public LogPriority LogPriority { get; set; }

        [BsonElement("logType")]
        public LogType LogType { get; set; }
    }
}

