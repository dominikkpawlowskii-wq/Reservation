namespace ServerLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdressRestaurantsController : ControllerBase
    {
        private readonly ReservationsContext _context;

        public AdressRestaurantsController(ReservationsContext context)
        {
            _context = context;
        }

        // GET: api/AdressRestaurants
        [HttpGet]
        [Route("AdressRestaurants")]
        public async Task<ActionResult<IEnumerable<RestaurantAddress>>> GetAdressesRestaurant()
        {
            return await _context.RestaurantAddresses.ToListAsync();
        }

        // GET: api/AdressRestaurants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantAddress>> GetAdressRestaurant(int id)
        {
            var adressRestaurant = await _context.RestaurantAddresses.FindAsync(id);

            if (adressRestaurant == null)
            {
                return NotFound();
            }

            return adressRestaurant;
        }

        // PUT: api/AdressRestaurants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdressRestaurant(int id, RestaurantAddress adressRestaurant)
        {
            if (id != adressRestaurant.Id)
            {
                return BadRequest();
            }

            _context.Entry(adressRestaurant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdressRestaurantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AdressRestaurants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RestaurantAddress>> PostAdressRestaurant(RestaurantAddress adressRestaurant)
        {
            _context.RestaurantAddresses.Add(adressRestaurant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdressRestaurant", new { id = adressRestaurant.Id }, adressRestaurant);
        }

        // DELETE: api/AdressRestaurants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdressRestaurant(int id)
        {
            var adressRestaurant = await _context.RestaurantAddresses.FindAsync(id);
            if (adressRestaurant == null)
            {
                return NotFound();
            }

            _context.RestaurantAddresses.Remove(adressRestaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdressRestaurantExists(int id)
        {
            return _context.RestaurantAddresses.Any(e => e.Id == id);
        }
    }
}
