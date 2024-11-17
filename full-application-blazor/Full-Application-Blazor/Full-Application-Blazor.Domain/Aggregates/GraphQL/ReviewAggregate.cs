using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Domain.Mappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Constants;
using System.Linq;

namespace Full_Application_Blazor.Domain.Aggregates.GraphQL
{
    public class ReviewAggregate : IReviewAggregate
    {
        private readonly IReviewService _service;
        private readonly IMapping<ReviewRequest, Review, ReviewResponse> _mapping;
        private readonly ILoggerService _logger;
        private readonly IFilterMapper _filterMapper;

        public ReviewAggregate(IReviewService service, IMapping<ReviewRequest, Review, ReviewResponse> mapping, ILoggerService logger, IFilterMapper filterMapper)
        {
            _service = service;
            _mapping = mapping;
            _logger = logger;
            _filterMapper = filterMapper;
        }

        public async Task<ReviewResponse> CreateAsync(ReviewRequest context)
        {
            try
            {
                if(context == null)
                    throw new ArgumentNullException(nameof(context));

                var review = await _mapping.MapToDatabaseModel(context);
                var result = await _service.CreateAsync(review);
                var reviewResponse = await _mapping.MapToAPIModel(result);
                return reviewResponse;
            } 
            
            catch (Exception e)
            {
                var log = new Log
                {
                    CustomMessage = ReviewConstants.REVIEW_FAILED +
                        $" :{System.Text.Json.JsonSerializer.Serialize(context)}",

                    ClassName = nameof(ReviewService.CreateAsync),
                    LogType = Utils.Helpers.Enums.LogType.ERROR,
                    LogPriority = Utils.Helpers.Enums.LogPriority.HIGH
                };

                await _logger.LogAsync(log);
                throw e;
            }
        }

        public async Task<ReviewResponse> UpdateAsync(ReviewRequest context, string? id)
        {
            try
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                var review = await _mapping.MapToDatabaseModel(context);
                review.Id= id;
                var result = await _service.UpdateAsync(review);
                var reviewResponse = await _mapping.MapToAPIModel(result);
                return reviewResponse;
            }

            catch (Exception e)
            {
                var log = new Log
                {
                    CustomMessage = ReviewConstants.REVIEW_FAILED + $" :{id}",
                    ClassName = nameof(ReviewService.CreateAsync),
                    LogType = Utils.Helpers.Enums.LogType.ERROR,
                    LogPriority = Utils.Helpers.Enums.LogPriority.HIGH
                };

                await _logger.LogAsync(log);
                throw e;
            }
        }

        public async Task<ListResponse<ReviewResponse>> GetAllAsync(ListRequest? listRequest)
        {
            try
            {
                if (listRequest == null)
                    listRequest = new ListRequest();

                var mappedFilters = _filterMapper.MapFilters(listRequest.Filters);
                List<Review> reviews = await _service.GetAllAsync(listRequest.Order, listRequest.PageNumber, listRequest.ItemsPerPage, listRequest.Search, mappedFilters);
                ListResponse<ReviewResponse> reviewResponse = new ListResponse<ReviewResponse>();
                reviewResponse.pageNumber = listRequest.PageNumber;

                foreach (var review in reviews)
                {
                    reviewResponse.Results.Add(await _mapping.MapToAPIModel(review));
                }

                reviewResponse.hasMoreResults = reviews.Count() >= listRequest.ItemsPerPage;

                return reviewResponse;
            }

            catch (Exception e)
            {
                var log = new Log
                {
                    CustomMessage = ReviewConstants.REVIEW_FAILED +
                    $" :{System.Text.Json.JsonSerializer.Serialize(listRequest)}",
                    ClassName = nameof(ReviewService.CreateAsync),
                    LogType = Utils.Helpers.Enums.LogType.ERROR,
                    LogPriority = Utils.Helpers.Enums.LogPriority.HIGH
                };

                await _logger.LogAsync(log);
                throw e;
            }
        }

        public async Task<ReviewResponse> GetAsync(string id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id));

                var review = await _service.GetAsync(id);
                var reviewResponse = await _mapping.MapToAPIModel(review);
                return reviewResponse;
            }

            catch (Exception e)
            {
                var log = new Log
                {
                    CustomMessage = ReviewConstants.REVIEW_FAILED + $" :{id}",
                    ClassName = nameof(ReviewService.CreateAsync),
                    LogType = Utils.Helpers.Enums.LogType.ERROR,
                    LogPriority = Utils.Helpers.Enums.LogPriority.HIGH
                };

                await _logger.LogAsync(log);
                throw e;
            }
        }

        public async Task<ReviewResponse> DeleteAsync(string id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id));

                var review = await _service.DeleteAsync(id);
                var reviewResponse = await _mapping.MapToAPIModel(review);
                return reviewResponse;
            }

            catch (Exception e)
            {
                var log = new Log
                {
                    CustomMessage = ReviewConstants.REVIEW_FAILED + $" :{id}",
                    ClassName = nameof(ReviewService.CreateAsync),
                    LogType = Utils.Helpers.Enums.LogType.ERROR,
                    LogPriority = Utils.Helpers.Enums.LogPriority.HIGH
                };

                await _logger.LogAsync(log);
                throw e;
            }
        }
    }
}
