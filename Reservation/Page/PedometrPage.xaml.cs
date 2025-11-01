namespace Reservations.Page;

public partial class PedometrPage : ContentPage
{
	public PedometrPage(PedometrViewModel pedometrViewModel)
	{
		InitializeComponent();
		BindingContext = pedometrViewModel;
	}

    /*protected override void OnAppearing()
    {
        base.OnAppearing();

		((PedometrViewModel)BindingContext).Switch();
		((PedometrViewModel)BindingContext).Accounts = ((PedometrViewModel)BindingContext).Accounts;
    }*/
}