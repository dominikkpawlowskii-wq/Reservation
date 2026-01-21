namespace Interfaces
{
    public interface IPaypalService
    {
        public void ConfigurePaypalApi(string ClientId, string SecretKey);
        public Task<ApiResponse<Order>> CreateOrderByPaypal(Reservation reservation);
        public Task<ApiResponse<Order>> CapturePayment(string orderId);
    }
}
