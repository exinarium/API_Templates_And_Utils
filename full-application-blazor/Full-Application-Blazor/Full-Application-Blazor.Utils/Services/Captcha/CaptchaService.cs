using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Constants;

namespace Full_Application_Blazor.Utils.Services
{
    public class CaptchaService : ICaptchaService
    {
        protected readonly Config _captchaConfig;
        private readonly IHttpClientWrapper _httpClient;

        public CaptchaService(IHttpClientWrapper httpClient, IOptions<Config> config)
        {
            _httpClient = httpClient;
            _captchaConfig = config.Value;
        }

        public async Task<bool> ValidateCaptcha(string token)
        {
            var secret = _captchaConfig.CaptchaConfig.PrivateKey;
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(token))
            {
                throw new System.Exception(CaptchaConstants.TOKEN_NOT_SET);
            }

            string url = $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}";
            var response = await _httpClient.GetAsync(url);
            var contents = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(contents);
            var captcha = JsonSerializer.Deserialize<Captcha>(doc);

            if (captcha.Success) { return true; }
            
            else 
                return false;
        }
    }
}
