using System.Security.Claims;
using System.Text.Json;
using Full_Application_Blazor.Utils.Helpers.Utilities;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using MongoDB.Bson;

namespace Full_Application_Blazor.API.Middleware
{
    public class RequestLogger
    {
        private readonly string[] EXCLUDED_ROUTES = new string[] { "auth" };
        private readonly RequestDelegate _next;
        private readonly IRepository<RequestLog> _repository;
        private readonly IHostEnvironment _hostEnvironment;

        public RequestLogger(RequestDelegate next, IHostEnvironment hostEnvironment, IRepository<RequestLog> repository)
        {
            this._next = next;
            this._repository = repository;
            this._hostEnvironment = hostEnvironment;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (EXCLUDED_ROUTES.Where(x => httpContext.Request.Path.Value.Contains(x)).Any())
            {
                await _next.Invoke(httpContext);
            }
            else
            {
                httpContext.Request.EnableBuffering();
                var originalBodyStream = httpContext.Response.Body;

                using (var responseBodyStream = new MemoryStream())
                {
                    httpContext.Response.Body = responseBodyStream;

                    httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                    var requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
                    BsonDocument? requestBodyDocument = !string.IsNullOrEmpty(requestBody) ?
                        BsonDocument.Parse(requestBody) : null;

                    var requestLog = new RequestLog
                    {
                        Environment = _hostEnvironment.EnvironmentName,
                        Url = httpContext.Request.Path,
                        HttpMethod = httpContext.Request.Method,
                        UserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                        RequestBody = requestBodyDocument
                    };

                    httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                    await _next.Invoke(httpContext);

                    responseBodyStream.Seek(0, SeekOrigin.Begin);

                    var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
                    BsonDocument? responseBodyDocument = !string.IsNullOrEmpty(responseBody) ?
                        BsonDocument.Parse(responseBody) : null;

                    requestLog.ResponseBody = responseBodyDocument;
                    requestLog.StatusCode = httpContext.Response.StatusCode;

                    await _repository.AddAsync(requestLog);

                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(originalBodyStream);
                    httpContext.Response.Body = originalBodyStream;
                }
            }
        }
    }
}
