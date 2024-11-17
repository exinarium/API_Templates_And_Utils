using System;
using System.Diagnostics.CodeAnalysis;
using AspNetCore.Identity.MongoDbCore.Models;
using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    [ExcludeFromCodeCoverage]
    public class Role: MongoIdentityRole<string>, IModel
    {
        [BsonElement("isDeleted")]
        public State IsDeleted { get; set; } = State.NOT_DELETED;

        [BsonElement("createdDateTime")]
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("modifiedDateTime")]
        public DateTime ModifiedDateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("auditLogId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AuditLogId { get; set; } = null;
    }
}

