using System;
using System.Text.Json.Serialization;
using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class BaseModel : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("isDeleted")]
        public State IsDeleted { get; set; } = State.NOT_DELETED;

        [BsonElement("createdDateTime")]
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("modifiedDateTime")]
        public DateTime ModifiedDateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("version")]
        public int Version { get; set; } = 1;

        [BsonElement("auditLogId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AuditLogId { get; set; } = null;
    }
}

