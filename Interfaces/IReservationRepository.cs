
using Model.Classes;
namespace Interfaces
{
    public interface IReservationRepository
    {
        public Task<Account?> GetAccount(string email);

        public Task AddAccount(Account accounts);

        public Task<(List<Restaurant>, List<AdressRestaurant>, List<RestaurantImage>)> GetAllRestaurantsAndAdressesAndRestaurantImages();

        public Task<TableRestaurants?> GetTableRestaurantWithNumberOfPlaceArgument(int? number, int? idRestaurant);
        public Task AddReservation(Reservation reservation);

        public Task SaveChanges();

        public Task<List<TempClass>> GetListTempClass(Account account);
        public Task RemoveReservation(Account account, Reservation reservation);
        public Task UpdateAccountReservation(TempClass tempClass, DateTime dateTime, TimeSpan timeSpan, int numberOfPersons);
        public Task UpdateNumberOfPoints(Account account, int sumOfPoints);
        public (Account,Reservation, int, string) ReturnArguments();
        public void PutArgument(Account account,Reservation reservation, int sumOfNumerPoints, string idOrder);
    }
}
