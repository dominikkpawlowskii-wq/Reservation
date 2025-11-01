using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Reservations;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "https",
    DataHost = "myapp",
    DataPathPrefix = "/",
    AutoVerify = false
)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var uri = Intent?.DataString;
        if (!string.IsNullOrWhiteSpace(uri))
        {
            App.Current?.SendOnAppLinkRequestReceived(new Uri(uri!));
        }
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);

        var uri = intent?.DataString;
        if (!string.IsNullOrWhiteSpace(uri))
        {
            App.Current?.SendOnAppLinkRequestReceived(new Uri(uri!));
        }
    }
}
