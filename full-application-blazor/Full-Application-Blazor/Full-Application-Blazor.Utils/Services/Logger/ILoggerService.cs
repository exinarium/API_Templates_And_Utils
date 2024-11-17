using System;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Models;

namespace Full_Application_Blazor.Utils.Services
{
    public interface ILoggerService
    {
        Task LogAsync(Log logMessage);
    }
}

