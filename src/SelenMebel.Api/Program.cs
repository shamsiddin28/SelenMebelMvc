
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SelenMebel.Api.Extensions;
using SelenMebel.Data.DbContexts;
using SelenMebel.Service.Helpers;
using SelenMebel.Service.Mappers;
using Serilog;

namespace SelenMebel.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SelenMebelDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Fix the Cycle
            //builder.Services.AddControllers()
            //     .AddNewtonsoftJson(options =>
            //     {
            //         options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //     });



            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddCustomService();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // Serialog
            var logger = new LoggerConfiguration()
               .ReadFrom.Configuration(builder.Configuration)
               .Enrich.FromLogContext()
               .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAutoMapper(typeof(MapperProfile));



            var app = builder.Build();

            WebHostEnviromentHelper.WebRootPath = Path.GetFullPath("wwwroot");

            if (app.Services.GetService<IHttpContextAccessor>() != null)
                HttpContextHelper.Accessor = app.Services.GetRequiredService<IHttpContextAccessor>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
