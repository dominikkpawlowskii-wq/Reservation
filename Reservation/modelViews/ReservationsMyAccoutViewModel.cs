using CommunityToolkit.Mvvm.Messaging;
using Mapsui.Utilities;

namespace Reservations.modelViews
{
    public partial class ReservationsMyAccoutViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial IUserService UserService {  get; set; }
        private readonly IReservationRepository _reservationRepozitory;
        public ObservableCollection<TempClass> AccountReservations { get; set; }
        public ReservationsMyAccoutViewModel(IReservationRepository reservationRepozitory, IUserService userService)
        {
            
            Title = "Moje rezerwacje";
            AccountReservations = [];
            _reservationRepozitory = reservationRepozitory;
            UserService = userService;

        }
        public async Task AddReservationsToCollection()
        {
            if (AccountReservations.Count() != 0)
                AccountReservations.Clear();

            var list = await _reservationRepozitory.GetListTempClass(UserService!.Account!);

            foreach (var tempClass in list)
            {
                AccountReservations.Add(tempClass);
            }
        }
        [RelayCommand]
        public async Task GoToMyReservationDetailPage(TempClass tempClass)
        {
            TimeOnly timeOnly = TimeOnly.Parse(tempClass!.Reservation!.hourStartReservations!);
            tempClass.Timesp = timeOnly.ToTimeSpan();
            tempClass.Account = UserService.Account;
            await Shell.Current.GoToAsync(nameof(MyReservationDetailPage), true, new Dictionary<string, object>
            {
                {nameof(TempClass), tempClass},
            });
        }
    }
}
