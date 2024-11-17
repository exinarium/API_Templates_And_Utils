using System.Collections.Generic;
using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class Email : BaseModel
    {
        [BsonElement("to")]
        public List<string> To { get; set; }

        [BsonElement("cc")]
        public List<string> CC { get; set; }

        [BsonElement("bcc")]
        public List<string> BCC { get; set; }

        [BsonElement("replyTo")]
        public List<string> ReplyTo { get; set; }

        [BsonElement("htmlBody")]
        public string HtmlBody { get; set; }

        [BsonElement("emailType")]
        public EmailType EmailType { get; set; }

        [BsonElement("fromName")]
        public string FromName { get; set; }

        [BsonElement("subject")]
        public string Subject { get; set; }

        [BsonElement("from")]
        public string From { get; set; }

        [BsonElement("attachmentFileNames")]
        public List<string> AttachmentFileNames { get; set; }

        [BsonElement("isHtml")]
        public bool IsHtml { get; set; }

        [BsonElement("isSMTPEmail")]
        public bool IsSMTPEmail { get; set; }

        [BsonElement("keyValuePairs")]
        public Dictionary<string, string> KeyValuePairs { get; set; }
    }
}

