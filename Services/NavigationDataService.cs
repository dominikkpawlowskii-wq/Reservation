namespace Services
{
    public class NavigationDataService : INavigationDataService
    {
        public string? OrderID { get; set; }
        public int NumberOfPoints { get; set; }
        public Reservation? Reservation { get; set; }

        public List<Restaurant>? Restaurants { get; set; }
        public List<RestaurantAddress>? AdressRestaurants { get; set; }
        public List<RestaurantImage>? RestaurantImages { get; set; }

        public void SetData(string OrderId, int points, Reservation reservation)
        {
            this.OrderID = OrderId; 
            this.NumberOfPoints = points;
            this.Reservation = reservation;
        }


        public void SetCollections(List<Restaurant> restaurants, List<RestaurantAddress> adressRestaurants, List<RestaurantImage> restaurantImages)
        {
            Restaurants = restaurants;
            AdressRestaurants = adressRestaurants;
            RestaurantImages = restaurantImages;
        }
        public (string, int, Reservation) GetTupleData()
        {
            return (OrderID, NumberOfPoints, Reservation)!;
        }

        public (List<Restaurant>, List<RestaurantAddress>, List<RestaurantImage>) GetTupleCollections()
        {
            return (Restaurants, AdressRestaurants, RestaurantImages)!;
        }
    }
}
