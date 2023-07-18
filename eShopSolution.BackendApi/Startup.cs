using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using eShopSolutiion.Data.EF;
using eShopSolutiion.Data.Entities;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Common;
using eShopSolution.Application.System.Users;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.System.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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

			services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<EShopDbContext>().AddDefaultTokenProviders();

			//Declare DI
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IStorageService, FileStorageService>();
			services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
			services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
			services.AddScoped<IUserService, UserService>();

			//services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
			//services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();

			services.AddControllers()

				//Register tất cả Validator có cùng DLL với LoginRequestValidator 
				.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger eShopSolution", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header,
						},
						new List<string>()
					}
				});
			});

			string issuer = Configuration.GetValue<string>("Tokens:Issuer");
			string sigingKey = Configuration.GetValue<string>("Tokens:Key");

			//Add Authentication
			services.AddAuthentication(opt =>
				{
					opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				//Add JwtBearer
				.AddJwtBearer(opt =>
				{
					opt.SaveToken = true;
					opt.RequireHttpsMetadata = false;
					opt.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidAudience = issuer,
						ValidIssuer = issuer,
						ValidateIssuerSigningKey = true,
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sigingKey))
					};
				});

			//Config register password
			services.Configure<IdentityOptions>(option =>
			{
				option.Password.RequireUppercase = false;
				option.Password.RequiredLength = 6;
				option.Password.RequireNonAlphanumeric = false;
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

			app.UseAuthentication();

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
