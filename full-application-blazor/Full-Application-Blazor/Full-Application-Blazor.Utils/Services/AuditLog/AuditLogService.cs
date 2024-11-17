using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Contants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Helpers.Utilities;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using MongoDB.Bson;

namespace Full_Application_Blazor.Utils.Services
{
    public class AuditLogService<T> : IAuditLogService<T>
        where T : IModel, new()
    {
        private readonly IRepository<Models.AuditLog> _auditLogRepository;
        private readonly IRepository<T> _documentRepository;

        public AuditLogService(IRepository<Models.AuditLog> auditLogRepository, IRepository<T> documentRepository)
        {
            _auditLogRepository = auditLogRepository;
            _documentRepository = documentRepository;
        }

        public async Task<List<Models.AuditLog>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null)
        {
            return await _auditLogRepository.ListAsync(order, pageNumber, itemsPerPage, search, filters);
        }

        public async Task<Models.AuditLog> GetAsync(string id)
        {
            return await _auditLogRepository.GetAsync(id);
        }

        public async Task LogAsync(T document, AuditEventType eventType)
        {
            if (document == null)
                throw new ArgumentNullException(AuditLogConstants.LOG_ASYNC_DOCUMENT_ERROR);

            var existingDocument = await _auditLogRepository.GetAsync(document.AuditLogId);

            if (existingDocument == null)
            {
                var auditLog = await CreateNewAuditLog(document, eventType);
                await UpdateDocument(document, auditLog.Id);
            }
            else
            {
                await UpdateExistingAuditLog(existingDocument, document, eventType);
            }
        }

        private BsonDocument GetHistoryDocument(T document, AuditEventType eventType)
        {
            var expando = ExpandoObjects<T>.FromObject(document);
            expando.TryAdd("eventType", Enums.GetEnumDescription(eventType));

            var bsonDocument = BsonDocument.Create(expando);
            return bsonDocument;
        }

        private async Task<Models.AuditLog> CreateNewAuditLog(T document, AuditEventType eventType)
        {
            var auditLog = new Models.AuditLog
            {
                CollectionName = nameof(T),
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow,
                CurrentVersion = 1,
                DocumentId = document.Id,
                IsDeleted = State.NOT_DELETED,
                Version = 1,
                History = new List<BsonDocument> {
                    GetHistoryDocument(document, eventType)
                }
            };

            return await _auditLogRepository.AddAsync(auditLog);
        }

        private async Task UpdateExistingAuditLog(Models.AuditLog existingDocument, T document, AuditEventType eventType)
        {
            existingDocument.ModifiedDateTime = DateTime.UtcNow;
            existingDocument.CurrentVersion = document.Version;            

            if (existingDocument.History == null)
            {
                existingDocument.History = new List<BsonDocument>();
            }

            existingDocument.History.Add(
                GetHistoryDocument(document, eventType)
            );

            await _auditLogRepository.UpdateAsync(existingDocument);
        }

        private async Task UpdateDocument(T document, string auditLogId)
        {
            document.AuditLogId = auditLogId;
            await _documentRepository.UpdateAsync(document);
        }
    }
}

