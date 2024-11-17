using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Full_Application_Blazor.Utils.Models
{
    public class Review : BaseModel
    {
        [BsonElement("reviewerProfileID")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReviewerProfileID { get; set; }

        [BsonElement("revieweeProfileID")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RevieweeProfileID { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; }

        [BsonElement("rating")]
        public int Rating { get; set; }
    }
}
