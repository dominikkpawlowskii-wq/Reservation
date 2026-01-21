using Models.Classes;
using System.Security.Cryptography;

namespace Services
{
    public class AuthenticationServise : IAuthenticationService
    {
        public ReservationsContext ReservationsContext { get; set; }
        public IAccountRefreshTokenService AccountRefreshTokenService { get; set; }

        public AuthenticationServise(ReservationsContext reservationsContext, IAccountRefreshTokenService accountRefreshTokenService)
        {
            ReservationsContext = reservationsContext;
            AccountRefreshTokenService = accountRefreshTokenService;
        }


        public async Task<TempDataAccount> GetAccountWithtokens(Authentication authentication)
        {
            Account account = await ReservationsContext.Accounts.SingleOrDefaultAsync(account => account.Email == authentication.Email) ?? throw new Exception("Invalid login or password!");
            var passwordMatch = PasswordHash(authentication.Password!, account.Password!);

            var AccessToken = GenerateJwtAccessToken(account!);

            (string claimKey,var RefreshToken) = GenerateJwtRefreshToken(account!);

            if (passwordMatch)
            {

                AccountRefreshToken accountRefreshToken = await AccountRefreshTokenService.GetAccountRefreshToken(account.Id);

                if (accountRefreshToken is not null)
                {
                    accountRefreshToken.ClaimKey = claimKey;

                    await AccountRefreshTokenService.UpdateAccountRefreshToken(accountRefreshToken);
                }
                else
                {

                    accountRefreshToken = new()
                    {
                        ClaimKey = claimKey,
                        IdAccount = account!.Id
                    };

                    await AccountRefreshTokenService.AddAccountRefreshToken(accountRefreshToken);

                }
                return new()
                {
                    Account = account,
                    Tokens = new Tokens
                    {
                        AccessToken = AccessToken,
                        RefreshToken = RefreshToken,
                    },
                };
            }
            else
            {
                throw new Exception("Invalid login or password!");
            }
            
        }

        public string GenerateJwtAccessToken(Account account)
        {
                Claim[] claims = [
                    new Claim(ClaimTypes.Name, account.Email!)];

                var secretKey = "VWTklpVcXyJSz9yXlKbwU5rR495vCFc5";
                var sequrityKey = Encoding.UTF8.GetBytes(secretKey);
                var symetricKey = new SymmetricSecurityKey(sequrityKey);
                var signingCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);

                var jwtSequrityToken = new JwtSecurityToken(
                    signingCredentials: signingCredentials,
                    issuer: "https://localhost:7082",
                    audience: "access",
                    expires: DateTime.UtcNow.AddMinutes(10),
                    claims: claims);

                return new JwtSecurityTokenHandler().WriteToken(jwtSequrityToken);
        }


        public bool PasswordHash(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        public (string,string) GenerateJwtRefreshToken(Account account)
        {
            byte[] bytes = new byte[32];
            using var rgn = RandomNumberGenerator.Create();

            rgn.GetBytes(bytes);

            string claimKey = Convert.ToBase64String(bytes);

                Claim[] claims = [
                    new Claim(ClaimTypes.Name, account.Email!)
                    ,
                    new Claim("refresh", claimKey)];

            var secretKey = "VWTklpVcXyJSz9yXlKbwU5rR495vCFc5";
            var sequrityKey = Encoding.UTF8.GetBytes(secretKey!);
            var symetricKey = new SymmetricSecurityKey(sequrityKey);
            var signingCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);

            var jwtSequrityToken = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                issuer: "https://localhost:7082",
                audience: "refresh",
                expires: DateTime.UtcNow.AddMinutes(240),
                claims: claims);

            return (claimKey,new JwtSecurityTokenHandler().WriteToken(jwtSequrityToken)); ;
        }

    }
    
}
