
using Interfaces;
using Mapsui.Utilities;
using Model.Classes;
namespace Services
{
    public class RestaurantService : IRestaurantService
    {
        private ObservableRangeCollection<Restaurant>? _restaurants;
        private ObservableRangeCollection<AdressRestaurant>? _adresses;
        private ObservableRangeCollection<RestaurantImage>? _restaurantImages;

        public void SetAllList(ObservableRangeCollection<AdressRestaurant> adresses, ObservableRangeCollection<Restaurant> restaurants, ObservableRangeCollection<RestaurantImage> restaurantImages)
        {
            _adresses = adresses;
            _restaurants = restaurants;
            _restaurantImages = restaurantImages;
        }

        public (ObservableRangeCollection<Restaurant>, ObservableRangeCollection<AdressRestaurant>, ObservableRangeCollection<RestaurantImage>) ReturnList()
        {
            return (_restaurants, _adresses, _restaurantImages)!;
        }

        public void SetRestaurant(ObservableRangeCollection<Restaurant> restaurants)
        {
            _restaurants = [.. restaurants];
        }

        public ObservableRangeCollection<Restaurant> GetRestaurantList()
        {
            return _restaurants!;
        }
    }
}
