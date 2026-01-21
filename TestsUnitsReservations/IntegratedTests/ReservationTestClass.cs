using Microsoft.AspNetCore.Authentication;

namespace TestsUnitsReservations.IntegratedTests
{
    [TestClass]
    public sealed class ReservationTestClass
    {
        public IReservationService? ReservationService { get; set; }
        public DbConnection? DbConnection { get; set; }
        public ReservationsContext? ReservationsContext { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            DbConnection = new SqliteConnection("Filename=:memory:");
            DbConnection.Open();
            var contextOptions = new DbContextOptionsBuilder<ReservationsContext>()
                .UseSqlite(DbConnection)
                .Options;
            ReservationsContext = new(contextOptions);
            ReservationService = new ReservationService(ReservationsContext);
        }

        [TestCleanup]
        public void CleanUp()
        {
            ReservationService = null;
            DbConnection?.Close();
        }
        [TestMethod]
        [TestCategory("AddReservation")]
        public async Task TestReservation_shouldAddReservationCorretly()
        {
            if(ReservationsContext.Database.EnsureCreated())
            {
                Reservation reservation = new()
                {
                    DateReservation = "10.10.2025",
                    HourStartReservation = "16:00",
                    HourEndReservation = "18:00",
                    IdAccount = 2,
                    IdRestaurantTable = 5,
                    Price = 24.99m
                };

                await ReservationService?.AddReservation(reservation)!;

                var reservations = await ReservationService.GetAllReservations();

                Assert.IsNotEmpty(reservations);
            }
        }
        [TestMethod]
        [TestCategory("AddReservationFailed")]
        public async Task TestReservation_shouldAddReservationFailed()
        {
            if(ReservationsContext.Database.EnsureCreated())
            {
                Reservation reservation = new()
                {
                    DateReservation = "",
                    HourStartReservation = "",
                    HourEndReservation = "",
                    IdAccount = 2,
                    IdRestaurantTable = 5,
                    Price = 24.99m
                };

                await Assert.ThrowsAsync<Exception>(async () =>
                {
                    await ReservationService?.AddReservation(reservation)!;
                });
            }    
        }
        [TestMethod]
        [TestCategory("ReservationAddTwoSameObject")]
        public async Task TestReservation_shouldDontAddTwoSameObject()
        {
            if(ReservationsContext.Database.EnsureCreated())
            {
                Reservation reservation = new()
                {
                    DateReservation = "25.05.2025",
                    HourStartReservation = "18:00",
                    HourEndReservation = "20:00",
                    IdAccount = 1,
                    IdRestaurantTable = 1,
                    Price = 19.00m
                };
                Reservation reservation1 = new()
                {
                    DateReservation = "25.05.2025",
                    HourStartReservation = "18:00",
                    HourEndReservation = "20:00",
                    IdAccount = 1,
                    IdRestaurantTable = 1,
                    Price = 19.00m
                };

                await ReservationService?.AddReservation(reservation)!;

                ReservationsContext.ChangeTracker.Clear();


                await Assert.ThrowsAsync<DbUpdateException>(async () =>
                {
                    await ReservationService?.AddReservation(reservation1)!;
                });
            }
        }
    }
}
