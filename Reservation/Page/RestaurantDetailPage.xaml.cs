namespace Reservations.Page;

public partial class RestaurantDetailPage : ContentPage
{
	public RestaurantDetailPage(RestaurantDetailViewModel restaurantDetailViewModel)
	{
		InitializeComponent();
		BindingContext = restaurantDetailViewModel;
	}
}