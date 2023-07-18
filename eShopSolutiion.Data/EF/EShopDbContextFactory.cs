using System.IO;
using eShopSolutiion.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.Data.EF
{
	public class EShopDbContextFactory:IDesignTimeDbContextFactory<EShopDbContext>
	{
		public EShopDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var connectionString = configuration.GetConnectionString("eShopSolution");

			var optionBuilder = new DbContextOptionsBuilder<EShopDbContext>();
			optionBuilder.UseSqlServer(connectionString);

			return new EShopDbContext(optionBuilder.Options);

		}
	}
}
