
namespace Reservations.Page;

public partial class SummaryReservationPage : ContentPage
{
	public SummaryReservationPage(SummaryViewModel summaryViewModel)
	{
		InitializeComponent();
		BindingContext = summaryViewModel;
	}

    /*protected override void OnAppearing()
    {
        base.OnAppearing();

        ((SummaryViewModel)BindingContext).SetNumberOfPoints();

    }*/


	/*private void RemoveAndAssignIsCheck()
	{
        if (Preferences.ContainsKey("selectedPaymentMethod"))
        {
            Preferences.Remove("selectedPaymentMethod");
        }
        ((SummaryViewModel)BindingContext).TempDataClass.IsCheck = false;

        SetImageSource();
    }

    private void SetImageSource()
    {
        var theme = Microsoft.Maui.Controls.Application.Current.Resources.MergedDictionaries.FirstOrDefault(theme => theme is darkTheme || theme is lightTheme);

        if (theme is darkTheme)
        {
            ((SummaryViewModel)BindingContext).TempDataClass.ImageSource = ImageSource.FromFile("light_card_icon.png");
        }
        else
        {
            ((SummaryViewModel)BindingContext).TempDataClass.ImageSource = ImageSource.FromFile("card_icon.png");
        }
    }*/
}