using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace TestsUnitsReservations.IntegratedTests
{
    [TestClass]
    public class AuthenticationTestClass
    {
        public IAuthenticationService? AuthenticationService { get; set; }
        public IAccountRefreshTokenService? AccountRefreshTokenService { get; set; }
        public IAccountService? AccountService { get; set; }
        public DbConnection? DbConnection { get; set; }
        public ReservationsContext? DatabaseContext { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            DbConnection = new SqliteConnection("Filename=:memory:");
            DbConnection.Open();
            var contextOptions = new DbContextOptionsBuilder<ReservationsContext>()
                .UseSqlite(DbConnection)
                .Options;
            DatabaseContext = new(contextOptions);
            AccountRefreshTokenService = new AccountRefreshTokenService(DatabaseContext);
            AccountService = new AccountService(DatabaseContext);
            AuthenticationService = new AuthenticationServise(DatabaseContext, AccountRefreshTokenService);
        }

        [TestCleanup]
        public void CleanUp()
        {
            AccountRefreshTokenService = null;
            AuthenticationService = null;
            DbConnection?.Close();
        }

        [TestMethod]
        [TestCategory("authentication")]
        public async Task TestIntegrated_shouldUpdateRefreshToken()
        {
            if (DatabaseContext!.Database.EnsureCreated())
            {
                AccountRefreshToken accountRefreshToken = new()
                {
                    IdAccount = 1,
                    ClaimKey = "MyrefreshToken!"
                };
                AccountRefreshToken accountRefreshTokenTwo = new()
                {
                    IdAccount = 1,
                    ClaimKey = "MyrefreshTokenAndUpdateThem!"
                };
                Account account = new()
                {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    Email = "example@gmail.com",
                    Telephone = "555555555",
                    NumberOfPoints = 1500,
                    Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
                };

                await AccountService!.AddAccount(account);
                await AccountRefreshTokenService!.AddAccountRefreshToken(accountRefreshToken);

                DatabaseContext.ChangeTracker.Clear();

                await AccountRefreshTokenService.UpdateAccountRefreshToken(accountRefreshTokenTwo);

                var newAccountToken = await AccountRefreshTokenService.GetAccountRefreshToken(account.Id);

                Assert.IsNotNull(newAccountToken);
                Assert.AreEqual(accountRefreshTokenTwo.ClaimKey, newAccountToken.ClaimKey);
            }
        }
    }
}
