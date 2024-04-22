using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.DbContexts;
using SelenMebel.Data.Interfaces.Commons;
using SelenMebel.Data.Repositories.Commons;

namespace SelenMebelMVC.Configuration.LayerConfigurations
{
	public static class DataAccessConfiguration
	{
		public static void ConfigureDataAccess(this IServiceCollection services, IConfiguration configuration)
		{
			AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.EnableLegacyTimestampBehavior", true);
			string connectionString = configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<SelenMebelDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
				options.EnableSensitiveDataLogging();
			});
			services.AddScoped<IUnitOfWork, UnitOfWork>();
		}
	}
}
