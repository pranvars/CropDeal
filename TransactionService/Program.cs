
using CustomersAPI;
using Microsoft.EntityFrameworkCore;
using TransactionService.Process;
using TransactionService.Repositories;

namespace TransactionService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<TransactionDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TransactionDBConnection")));

            // Register Repositories
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

            // Register Process Layer
            builder.Services.AddScoped<ITransactionProcess, TransactionProcess>();
            builder.Services.AddScoped<TokenValidator>();
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
