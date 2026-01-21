using NuGet.Protocol;

namespace ServerLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableRestaurantsController : ControllerBase
    {
        private readonly ReservationsContext _context;

        public TableRestaurantsController(ReservationsContext context)
        {
            _context = context;
        }

        // GET: api/TableRestaurants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantTable>>> GetTablesRestaurants()
        {
            return await _context.RestaurantTables.ToListAsync();
        }

        // GET: api/TableRestaurants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantTable>> GetTableRestaurant([FromRoute]int id)
        {
            var tableRestaurant = await _context.RestaurantTables.FindAsync(id);

            if (tableRestaurant == null)
            {
                return NotFound();
            }

            return tableRestaurant;
        }

        // PUT: api/TableRestaurants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTableRestaurant(int id, RestaurantTable tableRestaurant)
        {
            if (id != tableRestaurant.Id)
            {
                return BadRequest();
            }

            _context.Entry(tableRestaurant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableRestaurantExists(id))
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

        // POST: api/TableRestaurants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RestaurantTable>> PostTableRestaurant(RestaurantTable tableRestaurant)
        {
            _context.RestaurantTables.Add(tableRestaurant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTableRestaurant", new { id = tableRestaurant.Id }, tableRestaurant);
        }

        // DELETE: api/TableRestaurants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTableRestaurant(int id)
        {
            var tableRestaurant = await _context.RestaurantTables.FindAsync(id);
            if (tableRestaurant == null)
            {
                return NotFound();
            }

            _context.RestaurantTables.Remove(tableRestaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("TableRestaurants")]
        public async Task<ActionResult<RestaurantTable>> GetTableRestaurant([FromBody] TempObject tempObject)
        {
            RestaurantTable tableRestaurant = _context.RestaurantTables.SingleOrDefault(tr => tr.NumberPlace == tempObject.NumberOfSides && tr.Id == tempObject.IdTable)!;

            if(tableRestaurant is not null)
            {
                return Ok(tableRestaurant);
            }
            return NoContent();
        }

        private bool TableRestaurantExists(int id)
        {
            return _context.RestaurantTables.Any(e => e.Id == id);
        }
    }
}
