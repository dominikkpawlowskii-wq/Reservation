namespace Reservations.modelViews
{
    public partial class TitleViewModel : BaseViewModel
    {
        public TitleViewModel()
        {
            Title = "";
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
