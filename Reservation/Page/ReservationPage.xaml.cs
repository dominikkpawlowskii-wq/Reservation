namespace Reservations.Page;

public partial class ReservationPage : ContentPage
{
	public ReservationPage(ReservationViewModel reservationViewModel)
	{
		InitializeComponent();
		BindingContext = reservationViewModel;
	}

    /*protected override void OnAppearing()
    {
        base.OnAppearing();

		((ReservationViewModel)BindingContext).RefreshTime();
    }*/
}