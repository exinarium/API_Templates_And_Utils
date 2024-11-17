using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Full_Application_Blazor.Utils.Models;

namespace Full_Application_Blazor.Utils.Configuration
{
    [ExcludeFromCodeCoverage]
    public class SeedConfig
	{
        public List<Role> Roles { get; set; }
        public List<User> Users { get; set; }
        public List<Plan> Plans { get; set; }
        public List<SMSTemplate> SMSTemplates { get; set; }
    }
}

