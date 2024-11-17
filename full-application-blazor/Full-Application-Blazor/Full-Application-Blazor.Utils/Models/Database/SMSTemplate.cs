using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class SMSTemplate : BaseModel
    {
        [BsonElement("templateName")]
        public string TemplateName { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = false;

        [BsonElement("templateText")]
        public string TemplateText { get; set; }

        [BsonElement("isApproved")]
        public ApprovalStatus IsApproved { get; set; }

        [BsonElement("approvedUserId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ApprovedUserId { get; set; }
    }
}