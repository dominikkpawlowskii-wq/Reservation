using Models.Classes;

namespace ServerLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        public IReservationService ReservationService { get; set; }

        public ReservationsController(IReservationService reservationService)
        {
            ReservationService = reservationService;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var Reservations = await ReservationService.GetAllReservations();
                return Ok(Reservations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetReservation(int IdRestaurantTable, string DateReservation, string HourStartReservation, string HourEndReservation)
        {
            try
            {
                var reservation = await ReservationService.GetReservation(IdRestaurantTable, DateReservation, HourStartReservation, HourEndReservation);

                return Ok(reservation);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        [Route("ReservationAccount")]
        public async Task<IActionResult> GetReservationforAccount([FromBody] Account account)
        {
            try
            {
                var listObjectReservations = await ReservationService.GetReservationForAccount(account);

                return Ok(listObjectReservations);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.IdtableRestaurant)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Reservations")]
        public async Task<IActionResult> PostReservation(Reservation reservation)
        {
            try
            {
                await ReservationService.AddReservation(reservation);
                return CreatedAtAction("PostReservation", new { id = reservation.IdRestaurantTable }, reservation);
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }

        // DELETE: api/Reservations/5
        [HttpDelete]
        public async Task<IActionResult> DeleteReservation([FromBody]Reservation reservation)
        {
            try
            {
                var _reservation = await ReservationService.GetReservation(reservation.IdRestaurantTable, reservation.DateReservation, reservation.HourStartReservation, reservation.HourEndReservation);

                await ReservationService.DeleteReservation(_reservation);

                return NoContent();
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
