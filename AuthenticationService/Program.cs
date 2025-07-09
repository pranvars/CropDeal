
using AuthenticationService.EntityModel;
using AuthenticationService.Process;
using AuthenticationService.Repositories;
using AuthenticationService.Repository;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            builder.Services.AddDbContext<AuthenticationDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AuthenticationDBContext")));
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IUserRepository, UsersRepository>();
            builder.Services.AddScoped<AuthProcess>();
            builder.Services.AddScoped<TokenManager>();

            builder.Services.AddCors(policyConfig =>
             {
                 policyConfig.AddPolicy("allowAllPolicy",
                     policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
             });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseCors("allowAllPolicy");
            //app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();



            app.MapControllers();

            app.Run();
        }
    }
}
