using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Services;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class ReviewServiceTest : IDisposable
    {
        private IRepository<Review> _repository;
        private IRepository<Review> _repository1;
        private readonly Review _reviewModel;
        private readonly Review _reviewModel1;
        private readonly IReviewService _ratingService;
        private readonly IReviewService _ratingService1;

        public ReviewServiceTest()
        {
            _repository = new MockRepository<Review>();
            _repository1 = new MockRepository<Review>();

            _reviewModel = new Review
            {
                ReviewerProfileID = "12345",
                RevieweeProfileID = "12345",
                Comment = "Comment",
                Rating = 2
            };

            _reviewModel1 = new Review
            {
                ReviewerProfileID = "12345",
                RevieweeProfileID = "12345",
                Comment = null,
                Rating = 2
            };

            _repository = new MockRepository<Review>
            {
                Value = _reviewModel,
            };

            _repository1 = new MockRepository<Review>
            {
                Value = _reviewModel1,
            };

            _ratingService = new ReviewService(_repository);
            _ratingService1 = new ReviewService(_repository1);
        }

        public void Dispose()
        {
            _repository = null;
        }

        [Fact]
        public async Task SaveNewRatingAsync()
        {
            await _ratingService.CreateAsync(_reviewModel);
            var test = await _ratingService.GetAsync(_reviewModel.Id);
            Assert.True(test != null);
        }

        [Fact]
        public async Task SaveNewRatingSomeNullAsync()
        {
            await _ratingService1.CreateAsync(_reviewModel1);
            Assert.True(true);

            await _ratingService.CreateAsync(_reviewModel1);
            var test = await _ratingService1.GetAsync(_reviewModel1.Id);
            Assert.True(test != null);
        }

        [Fact]
        public async void UpdateRating()
        {
            _reviewModel.Rating = 1;
            _repository = new MockRepository<Review>
            {
                Value = _reviewModel,
            };

            await _ratingService.UpdateAsync(_reviewModel);
            var test = await _ratingService.GetAsync(_reviewModel.Id);
            Assert.True(test.Rating == 1);
        }

        [Fact]
        public async void ListRating()
        {
            var list = await _ratingService.GetAllAsync();
            Assert.True(list.Any());
        }

        [Fact]
        public async void GetRating()
        {
            var value = await _ratingService.GetAsync(_reviewModel.Id);
            Assert.True(value != null);
        }

        [Fact]
        public async void DeleteRating()
        {
            var val = await _ratingService.DeleteAsync(_reviewModel.Id);
            Assert.True(val.IsDeleted == Full_Application_Blazor.Utils.Helpers.Enums.State.DELETED);
        }
    }
}
