namespace Reservations.Page;

public partial class ProfilPage : ContentPage
{
	public ProfilPage(ProfilViewModel ProfilViewModel)
	{
		InitializeComponent();
		BindingContext = ProfilViewModel;

    }
}