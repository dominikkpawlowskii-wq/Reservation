namespace Reservations.Page;

public partial class TitlePage : ContentPage
{
	public TitlePage(TitleViewModel TitleViewModel)
	{
		InitializeComponent();
		BindingContext = TitleViewModel;
	}
}