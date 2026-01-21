using System.Text.RegularExpressions;

namespace Services
{
    public class AccountService : IAccountService
    {
        public ReservationsContext ReservationsContext { get; set; }
        public AccountService(ReservationsContext reservationsContext)
        {
            ReservationsContext = reservationsContext;
        }

        public async Task UpdatePoints(Account account)
        {
            var exist = await ReservationsContext.Accounts.AnyAsync(a => a.FirstName == account.FirstName);

            if (!exist)
            {
                throw new Exception("Account doesnt exist!");
            }
            ReservationsContext.Accounts.Update(account);
            await ReservationsContext.SaveChangesAsync();
        }
        public async Task DeleteAccount(Account account)
        {
            bool exists = await ReservationsContext.Accounts.AnyAsync(userAccount => userAccount.Email == account.Email);

            if (!exists)
            {
                throw new Exception("Account doesnt exist!");
            }
            ReservationsContext.Accounts.Remove(account);
            await ReservationsContext.SaveChangesAsync();
        }
        public async Task<Account> GetAccount(string email, string firstName)
        {
            var account = await ReservationsContext.Accounts.SingleOrDefaultAsync(a => a.Email == email && a.FirstName == firstName);

            return account is null ? throw new Exception("Account is null!") : account;
        }
        public async Task<Account> GetAccount(int id)
        {
            var account =  await ReservationsContext.Accounts.FindAsync(id);

            return account is null ? throw new Exception("Account is null!") : account;
        }
        public async Task<List<Account>> GetAllAccount()
        {
            var accounts = await ReservationsContext.Accounts.ToListAsync();

            if (accounts.Count == 0)
                throw new Exception("List Empty!");
            return accounts;
        }

        public async Task AddAccount(Account account)
        {
            if (account.Password?.Length <= 8 && !CheckFormatEmail(account.Email!) && !IsStrongPassword(account.Password))
            {
                throw new Exception("Validation incorrect!");
            }

            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

            ReservationsContext.Accounts.Add(account);
            await ReservationsContext.SaveChangesAsync();
        }

        public bool CheckFormatEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        public bool IsStrongPassword(string password)
        {
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*(),.?\:{ }|<>]");

            if(hasUpper && hasLower && hasDigit && hasSpecial)
                return true;

            return false;

        }
    }
}
