using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Domain.Aggregates.GraphQL
{
    public interface IReviewAggregate
    {
        Task<ReviewResponse> CreateAsync(ReviewRequest context);
        Task<ReviewResponse> UpdateAsync(ReviewRequest context, string? id);
        Task<ReviewResponse> GetAsync(string id);
        Task<ListResponse<ReviewResponse>> GetAllAsync(ListRequest? listRequest);
        Task<ReviewResponse> DeleteAsync(string id);
    }
}
