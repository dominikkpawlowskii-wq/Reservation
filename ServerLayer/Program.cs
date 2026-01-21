public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddScoped<IPaypalService, PaypalService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationServise>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IReservationService, ReservationService>();
        builder.Services.AddScoped<IAccountRefreshTokenService, AccountRefreshTokenService>();
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<ReservationsContext>(dbOption =>
        {
            SQLitePCL.Batteries.Init();

            dbOption.UseSqlite($@"{builder.Configuration.GetConnectionString("SQLiteConnectionString")}");
        });
        var SecretKey = builder.Configuration.GetSection("SecretKey").Value;

        var Issuer = builder.Configuration.GetSection("Issuer").Value;

        ConfigureServices(builder.Services, Issuer ?? "", SecretKey ?? "");

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, string Issuer, string secretKey)
    {
        JwtBearerOptions options(JwtBearerOptions jwtBearerOptions, string audience)
        {
            jwtBearerOptions.RequireHttpsMetadata = false;
            jwtBearerOptions.SaveToken = true;
            jwtBearerOptions.TokenValidationParameters = new
            TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidIssuer = Issuer
            };
            if (audience == "access")
            {
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            }
            return jwtBearerOptions;
        }
        services.AddAuthentication(configureOption =>
        {
            configureOption.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;
            configureOption.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer("access", jwtBearerOptions =>
        options(jwtBearerOptions, "access"))
        .AddJwtBearer("refresh", jwtBearerOptions => 
        options(jwtBearerOptions, "refresh"));
    }
}