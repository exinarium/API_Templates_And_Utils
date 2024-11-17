using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Full_Application_Blazor.Utils.Models
{
    public class Plan : BaseModel
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("planType")]
        public PlanType PlanType { get; set; }

}
}
