using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class RequestLog : BaseModel
    {
        [BsonElement("requestBody")]
        public BsonDocument RequestBody { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("httpMethod")]
        public string HttpMethod { get; set; }

        [BsonElement("statusCode")]
        public int StatusCode { get; set; }

        [BsonElement("responseBody")]
        public BsonDocument ResponseBody { get; set; }

        [BsonElement("environment")]
        public string Environment { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("userId")]
        public string UserId { get; set; }
    }
}

