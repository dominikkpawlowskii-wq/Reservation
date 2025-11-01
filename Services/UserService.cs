
using CommunityToolkit.Mvvm.ComponentModel;
using Interfaces;
using Model.Classes;
namespace Services
{
    public partial class UserService : ObservableObject, IUserService
    {
        [ObservableProperty]
        public partial Account? Account { get; set; }
        public void LogoutAccount()
        {
            Account = null;
        }

        public void AddAccountToService(Account account)
        {
            this.Account = account;
        }
    }
}
