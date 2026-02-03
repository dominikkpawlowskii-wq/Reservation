namespace ServerLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantImagesController : ControllerBase
    {
        private readonly ReservationsContext _context;

        public RestaurantImagesController(ReservationsContext context)
        {
            _context = context;
        }

        // GET: api/RestaurantImages
        [HttpGet]
        [Route("restaurantImages")]
        public async Task<ActionResult<IEnumerable<RestaurantImage>>> GetRestaurantImages()
        {
            return await _context.RestaurantImages.ToListAsync();
        }

        // GET: api/RestaurantImages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantImage>> GetRestaurantImage(int id)
        {
            var restaurantImage = await _context.RestaurantImages.FindAsync(id);

            if (restaurantImage == null)
            {
                return NotFound();
            }

            return restaurantImage;
        }

        // PUT: api/RestaurantImages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurantImage(int id, RestaurantImage restaurantImage)
        {
            if (id != restaurantImage.Id)
            {
                return BadRequest();
            }

            _context.Entry(restaurantImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantImageExists(id))
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

        // POST: api/RestaurantImages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RestaurantImage>> PostRestaurantImage(RestaurantImage restaurantImage)
        {
            _context.RestaurantImages.Add(restaurantImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRestaurantImage", new { id = restaurantImage.Id }, restaurantImage);
        }

        // DELETE: api/RestaurantImages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurantImage(int id)
        {
            var restaurantImage = await _context.RestaurantImages.FindAsync(id);
            if (restaurantImage == null)
            {
                return NotFound();
            }

            _context.RestaurantImages.Remove(restaurantImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RestaurantImageExists(int id)
        {
            return _context.RestaurantImages.Any(e => e.Id == id);
        }
    }
}
