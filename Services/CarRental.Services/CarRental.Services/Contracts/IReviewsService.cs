using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Services.Contracts
{
    public interface IReviewsService
    {

        bool CreateReview(string orderId, int rating, string comment);

    }
}
