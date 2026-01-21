

namespace TestsUnitsReservations.IntegratedTests
{
    [TestClass]
    public sealed class AccountTestClass
    {
        public IAccountRefreshTokenService? AccountRefreshTokenService { get; set; }
        public IAuthenticationService? AuthenticationService { get; set; }
        public IAccountService? AccountService { get; set; }
        public DbConnection? DbConnection {  get; set; }
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
            AccountService = new AccountService(ReservationsContext);
            AccountRefreshTokenService = new AccountRefreshTokenService(ReservationsContext);
            AuthenticationService = new AuthenticationServise(ReservationsContext, AccountRefreshTokenService);
        }

        [TestCleanup]
        public void CleanUp()
        {
            AuthenticationService = null;
            AccountService = null;
            DbConnection?.Close();
        }


        [TestMethod]
        [TestCategory("authentication")]
        public async Task TestIntegratet_shouldLoginCorretly()
        {

            if(ReservationsContext!.Database.EnsureCreated())
            {
                Account account = new()
                {
                    FirstName = "Test",
                    LastName = "Test",
                    Email = "example@gmail.com",
                    Telephone = "555555555",
                    NumberOfPoints = 1500,
                    Password = "Domino22#"
                };


                Authentication authentication = new()
                {
                    Email = "example@gmail.com",
                    Password = "Domino22#"
                };

                await AccountService?.AddAccount(account)!;
                TempDataAccount dataAccount = await AuthenticationService?.GetAccountWithtokens(authentication)! ?? throw new Exception("konto nie istnieje!");
                Assert.IsNotNull(dataAccount);
                Assert.AreEqual(dataAccount?.Account?.Email, authentication.Email);
            }
        }
        [TestMethod]
        [TestCategory("authenticationFailed")]
        public async Task TestIntegratet_shouldLoginFailed()
        {

            if(ReservationsContext.Database.EnsureCreated())
            {
                Account account = new()
                {
                    FirstName = "Test2",
                    LastName = "Test2",
                    Email = "example2@gmail.com",
                    Telephone = "555555555",
                    NumberOfPoints = 1500,
                    Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
                };


                Authentication authentication = new()
                {
                    Email = "email@gmail.com",
                    Password = "Password1234"
                };

                await AccountService?.AddAccount(account)!;
                

                await Assert.ThrowsAsync<Exception>(async () =>
                {
                    TempDataAccount dataAccount = await AuthenticationService?.GetAccountWithtokens(authentication)!;
                });
            }
        }

        [TestMethod]
        [TestCategory("RegisterAccount")]
        public async Task TestIntegratet_shouldAddAccountCorretly()
        {
            if(ReservationsContext.Database.EnsureCreated())
            {
                Account account = new()
                {
                    FirstName = "Test3",
                    LastName = "Test3",
                    Email = "example3@gmail.com",
                    Telephone = "666666666",
                    NumberOfPoints = 1500,
                    Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
                };

                await AccountService?.AddAccount(account)!;

                var accounts = await AccountService.GetAllAccount();
                var accountFromDatabase = accounts.SingleOrDefault();
                Assert.IsNotEmpty(accounts);
                Assert.AreEqual(account.FirstName, accountFromDatabase!.FirstName);
            }
        }

        [TestMethod]
        [TestCategory("RegisterAccountFailed")]
        public async Task TestIntegratet_shouldAddAccountFailed()
        {
            if(ReservationsContext.Database.EnsureCreated())
            {
                Account account = new()
                {
                    FirstName = "Test4",
                    LastName = "Test4",
                    Email = "exampleGmainlJakisCom",
                    Telephone = "999999999",
                    NumberOfPoints = 1500,
                    Password = "$2a$"
                };

                await Assert.ThrowsAsync<Exception>(async () =>
                {
                    await AccountService?.AddAccount(account)!;
                });
            }
        }

