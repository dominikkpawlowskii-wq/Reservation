
namespace Model.Classes
{
    public class TableRestaurants
    {
        public int Id { get; set; }
        public int NumberPlace { get; set; }
        public int IdAdressRestaurant { get; set; }
        public int IsEnabled { get; set; }
        public float FeeForTable {get;set;}
    }
}
