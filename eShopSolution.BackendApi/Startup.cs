using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolutiion.Data.EF;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Common;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Utilities.Constants;
using Microsoft.OpenApi.Models;

namespace eShopSolution.BackendApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<EShopDbContext>(option =>
				option.UseSqlServer(Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

			//Declare DI
			services.AddScoped<IPublicProductService, PublicProductService>();
			services.AddScoped<IManageProductService, ManageProductService>();
			services.AddScoped<IStorageService, FileStorageService>();

			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo(){Title = "Swagger eShopSolution", Version = "v1"});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger eShopSolution v1");
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
