

namespace Interfaces
{
    public interface IReservationHttpClient
    {
        public HttpClient? HttpClient { get; set; }
        public Task<TempDataAccount> Login(Authentication authentication);
        public Task<bool> Register(Account account);
        public Task<List<Restaurant>> GetAllRestaurants();
        public Task<List<RestaurantAddress>> GetAllAdressRestaurant();
        public Task<List<RestaurantImage>> GetAllImageRestaurants();
        public Task<RestaurantTable> GetTableRestaurant(TempObject tempObject);
        public Task<ApiResponse<Order>> CreateOrder(Reservation reservation);
        public Task<ApiResponse<Order>> CaptureOrder(string orderId);
        public Task<HttpResponseMessage> CreateReservation(Reservation reservation);
        public Task<HttpResponseMessage> UpdateAccountPoints(Account account);
        public Task<List<TempClass>> GetAccountReservations(Account account);
        public Task<List<Account>> GetAllAccounts();
    }
}
