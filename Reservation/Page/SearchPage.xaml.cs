namespace Reservations.Page;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchViewModel lookViewModel)
	{
		InitializeComponent();
		BindingContext = lookViewModel;
	}
}