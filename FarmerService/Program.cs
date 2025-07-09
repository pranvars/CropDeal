
using CustomersAPI;
using FarmerService.Process;
using FarmerService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FarmerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<FarmerDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FarmerDBConnection")));
            builder.Services.AddScoped<IFarmerRepository, FarmerRepository>();
            builder.Services.AddScoped<IFarmerProcess, FarmerProcess>();
            builder.Services.AddHttpClient<IFarmerProcess, FarmerProcess>(); // For external API calls
            builder.Services.AddScoped<TokenValidator>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCors(setup =>
            {
                setup.AddPolicy("AllowAll", config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
