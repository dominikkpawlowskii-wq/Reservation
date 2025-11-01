
namespace Reservations.CustomView;

public partial class SettingItem : ContentView
{
	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(SettingItem));

    public static readonly BindableProperty IsToggledProperty =
        BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(SettingItem), false, BindingMode.TwoWay);

    public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public bool IsToggled
    {
		get => (bool)GetValue(IsToggledProperty);
		set => SetValue(IsToggledProperty, value);
	}


	public SettingItem()
	{
		InitializeComponent();
	}
}