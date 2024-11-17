using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public class BankingDetailService : IBankingDetailService
    {
        private readonly IRepository<Entity> _entityRepository;

        public BankingDetailService(IRepository<Entity> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<BankingDetail> CreateAsync(BankingDetail model, string entityId)
        {
            var entity = await _entityRepository.GetAsync(entityId);
            model.CreatedDateTime = System.DateTime.UtcNow;
            entity.BankingDetail = model;
            await _entityRepository.UpdateAsync(entity);
            return model;
        }

        public async Task<BankingDetail> GetAsync(string id)
        {
            var entity = await _entityRepository.GetAsync(id);
            BankingDetail bankingDetails = entity.BankingDetail;
            return bankingDetails;
        }

        public async Task<BankingDetail> UpdateAsync(BankingDetail model, string entityId)
        {
            var entity = await _entityRepository.GetAsync(entityId);

            entity.BankingDetail.AccountHolderName = model.AccountHolderName;
            entity.BankingDetail.BankName = model.BankName;
            entity.BankingDetail.AccountType= model.AccountType;
            entity.BankingDetail.BranchNumber= model.BranchNumber;
            entity.BankingDetail.AccountNumber= model.AccountNumber;
            entity.BankingDetail.SwiftCode= model.SwiftCode;
            entity.BankingDetail.ModifiedDateTime = System.DateTime.Today;

            await _entityRepository.UpdateAsync(entity);
            return entity.BankingDetail;
        }

        public async Task<BankingDetail> DeleteAsync(string id)
        {
            var entity = await _entityRepository.GetAsync(id);

            entity.BankingDetail.IsDeleted = Helpers.Enums.State.DELETED;
            entity.BankingDetail.ModifiedDateTime = System.DateTime.Today;

            await _entityRepository.UpdateAsync(entity);
            return entity.BankingDetail;
        }
    }
}
