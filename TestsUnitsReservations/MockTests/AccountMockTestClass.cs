using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Framework;

namespace TestsUnitsReservations.MockTests
{
    [TestClass]
    public sealed class AccountMockTestClass
    {
        public IAccountService? AccountService { get; set; }
        public DbConnection? DbConnection { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            DbConnection = new SqliteConnection("Filename=:memory:");
            DbConnection?.Open();
            var contextOptions = new DbContextOptionsBuilder<ReservationsContext>()
                .UseSqlite(DbConnection)
                .Options;
            ReservationsContext reservationService = new(contextOptions);
            AccountService = new AccountService(reservationService);
        }

        [TestCleanup]
        public void CleanUp()
        {
            AccountService = null;
            DbConnection?.Close();
        }
        [TestMethod]
        [TestCategory("AddAccountMock")]
        public async Task TestMockAddAccount()
        {
            Account account = new()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "example@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 1500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };

            var mockService = new Mock<IAccountService>();
            mockService.Setup(s => s.AddAccount(account)).Returns(Task.CompletedTask);

            var AccountControler = new AccountsController(mockService.Object);

            var result = await AccountControler.PostAccount(account);

            var createdAtActionResult = result as CreatedAtActionResult;

            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            mockService.Verify(s => s.AddAccount(account), Times.Once);
        }

        [TestMethod]
        [TestCategory("AddAccountMock")]
        public async Task TestMockNotAddAccount()
        {
            Account account = new()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "exampgmailCom",
                Telephone = "555555555",
                NumberOfPoints = 1500,
                Password = "$2a"
            };

            var mockService = new Mock<IAccountService>();
            mockService.Setup(s => s.AddAccount(account)).ThrowsAsync(new Exception());

            var AccountControler = new AccountsController(mockService.Object);

            var result = await AccountControler.PostAccount(account);

            var BadRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(BadRequestResult);
            Assert.AreEqual(400, BadRequestResult.StatusCode);
            mockService.Verify(s => s.AddAccount(account), Times.Once);
        }
        [TestMethod]
        [TestCategory("UpdateAccountPointMock")]
        public async Task TestUpdatePointMock()
        {
            Account account = new()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "example@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 1500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };

            var mockService = new Mock<IAccountService>();

            mockService.Setup(s => s.UpdatePoints(account)).Returns(Task.CompletedTask);

            var accountController = new AccountsController(mockService.Object);

            var result = await accountController.PatchAccount(account);

            var okResult = result as OkResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            mockService.Verify(s => s.UpdatePoints(account), Times.Once);
        }

        [TestMethod]
        [TestCategory("UpdateAccountPointMock")]
        public async Task TestNotUpdatePointMock()
        {
            Account account = new()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "example@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 1500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };

            var mockService = new Mock<IAccountService>();

            mockService.Setup(s => s.UpdatePoints(account)).ThrowsAsync(new Exception());
            
            var accountController = new AccountsController(mockService.Object);

            var result = await accountController.PatchAccount(account);

            var okResult = result as BadRequestObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(400, okResult.StatusCode);
            mockService.Verify(s => s.UpdatePoints(account), Times.Once);
        }
        [TestMethod]
        [TestCategory("GetAllAccounts")]
        public async Task TestGetAllAccounts()
        {
            Account account = new()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "example@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 1500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };

            List<Account> accounts = [];

            accounts.Add(account);

            var mockService = new Mock<IAccountService>();

            mockService.Setup(s => s.GetAllAccount()).ReturnsAsync(accounts);

            var accountController = new AccountsController(mockService.Object);

            var result = await accountController.GetAccounts();

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            mockService.Verify(s => s.GetAllAccount(), Times.Once);
        }
        [TestMethod]
        [TestCategory("GetAllAccounts")]
        public async Task TestNotGetAllAccounts()
        {
            List<Account> accounts = [];

            var mockService = new Mock<IAccountService>();

            mockService.Setup(s => s.GetAllAccount()).ThrowsAsync(new Exception());

            var accountController = new AccountsController(mockService.Object);

            var result = await accountController.GetAccounts();

            var okResult = result as NotFoundObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(404, okResult.StatusCode);
            mockService.Verify(s => s.GetAllAccount(), Times.Once);
        }

        [TestMethod]
        [TestCategory("GetAccount")]
        public async Task TestGetAccount()
        {
            Account account = new()
            {
                Id = 1,
                FirstName = "Test6",
                LastName = "Test6",
                Email = "example6@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 2500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };
            var mockService = new Mock<IAccountService>();

            mockService.Setup(s => s.GetAccount(account.Id)).ReturnsAsync(account);
            
            var accountController = new AccountsController(mockService.Object);

            var result = await accountController.GetAccount(account.Id);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            mockService.Verify(s => s.GetAccount(1), Times.Once);
        }
        [TestMethod]
        [TestCategory("GetAccount")]
        public async Task TestNotGetAccount()
        {
            Account account = new()
            {
                Id = 1,
                FirstName = "Test6",
                LastName = "Test6",
                Email = "example6@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 2500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };
            var mockService = new Mock<IAccountService>();

            mockService.Setup(s => s.GetAccount(account.Id)).ThrowsAsync(new Exception());

            var accountController = new AccountsController(mockService.Object);

            var result = await accountController.GetAccount(account.Id);

            var okResult = result as NotFoundObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(404, okResult.StatusCode);
            mockService.Verify(s => s.GetAccount(1), Times.Once);
        }

        [TestMethod]
        [TestCategory("DeleteAccount")]
        public async Task TestDeleteAccountMock()
        {
            Account account = new()
            {
                Id = 1,
                FirstName = "Test8",
                LastName = "Test8",
                Email = "example8@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 3500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };
            var mockService = new Mock<IAccountService>();

            
            mockService.Setup(s => s.DeleteAccount(account)).Returns(Task.CompletedTask);
            mockService.Setup(s => s.GetAccount(account.Id)).ReturnsAsync(account);
            var accountController = new AccountsController(mockService.Object);

            var result = await accountController.DeleteAccount(account.Id);

            var okResult = result as NoContentResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(204, okResult.StatusCode);
            mockService.Verify(s => s.DeleteAccount(account), Times.Once);
            mockService.Verify(s => s.GetAccount(account.Id), Times.Once);
        }
        [TestMethod]
        [TestCategory("DeleteAccount")]
        public async Task TestNotDeleteAccountMock()
        {
            Account account = new()
            {
                Id = 1,
                FirstName = "Test8",
                LastName = "Test8",
                Email = "example8@gmail.com",
                Telephone = "555555555",
                NumberOfPoints = 3500,
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };
            var mockService = new Mock<IAccountService>();

            mockService.Setup(s => s.GetAccount(account.Id)).ReturnsAsync(account);
            mockService.Setup(s => s.DeleteAccount(account)).ThrowsAsync(new Exception());
            
            var accountController = new AccountsController(mockService.Object);

            var result = await accountController.DeleteAccount(account.Id);

            var okResult = result as NotFoundObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(404, okResult.StatusCode);
            mockService.Verify(s => s.DeleteAccount(account), Times.Once);

        }


    }
}
