
using Mapsui.Utilities;

namespace Reservations.modelViews
{
    public partial class SearchViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial string? TextFiltr {  get; set; }
        [ObservableProperty]
        public partial int SelectedIndex {  get; set; }

        public ObservableCollection<Restaurant> Restaurants { get; set; }
        public ObservableCollection<RestaurantAddress> RestaurantAddresses { get; set; }
        public ObservableCollection<RestaurantImage> ImagesRestaurants { get; set; }
        public ObservableCollection<Restaurant>? FiltrRestaurant { get; set; }
        public List<string> NumberOfPersons { get; set; }

        public INavigationDataService navigationDataService { get; set; }
        public IReservationHttpClient ReservationService { get; set; }
        public ILoginService UserService { get; set; }

        public SearchViewModel(IReservationHttpClient reservationService, ILoginService userService, INavigationDataService navigationDataService)
        {
            Restaurants = [];
            ImagesRestaurants = [];
            RestaurantAddresses = [];
            FiltrRestaurant = [];
            NumberOfPersons = [];
            SelectedIndex = Preferences.Get("index", 0);
            Title = "ZnajdzStolik.pl";
            ReservationService = reservationService;
            UserService = userService;

            AddNumberOfPersons();
            _ = LoadRestaurantAndAdressesRestaurantAndRestaurantImages();
            this.navigationDataService = navigationDataService;
        }

        public async Task LoadRestaurantAndAdressesRestaurantAndRestaurantImages()
        {
           if(IsBusy)
            {
                return;
            }

           try
            {
                IsBusy = true;
                ClearList();

                var restaurants = await ReservationService.GetAllRestaurants();
                var adressesRestaurants = await ReservationService.GetAllAdressRestaurant();
                var ImageRestaurants = await ReservationService.GetAllImageRestaurants();

                Restaurants = new ObservableCollection<Restaurant>(restaurants);
                RestaurantAddresses = new ObservableCollection<RestaurantAddress>(adressesRestaurants);
                ImagesRestaurants = new ObservableCollection<RestaurantImage>(ImageRestaurants);

                foreach (var r in Restaurants)
                    FiltrRestaurant!.Add(r);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlertAsync("Nie powiodło się", "Coś poszło nie tak!", "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ClearList()
        {
            if (Restaurants.Count != 0)
                Restaurants.Clear();
            if(RestaurantAddresses.Count != 0)
                RestaurantAddresses.Clear();
            if(ImagesRestaurants.Count != 0)
                ImagesRestaurants.Clear();
            if(FiltrRestaurant?.Count != 0)
                FiltrRestaurant?.Clear();
        }
        [RelayCommand]
        private async Task ReloadList()
        {
            await LoadRestaurantAndAdressesRestaurantAndRestaurantImages();
        }

        [RelayCommand]
        private async Task GoToRestaurantDetailPage(Restaurant restaurants)
        {
            var tempObject = from r in Restaurants
                             join a in RestaurantAddresses on r.Id equals a.Id
                             where a.IdRestaurant == restaurants.Id
                             select new TempClass
                             {
                                 Restaurant = r,
                                 AdressRestaurant = a,
                                 ImagesRestaurant = [..(from Images in ImagesRestaurants
                                                    where Images.IdRestaurant == r.Id
                                                    select Images)],
                                 Account = UserService.GetAccount()
                             };
            TempClass? tempClass = tempObject.FirstOrDefault();


            await Shell.Current.GoToAsync(nameof(RestaurantDetailPage), true, new Dictionary<string, object>
            {
                {"Restaurant",tempClass! }
            });

        }

        private void AddNumberOfPersons()
        {
            if (NumberOfPersons.Count != 0)
                NumberOfPersons.Clear();

            for (int i = 0; i < 10; i++)
            {
                if(i == 0)
                {
                    string temp = $"{i + 1} osoba";
                    NumberOfPersons.Add(temp);
                }
                else if(i > 0 && i < 4)
                {
                    string temp = $"{i + 1} osoby";
                    NumberOfPersons.Add(temp);
                }
                else
                {
                    string temp = $"{i + 1} osób";
                    NumberOfPersons.Add(temp);
                }
            }
        }

        partial void OnSelectedIndexChanged(int value)
        {
            Preferences.Set("index", value - value);
        }

        partial void OnTextFiltrChanged(string? oldValue, string? newValue)
        {
                FiltrRestaurant!.Clear();

                var filtered = string.IsNullOrEmpty(newValue)
                ? Restaurants
                : (from r in Restaurants
                   join a in RestaurantAddresses
                   on r.Id equals a.IdRestaurant
                   where a!.City!.Contains(newValue)
                   select r);

                foreach (var data in filtered)
                    FiltrRestaurant.Add(data);

                //navigationDataService.SetCollections(Restaurants, AdressesRestaurants, ImagesRestaurants);
            
        }

    }
}
