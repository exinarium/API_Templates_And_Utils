using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public interface IDocumentService
    {
        Task UploadAsync(byte[] source, Document data);
        Task<byte[]> DownloadAsync(string id);
        Task<List<Document>> GetAllAsync(Order? order = null, int pageNumber = 1, int itemsPerPage = 10, Search? search = null, List<IFilter>? filters = null);
        Task RenameAsync(string id, string newFileName);
        Task<Document> DeleteAsync(string id);
    }
}
