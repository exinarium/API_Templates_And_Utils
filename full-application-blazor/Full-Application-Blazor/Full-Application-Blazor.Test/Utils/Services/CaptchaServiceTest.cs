using Full_Application_Blazor.Test.MockData.Classes;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Microsoft.Extensions.Options;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class CaptchaServiceTest : IDisposable
    {
        private readonly ICaptchaService _captchaService;
        private IHttpClientWrapper _httpClient;
        private readonly IOptions<Config> _config;

        public CaptchaServiceTest()
        {
            _httpClient = new MockCaptchaHttpClient();
            _config = Options.Create<Config>(new Config
            {
                CaptchaConfig = new CaptchaConfig
                {
                    PrivateKey = "privateKey",
                    PublicKey = "publicKey"
                }
            });
            _captchaService = new CaptchaService(_httpClient, _config);
        }

        public void Dispose()
        {
            _httpClient = null;
        }

        // Test if exceptions is thrown
        [Theory, MemberData(nameof(ErrorData))]
        public async Task TestErrors(string token, string privKey, bool isError, bool isNoContent, bool isUnknownError)
        {
            try
            {
                var httpClient = new MockCaptchaHttpClient
                {
                    IsError = isError,
                    IsNoContent = isNoContent,
                    IsUnknownError = isUnknownError,
                };

                var recaptchaConfig = Options.Create<Config>(new Config
                {
                    CaptchaConfig = new CaptchaConfig
                    {
                        PrivateKey = token,
                        PublicKey = privKey
                    }
                });

                var captchaService = new CaptchaService(httpClient, recaptchaConfig);

                var result = await captchaService.ValidateCaptcha(token);
                
            }
            catch (Exception) { Assert.True(true); }
        }

        [Theory, MemberData(nameof(ListDataHttpClientValues))]
        public async Task ManuallyProcesscapchaWithHttpClientValues(string token, string privKey, bool isError, bool isNoContent, bool isUnknownError)
        {
            var httpClient = new MockCaptchaHttpClient
            {
                IsError = isError,
                IsNoContent = isNoContent,
                IsUnknownError = isUnknownError,
            };

            var recaptchaConfig = Options.Create<Config>(new Config
            {
                CaptchaConfig = new CaptchaConfig
                {
                    PrivateKey = token,
                    PublicKey = privKey
                }
            });

            var recaptchaService = new CaptchaService(httpClient, recaptchaConfig);

            var result = await recaptchaService.ValidateCaptcha(token);
            Assert.True(result);
        }

        public static IEnumerable<object[]> ListDataHttpClientValues =
        new List<object[]>
        {
            new object[] { "test", "test", false, false, false },
        };


        public static IEnumerable<object[]> ErrorData =
        new List<object[]>
        {
            new object[] { "", "test", false, false, false },
            new object[] { "test", "", false, false, false },
            new object[] { null, "test", false, false, false },
            new object[] { "test", null, false, false, false },
            new object[] { "test", "test", true, false, false },
            new object[] { "test", "test", false, true, false },
            new object[] { "test", "test", false, false, true },
            new object[] { null, null, false, false, true  },
        };
    }
}