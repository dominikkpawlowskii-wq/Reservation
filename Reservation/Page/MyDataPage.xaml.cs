using CommunityToolkit.Maui.Core;

namespace Reservations.Page;

public partial class MyDataPage : ContentPage
{
	public MyDataPage(MyDataViewModel myDataViewModel)
	{
		InitializeComponent();
		BindingContext = myDataViewModel;
	}

	
}