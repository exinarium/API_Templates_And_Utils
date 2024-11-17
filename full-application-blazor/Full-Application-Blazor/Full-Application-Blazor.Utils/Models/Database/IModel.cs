using System;
using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public interface IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        string Id { get; set; }

        [BsonElement("isDeleted")]
        State IsDeleted { get; set; }

        [BsonElement("createdDateTime")]
        DateTime CreatedDateTime { get; set; }

        [BsonElement("modifiedDateTime")]
        DateTime ModifiedDateTime { get; set; }

        [BsonElement("version")]
        int Version { get; set; }

        [BsonElement("auditLogId")]
        [BsonRepresentation(BsonType.ObjectId)]
        string AuditLogId { get; set; }
    }
}

