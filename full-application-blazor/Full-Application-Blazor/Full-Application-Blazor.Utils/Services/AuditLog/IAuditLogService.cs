using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IAuditLogService<T>
        where T : IModel, new()
    {
        Task LogAsync(T document, AuditEventType eventType);
        Task<List<Models.AuditLog>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null);
        Task<Models.AuditLog> GetAsync(string id);
    }
}

