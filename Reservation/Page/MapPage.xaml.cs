namespace Reservations.Page;

public partial class MapPage : ContentPage
{

	public MapPage(MapViewModel mapViewModel)
	{
		InitializeComponent();
		BindingContext = mapViewModel;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
    }
}