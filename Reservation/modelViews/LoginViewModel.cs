using Services;
namespace Reservations.modelViews
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IReservationRepository _reservationRepozitory;

        [ObservableProperty]
        public partial string? Email { get; set; }
        [ObservableProperty]
        public partial string? Password { get; set; }
        [ObservableProperty]
        public partial bool InvalidEmail { get; set; }
        [ObservableProperty]
        public partial IUserService UserService { get; set; }
        private readonly ConnectivityCheckerService _connectivityCheckerService;
        public LoginViewModel(IReservationRepository reservationRepozitory,IUserService userService, ConnectivityCheckerService checkerService)
        {
            Title = "";
            this._reservationRepozitory = reservationRepozitory;
            this.UserService = userService;
            this._connectivityCheckerService = checkerService;


        }
        [RelayCommand]
        private  async Task GoToRegisterPage()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage), true);
        }
        [RelayCommand]
        private async Task GoToBackPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(TitlePage)}", true);
        }

        [RelayCommand]
        private async Task Login()
        {
            if(IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                if (!CheckFormatEmail(Email!))
                {
                    InvalidEmail = true;
                    IsActive = false;
                    return;
                }

                InvalidEmail = false;

                if (_connectivityCheckerService.CheckNetworkEnabled() == NetworkAccess.Internet)
                {
                    var response = await _connectivityCheckerService.HttpClient.GetAsync("https://www.google.com");

                    if (response.IsSuccessStatusCode)
                    {
                        var Account = await _reservationRepozitory.GetAccount(Email!);
  
                        if (Account != null)
                        {
                            var passwordMatch = await Task.Run(() => BCrypt.Net.BCrypt.Verify(Password, Account.Password));

                            if (passwordMatch)
                            {

                                UserService.AddAccountToService(Account);
                                await Task.Delay(1000);
                                await Shell.Current.GoToAsync($"//mainApp/look", true);
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Nie powiodło się", "Niepoprawny email lub hasło!", "ok");
                            }


                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Nie powiodło się", "Niepoprawny email lub hasło!", "ok");
                        }
                    }
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Brak internetu", "Brak połączenia z internetem! Włącz internet na urządzeniu.", "Ok");
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlert("nie udało się", $"{ex.Message}", "ok");
            }
            finally
            {
                IsBusy = false;

            }
        }
                

        partial void OnEmailChanged(string? value)
        {
            UpdateButton();
        }
        partial void OnPasswordChanged(string? value)
        {
            UpdateButton();
        }

        public void UpdateButton()
        {
            if(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                IsActive = false;
            }
            else if(!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {
                IsActive = true;
            }
            
        }

        private static bool CheckFormatEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        
    }
}
