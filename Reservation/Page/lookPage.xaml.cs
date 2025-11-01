namespace Reservations.Page;

public partial class LookPage : ContentPage
{
	public LookPage(LookViewModel lookViewModel)
	{
		InitializeComponent();
		BindingContext = lookViewModel;
	}
}