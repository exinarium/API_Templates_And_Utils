using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Entity> _entityRepository;

        public ProfileService(IRepository<User> userRepository, IRepository<Entity> entityRepository) 
        {
            _userRepository = userRepository;
            _entityRepository = entityRepository;
        }

        public async Task<Profile> CreateAsync(Profile model, string userId)
        {
            var user = await _userRepository.GetAsync(userId);
            model.CreatedDateTime = System.DateTime.UtcNow;
            user.Profile.Add(model);
            await _userRepository.UpdateAsync(user);
            return model;
        }

        public async Task<Profile> GetAsync(string profileId, string userId)
        {
            var user = await _userRepository.GetAsync(userId);
            Profile profile = user.Profile.FirstOrDefault(x => x.Id == profileId);
            return profile;
        }

        public async Task<Profile> UpdateAsync(Profile model, string userId)
        {
            var user = await _userRepository.GetAsync(userId);
            var entity = await _entityRepository.GetAsync(user.EntityId);
            var profile = user.Profile.Find(x => x.Id == model.Id);

            profile.Id = model.Id;
            profile.Version = model.Version;
            profile.AuditLogId = model.AuditLogId;
            profile.ModifiedDateTime = System.DateTime.UtcNow;

            if (entity.EntityType == Utils.Helpers.Enums.EntityType.ORGANIZATION)
            {
                profile.OrganizationPhotos = model.OrganizationPhotos;
                profile.VideoUrl = model.VideoUrl;
            }

            else if (entity.EntityType == Utils.Helpers.Enums.EntityType.INDIVIDUAL)
            {
                profile.PhysicalAddress = model.PhysicalAddress;
                profile.PostalAddress = model.PostalAddress;
            }

            await _userRepository.UpdateAsync(user);
            return profile;
        }

        public async Task<Profile> DeleteAsync(Profile model, string userId)
        {
            var user = await _userRepository.GetAsync(userId);
            var profile = user.Profile.Find(x => x.Id == model.Id);
            profile.ModifiedDateTime = System.DateTime.UtcNow;
            profile.IsDeleted = Helpers.Enums.State.DELETED;
            await _userRepository.UpdateAsync(user);
            return profile;
        }
    }
}
