using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Paul.ParkingReservations.Api.Extensions;

namespace Paul.ParkingReservations.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

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
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Template");
                    c.RoutePrefix = string.Empty;
                });
            }

            ////app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
