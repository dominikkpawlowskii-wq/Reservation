namespace Reservations.modelViews
{
    public partial class ReservationsMyAccoutViewModel : BaseViewModel
    {
        public ILoginService? UserService {  get; set; }
        public IReservationHttpClient ReservationService { get; set; }
        public ObservableCollection<TempClass> AccountReservations { get; set; }
        public ReservationsMyAccoutViewModel(IReservationHttpClient reservationService, ILoginService userService)
        {
            
            Title = "Moje rezerwacje";
            AccountReservations = [];
            ReservationService = reservationService;
            UserService = userService;

        }
        public async Task AddReservationsToCollection()
        {
            if (AccountReservations.Count != 0)
                AccountReservations.Clear();

            var list = await ReservationService.GetAccountReservations(UserService!.GetAccount());

            foreach (var tempClass in list)
            {
                AccountReservations.Add(tempClass);
            }
        }
        [RelayCommand]
        public async Task GoToMyReservationDetailPage(TempClass tempClass)
        {
            TimeOnly timeOnly = TimeOnly.Parse(tempClass!.Reservation!.HourStartReservation!);
            tempClass.Timesp = timeOnly.ToTimeSpan();
            await Shell.Current.GoToAsync(nameof(MyReservationDetailPage), true, new Dictionary<string, object>
            {
                {nameof(TempClass), tempClass},
            });
        }
    }
}
