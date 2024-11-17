using System;
using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.Utils.Configuration
{
    [ExcludeFromCodeCoverage]
    public class MetricsConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Endpoint { get; set; }
    }
}
