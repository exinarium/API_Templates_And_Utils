using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class BankingDetail : BaseModel
    {
        [BsonElement("accountHolderName")]
        public string AccountHolderName { get; set; }

        [BsonElement("bankName")]
        public string BankName { get; set; }

        [BsonElement("accountType")]
        public AccountType AccountType { get; set; }

        [BsonElement("branchNumber")]
        public string BranchNumber { get; set; }

        [BsonElement("accountNumber")]
        public string AccountNumber { get; set; }

        [BsonElement("swiftCode")]
        public string? SwiftCode { get; set; }
    }
}
