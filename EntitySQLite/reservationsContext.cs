namespace EntitySQLite
{
    public partial class ReservationsContext : DbContext
    {
        public DbSet<RestaurantImage> RestaurantImages { get; set; }
        public DbSet<Restaurant> Restaurants {  get; set; }
        public DbSet<RestaurantAddress> RestaurantAddresses { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRefreshToken> AccountRefreshTokens { get; set; }

        public ReservationsContext(DbContextOptions option)
            : base(option)
        {
            
        }
    }
}
