using Microsoft.EntityFrameworkCore;
using Model.Classes;
namespace EntitySQLite
{
    public partial class ReservationsContext : DbContext
    {
        public DbSet<RestaurantImage> restaurantImages { get; set; }
        public DbSet<Restaurant> restaurants {  get; set; }
        public DbSet<AdressRestaurant> adressRestaurant { get; set; }
        public DbSet<TableRestaurants> tableRestaurants { get; set; }
        public DbSet<Reservation> reservation { get; set; }
        public DbSet<Account> accounts { get; set; }

        public ReservationsContext(DbContextOptions option)
            : base(option)
        {
            
        }
    }
}
