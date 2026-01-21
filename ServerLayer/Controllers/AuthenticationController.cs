// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace ServerLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IAuthenticationService AuthenticationService { get; set; }
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;

        }

        // POST api/<AuthentifcationController>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> PostAuthentication([FromBody] Authentication authentication)
        {
            try
            {
                TempDataAccount dataAccount = await AuthenticationService.GetAccountWithtokens(authentication);

                return Ok(dataAccount); 
            }
            catch(Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
