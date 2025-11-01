using Model.Classes;

namespace Interfaces
{
    public interface IUserService
    {
        public Account? Account { get; set; }

        public void AddAccountToService(Account account);
        public void LogoutAccount();
    }
}
