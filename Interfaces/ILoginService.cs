namespace Interfaces
{
    public interface ILoginService
    {
        public void Login(Account account, Tokens tokens);
        public Account GetAccount();

        public void Logout();
    }
}
