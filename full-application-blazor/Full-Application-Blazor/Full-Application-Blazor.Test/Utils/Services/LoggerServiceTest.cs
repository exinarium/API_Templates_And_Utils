using System;
using System.Text.Json;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class LoggerServiceTest : IDisposable
    {
        private IRepository<Log> _repository;

        public LoggerServiceTest()
        {
            _repository = new MockRepository<Log>();
        }

        public void Dispose()
        {
            _repository = null;
        }

        [Fact]
        public void TestLogAsync()
        {
            try
            {
                var logMessage = new Log
                {
                    ClassName = "TestClass",
                    CustomMessage = "This is a custom error message",
                    LogPriority = LogPriority.HIGH,
                    LogType = LogType.ERROR,
                    RequestId = Guid.NewGuid().ToString(),
                    SystemMessage = JsonSerializer.Serialize(new InvalidOperationException())
                };

                var loggerService = new LoggerService(_repository);
                loggerService.LogAsync(logMessage).GetAwaiter().GetResult();

                Assert.True(true);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }
    }
}

