using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text.Json;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Full_Application_Blazor.Test.MockData.Classes
{
    public class MockPayfastHttpClient : IHttpClientWrapper
    {
        private readonly HttpRequestMessage _requestMessage;

        public MockPayfastHttpClient()
        {
            _requestMessage = new HttpRequestMessage();
        }

        public Uri BaseAddress { get; set; }

        public HttpRequestHeaders DefaultRequestHeaders
        {
            get => _requestMessage.Headers;
        }

        public bool IsError { get; set; } = false;
        public bool IsNoContent { get; set; } = false;
        public bool IsUnknownError { get; set; } = false;

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            if (IsError)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Content = new StringContent(JsonSerializer.Serialize(new
                    {
                        data = new
                        {
                            response = "This is an error",
                            code = 404
                        }
                    }))
                };
            }
            else if (IsUnknownError)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Content = new StringContent(JsonSerializer.Serialize(new
                    {
                        data = new
                        {
                        }
                    }))
                };
            }
            else if (IsNoContent)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NoContent,
                    Content = new StringContent(string.Empty)
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new
                    {
                        data = new
                        {
                            response = new
                            {
                                invoiceId = "123",
                                amount = 111,
                                status = "success"
                            },
                            code = 200
                        }
                    }))
                };
            }
        }

        public Task<HttpResponseMessage> GetAsync(string? requestUri)
        {
            if (IsError)
            {
                return Task.FromResult(
                    new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent(JsonSerializer.Serialize(new
                        {
                            data = new
                            {
                                success = "False",
                                error_code = "invalid-input-secret"
                            }
                        }))
                    });

            }
            else if (IsUnknownError)
            {
                return Task.FromResult(
                    new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.InternalServerError,
                        Content = new StringContent(JsonSerializer.Serialize(new
                        {
                            data = new
                            {
                                success = "False",
                                error_code = "Something Went Wrong"
                            }
                        }))
                    });
            }
            else
            {
                return Task.FromResult(
                    new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent(JsonSerializer.Serialize(new
                        {
                            data = new
                            {
                                success = "True",
                                challenge_ts = DateTime.Now,
                                hostname = "testkey.google.com"
                            }
                        }))
                    });
            }
        }
    }
}

