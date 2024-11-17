using System.Diagnostics.CodeAnalysis;

namespace Full_Application_Blazor.Utils.Configuration
{
    public class Config
    {
        public SendGridConfig SendGridConfig { get; set; }
        public EmailConfig EmailConfig { get; set; }
        public DatabaseConfig DatabaseConfig { get; set; }
        public PaymentConfig PaymentConfig { get; set; }
        public CaptchaConfig CaptchaConfig { get; set; }
        public SMSConfig SMSConfig { get; set; }

        [ExcludeFromCodeCoverage]
        public MetricsConfig MetricsConfig { get; set; }

        [ExcludeFromCodeCoverage]
        public JWTConfig JWTConfig { get; set; }

        [ExcludeFromCodeCoverage]
        public SeedConfig SeedConfig { get; set; }
    }
}

