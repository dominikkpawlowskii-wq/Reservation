namespace Interfaces
{
    public interface IReservationService
    {
        public Task AddReservation(Reservation reservation);
        public Task<List<Reservation>> GetAllReservations();
        public Task<Reservation> GetReservation(int IdtableRestaurant, string DateReservation, string HourStartReservation, string HourEndReservation);
        public Task<List<TempClass>> GetReservationForAccount(Account account);
        public Task DeleteReservation(Reservation reservation);
    }
}
