
namespace Model.Classes
{
    public class AdressRestaurant
    {
        public int Id { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public int apartmentNumber { get; set; }
        public TimeOnly defaultTimeReservation { get; set; }
        public int IdRestaurant { get; set; }
    }
}
