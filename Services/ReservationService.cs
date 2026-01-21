using System.Resources;

namespace Services
{
    public class ReservationService : IReservationService
    {
        public ReservationsContext ReservationsContext { get; set; }
        public ReservationService(ReservationsContext context)
        {
            ReservationsContext = context;
        }

        public async Task AddReservation(Reservation reservation)
        {
                if(reservation?.DateReservation?.Length < 2 && 
                    reservation?.HourStartReservation?.Length < 2 &&
                    reservation?.HourEndReservation?.Length < 2)
                    {
                        throw new Exception("Reservation incorrect!");
                    }
                await ReservationsContext.Reservations.AddAsync(reservation);
                await ReservationsContext.SaveChangesAsync();
        }
        public async Task<List<Reservation>> GetAllReservations()
        {
            var reservations = await ReservationsContext.Reservations.ToListAsync();
            if (reservations.Count == 0)
                throw new Exception("Reservations Empty!");
            return reservations;
        }
        public async Task<Reservation> GetReservation(int IdtableRestaurant, string DateReservation, string HourStartReservation, string HourEndReservation)
        {
            var _reservation = await ReservationsContext.Reservations.SingleOrDefaultAsync(r =>
            r.IdRestaurantTable == IdtableRestaurant
            && r.DateReservation == DateReservation
            && r.HourStartReservation == HourStartReservation
            && r.HourEndReservation == HourEndReservation);

            if (_reservation is null)
                throw new Exception("Reservation empty!");

            return _reservation!;
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            var reserv = ReservationsContext.Reservations.SingleOrDefaultAsync(r => r.IdRestaurantTable == reservation.IdRestaurantTable
            && r.HourStartReservation == reservation.HourStartReservation
            && r.HourEndReservation == reservation.HourEndReservation
            && r.DateReservation == reservation.DateReservation);

            if (reserv is null)
                throw new Exception("reservation doesnt exist!");

            ReservationsContext.Reservations.Remove(reservation);
            await ReservationsContext.SaveChangesAsync();
        }

        public async Task<List<TempClass>> GetReservationForAccount(Account account)
        {
            var listObjectReservations = await (from r in ReservationsContext.Restaurants
                                                join aRestaurant in ReservationsContext.RestaurantAddresses on r.Id equals aRestaurant.IdRestaurant
                                                join tRestaurant in ReservationsContext.RestaurantTables on aRestaurant.Id equals tRestaurant.IdRestaurantAddress
                                                join reservation in ReservationsContext.Reservations on tRestaurant.Id equals reservation.IdRestaurantTable
                                                where reservation.IdAccount == account.Id
                                                select new TempClass
                                                {
                                                    RestaurantImage = ReservationsContext.RestaurantImages.SingleOrDefault(rImage => rImage.IdRestaurant == r.Id),
                                                    Reservation = reservation,
                                                    Restaurant = r,
                                                    AdressRestaurant = aRestaurant,
                                                    TableRestaurants = tRestaurant,
                                                }).ToListAsync();
            if (listObjectReservations.Count == 0)
                throw new Exception("List empty!");
            return listObjectReservations;
        }
    }
}
