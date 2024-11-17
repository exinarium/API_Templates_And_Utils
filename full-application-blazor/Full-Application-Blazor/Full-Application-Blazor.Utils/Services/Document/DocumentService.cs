using Full_Application_Blazor.Utils.Models;
using MongoDB.Driver.GridFS;
using MongoDB.Bson;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Full_Application_Blazor.Utils.Services
{

    /// <summary>
    /// * NOTES:
    ///     GridFS is a specification for storing and retrieving files that exceed the BSON-document size limit of 16 megabytes.
    ///     Instead of storing a file in a single document, GridFS divides a file into parts, or chunks, and stores each of those chunks as a separate document.
    ///     By default, GridFS limits chunk size to 255 kilobytes. Chunks stores the binary chunks. Files stores the file's metadata.
    /// * More Info:
    ///    https://www.mongodb.com/docs/manual/core/gridfs/
    ///    https://mongodb.github.io/mongo-csharp-driver/2.14/reference/gridfs/
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IGridFSBucket _bucket;
        private readonly IRepository<Document> _repository;

        public DocumentService(IGridFSBucket bucket, IRepository<Document> repository)
        {
            _bucket = bucket;
            _repository = repository;
        }

        public async Task UploadAsync(byte[] source, Document data)
        {
            var id = await _bucket.UploadFromBytesAsync(data.Filename, source);
            data.DocumentId = id.ToString();

            await _repository.AddAsync(data);
        }

        public async Task<byte[]> DownloadAsync(string documentId)
        {
            return await _bucket.DownloadAsBytesAsync(ObjectId.Parse(documentId));
        }

        public async Task<List<Document>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null)
        {
            return await _repository.ListAsync(order, pageNumber, itemsPerPage, search, filters);
        }

        public async Task RenameAsync(string id, string newFileName)
        {
            var documents = await GetAllAsync(null, 1, 1, null, new List<IFilter>
            {
                new Filter<string>
                {
                    Property = "DocumentId",
                    Operator = Helpers.Enums.Operator.EQ,
                    Value = id
                }
            });

            var document = documents.FirstOrDefault();
            if (document != null) 
            {
                document.Filename = newFileName;
                await _bucket.RenameAsync(ObjectId.Parse(id), newFileName);
                await _repository.UpdateAsync(document);
            }
        }

        public async Task<Document> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
