
namespace Reservations
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public PaypalApi PaypalApi { get; set; }
        public IReservationRepository ReservationRepository { get; set; }

        public App(PaypalApi paypalApi, IReservationRepository reservationRepository)
        {
            InitializeComponent();

            SQLitePCL.Batteries.Init();

            bool Istoggled = Preferences.Get("IsToggled", false);
            ApplyTheme(Istoggled);

            PaypalApi = paypalApi;
            ReservationRepository = reservationRepository;
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
            if (uri.Scheme == "https" && uri.Host == "myapp" && uri.Segments.Last() == "Cancel/")
                await Shell.Current.DisplayAlert("Rezerwacja nieudana", "Nie udało sie zarezerwować stolika! spróbuj ponownie", "ok");
            else if (uri.Scheme == "https" && uri.Host == "myapp" && uri.Segments.Last() == "Capture/")
            {
                (Account account, Reservation reservation, int points, string OrderId) = ReservationRepository.ReturnArguments();
                ApiResponse<Order> resutlCapture = await PaypalApi.CapturePayment(OrderId);

                if (resutlCapture.Data.Status == OrderStatus.Completed)
                {
                    await Task.WhenAll(ReservationRepository.AddReservation(reservation), ReservationRepository.UpdateNumberOfPoints(account, points));
                    await Shell.Current.DisplayAlert("Rezerwacja powiodła się!", "Udało się dokonać rezerwacji! możesz zobaczyc rezerwacje w zakładce rezerwacje", "ok");
                    await Shell.Current.GoToAsync("//mainApp/look", true);
                }
            }

        }

    }
}