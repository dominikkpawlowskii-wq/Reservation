namespace Reservations.Page;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel registerViewModel)
	{
		InitializeComponent();
		BindingContext = registerViewModel;
	}


    /*protected override void OnAppearing()
    {
        base.OnAppearing();

        clearAllEntry();
    }

    private void clearAllEntry()
    {
        ((RegisterViewModel)BindingContext).MetterPassword = false;
        ((RegisterViewModel)BindingContext).Name = string.Empty;
        ((RegisterViewModel)BindingContext).LastName = string.Empty;
        ((RegisterViewModel)BindingContext).Email = string.Empty;
        ((RegisterViewModel)BindingContext).Password = string.Empty;
        ((RegisterViewModel)BindingContext).Telephone = string.Empty;
    }*/
}