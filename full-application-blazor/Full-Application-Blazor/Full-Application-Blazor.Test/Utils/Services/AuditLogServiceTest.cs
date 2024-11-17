using System;
using Full_Application_Blazor.Test.MockData.Models;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Helpers.Contants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Services;
using MongoDB.Bson;
using AuditLogService = Full_Application_Blazor.Utils.Services;
using IAuditLogService = Full_Application_Blazor.Utils.Services;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class AuditLogServiceTest : IDisposable
    {
        private IRepository<AuditLog> _auditLogRepository;
        private IRepository<Student> _studentRepository;
        private IAuditLogService<Student> _auditLogService;

        public AuditLogServiceTest()
        {
            _auditLogRepository = new MockRepository<AuditLog>();
            _studentRepository = new MockRepository<Student>();
            _auditLogService = new AuditLogService<Student>(_auditLogRepository, _studentRepository);
        }

        public void Dispose()
        {
            _auditLogRepository = null;
            _studentRepository = null;
        }

        [Fact]
        public void GetAuditLogList()
        {
            var results = _auditLogService.GetAllAsync().GetAwaiter().GetResult();
            Assert.NotNull(results);
            Assert.True(results.Count() > 0);
        }

        [Fact]
        public void GetAuditLogById()
        {
            var generatedId = ObjectId.GenerateNewId().ToString();
            var result = _auditLogService.GetAsync(generatedId).GetAwaiter().GetResult();

            Assert.NotNull(result);
            Assert.True(result.Id == generatedId);
        }

        [Theory, MemberData(nameof(ListData))]
        public void CreateAuditLog(Student document, AuditEventType eventType)
        {

            if (document == null)
            {
                var action = () => _auditLogService.LogAsync(document, eventType);
                Assert.Null(document);
                Assert.ThrowsAsync<ArgumentNullException>(AuditLogConstants.LOG_ASYNC_DOCUMENT_ERROR, action).GetAwaiter().GetResult();
            }
            else
            {
                _auditLogService.LogAsync(document, eventType).GetAwaiter().GetResult();
                Assert.True(true);
            }

        }

        public static IEnumerable<object[]> ListData =
        new List<object[]>
        {
            new object[] { null, AuditEventType.CREATE },
            new object[] { new Student
            {
                Id = ObjectId.GenerateNewId().ToString(),
                AuditLogId = null,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow,
                IsDeleted = State.NOT_DELETED,
                Name = "Foo",
                Surname = "Bar",
                Version = 1
            }, AuditEventType.UPDATE },
            new object[] { new Student
            {
                Id = ObjectId.GenerateNewId().ToString(),
                AuditLogId = ObjectId.GenerateNewId().ToString(),
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow,
                IsDeleted = State.NOT_DELETED,
                Name = "Foo",
                Surname = "Bar",
                Version = 2
            }, AuditEventType.DELETE }
        };
    }
}

