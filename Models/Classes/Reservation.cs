namespace Models.Classes
{
    [PrimaryKey(nameof(IdRestaurantTable), nameof(DateReservation), nameof(HourStartReservation), nameof(HourEndReservation))]
    public class Reservation
    {
        public int IdAccount {  get; set; }
        public int IdRestaurantTable { get; set; }
        public string? DateReservation { get; set; }
        public string? HourStartReservation { get; set; }
        public string? HourEndReservation { get; set;}
        public decimal Price { get; set; }
    }
}
