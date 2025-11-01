using System.Threading.Tasks;

namespace Reservations.Page;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		BindingContext = loginViewModel;
	}

    /*protected override void OnAppearing()
    {
        base.OnAppearing();

		clearAllEntry();
    }

	private void clearAllEntry()
	{
        ((LoginViewModel)BindingContext).InvalidEmail = false;
        ((LoginViewModel)BindingContext).Email = string.Empty;
        ((LoginViewModel)BindingContext).Password = string.Empty;
    }*/
}