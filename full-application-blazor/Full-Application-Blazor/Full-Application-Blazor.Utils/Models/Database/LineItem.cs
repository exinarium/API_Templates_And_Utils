using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Helpers.Classes
{
    public class LineItem
    {
        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }

        [BsonElement("quantity")]
        public double Quantity { get; set; }

        [BsonElement("discountAmount")]
        public double DiscountAmount { get; set; }

        [BsonElement("discountPercentage")]
        public double DiscountPercentage { get; set; }

        [BsonElement("itemId")]
        public string ItemId { get; set; }

    }
}

