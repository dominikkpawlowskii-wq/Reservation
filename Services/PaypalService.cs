

namespace Services
{
    public class PaypalService : IPaypalService
    {
        public PaypalServerSdkClient? Client { get; set; }

        public PaypalService()
        {

        }

        public void ConfigurePaypalApi(string clientId, string secretKey)
        {
            Client = new PaypalServerSdkClient.Builder()
                .ClientCredentialsAuth(
                    new ClientCredentialsAuthModel.Builder(
                        clientId,
                        secretKey
                        )
                    .OAuthTokenProvider(async (credientalMenager, token) =>
                    {
                        return await credientalMenager.FetchTokenAsync();
                    })
                    .Build()
                )
                .Environment(PaypalServerSdk.Standard.Environment.Sandbox)
                .LoggingConfig(config => config
                    .LogLevel(LogLevel.Information)
                    .RequestConfig(reqconfig => reqconfig.Body(true))
                    .ResponseConfig(respConfig => respConfig.Headers(true))
                )
                .Build();
        }

        public async Task<ApiResponse<Order>> CreateOrderByPaypal(Reservation reservation)
        {
            CreateOrderInput input = new()
            {
                Body = new OrderRequest
                {
                    Intent = CheckoutPaymentIntent.Capture,
                    PaymentSource = new PaymentSource
                    {
                        Paypal = new PaypalWallet
                        {
                            ExperienceContext = new PaypalWalletExperienceContext
                            {
                                PaymentMethodPreference = PayeePaymentMethodPreference.ImmediatePaymentRequired,
                                LandingPage = PaypalExperienceLandingPage.Login,
                                UserAction = PaypalExperienceUserAction.PayNow,
                                ReturnUrl = "https://myapp/Capture/",
                                CancelUrl = "https://myapp/Cancel/"
                            }
                        }
                    },
                    PurchaseUnits =
                    [
                        new() {
                            InvoiceId = Guid.NewGuid().ToString(),
                            Amount = new AmountWithBreakdown
                            {
                                CurrencyCode = "PLN",
                                MValue = reservation.Price.ToString(CultureInfo.InvariantCulture),
                                Breakdown = new AmountBreakdown
                                {
                                    ItemTotal = new Money
                                    {
                                        CurrencyCode = "PLN",
                                        MValue = reservation.Price.ToString(CultureInfo.InvariantCulture)
                                    }
                                }
                            },
                            Items =
                            [
                                new ItemRequest
                                {
                                    Name = "feeForTable",
                                    Description = "temp",
                                    UnitAmount = new Money
                                    {
                                        CurrencyCode = "PLN",
                                        MValue = reservation.Price.ToString(CultureInfo.InvariantCulture)
                                    },
                                    Quantity = "1",
                                    Category = ItemCategory.PhysicalGoods
                                }
                            ]
                        }
                    ]
                },
                Prefer = "return=minimal",
            };
            OrdersController ordersController = Client!.OrdersController;

            ApiResponse<Order> result = await ordersController.CreateOrderAsync(input);

            return result;
        }

        public async Task<ApiResponse<Order>> CapturePayment(string orderId)
        {
                CaptureOrderInput captureOrder = new()
                {
                    Id = orderId,
                    Prefer = "return=minimal"
                };
                OrdersController ordersController1 = Client!.OrdersController;

                ApiResponse<Order> result = await ordersController1.CaptureOrderAsync(captureOrder);

                return result;
        }
    }
}
