using System.Threading.Tasks;

namespace Reservations.Page;

public partial class ReservationsMyAccountPage : ContentPage
{
	public ReservationsMyAccountPage(ReservationsMyAccoutViewModel reservationsMyAccoutViewModel)
	{
		InitializeComponent();
		BindingContext = reservationsMyAccoutViewModel;
	}



    protected async override void OnAppearing()
    {
        base.OnAppearing();

        await (BindingContext as ReservationsMyAccoutViewModel)?.AddReservationsToCollection()!;
        
    }
}