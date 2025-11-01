using CommunityToolkit.Maui.Views;

namespace Reservations.Page;

public partial class MyPopup : Popup
{
	public MyPopup(PopupViewModel popupViewModel)
	{
		InitializeComponent();
        BindingContext = popupViewModel;
	}
}