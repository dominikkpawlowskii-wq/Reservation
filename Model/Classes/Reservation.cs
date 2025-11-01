
namespace Model.Classes
{
    public class Reservation
    {
        public int Id { get; set; }
        public int IdAccount {  get; set; }
        public int IdtableRestaurant { get; set; }
        public string? dateReservation { get; set; }
        public string?  hourStartReservations { get; set; }
        public string? hourEndReservations { get; set;}
        public float prize { get; set; }

    }
}
