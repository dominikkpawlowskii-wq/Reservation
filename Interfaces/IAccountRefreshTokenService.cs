namespace Interfaces
{
    public interface IAccountRefreshTokenService
    {
        public Task<AccountRefreshToken> GetAccountRefreshToken(int id);
        public Task AddAccountRefreshToken(AccountRefreshToken accountRefreshToken);
        public Task UpdateAccountRefreshToken(AccountRefreshToken accountRefreshToken);
    }
}
