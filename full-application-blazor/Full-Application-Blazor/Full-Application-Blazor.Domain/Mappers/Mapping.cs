using Full_Application_Blazor.Common.Objects.Base;
using Full_Application_Blazor.Utils.Helpers.Utilities;
using Full_Application_Blazor.Utils.Models;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Domain.Mappers
{
    [ExcludeFromCodeCoverage]
    public class Mapping<T, U, V> : IMapping<T, U, V> where T : BaseRequestModel, new() where U : BaseModel, new() where V : BaseResponseModel, new()
    {
        public async Task<V> MapToAPIModel(U value)
        {
            var expando = ExpandoObjects<U>.FromObject(value);
            if (expando == null) 
            {
                return default(V);
            }

            var apiModel = ExpandoObjects<V>.ToObject(expando);
            return apiModel;
        }

        public async Task<U> MapToDatabaseModel(T value, string? id = null)
        {
            var expando = ExpandoObjects<T>.FromObject(value);
            if (expando == null)
            {
                return default(U);
            }

            var databaseModel = ExpandoObjects<U>.ToObject(expando);
            databaseModel.Id= id;
            return databaseModel;
        }
    }
}
