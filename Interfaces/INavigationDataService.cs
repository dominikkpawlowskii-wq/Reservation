using System.Collections.ObjectModel;

namespace Interfaces
{
    public interface INavigationDataService
    {
        public void SetData(string OrderId, int points, Reservation reservation);
        public (string, int, Reservation) GetTupleData();
        public (List<Restaurant>, List<RestaurantAddress>, List<RestaurantImage>) GetTupleCollections();
        public void SetCollections(List<Restaurant> restaurants, List<RestaurantAddress> adressRestaurants, List<RestaurantImage> restaurantImages);
    }
}
