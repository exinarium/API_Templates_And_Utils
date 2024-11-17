using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Services;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Full_Application_Blazor.Test.Helpers.Services;
using Full_Application_Blazor.Utils.Helpers.Enums;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class DocumentServiceTest : MongoIntegrationTest, IDisposable
    {
        private readonly string _testFile = $".{Path.DirectorySeparatorChar}MockData{Path.DirectorySeparatorChar}Files{Path.DirectorySeparatorChar}MockTest.txt";
        private IGridFSBucket _bucket;
        private byte[] _source;
        private IDocumentService _service;
        private IRepository<Document> _repository;
        private Document _documentModel;

        public DocumentServiceTest()
        {
            CreateConnection();
            _bucket = new GridFSBucket(_database);
            _source = File.ReadAllBytes(_testFile);

            _documentModel = new Document
            {
                EntityId = "id",
                Filename = "MockTest.txt",
                ContentType = "docx",
            };

            _repository = new Repository<Document>(_client, _options);
            _service = new DocumentService(_bucket, _repository);
        }

        public void Dispose()
        {
            _client.DropDatabase("Test");
            _runner.Dispose();
        }

        [Fact]
        public async Task UploadAsyncTest()
        {
            await _service.UploadAsync(_source, _documentModel);
            var data = await _repository.GetAsync(_documentModel.Id);
            Assert.True(data.ContentType == "docx");
        }

        [Fact]
        public async Task DownloadAsyncTest()
        {
            await _service.UploadAsync(_source, _documentModel);
            var result = await _service.DownloadAsync(_documentModel.DocumentId);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindAsyncTest()
        {
            await _service.UploadAsync(_source, _documentModel);
            var list = await _service.GetAllAsync();
            Assert.True(list.Any());
        }

        [Fact]
        public async Task RenameAsyncWithCorrectIdTest()
        {
            await _service.UploadAsync(_source, _documentModel);
            await _service.RenameAsync(_documentModel.DocumentId, "tester");
            var result = (await _service.GetAllAsync()).FirstOrDefault();
            Assert.True(result?.Filename == "tester");
        }

        [Fact]
        public async Task RenameAsyncWithIncorrectIdTest()
        {
            await _service.UploadAsync(_source, _documentModel);
            await _service.RenameAsync("id", "tester");
            var result = (await _service.GetAllAsync()).FirstOrDefault();
            Assert.True(result?.Filename == _documentModel.Filename);
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            await _service.UploadAsync(_source, _documentModel);
            var val = await _service.DeleteAsync(_documentModel.Id);
            Assert.Equal(State.DELETED, val.IsDeleted);
        }
    }
}