namespace Reservations.Page;

public partial class SettingPage : ContentPage
{
	public SettingPage(SettingViewModel settingViewModel)
	{
		InitializeComponent();
		BindingContext = settingViewModel;
	}

    
}