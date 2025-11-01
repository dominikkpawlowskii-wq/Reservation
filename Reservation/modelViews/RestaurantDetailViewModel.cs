

namespace Reservations.modelViews
{
    public partial class RestaurantDetailViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        public partial TempClass? TempClass { get; set; }
        [ObservableProperty]
        public partial int CountImages {  get; set; }
   
        public RestaurantDetailViewModel()
        {
            Title = "Restauracja";
        }
        [RelayCommand]
        private async Task GoToReservationPage()
        {
            await Shell.Current.GoToAsync(nameof(ReservationPage), true, new Dictionary<string, object>
            {
                {nameof(TempClass), TempClass! }
            });
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            TempClass = query["Restaurant"] as TempClass;
            OnPropertyChanged(nameof(TempClass));
        }

        partial void OnTempClassChanged(TempClass? value)
        {
            CountImages = value!.ImagesRestaurant!.Count;
        }
    }
}
