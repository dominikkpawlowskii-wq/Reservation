namespace Models.Classes
{
    public class RestaurantTable
    {
        public int Id { get; set; }
        public int NumberPlace { get; set; }
        public int IdRestaurantAddress { get; set; }
        public int IsEnabled { get; set; }
        public decimal FeeForTable {get;set;}
    }
}
