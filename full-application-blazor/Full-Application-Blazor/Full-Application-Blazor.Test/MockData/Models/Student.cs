using System;
using Full_Application_Blazor.Utils.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Test.MockData.Models
{
    public class Student : BaseModel
    {
        [BsonElement("name")]
        public string Name { get; set; }

        public string Surname { get; set; }
    }
}

