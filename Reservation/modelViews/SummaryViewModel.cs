using PaypalServerSdk.Standard.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Reservations.modelViews
{
    public partial class SummaryViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        public partial TempClass? TempClass { get; set; }
        [ObservableProperty]
        [field:MaybeNull]
        public partial ImageSource ImageSource {  get; set; }

        [ObservableProperty]
        public partial int PktForReservation {  get; set; }
        [ObservableProperty]
        public partial int SumAllPkt {  get; set; }
        private readonly IPopupService _popupService;
        public IReservationHttpClient? ReservationHttpClient { get; set; }
        public INavigationDataService NavigationDataService { get; set; }
        public SummaryViewModel(IPopupService popup, IReservationHttpClient reservationService,
            INavigationDataService navigationDataService)
        {
            Title = "Podsumowanie";
            _popupService = popup;
            ReservationHttpClient = reservationService;
            NavigationDataService = navigationDataService;
        }

        [RelayCommand]
        private async Task InvokePayment()
        {
            if(IsBusy)
                return;

            try
            {
                IsBusy = true;

                await Task.Yield();

                Reservation reservation = new()
                {
                    IdAccount = TempClass!.Account!.Id,
                    IdRestaurantTable = TempClass!.TableRestaurants!.Id,
                    DateReservation = TempClass.Date.ToString(),
                    HourStartReservation = TempClass.Time.ToString(),
                    HourEndReservation = TempClass?.AdressRestaurant?.DefaultTimeReservation.ToString(),
                    Price = TempClass!.TableRestaurants.FeeForTable
                };

                    ApiResponse<Order> result = await ReservationHttpClient!.CreateOrder(reservation);

                    if (result.StatusCode == 200 && result.Data.Status == OrderStatus.PayerActionRequired)
                    {
                        var uri = result.Data.Links.SingleOrDefault(link => link.Rel == "payer-action")?.Href;

                        NavigationDataService.SetData(result.Data.Id, SumAllPkt, reservation);

                        await Launcher.OpenAsync(uri!);    
                    }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void SetNumberOfPoints()
        {
            if(TempClass?.TableRestaurants?.FeeForTable > 15.00m && TempClass.TableRestaurants.FeeForTable < 25.00m)
            {
                PktForReservation = 100;
            }
            else if(TempClass?.TableRestaurants?.FeeForTable > 25.00m)
            {
                PktForReservation = 200;
            }

            SumAllPkt = (int)((TempClass?.Account?.NumberOfPoints ?? 0) + PktForReservation);
        }

        partial void OnTempClassChanged(TempClass? value)
        {
            SetNumberOfPoints();
            var image = value?.ImagesRestaurant?.FirstOrDefault(image => image.IdRestaurant == value?.Restaurant?.Id);

            ImageSource = ImageSource.FromUri(new Uri(image?.Image ?? "jest nullem"));

        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            TempClass = query[nameof(TempClass)] as TempClass;
            OnPropertyChanged(nameof(TempClass));
        }
    }
}
