namespace Services
{
    public class AccountRefreshTokenService : IAccountRefreshTokenService
    {
        public ReservationsContext? ReservationsContext { get; set; }
        public AccountRefreshTokenService(ReservationsContext reservationsContext)
        {
            ReservationsContext = reservationsContext;
        }
        public async Task<AccountRefreshToken> GetAccountRefreshToken(int id)
        {
            return await ReservationsContext.AccountRefreshTokens.SingleOrDefaultAsync(art => art.IdAccount == id);
        }

        public async Task AddAccountRefreshToken(AccountRefreshToken accountRefreshToken)
        {
            await ReservationsContext!.AccountRefreshTokens.AddAsync(accountRefreshToken);
            await ReservationsContext.SaveChangesAsync();
        }
        public async Task UpdateAccountRefreshToken(AccountRefreshToken accountRefreshToken)
        {
            ReservationsContext!.AccountRefreshTokens.Update(accountRefreshToken);
            await ReservationsContext.SaveChangesAsync();
        }
    }
}
