using System;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;

namespace Full_Application_Blazor.Utils.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly IRepository<Log> _repository;

        public LoggerService(IRepository<Log> repository)
        {
            _repository = repository;
        }

        public async Task LogAsync(Log logMessage)
        {
            await _repository.AddAsync(logMessage);
        }
    }
}

