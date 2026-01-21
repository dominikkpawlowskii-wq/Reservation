namespace Reservations.modelViews
{
    public partial class ProfilViewModel : BaseViewModel
    {
        public ILoginService UserService { get; set; }
        public ProfilViewModel(ILoginService userService)
        {
            Title = "Profil";
            UserService = userService;
        }

        [RelayCommand]
        private async Task LoginOut()
        {
            UserService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(TitlePage)}", true);
        }

        [RelayCommand]
        private async Task GoToAboutPage()
        {
            await Shell.Current.GoToAsync(nameof(AboutPage), true);
        }
        [RelayCommand]
        private async Task GoToSettingPage()
        {
            await Shell.Current.GoToAsync(nameof(SettingPage), true);
        }
        [RelayCommand]
        private async Task GoToMyDataPage()
        {
            await Shell.Current.GoToAsync(nameof(MyDataPage), true);
        }

    }
}
