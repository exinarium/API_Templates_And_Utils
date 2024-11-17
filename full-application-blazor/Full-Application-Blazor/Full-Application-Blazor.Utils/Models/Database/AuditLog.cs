using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class AuditLog : BaseModel
    {
        [BsonElement("collectionName")]
        public string CollectionName { get; set; }

        [BsonElement("documentId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DocumentId { get; set; }

        [BsonElement("currentVersion")]
        public int CurrentVersion { get; set; } = 1;

        [BsonElement("history")]
        public List<BsonDocument> History { get; set; }
    }
}

