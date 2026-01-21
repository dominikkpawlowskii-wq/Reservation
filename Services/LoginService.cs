namespace Services
{
    public partial class LoginService : ObservableObject, ILoginService
    {
        [ObservableProperty]
        public partial Account? Account {  get; set; }
        public Tokens? Tokens { get; set; }

        public void Login(Account account, Tokens tokens)
        {
            Account = account;
            Tokens = tokens;
        }

        public void Logout()
        {
            Account = null;
        }

        public Account GetAccount()
        {
            return Account!;
        }
    }
}