        [TestMethod]
        [TestCategory("GetAccounts")]
        public async Task TestIntegratet_shouldGetAllAccounts()
        {
            if(ReservationsContext.Database.EnsureCreated())
            {
                Account account = new()
                {
                    FirstName = "Tests",
                    LastName = "Tests",
                    Email = "Examplesed@gmail.com",
                    Telephone = "999999999",
                    NumberOfPoints = 2500,
                    Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
                };

                List<Account> accounts = [];

                await AccountService!.AddAccount(account);

                accounts = await AccountService!.GetAllAccount();

                Assert.IsNotNull(accounts);
            }
        }
        [TestMethod]
        [TestCategory("UpdateAccountPoints")]
        public async Task TestIntegratet_shouldUpdatePoints()
        {
            if (ReservationsContext.Database.EnsureCreated())
            {
                Account account = new()
                {
                    Id = 1,
                    FirstName = "Test5",
                    LastName = "Test5",
                    Email = "example5@gmail.com",
                    Telephone = "555555555",
                    NumberOfPoints = 2500,
                    Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
                };
                await AccountService!.AddAccount(account);

                await AccountService!.UpdatePoints(account);

                var userAccount = await AccountService.GetAccount(account.Id);

                Assert.AreEqual(account.NumberOfPoints, userAccount.NumberOfPoints);
            }  
        }
        [TestMethod]
        [TestCategory("UpdateAccountPoints")]
        public async Task TestIntegratet_shouldNotUpdatePoints()
        {
            if (ReservationsContext.Database.EnsureCreated())
            {
                Account account = new()
                {
                    Id = 1,
                    FirstName = "Test5",
                    LastName = "Test5",
                    Email = "example5@gmail.com",
                    Telephone = "555555555",
                    NumberOfPoints = 2500,
                    Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
                };

                await Assert.ThrowsAsync<Exception>(async () =>
                {
                    await AccountService!.UpdatePoints(account);
                });
            }
        }
        [TestMethod]
        [TestCategory("DeleteAccount")]
        public async Task TestIntegratet_shouldDeleteAccount()
        {
            if (ReservationsContext.Database.EnsureCreated())
            {
                Account account = new()
                {
                    Id = 1,
                    FirstName = "Test5",
                    LastName = "Test5",
                    Email = "example5@gmail.com",
                    Telephone = "555555555",
                    NumberOfPoints = 2500,
                    Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
                };
                await AccountService!.AddAccount(account);
                
                await AccountService.DeleteAccount(account);

                
                await Assert.ThrowsAsync<Exception>(async () =>
                {
                    var accounts = await AccountService.GetAllAccount();
                });
               
            }
        }
        [TestMethod]
        [TestCategory("DeleteAccount")]
        public async Task TestIntegratet_shouldNotDeleteAccount()
        {
            if (ReservationsContext.Database.EnsureCreated())
            {
                Account account = new()
                {
                    Id = 1,
                    FirstName = "Test5",
                    LastName = "Test5",
                    Email = "example5@gmail.com",
                    Telephone = "555555555",
                    NumberOfPoints = 2500,
                    Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
                };

                await Assert.ThrowsAsync<Exception>(async () =>
                {
                    await AccountService.DeleteAccount(account);
                });

            }
        }
        [TestMethod]
        [TestCategory("GetAccount")]
        public async Task TestIntegratet_shouldGetAccount()
        {
            if (ReservationsContext.Database.EnsureCreated())
            {
                Account account = new()
                {
                    Id = 1,
                    FirstName = "Test5",
                    LastName = "Test5",
                    Email = "example5@gmail.com",
                    Telephone = "555555555",
                    NumberOfPoints = 2500,
                    Password = "$2a$11$H2HZUrnOqiv/IhC6bSfavOee3Xg49Zy2ubyqfmjRvyjTQrsqTGyGW"
                };

                await AccountService!.AddAccount(account);
                var userAccount = await AccountService.GetAccount(account.Email, account.FirstName);

                Assert.AreEqual(account.Password, userAccount.Password);

            }
        }
        [TestMethod]
        [TestCategory("GetAccounts")]
        public async Task TestIntegratet_shouldNotGetAllAccounts()
        {
            if (ReservationsContext.Database.EnsureCreated())
            {
                List<Account> accounts = [];

                await Assert.ThrowsAsync<Exception>(async () =>
                {
                    accounts = await AccountService.GetAllAccount();
                });

            }
        }

    }
}
