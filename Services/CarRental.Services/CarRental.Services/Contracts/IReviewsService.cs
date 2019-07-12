using CarRental.DTOs.Reviews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Services.Contracts
{
    public interface IReviewsService
    {

        Task<bool> CreateReview(string orderId, int rating, string comment);
        ICollection<ListReviewDto> GetAllReviews();

        Task<bool> DeleteReview(int id);
    }
}
