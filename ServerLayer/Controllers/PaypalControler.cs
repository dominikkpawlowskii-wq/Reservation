// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace ServerLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaypalController : ControllerBase
    {
        public IPaypalService PaypalApi { get; set; }
        public IConfiguration Configuration { get; set; }
        public PaypalController(IConfiguration configuration, IPaypalService paypalApi)
        {
            Configuration = configuration;
            PaypalApi = paypalApi;

            string CId = Configuration.GetSection("PaypalData:PaypalClientId").Value!;
            string SKey = Configuration.GetSection("PaypalData:PaypalSecretKey").Value!;
            PaypalApi.ConfigurePaypalApi(CId, SKey);
        }

        [HttpPost]
        [Route("Order")]
        public async Task<IActionResult> CreateOrder([FromBody] Reservation reservation)
        {
            ApiResponse<Order> result = await PaypalApi.CreateOrderByPaypal(reservation);

            if(result.StatusCode == 200 && result.Data.Status == OrderStatus.PayerActionRequired)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Capture")]
        public async Task<IActionResult> CaptureOrder([FromBody]string OrderId)
        {
                ApiResponse<Order> result = await PaypalApi.CapturePayment(OrderId!);

                if (result.StatusCode == 201)
                {
                    return Ok(result);
                }
                return BadRequest();
        }


    }
}
