using System;
using System.Security.Claims;
using Full_Application_Blazor.API.Middleware;
using Full_Application_Blazor.Test.MockData.Classes;
using Full_Application_Blazor.Test.MockData.Models;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Text.Json;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Microsoft.AspNetCore.Http.Features;
using Full_Application_Blazor.Utils.Repositories;

namespace Full_Application_Blazor.Test.Api.Middleware
{
    public class RequestLoggerTest : IDisposable
    {
        private readonly DefaultHttpContext _defaultContext;
        private readonly MockHostEnvironment _environment;
        private readonly MockRepository<RequestLog> _repository;
        private RequestLogger _requestLogger;

        public RequestLoggerTest()
        {
            _defaultContext = new DefaultHttpContext();
            _defaultContext.Response.Body = new MemoryStream();
            _defaultContext.Request.Body = new MemoryStream();
            _defaultContext.Request.Path = "/";

            _repository = new MockRepository<RequestLog>();
            _environment = new MockHostEnvironment
            {
                EnvironmentName = "Test"
            };

            _requestLogger = new RequestLogger(next: (innerContext) =>
            {
                return Task.CompletedTask;
            }, _environment, _repository);
        }

        public void Dispose()
        {
        }

        [Fact]
        public void TestRequestLoggerStatusCode()
        {
            _requestLogger.Invoke(_defaultContext).GetAwaiter().GetResult();
            Assert.Equal(200, _defaultContext.Response.StatusCode);
        }

        [Fact]
        public void TestRequestLoggerWithUser()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, ObjectId.GenerateNewId().ToString())
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _defaultContext.User = claimsPrincipal;

            _requestLogger.Invoke(_defaultContext).GetAwaiter().GetResult();
            Assert.Equal(200, _defaultContext.Response.StatusCode);
        }

        [Fact]
        public void TestRequestLoggerWithRequestBody()
        {
            var student = new Student
            {
                Id = ObjectId.GenerateNewId().ToString(),
                AuditLogId = null,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow,
                IsDeleted = State.NOT_DELETED,
                Name = "Foo",
                Surname = "Bar",
                Version = 1
            };

            var json = JsonSerializer.Serialize(student);            

            var content = new StringContent(json);
            var memoryStream = new MemoryStream();
            content.CopyToAsync(memoryStream).GetAwaiter().GetResult();

            _defaultContext.Request.Body = memoryStream;
            _defaultContext.Request.Method = "POST";

            _requestLogger.Invoke(_defaultContext).GetAwaiter().GetResult();
            Assert.Equal(200, _defaultContext.Response.StatusCode);
        }

        [Fact]
        public void TestRequestLoggerWithResponseBody()
        {
            _defaultContext.Request.Method = "GET";

            _requestLogger = new RequestLogger(next: (innerContext) =>
            {
                var student = new Student
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    AuditLogId = null,
                    CreatedDateTime = DateTime.UtcNow,
                    ModifiedDateTime = DateTime.UtcNow,
                    IsDeleted = State.NOT_DELETED,
                    Name = "Foo",
                    Surname = "Bar",
                    Version = 1
                };

                var json = JsonSerializer.Serialize(student);

                var content = new StringContent(json);
                content.CopyToAsync(innerContext.Response.Body).GetAwaiter().GetResult();
                return Task.CompletedTask;
            }, _environment, _repository);

            _requestLogger.Invoke(_defaultContext).GetAwaiter().GetResult();
            Assert.Equal(200, _defaultContext.Response.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TestRequestLoggerWithoutResponseBody(string baseContent)
        {
            var memoryStream = new MemoryStream();

            if (baseContent != null)
            {
                var content = new StringContent(baseContent);                
                content.CopyToAsync(memoryStream).GetAwaiter().GetResult();                
            }

            _defaultContext.Response.Body = memoryStream;
            _defaultContext.Request.Method = "GET";

            _requestLogger.Invoke(_defaultContext).GetAwaiter().GetResult();
            Assert.Equal(200, _defaultContext.Response.StatusCode);
        }

        [Fact]
        public void TestRequestLoggerWithExcludedRoute()
        {
            _defaultContext.Request.Path = "/api/v1/auth";
            _requestLogger.Invoke(_defaultContext).GetAwaiter().GetResult();
            Assert.Equal(200, _defaultContext.Response.StatusCode);
        }
    }
}

