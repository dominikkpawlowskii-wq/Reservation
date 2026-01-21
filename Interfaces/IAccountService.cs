namespace Interfaces
{
    public interface IAccountService
    {
        public Task AddAccount(Account account);
        public Task<List<Account>> GetAllAccount();
        public Task<Account> GetAccount(string email, string firstName);
        public Task<Account> GetAccount(int id);
        public Task UpdatePoints(Account account);
        public Task DeleteAccount(Account account);
        public bool CheckFormatEmail(string email);
        public bool IsStrongPassword(string password);
    }
}
