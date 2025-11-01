
using Mapsui.Utilities;

namespace Reservations.modelViews
{
    public partial class LookViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial string? TextFiltr {  get; set; }
        [ObservableProperty]
        public partial int SelectedIndex {  get; set; }

        [ObservableProperty]
        public partial IUserService UserService { get; set; }
        public ObservableRangeCollection<Restaurant> Restaurants;
        public ObservableRangeCollection<AdressRestaurant> Adresses;
        public ObservableRangeCollection<RestaurantImage> ImagesRestaurants { get; set; }
        public ObservableRangeCollection<Restaurant>? FiltrRestaurant { get; set; }
        public List<string> NumberOfPersons { get; set; }

        private readonly IReservationRepository _reservationRepozitory;
        private readonly IRestaurantService _restaurantService;
        public LookViewModel(IReservationRepository reservationRepozitory, IRestaurantService restaurantService, IUserService userService)
        {
            SelectedIndex = Preferences.Get("index", 0);

            Title = "ZnajdzStolik.pl";

            _restaurantService = restaurantService;
            _reservationRepozitory = reservationRepozitory;
            ImagesRestaurants = [];
            Restaurants = new ObservableRangeCollection<Restaurant>();
            Adresses = [];

            NumberOfPersons = [];
            AddNumberOfPersons();

            _ = LoadRestaurantAndAdressesRestaurantAndRestaurantImages();
            FiltrRestaurant = [.. Restaurants];
            this.UserService = userService;
        }

        private async Task LoadRestaurantAndAdressesRestaurantAndRestaurantImages()
        {
           if(IsBusy)
            {
                return;
            }

           try
            {
                IsBusy = true;
                ClearList();

                (var list,var adreses, var images) = await _reservationRepozitory.GetAllRestaurantsAndAdressesAndRestaurantImages();

                Restaurants.AddRange(list);
                Adresses.AddRange(adreses);
                ImagesRestaurants.AddRange(images);

                _restaurantService.SetAllList(Adresses,Restaurants, ImagesRestaurants);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlert("Nie powiodło się", "Coś poszło nie tak!", "ok");
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
            if(Adresses.Count != 0)
                Adresses.Clear();
            if(ImagesRestaurants.Count != 0)
                ImagesRestaurants.Clear();
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
                             join a in Adresses on r.Id equals a.Id
                             where a.IdRestaurant == restaurants.Id
                             select new TempClass
                             {
                                 Restaurant = r,
                                 AdressRestaurant = a,
                                 ImagesRestaurant = [..(from Images in ImagesRestaurants
                                                    where Images.IdRestaurant == r.Id
                                                    select Images)],
                                 Account = UserService.Account
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
                   join a in Adresses
                   on r.Id equals a.IdRestaurant
                   where a!.city!.Contains(newValue)
                   select r);

                FiltrRestaurant.AddRange(filtered);

               _restaurantService.SetRestaurant(FiltrRestaurant);
            
        }

    }
}
