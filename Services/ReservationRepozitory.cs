using EntitySQLite;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Classes;

namespace Services
{
    public class ReservationRepozitory : IReservationRepository
    {
        private readonly ReservationsContext _context;
        public Reservation? Reservation { get; set; }
        public Account? Account { get; set; }
        public int Points { get; set; }
        public string? IdOrder { get; set; }

        public ReservationRepozitory(ReservationsContext reservationsContext)
        {
            this._context = reservationsContext;
        }

        public async Task<Account?> GetAccount(string email)
        {
            return await Task.Run(() => _context.accounts.FirstOrDefaultAsync(a => a.Email == email)); 
        }

        public async Task AddAccount(Account accounts)
        {
            await _context.accounts.AddAsync(accounts);
            await SaveChanges();
        }

        public async Task<(List<Restaurant>,List<AdressRestaurant>, List<RestaurantImage>)> GetAllRestaurantsAndAdressesAndRestaurantImages()
        {
            var restaurant = _context.restaurants.ToListAsync();
            var adresses = _context.adressRestaurant.ToListAsync();
            var restaurantImages = _context.restaurantImages.ToListAsync();

            await Task.WhenAll(restaurant, adresses, restaurantImages);

            return (await restaurant, await adresses, await restaurantImages);
        }

        public async Task<TableRestaurants?> GetTableRestaurantWithNumberOfPlaceArgument(int? number, int? idRestaurant)
        {
            return await _context.tableRestaurants.FirstOrDefaultAsync(tR => tR.NumberPlace == number && tR.IdAdressRestaurant == idRestaurant);
        }

        public async Task AddReservation(Reservation reservation)
        {
            await _context.reservation.AddAsync(reservation);
            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<TempClass>> GetListTempClass(Account account)
        {
            var tempclass = (from r in _context.restaurants
                             join rImages in _context.restaurantImages on r.Id equals rImages.Id
                             join a in _context.adressRestaurant on r.Id equals a.IdRestaurant
                             join tr in _context.tableRestaurants on a.Id equals tr.IdAdressRestaurant
                             join reserv in _context.reservation on tr.Id equals reserv.IdtableRestaurant
                             where reserv.IdAccount == account.Id
                             select new TempClass
                             {
                                 Restaurant = r,
                                 RestaurantImage = rImages,
                                 AdressRestaurant = a,
                                 TableRestaurants = tr,
                                 Reservation = reserv
                             }).ToListAsync();

            return await tempclass;
        }

        public async Task RemoveReservation(Account account, Reservation reservation)
        {
            if(reservation.IdAccount == account.Id)
            {
                _context.reservation.Remove(reservation);
                await SaveChanges();
            }

           

        }
        public async Task UpdateAccountReservation(TempClass tempClass, DateTime dateTime, TimeSpan timeSpan, int numberOfPersons)
        {
            var OtherTempClass = (from r in await _context.reservation.ToListAsync()
                                 join tR in await _context.tableRestaurants.ToListAsync() on r.IdtableRestaurant equals tR.Id
                                 where r.IdAccount == tempClass!.Account!.Id && r.Id == tempClass!.Reservation!.Id
                                 select new TempClass
                                 {
                                     Reservation = r,
                                     TableRestaurants = tR,
                                 }).FirstOrDefault();

            OtherTempClass!.Reservation!.dateReservation = dateTime.ToShortDateString();
            OtherTempClass.Reservation.hourStartReservations = timeSpan.ToString(@"hh\:mm");
            OtherTempClass!.TableRestaurants!.NumberPlace = numberOfPersons;

            await SaveChanges();
        }

        public async Task UpdateNumberOfPoints(Account Account, int sumOfPoints)
        {
            var account = await _context.accounts.SingleOrDefaultAsync(a => a.Id == Account!.Id);

            account!.NumberOfPoints = sumOfPoints;
            await SaveChanges();
        }

        public (Account,Reservation, int, string) ReturnArguments()
        {
            return (Account,Reservation, Points, IdOrder)!;
        }
        public void PutArgument(Account account,Reservation reservation, int sumOfNumerPoints, string idOrder)
        {
            this.Account = account;
            this.Reservation = reservation;
            this.Points = sumOfNumerPoints;
            this.IdOrder = idOrder;
        }
    }
}
