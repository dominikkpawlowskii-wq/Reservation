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

        [ObservableProperty]
        public partial TempDataClass? TempDataClass {  get; set; }
        private readonly IPopupService _popupService;
        private readonly IReservationRepository reservationRepozitory;
        private readonly PaypalApi paypalApi;
        public SummaryViewModel(IPopupService popup, TempDataClass tempDataClass, IReservationRepository reservationRepozitory, PaypalApi paypalApi)
        {
            Title = "Podsumowanie";
            _popupService = popup;
            TempDataClass = tempDataClass;
            TempDataClass.Text = "Wybierz metode płatności";
            this.reservationRepozitory = reservationRepozitory;
            this.paypalApi = paypalApi;
        }

        /*[RelayCommand]
        private async Task DisplayPaymentMethodPopup()
        {
            if(IsBusy)
                return;

            try
            {
                IsBusy = true;
                await Task.Delay(2000);
                
                //await _popupService.ShowPopupAsync<PaymentMethodViewModel>(she);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }*/

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
                    IdtableRestaurant = TempClass!.TableRestaurants!.Id,
                    dateReservation = TempClass.Date.ToString(),
                    hourStartReservations = TempClass.Time.ToString(),
                    hourEndReservations = TempClass?.AdressRestaurant?.defaultTimeReservation.ToString(),
                    prize = TempClass!.TableRestaurants.FeeForTable
                };

                await Task.Run(async () => await paypalApi.ReadLinesInJsonFile("appSettings.json"));
                    paypalApi.ConfigurePaypalApi();

                    ApiResponse<Order> result = await paypalApi.CreateOrder();
                    if (result.StatusCode == 200 && result.Data.Status == OrderStatus.PayerActionRequired)
                    {
                        var uri = result.Data.Links.SingleOrDefault(link => link.Rel == "payer-action")?.Href;
                        reservationRepozitory.PutArgument(TempClass.Account,reservation, SumAllPkt, result.Data.Id);
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
            if(TempClass?.TableRestaurants?.FeeForTable > 15.00 && TempClass.TableRestaurants.FeeForTable < 25.00)
            {
                PktForReservation = 100;
            }
            else if(TempClass?.TableRestaurants?.FeeForTable > 25.00)
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
