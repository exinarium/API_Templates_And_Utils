using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Domain.Aggregates.GraphQL;
using Full_Application_Blazor.Domain.Mappers;
using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Services;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace Full_Application_Blazor.Test.Domain.Aggregates
{
    public class ReviewAggregateTest : IDisposable
    {

        private IRepository<Review> _repository;
        private Review _reviewModel;
        private Review _reviewModel1;
        private IReviewAggregate _ratingAggregate;
        private readonly IReviewService _ratingService;
        private readonly IOptions<Config> _config;
        private ReviewRequest _context;
        private readonly ReviewRequest _context1;
        private readonly IMapping<ReviewRequest, Review, ReviewResponse> _mapping;
        private readonly ILoggerService _logger;

        private ListRequest _listRequest;
        private readonly List<Filter<string>> _filters;
        private readonly Order _order;
        private readonly Search _search;
        private readonly IFilterMapper _filterMapper;

        public ReviewAggregateTest()
        {
            _config = Options.Create<Config>(new Config { });
            _repository = new MockRepository<Review>();
            _mapping = new Mapping<ReviewRequest, Review, ReviewResponse>();
            _logger = new LoggerService(new MockRepository<Log>());
            _filterMapper = new FilterMapper();

            _reviewModel = new Review
            {
                Id = "63ee5628f0668a7da3df8b0b",
                ReviewerProfileID = "63ee5628f0668a7da3df8b0b",
                RevieweeProfileID = "63ee5628f0668a7da3df8b0b",
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

            _listRequest = new ListRequest
            {
                Order = _order,
                PageNumber = 2,
                ItemsPerPage = 7,
                Search = _search,
            };

            _filters = new List<Filter<string>>
            {
                new Filter<string>
                {

                }
            };

            _order = new Order
            {
                PropertyName = nameof(Review),
                SortDirection = MongoDB.Driver.SortDirection.Ascending,
            };

            _search = new Search
            {
                Properties = new List<string> { _reviewModel.Comment },
                SearchString = _reviewModel.Comment
            };

            _repository = new MockRepository<Review>
            {
                Value = _reviewModel,
            };

            _context = new ReviewRequest();
            _context1 = new ReviewRequest();

            _ratingService = new ReviewService(_repository);
            _ratingAggregate = new ReviewAggregate(_ratingService, _mapping, _logger, _filterMapper);
        }

        public void Dispose()
        {
            _repository = null;
        }

        [Fact]
        public async Task SaveNewRatingAsync()
        {
            await _ratingAggregate.CreateAsync(_context);
            var test = await _ratingAggregate.GetAsync(_reviewModel.Id);
            Assert.True(test != null);
        }

        [Fact]
        public async Task SaveNewRatingSomeNullAsync()
        {
            await _ratingAggregate.CreateAsync(_context1);
            Assert.True(true);

            await _ratingAggregate.CreateAsync(_context1);
            var test = await _ratingAggregate.GetAsync(_reviewModel1.Id);
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

            await _ratingAggregate.UpdateAsync(_context, "");
            var test = await _ratingAggregate.GetAsync(_reviewModel.Id);
            Assert.True(test.Rating == 1);
        }

        [Fact]
        public async void ListRating()
        {
            for (int i = 0; i < 10; i++)
            {
                await _ratingAggregate.CreateAsync(_context);
            }
            var list = await _ratingAggregate.GetAllAsync(_listRequest);
            Assert.True(list.Results.Any());
        }

        [Fact]
        public async void ListRatingException()
        {
            _ratingAggregate = new ReviewAggregate(null, _mapping, _logger, _filterMapper);
            await Assert.ThrowsAsync<NullReferenceException>(() => _ratingAggregate.GetAllAsync(_listRequest));
        }

        [Fact]
        public async void ListRatingNull()
        {
            var list = await _ratingAggregate.GetAllAsync(null);
            Assert.True(list.Results.Any());
        }

        [Fact]
        public async void GetRating()
        {
            var value = await _ratingAggregate.GetAsync(_reviewModel.Id);
            Assert.True(value != null);
        }

        [Fact]
        public async void DeleteRating()
        {
            var val = await _ratingAggregate.CreateAsync(_context);
            var reviewResponse = await _ratingAggregate.DeleteAsync("63ee5628f0668a7da3df8b0b");
            Assert.True(reviewResponse.RevieweeProfileID == null);
        }

        [Fact]
        public async Task UpdateAsyncAsyncModelNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ratingAggregate.UpdateAsync(null, null));
        }

        [Fact]
        public async Task SaveNewRatingAsyncModelNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ratingAggregate.CreateAsync(null));
        }

        [Fact]
        public async void GetRatingIDNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ratingAggregate.GetAsync(null));
        }

        [Fact]
        public async void DeleteRatingIdNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ratingAggregate.DeleteAsync(null));
        }
    }
}