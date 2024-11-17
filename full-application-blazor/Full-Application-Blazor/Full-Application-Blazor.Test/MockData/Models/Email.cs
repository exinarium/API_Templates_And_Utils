using System.Collections.Generic;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Helpers.Enums;

namespace Full_Application_Blazor.Test.MockData.Models
{
    public class Email : BaseModel
    {
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public List<string> ReplyTo { get; set; }
        public string HtmlBody { get; set; }
        public EmailType EmailType { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public List<string> AttachmentFileNames { get; set; }
        public bool IsHtml { get; set; }
        public bool IsSMTPEmail { get; set; }
        public Dictionary<string, string> keyValuePairs { get; set; }
    }
}

