
using Services;

namespace Reservations.modelViews
{
    public partial class RegisterViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial string? Name { get; set; }
        [ObservableProperty]
        public partial string? LastName { get; set; }
        [ObservableProperty]
        public partial string? Email { get; set; }
        [ObservableProperty]
        public partial string? Password { get; set; }
        [ObservableProperty]
        public partial string? Telephone { get; set; }

        [ObservableProperty]
        public partial string? TextMeter { get; set; }

        [ObservableProperty]
        public partial bool InvalidEmail { get; set; }
        [ObservableProperty]
        public partial Microsoft.Maui.Graphics.Color? Color { get; set; }
        [ObservableProperty]
        public partial bool MetterPassword { get; set; }

        public IConnectivity Connectivity { get; set; }
        public IReservationHttpClient ReservationHttpClient { get; set; }
        
        public RegisterViewModel(IConnectivity connectivity, IReservationHttpClient reservationService)
        {
            this.Connectivity = connectivity;
            this.ReservationHttpClient = reservationService;
        }


        [RelayCommand]
        private async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage), true);
        }
        [RelayCommand]
        private async Task GoToBackPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(TitlePage)}", true);
        }


        [RelayCommand]
        private async Task RegisterAccount()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                if(!CheckFormatEmail(Email!))
                {
                    InvalidEmail = true;
                    return;
                }
                
                InvalidEmail = false;

                if(Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    bool returnSuccess = false;

                    var Account = new Account
                    {
                        FirstName = Name,
                        LastName = LastName,
                        Email = Email,
                        Password = Password,
                        Telephone = Telephone
                    };

                    if (Account.FirstName == "admin")
                    {
                        Account.NumberOfPoints = null;
                    }
                    else
                    {
                        Account.NumberOfPoints = 1000;
                    }

                    returnSuccess = await ReservationHttpClient.Register(Account);

                    if(returnSuccess)
                    {
                        await Shell.Current.DisplayAlertAsync("Rejestracja udana", "konto zostało zarejestrowane! kliknij ok by zalogować się!", "ok");

                        await Shell.Current.GoToAsync(nameof(LoginPage));
                    }
                    else
                    {
                        await Shell.Current.DisplayAlertAsync("Niepowodzenie!", "Nie udało się zarejestrować! spróbuj ponownie.", "ok");
                    }
                    
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlertAsync("Brak internetu", "Brak połączenia z internetem! Włącz internet na urządzeniu.", "Ok");
                    return;
                }
            }
            catch(Exception ex)
            {
                //Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlertAsync("ok", ex.Message, "ok");
                
            }
            finally
            {
                IsBusy = false;
            }
        }

        partial void OnNameChanged(string? value)
        {
            UpdateButton();
        }
        partial void OnLastNameChanged(string? value)
        {
            UpdateButton();
        }
        partial void OnEmailChanged(string? value)
        {
            UpdateButton();
        }
        partial void OnPasswordChanged(string? value)
        {
            UpdateButton();
            if(!IsStrongPassword(Password!) && !string.IsNullOrEmpty(Password))
            {
                MetterPassword = true;
                Color = Colors.Red;
                TextMeter = "Ten miernik sprawdza jak silne jest hasło. Dłuższe hasło jest generalnie silniejsze";
                IsActive = false;
            }
            else if(IsStrongPassword(Password!) && Password!.Length! > 3 && Password.Length < 8)
            {
                Color = Colors.Orange;
                MetterPassword = true;
                TextMeter = "Nie złe hasło!";
            }
            else if(IsStrongPassword(Password!))
            {
                MetterPassword = true;
                Color = Colors.Green;
                TextMeter = "Silne hasło";
            }
        }
        partial void OnTelephoneChanged(string? value)
        {
            UpdateButton();
        }


        public void UpdateButton()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Telephone))
            {
                IsActive = false;
                MetterPassword = false;
            }
            else if (!string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(LastName) || !string.IsNullOrEmpty(Email) || !string.IsNullOrEmpty(Password) || !string.IsNullOrEmpty(Telephone))
            {
                IsActive = true;
            }

        }

        private static bool CheckFormatEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        private bool IsStrongPassword(string password)
        {

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*(),.?\:{ }|<>]");

            
            if(!hasUpper && !hasDigit && !hasSpecial)
            {
                return false;
            }
            else if(!hasUpper && !hasSpecial && !hasLower)
            {
                return false;
            }
            else if(hasUpper && hasLower && hasDigit && hasSpecial)
            {
                return true;
            }
            else if(hasLower && hasDigit)
            {
                return true;
            }
            else if ((hasUpper || hasSpecial) && Password!.Length! > 3)
            {
                return true;
            }


                return false;

        }
    }
}
