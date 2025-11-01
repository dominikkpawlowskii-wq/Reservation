
using Services;

namespace Reservations.modelViews
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly IReservationRepository _reservationRepozitory;
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
        private readonly ConnectivityCheckerService _connectivityCheckerService;
        
        public RegisterViewModel(IReservationRepository reservationRepozitory, ConnectivityCheckerService connectivityChecker)
        {
            this._reservationRepozitory = reservationRepozitory;
            this._connectivityCheckerService = connectivityChecker;
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

                if(_connectivityCheckerService.CheckNetworkEnabled() == NetworkAccess.Internet)
                {
                    var Account = await _reservationRepozitory.GetAccount(Email!);
                    if (Account != null)
                    {
                        var passwordMath = await Task.Run(() => BCrypt.Net.BCrypt.Verify(Password, Account.Password));
                        if (passwordMath)
                            await Shell.Current.DisplayAlert("istnieje konto", "konto o takich danych istnieje!", "ok");

                        return;

                    }
                    else
                    {
                        Account = new Account
                        {
                            FirstName = Name,
                            LastName = LastName,
                            Email = Email,
                            Password = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(Password)),
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
                        await _reservationRepozitory.AddAccount(Account);
                        await Shell.Current.DisplayAlert("Rejestracja udana", "konto zostało zarejestrowane! kliknij ok by zalogować się!", "ok");

                        await Shell.Current.GoToAsync(nameof(LoginPage));
                    }
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Brak internetu", "Brak połączenia z internetem! Włącz internet na urządzeniu.", "Ok");
                    return;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlert("Niepowodzenie!", "coś poszło nie tak", "ok");
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
