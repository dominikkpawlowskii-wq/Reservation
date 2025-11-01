using Microsoft.Extensions.Logging;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Http.Response;
using PaypalServerSdk.Standard.Models;
using System.Text.Json;
namespace Model.Classes
{
    public class PaypalApi
    {
        public string? ClientId {  get; set; }
        public string? SecretKey { get; set; }
        public PaypalServerSdkClient? Client { get; set; }

        public void ConfigurePaypalApi()
        {
            Client = new PaypalServerSdkClient.Builder()
                .ClientCredentialsAuth(
                    new ClientCredentialsAuthModel.Builder(
                        ClientId,
                        SecretKey
                        )
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

        public async Task ReadLinesInJsonFile(string path)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(path);
            using var readStream = new StreamReader(stream);
            string json = await readStream.ReadToEndAsync();
            var Object = JsonSerializer.Deserialize<PaypalApi>(json);

            if(Object != null)
            {
                this.ClientId = Object.ClientId;
                this.SecretKey = Object.SecretKey;
            }
        }

        public async Task<ApiResponse<Order>> CreateOrder()
        {
            CreateOrderInput input = new CreateOrderInput
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
                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                        new PurchaseUnitRequest
                        {
                            Amount = new AmountWithBreakdown
                            {
                                CurrencyCode = "USD",
                                MValue = "40.00",
                                Breakdown = new AmountBreakdown
                                {
                                    ItemTotal = new Money
                                    {
                                        CurrencyCode = "USD",
                                        MValue = "40.00"
                                    }
                                }
                            },
                            Items = new List<Item>
                            {
                                new Item
                                {
                                    Name = "feeForTable",
                                    Description = "temp",
                                    UnitAmount = new Money
                                    {
                                        CurrencyCode = "USD",
                                        MValue = "40.00"
                                    },
                                    Quantity = "1",
                                    Category = ItemCategory.PhysicalGoods
                                }
                            }
                        }
                    }
                },
                Prefer = "return=minimal",
            };
            OrdersController ordersController = Client!.OrdersController;

            ApiResponse<Order> result = await ordersController.CreateOrderAsync(input);

            return result;
        }

        public async Task<ApiResponse<Order>> CapturePayment(string orderId)
        {
            CaptureOrderInput captureOrder = new CaptureOrderInput
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
