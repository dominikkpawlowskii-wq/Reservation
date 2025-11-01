
using Mapsui.Utilities;
namespace Model.Classes
{
    public class TempClass
    {
        public Restaurant? Restaurant { get; set; }
        public AdressRestaurant? AdressRestaurant { get; set; }

        public TableRestaurants? TableRestaurants { get; set; }

        public Reservation? Reservation { get; set; }

        public Account? Account { get; set; }

        public ObservableRangeCollection<RestaurantImage>? ImagesRestaurant { get; set; }
        public RestaurantImage? RestaurantImage { get; set; }
        public DateOnly? Date { get; set; }

        public string? NumberOfPerson { get; set; }

        public TimeOnly? Time { get; set; }

        public TimeSpan? Timesp { get; set; }

        public string FullAdress => GetFullAdressRestaurant();

        public string GetFullAdressRestaurant()
        {
            return $"{AdressRestaurant?.street} {AdressRestaurant?.apartmentNumber}, {AdressRestaurant?.city}";
        }
    }
}
