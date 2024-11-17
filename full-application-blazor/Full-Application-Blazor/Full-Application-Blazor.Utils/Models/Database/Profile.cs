using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Full_Application_Blazor.Utils.Models
{
	public class Profile : BaseModel
	{
        [BsonElement("physicalAddress")]
		public string PhysicalAddress { get; set; }

        [BsonElement("postalAddress")]
        public string PostalAddress { get; set; }

        [BsonElement("profilePhoto")]
        public string ProfilePhoto { get; set; }

        [BsonElement("videoUrl")]
        public string VideoUrl { get; set; }

        [BsonElement("organizationPhotos")]
        public List<string> OrganizationPhotos { get; set; }
    }
}

