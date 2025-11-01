using CommunityToolkit.Maui.Views;

namespace Reservations.Page;

public partial class PaymentMethodsPopup : Popup
{
	public PaymentMethodsPopup(PaymentMethodViewModel paymentMethodViewModel)
	{
		InitializeComponent();
		BindingContext = paymentMethodViewModel;
	}
}