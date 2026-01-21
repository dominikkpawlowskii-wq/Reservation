namespace Interfaces
{
    public interface IAuthenticationService
    {
        public Task<TempDataAccount> GetAccountWithtokens(Authentication authentication);
        public string GenerateJwtAccessToken(Account account);
        public (string,string) GenerateJwtRefreshToken(Account account);
        public bool PasswordHash(string password, string hash);
    }
}
