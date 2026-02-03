
namespace Reservations
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public IReservationHttpClient ReservationHttpClient { get; set; }
        public INavigationDataService NavigationDataService { get; set; }
        public ILoginService UserService { get; set; }

        public App(IReservationHttpClient reservationService, INavigationDataService navigationDataService,
            ILoginService userService)
        {
            InitializeComponent();

            bool Istoggled = Preferences.Get("IsToggled", false);
            ApplyTheme(Istoggled);

            ReservationHttpClient = reservationService;
            NavigationDataService = navigationDataService;
            this.UserService = userService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        public static void ApplyTheme(bool isDark)
        {
            ICollection<ResourceDictionary> mergedDictioneries = Microsoft.Maui.Controls.Application.Current!.Resources.MergedDictionaries;
            if (mergedDictioneries != null)
            {
                var existingTheme = mergedDictioneries.FirstOrDefault(d => d is darkTheme || d is lightTheme);

                if (existingTheme != null)
                {
                    mergedDictioneries.Remove(existingTheme);
                }
                mergedDictioneries.Add(isDark ? new darkTheme() : new lightTheme());
            }
        }
        protected async override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            (string OrderId, int points, Reservation reservation) = NavigationDataService.GetTupleData();


            if (uri.Scheme == "https" && uri.Host == "myapp" && uri.Segments.Last() == "Cancel/")
            {
                    await Shell.Current.DisplayAlertAsync("Rezerwacja nieudana", "Nie udało sie zarezerwować stolika! spróbuj ponownie", "ok");
            }
            else if (uri.Scheme == "https" && uri.Host == "myapp" && uri.Segments.Last() == "Capture/")
            {

                var resultCapture = await ReservationHttpClient.CaptureOrder(OrderId);

                var account = UserService.GetAccount();

                if (resultCapture.Data.Status == OrderStatus.Completed)
                {
                    account.NumberOfPoints = points;

                    await Task.WhenAll(ReservationHttpClient.CreateReservation(reservation),ReservationHttpClient.UpdateAccountPoints(account));
                    await Shell.Current.DisplayAlertAsync("Rezerwacja powiodła się!", "Udało się dokonać rezerwacji! możesz zobaczyc rezerwacje w zakładce rezerwacje", "ok");
                    await Shell.Current.GoToAsync("//mainApp/search", true); 
                }
            }
        }
      

    }

}