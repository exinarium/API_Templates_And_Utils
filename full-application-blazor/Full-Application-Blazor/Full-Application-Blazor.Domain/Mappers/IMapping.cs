using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Full_Application_Blazor.Common.Objects.Base;
using Full_Application_Blazor.Utils.Models;

namespace Full_Application_Blazor.Domain.Mappers
{
    public interface IMapping<T, U, V> where T: BaseRequestModel where U : BaseModel where V : BaseResponseModel
	{
		Task<U> MapToDatabaseModel(T value, string? id = null);
        Task<V> MapToAPIModel(U value);
    }
}

