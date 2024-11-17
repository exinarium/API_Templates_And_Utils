using AspNetCore.Identity.MongoDbCore.Models;
using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.Utils.Models
{
    [ExcludeFromCodeCoverage]
    public class User : MongoIdentityUser<string>, IModel
    {
        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("isDeleted")]
        public State IsDeleted { get; set; } = State.NOT_DELETED;

        [BsonElement("createdDateTime")]
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("modifiedDateTime")]
        public DateTime ModifiedDateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("auditLogId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AuditLogId { get; set; } = null;

        [BsonElement("entityId")]
        public string EntityId { get; set; }

        [BsonElement("profiles")]
        public List<Profile> Profile { get; set; } = new List<Profile> { };

        [BsonIgnore]
        public string? Password { get; set; }
    }
}