namespace Reservations.Page;

public partial class MyReservationDetailPage : ContentPage
{
	public MyReservationDetailPage(MyReservationDetailViewModel myReservationDetailViewModel)
	{
		InitializeComponent();
		BindingContext = myReservationDetailViewModel;
	}
}