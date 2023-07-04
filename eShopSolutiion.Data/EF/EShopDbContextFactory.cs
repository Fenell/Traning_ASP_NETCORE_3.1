using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace eShopSolutiion.Data.EF
{
	internal class EShopDbContextFactory:IDesignTimeDbContextFactory<EShopDbContext>
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
