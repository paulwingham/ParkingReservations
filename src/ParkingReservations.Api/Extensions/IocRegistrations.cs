using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto.DomainSettings;
using Paul.ParkingReservations.Infrastructure.SqlLite;

namespace Paul.ParkingReservations.Api.Extensions;

public static class IocRegistrations
{
    public static void RegisterIocServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
        serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        serviceCollection.AddOptions<ParkingReservationsAppConfigSettings>().Bind(configuration.GetSection("AppSettings"));

        serviceCollection.AddTransient<IBookingService, BookingService>();
        serviceCollection.AddTransient<ILoginService, LoginService>();
        serviceCollection.AddTransient<IParkingStructureService, ParkingStructureService>();
        serviceCollection.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();

        serviceCollection.AddTransient<IDapperProvider, DapperProvider>();
        serviceCollection.AddTransient<ISqliteConnectionProvider>(conn => new SqliteConnectionProvider(configuration.GetConnectionString("ParkingReservationConnectionString")));
    }
}