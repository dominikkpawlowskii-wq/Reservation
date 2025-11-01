
using Services;

namespace Reservations.modelViews
{
    public partial class TitleViewModel : BaseViewModel
    {
        public ConnectivityCheckerService ConnectivityCheckerService { get; set; }
        public TitleViewModel(ConnectivityCheckerService connectivityCheckerService)
        {
            Title = "";
            ConnectivityCheckerService = connectivityCheckerService;
        }

        [RelayCommand]
        private async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        [RelayCommand]
        private async Task GoToRegisterPage()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
    }
}
