using EntitySQLite;

namespace Reservations.modelViews
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial string? Email { get; set; }
        [ObservableProperty]
        public partial string? Password { get; set; }
        [ObservableProperty]
        public partial bool InvalidEmail { get; set; }

        public IReservationHttpClient ReservationHttpClient { get; set; }
        public IConnectivity Connectivity { get; set; }
        public ILoginService UserService { get; set; }
        public ISecureStorage? SecureStorage { get; set; }
        public LoginViewModel(ISecureStorage secureStorage, IConnectivity connectivity, IReservationHttpClient reservationService, ILoginService userService)
        {
            Title = "";
            Connectivity = connectivity;
            ReservationHttpClient = reservationService;
            UserService = userService;
            SecureStorage = secureStorage;
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

                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var authentication = new Authentication
                    {
                        Email = Email,
                        Password = Password,
                    };

                    TempDataAccount dataAccount = await ReservationHttpClient.Login(authentication);
                    
                    if(dataAccount is not null)
                    {
                        await Task.WhenAll(SecureStorage!.SetAsync($"{dataAccount.Account?.Email!}_access", dataAccount.Tokens?.AccessToken!),
                            SecureStorage!.SetAsync($"{dataAccount.Account?.Email!}_refresh", dataAccount.Tokens?.RefreshToken!));

                        UserService.Login(dataAccount.Account!, dataAccount.Tokens!);
                        await Shell.Current.GoToAsync($"//mainApp/search", true);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlertAsync("Nie udało się", "Niepoprawny email lub hasło! spróbuj ponownie.", "Ok");
                    }
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlertAsync("Brak internetu", "Brak połączenia z internetem! Włącz internet na urządzeniu.", "Ok");
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlertAsync("nie udało się", $"{ex.Message}", "ok");
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
