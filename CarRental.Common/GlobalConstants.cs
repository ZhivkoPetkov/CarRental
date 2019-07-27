namespace CarRental.Common
{
    public static class GlobalConstants
    {
        public const string AdministratorRoleName = "Administrator";
        public const string UserRoleName = "User";

        // In percentage
        public const int MinimumVoucherDiscount = 1;
        public const int MaximumVoucherDiscount = 5;

        public const string OrderCarsByRentsAscending = "TimesRentAscending";
        public const string OrderCarsByRentsDescending = "TimesRentDescending";
        public const string OrderCarsByRatingDescending = "RatingDescending";
        public const string OrderCarsByLastAdded = "LastAdded";
        public const string OrderCarsByPriceAscending = "PriceAscending";
        public const string OrderCarsByPriceDescending = "PriceDescending";

        public const string DefaultLocationName = "Sofia, Airport Terminal 1";

        public const string DefaultVoucherCode = "none";
        public const string SignlaRMessageForNewORder = "Someone just rented a car {0} for {1} day/s.";
    }
}
