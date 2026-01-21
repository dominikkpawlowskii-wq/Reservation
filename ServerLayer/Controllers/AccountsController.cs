using Models.Classes;

namespace ServerLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        public IAccountService AccountService { get; set; }

        public AccountsController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        // GET: api/Accounts
        [HttpGet]
        [Authorize(AuthenticationSchemes = "access")]
        [Route("GetAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var accounts = await AccountService.GetAllAccount();

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            try
            {
                var account = await AccountService.GetAccount(id);

                return Ok(account);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        [HttpPatch]
        [Route("UpdateAccount")]
        public async Task<IActionResult> PatchAccount([FromBody] Account account)
        {
            try
            {
                await AccountService.UpdatePoints(account);

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> PostAccount([FromBody] Account account)
        {
            try
            {
                await AccountService.AddAccount(account);

                return CreatedAtAction("RegisterAccount", new { id = account?.Id }, account);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute]int id)
        {
            try
            {
                Account account = await AccountService.GetAccount(id);
                await AccountService.DeleteAccount(account);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
