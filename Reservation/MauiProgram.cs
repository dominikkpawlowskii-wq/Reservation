using EntitySQLite;
using Microsoft.Extensions.Configuration;
using Services;

namespace Reservations;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<ILoginService, LoginService>();

        builder.Services.AddSingleton<RestaurantDetailViewModel>();
        builder.Services.AddTransient<ReservationViewModel>();
        builder.Services.AddSingleton<SummaryViewModel>();
        builder.Services.AddTransient<MyReservationDetailViewModel>();
        builder.Services.AddTransient<PedometrViewModel>();
        builder.Services.AddTransient<ReservationsMyAccoutViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<MyDataViewModel>();
        
        builder.Services.AddTransientPopup<MyPopup, PopupViewModel>();
        builder.Services.AddTransientPopup<PaymentMethodsPopup, PaymentMethodViewModel>();

        builder.Services.AddSingleton<SettingViewModel>();
        builder.Services.AddSingleton<AboutViewModel>();
        builder.Services.AddSingleton<TitleViewModel>();

        builder.Services.AddSingleton<ProfilViewModel>();
        builder.Services.AddSingleton<INavigationDataService, NavigationDataService>();
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IReservationHttpClient, ReservationHttpClient>();
        builder.Services.AddSingleton<MapViewModel>();
        builder.Services.AddSingleton<SearchViewModel>();
        builder.Services.AddSingleton<IGeocoding>(Geocoding.Default);
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
