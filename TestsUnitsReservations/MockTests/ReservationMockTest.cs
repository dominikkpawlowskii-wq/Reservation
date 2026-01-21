using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TestsUnitsReservations.MockTests
{
    [TestClass]
    public sealed class ReservationMockTest
    {
        public IReservationService? ReservationService { get; set; }
        public DbConnection? DbConnection { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            DbConnection = new SqliteConnection("Filename=:memory:");
            var contextOptions = new DbContextOptionsBuilder<ReservationsContext>()
                .UseSqlite(DbConnection)
                .Options;
            ReservationsContext reservationService = new(contextOptions);
            ReservationService = new ReservationService(reservationService);
        }

        [TestCleanup]
        public void CleanUp()
        {
            ReservationService = null;
            DbConnection?.Close();
        }

        [TestMethod]
        [TestCategory("AddReservationMock")]
        public async Task TestMockAddReservation()
        {
            Reservation reservation = new()
            {
                DateReservation = "10.12.2025",
                HourStartReservation = "14:00",
                HourEndReservation = "16:00",
                IdAccount = 1,
                IdRestaurantTable = 1,
                Price = 19.00m
            };

            var mockService = new Mock<IReservationService>();

            mockService.Setup(s => s.AddReservation(reservation)).Returns(Task.CompletedTask);

            var reservationController = new ReservationsController(mockService.Object);

            var result = await reservationController.PostReservation(reservation);

            var CreatedAtActionResult = result as CreatedAtActionResult;

            Assert.IsNotNull(CreatedAtActionResult);
            Assert.AreEqual(201, CreatedAtActionResult.StatusCode);
            mockService.Verify(s => s.AddReservation(reservation), Times.Once);
        }

        [TestMethod]
        [TestCategory("DeleteReservationMock")]
        public async Task TestMockDeleteReservation()
        {
            Reservation reservation = new()
            {
                DateReservation = "10.12.2025",
                HourStartReservation = "14:00",
                HourEndReservation = "16:00",
                IdAccount = 1,
                IdRestaurantTable = 1,
                Price = 19.00m
            };
            
            var mockService = new Mock<IReservationService>();
            mockService.Setup(s => s.GetReservation(reservation.IdRestaurantTable, reservation.DateReservation, reservation.HourStartReservation, reservation.HourEndReservation)).ReturnsAsync(reservation);
            mockService.Setup(s => s.DeleteReservation(reservation)).Returns(Task.CompletedTask);

            var reservationController = new ReservationsController(mockService.Object);

            var result = await reservationController.DeleteReservation(reservation);

            var noContentResult = result as NoContentResult;

            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
            mockService.Verify(s => s.DeleteReservation(reservation), Times.Once);
        }
        [TestMethod]
        [TestCategory("DeleteReservationMock")]
        public async Task TestMockNotDeleteReservation()
        {
            Reservation reservation = new()
            {
                DateReservation = "10.12.2025",
                HourStartReservation = "14:00",
                HourEndReservation = "16:00",
                IdAccount = 1,
                IdRestaurantTable = 1,
                Price = 19.00m
            };

            var mockService = new Mock<IReservationService>();

            var reservationController = new ReservationsController(mockService.Object);

            mockService.Setup(s => s.GetReservation(reservation.IdRestaurantTable, reservation.DateReservation, reservation.HourStartReservation, reservation.HourEndReservation)).ThrowsAsync(new Exception());

            var result = await reservationController.DeleteReservation(reservation);

            var notFoundResult = result as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
        [TestMethod]
        [TestCategory("GetAllReservationMock")]
        public async Task TestMockGetAllReservation()
        {
            List<Reservation> reservations = [];

            var mockService = new Mock<IReservationService>();

            var reservationController = new ReservationsController(mockService.Object);

            mockService.Setup(s => s.GetAllReservations()).ReturnsAsync(reservations);

            var result = await reservationController.GetReservations();

            var okObjectResult = result as OkObjectResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
        }
        [TestMethod]
        [TestCategory("GetAllReservationMock")]
        public async Task TestMockNotGetAllReservation()
        {
            List<Reservation> reservations = [];

            var mockService = new Mock<IReservationService>();

            var reservationController = new ReservationsController(mockService.Object);

            mockService.Setup(s => s.GetAllReservations()).ThrowsAsync(new Exception());

            var result = await reservationController.GetReservations();

            var okObjectResult = result as BadRequestObjectResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(400, okObjectResult.StatusCode);
        }
        [TestMethod]
        [TestCategory("GetReservationForAccountMock")]
        public async Task TestMockGetReservationForAccount()
        {
            Account account = new()
            {
                Id = 1,
                FirstName = "Test8",
                LastName = "Test8",
                Email = "example8@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 2500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };
            List<TempClass> reservationsForAccount = [];

            var mockService = new Mock<IReservationService>();

            var reservationController = new ReservationsController(mockService.Object);

            mockService.Setup(s => s.GetReservationForAccount(account)).ReturnsAsync(reservationsForAccount);

            var result = await reservationController.GetReservationforAccount(account);

            var okObjectResult = result as OkObjectResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
            mockService.Verify(s => s.GetReservationForAccount(account), Times.Once);
        }
        [TestMethod]
        [TestCategory("GetReservationForAccountMock")]
        public async Task TestMockNotGetReservationForAccount()
        {
            Account account = new()
            {
                Id = 1,
                FirstName = "Test7",
                LastName = "Test7",
                Email = "example7@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 2500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };
            List<TempClass> reservationsForAccount = [];

            var mockService = new Mock<IReservationService>();

            var reservationController = new ReservationsController(mockService.Object);

            mockService.Setup(s => s.GetReservationForAccount(account)).ThrowsAsync(new Exception());

            var result = await reservationController.GetReservationforAccount(account);

            var okObjectResult = result as BadRequestObjectResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(400, okObjectResult.StatusCode);
        }

        [TestMethod]
        [TestCategory("GetReservationMock")]
        public async Task TestMockGetReservation()
        {
            Reservation reservation = new()
            {
                DateReservation = "10.12.2025",
                HourStartReservation = "14:00",
                HourEndReservation = "16:00",
                IdAccount = 1,
                IdRestaurantTable = 1,
                Price = 19.00m
            };

            var mockService = new Mock<IReservationService>();

            var reservationController = new ReservationsController(mockService.Object);

            mockService.Setup(s => s.GetReservation(reservation.IdRestaurantTable, reservation.DateReservation, reservation.HourStartReservation, reservation.HourEndReservation)).ReturnsAsync(reservation);

            var result = await reservationController.GetReservation(reservation.IdRestaurantTable, reservation.DateReservation, reservation.HourStartReservation, reservation.HourEndReservation);

            var okObjectResult = result as OkObjectResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
        }
        [TestMethod]
        [TestCategory("GetReservationMock")]
        public async Task TestMockNotGetReservation()
        {
            Reservation reservation = new()
            {
                DateReservation = "10.12.2025",
                HourStartReservation = "14:00",
                HourEndReservation = "16:00",
                IdAccount = 1,
                IdRestaurantTable = 1,
                Price = 19.00m
            };

            var mockService = new Mock<IReservationService>();

            var reservationController = new ReservationsController(mockService.Object);

            mockService.Setup(s => s.GetReservation(reservation.IdRestaurantTable, reservation.DateReservation, reservation.HourStartReservation, reservation.HourEndReservation)).ThrowsAsync(new Exception());

            var result = await reservationController.GetReservation(reservation.IdRestaurantTable, reservation.DateReservation, reservation.HourStartReservation, reservation.HourEndReservation);

            var okObjectResult = result as NotFoundObjectResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(404, okObjectResult.StatusCode);
        }
        [TestMethod]
        [TestCategory("UniqeConstraintReservationsMock")]
        public async Task TestMockUniqueConstraintReservations()
        {
            Reservation reservation = new()
            {
                DateReservation = "10.12.2025",
                HourStartReservation = "14:00",
                HourEndReservation = "16:00",
                IdAccount = 1,
                IdRestaurantTable = 1,
                Price = 19.00m
            };
            Reservation reservation1 = new()
            {
                DateReservation = "10.12.2025",
                HourStartReservation = "14:00",
                HourEndReservation = "16:00",
                IdAccount = 1,
                IdRestaurantTable = 1,
                Price = 19.00m
            };

            var mockService = new Mock<IReservationService>();

            var reservationController = new ReservationsController(mockService.Object);

            mockService.Setup(s => s.AddReservation(reservation)).Returns(Task.CompletedTask);
            mockService.Setup(s => s.AddReservation(reservation1)).Throws<DbUpdateException>();
                        
            var result = await reservationController.PostReservation(reservation);
            var result1 = await reservationController.PostReservation(reservation1);

            var okObjectResult = result as CreatedAtActionResult;
            var coflictResult = result1 as ConflictResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(201, okObjectResult.StatusCode);

            Assert.IsNotNull(coflictResult);
            Assert.AreEqual(409, coflictResult.StatusCode);
        }
    }
}
