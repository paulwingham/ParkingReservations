using Microsoft.OpenApi.Models;
using Paul.ParkingReservations.Api.Extensions;

namespace Paul.ParkingReservations.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.IncludeApiVersioning();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parking Reservations API - V1", Version = "v1.0" });
            });

            builder.Services.RegisterIocServices(builder.Configuration);

            var app = builder.Build();
            app.UseHttpsRedirection();

            if (app.Environment.IsDevelopment())
            {
                ////https://localhost:7235/index.html
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking Reservation API v1 ");
                    c.RoutePrefix = string.Empty;
                });
            }

            ////app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
