using Mapsui.Utilities;
using Model.Classes;
using System.Collections.ObjectModel;

namespace Interfaces
{
    public interface IRestaurantService
    {
        public void SetAllList(ObservableRangeCollection<AdressRestaurant> adresses, ObservableRangeCollection<Restaurant> restaurants, ObservableRangeCollection<RestaurantImage> restaurantImages);
        public (ObservableRangeCollection<Restaurant>, ObservableRangeCollection<AdressRestaurant>, ObservableRangeCollection<RestaurantImage>) ReturnList();
        public void SetRestaurant(ObservableRangeCollection<Restaurant> restaurants);
        public ObservableRangeCollection<Restaurant> GetRestaurantList();
    }
}
