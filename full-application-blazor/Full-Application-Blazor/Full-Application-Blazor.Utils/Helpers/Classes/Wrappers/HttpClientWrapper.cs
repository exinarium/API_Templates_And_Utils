using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Interfaces;

namespace Full_Application_Blazor.Utils.Helpers.Classes
{
    [ExcludeFromCodeCoverage]
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        private readonly HttpRequestMessage _requestMessage;

        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
            _requestMessage = new HttpRequestMessage();
        }

        public Uri BaseAddress
        {
            get
            {
                return _httpClient.BaseAddress;
            }
            set
            {
                _httpClient.BaseAddress = value;
            }
        }

        public HttpRequestHeaders DefaultRequestHeaders
        {
            get
            {
                return _httpClient.DefaultRequestHeaders;
            }
        }

        public Task<HttpResponseMessage> GetAsync(string? requestUri)
        {
            _httpClient.BaseAddress = BaseAddress;

            foreach(var header in DefaultRequestHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            return _httpClient.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            _httpClient.BaseAddress = BaseAddress;

            foreach (var header in DefaultRequestHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            return await _httpClient.PostAsync(requestUri, content);
        }
    }
}