namespace Models.Classes
{
    public class RestaurantAddress
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public int ApartmentNumber { get; set; }
        public TimeOnly DefaultTimeReservation { get; set; }
        public int IdRestaurant { get; set; }
    }
}
