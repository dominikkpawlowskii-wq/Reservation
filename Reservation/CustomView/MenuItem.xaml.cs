using System.Windows.Input;

namespace Reservations.CustomView;

public partial class MenuItem : ContentView
{
	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(nameof(Icon), typeof(string), typeof(MenuItem));

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(MenuItem));


	public static readonly BindableProperty CommandProperty =
	BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MenuItem));


    public string Icon
	{
		get => (string)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public string Text
	{
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

	public ICommand Command
	{
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

	public MenuItem()
	{
		InitializeComponent();
	}
}