using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class Document : BaseModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("documentId")]
        public string DocumentId { get; set; }

        [BsonElement("entityId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EntityId { get; set; }

        [BsonElement("filename")]
        public string Filename { get; set; }

        [BsonElement("contentType")]
        public string ContentType { get; set; }
    }
}
