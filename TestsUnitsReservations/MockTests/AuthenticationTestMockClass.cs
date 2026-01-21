namespace TestsUnitsReservations.MockTests
{
    [TestClass]
    public sealed class AuthenticationTestMockClass
    {
        public IAccountRefreshTokenService? AccountRefreshTokenService { get; set; }
        public IAuthenticationService? AuthenticationService { get; set; }
        public DbConnection? DbConnection { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            DbConnection = new SqliteConnection("Filename=:memory:");
            var contextOptions = new DbContextOptionsBuilder<ReservationsContext>()
                .UseSqlite(DbConnection)
                .Options;
            ReservationsContext reservationService = new(contextOptions);
            AccountRefreshTokenService = new AccountRefreshTokenService(reservationService);
            AuthenticationService = new AuthenticationServise(reservationService, AccountRefreshTokenService);
        }

        [TestCleanup]
        public void CleanUp()
        {
            AuthenticationService = null;
            DbConnection?.Close();
        }
        [TestMethod]
        [TestCategory("authenticationMock")]
        public void TestAuthenticationMock()
        {
            Authentication authentication = new()
            {
                Email = "dominikk.pawlowskii@gmail.com",
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };

            TempDataAccount tempDataAccount = new()
            {
                Account = new Account
                {
                    Id = 1,
                    FirstName = "Dominik",
                    LastName = "Pawlowski",
                    Password = "MyPassword",
                    Email = "example@gmail.com",
                    NumberOfPoints = 1500
                },
                Tokens = new Tokens
                {
                    AccessToken = "MyAccesTokens!",
                    RefreshToken = "MyRefreshTokens!"
                }
            };

            var mockAuthenticationService = new Mock<IAuthenticationService>();

            mockAuthenticationService.Setup(s => s.GetAccountWithtokens(authentication)).ReturnsAsync(tempDataAccount);

            var controler = new AuthenticationController(mockAuthenticationService.Object);

            var result = controler.PostAuthentication(authentication);

            var okResult = result.Result as OkObjectResult;

            var returnObject = okResult?.Value as TempDataAccount;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(tempDataAccount.Account.Password, returnObject?.Account?.Password);
            mockAuthenticationService.Verify(s => s.GetAccountWithtokens(authentication), Times.Once);
        }

        [TestMethod]
        [TestCategory("authenticationMock")]
        public async Task TestNotAuthenticationMock()
        {
            Authentication authentication = new Authentication
            {
                Email = "dominikk.pawlowskii@gmail.com",
                Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
            };

            TempDataAccount tempDataAccount = new()
            {
                Account = new Account
                {
                    Id = 1,
                    FirstName = "Dominik",
                    LastName = "Pawlowski",
                    Password = "MyPassword",
                    Email = "example@gmail.com",
                    NumberOfPoints = 1500
                },
                Tokens = new Tokens
                {
                    AccessToken = "MyAccesTokens!",
                    RefreshToken = "MyRefreshTokens!"
                }
            };

            var mockAuthenticationService = new Mock<IAuthenticationService>();

            mockAuthenticationService.Setup(s => s.GetAccountWithtokens(authentication)).ThrowsAsync(new Exception());

            var controler = new AuthenticationController(mockAuthenticationService.Object);

            var result = await controler.PostAuthentication(authentication);

            var okResult = result as UnauthorizedObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(401, okResult.StatusCode);
        }
        

    }
}
