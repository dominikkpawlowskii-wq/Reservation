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
            .UseSkiaSharp(true)
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddDbContext<ReservationsContext>(async option =>
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("appSettings.json");

            builder.Configuration.AddJsonStream(stream);

            await DatabaseHelper.CreateDbFileInDevice("reservations.db");

            var connectionString = builder.Configuration.GetConnectionString("dbContext");

            string fullDbPath = Path.Combine(FileSystem.AppDataDirectory, connectionString!);

            option.UseSqlite($"data source={fullDbPath}");

        });
        builder.Services.AddSingleton<RestaurantDetailViewModel>();
        builder.Services.AddTransient<ReservationViewModel>();
        builder.Services.AddSingleton<PaypalApi>();
        builder.Services.AddSingleton<SummaryViewModel>();
        builder.Services.AddTransient<MyReservationDetailViewModel>();
        builder.Services.AddTransient<PedometrViewModel>();
        builder.Services.AddTransient<ReservationsMyAccoutViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<MyDataViewModel>();
        builder.Services.AddTransientPopup<MyPopup, PopupViewModel>();
        builder.Services.AddTransientPopup<PaymentMethodsPopup, PaymentMethodViewModel>();

        builder.Services.AddSingleton<TempDataClass>();
        builder.Services.AddSingleton<SettingViewModel>();
        builder.Services.AddSingleton<AboutViewModel>();
        builder.Services.AddSingleton<IReservationRepository, ReservationRepozitory>();
        builder.Services.AddSingleton<TitleViewModel>();

        builder.Services.AddSingleton<ProfilViewModel>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IRestaurantService, RestaurantService>();
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<ConnectivityCheckerService>();
        builder.Services.AddSingleton<MapViewModel>();
        builder.Services.AddSingleton<LookViewModel>();
        builder.Services.AddSingleton<IGeocoding>(Geocoding.Default);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
