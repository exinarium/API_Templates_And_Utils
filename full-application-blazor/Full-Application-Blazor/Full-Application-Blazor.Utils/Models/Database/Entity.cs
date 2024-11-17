using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class Entity : BaseModel
    {
        [BsonElement("planId")]
        public string PlanId { get; set; }

        [BsonElement("entityType")]
        public EntityType EntityType { get; set; }

        [BsonElement("orginizationName")]
        public string? OrginizationName { get; set; }

        [BsonElement("registrationNumber")]
        public string? RegistrationNumber { get; set; }

        [BsonElement("vatNumber")]
        public string? VATNumber { get; set; }

        [BsonElement("physicalAddress")]
        public string PhysicalAddress { get; set; }

        [BsonElement("postalAddress")]
        public string PostalAddress { get; set; }

        [BsonElement("contact")]
        public Contact Contact { get; set; }

        [BsonElement("bankingDetail")]
        public BankingDetail BankingDetail { get; set; }
    }
}

