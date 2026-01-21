namespace Models.Classes
{
    public class TempClass
    {
        public Restaurant? Restaurant { get; set; }
        public RestaurantAddress? AdressRestaurant { get; set; }

        public RestaurantTable? TableRestaurants { get; set; }

        public Reservation? Reservation { get; set; }

        public Account? Account { get; set; }

        public List<RestaurantImage>? ImagesRestaurant { get; set; }
        public RestaurantImage? RestaurantImage { get; set; }
        public DateOnly? Date { get; set; }

        public string? NumberOfPerson { get; set; }

        public TimeOnly? Time { get; set; }

        public TimeSpan? Timesp { get; set; }

        public string FullAdress => GetFullAdressRestaurant();

        public string GetFullAdressRestaurant()
        {
            return $"{AdressRestaurant?.Street} {AdressRestaurant?.ApartmentNumber}, {AdressRestaurant?.City}";
        }
    }
}
