namespace Reservations
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(RestaurantDetailPage), typeof(RestaurantDetailPage));
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(MyDataPage), typeof(MyDataPage));
            Routing.RegisterRoute(nameof(ReservationPage), typeof(ReservationPage));
            Routing.RegisterRoute(nameof(SummaryReservationPage), typeof(SummaryReservationPage));
            Routing.RegisterRoute(nameof(MyReservationDetailPage), typeof(MyReservationDetailPage));
        }
    }
}
