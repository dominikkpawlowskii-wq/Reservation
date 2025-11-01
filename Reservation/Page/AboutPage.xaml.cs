namespace Reservations.Page;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel aboutViewModel)
	{
		InitializeComponent();
		BindingContext = aboutViewModel;
	}
}