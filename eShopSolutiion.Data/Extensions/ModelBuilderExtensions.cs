using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Entities;
using eShopSolutiion.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShopSolutiion.Data.Extensions
{
	public static class ModelBuilderExtensions
	{
		public static void Seed(this ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AppConfig>().HasData(
				new AppConfig() { Key = "Home title", Value = "This is home page of EShopSolution" },
				new AppConfig() { Key = "HomeKeyword", Value = "This is key word of EShopSolution" }
			);

			modelBuilder.Entity<Language>().HasData(
				new Language() { Id = "vi-VN", Name = "Tiếng Việt", IdDefault = true },
				new Language() { Id = "en-US", Name = "English", IdDefault = false }
			);

			modelBuilder.Entity<Category>().HasData(
				new Category()
				{
					Id = 1,
					IsShowOnHome = true,
					ParentId = null,
					SortOrder = 1,
					Status = Status.Active,
				},
				new Category()
				{
					Id = 2,
					IsShowOnHome = true,
					ParentId = null,
					SortOrder = 2,
					Status = Status.Active,
				}
			);
			modelBuilder.Entity<CategoryTranslation>().HasData(
				new CategoryTranslation()
				{
					Id = 1,
					CategoryId = 1,
					Name = "Áo nam",
					LanguageId = "vi-VN",
					SeoAlias = "ao-nam",
					SeoDescription = "Sản phẩm thời trang nam",
					SeoTitle = "Sản phẩm thời trang nam"
				},
				new CategoryTranslation()
				{
					Id = 2,
					CategoryId = 1,
					Name = "Men shirt",
					LanguageId = "en-US",
					SeoAlias = "men-shirt",
					SeoDescription = "The shirt products for men",
					SeoTitle = "The shirt products for men"
				},
				new CategoryTranslation()
				{
					Id = 3,
					CategoryId = 2,
					Name = "Áo nữ",
					LanguageId = "vi-VN",
					SeoAlias = "ao-nu",
					SeoDescription = "Sản phẩm thời trang nữ",
					SeoTitle = "Sản phẩm thời trang nữ"
				},
				new CategoryTranslation()
				{
					Id = 4,
					CategoryId = 2,
					Name = "Woman shirt",
					LanguageId = "en-US",
					SeoAlias = "men-shirt",
					SeoDescription = "The shirt products for woman",
					SeoTitle = "The shirt products for woman"
				});

			modelBuilder.Entity<Product>().HasData(
				new Product()
				{
					Id = 1,
					DateCreated = DateTime.Now,
					OriginalPrice = 100000,
					Price = 200000,
					Stock = 0,
					ViewCount = 0,
				}
			);

			modelBuilder.Entity<ProductTranslation>().HasData(
				new ProductTranslation()
				{
					Id = 1,
					ProductId = 1,
					Name = "Áo sơ mi trắng CoolMate",
					LanguageId = "vi-VN",
					SeoAlias = "ao-so-mi-trang-coolmate",
					SeoDescription = "Áo sơ mi trắng coolmate",
					SeoTitle = "Áo sơ mi trắng coolmate",
					Detail = "Áo sơ mi trắng coolmate",
					Description = "Áo sơ mi trắng coolmate"
				},

				new ProductTranslation()
				{
					Id = 2,
					ProductId = 1,
					Name = "Coolmate Men T-shirt",
					LanguageId = "en-US",
					SeoAlias = "coolmate-men-t-shirt",
					SeoDescription = "Coolmate Men T-shirt",
					SeoTitle = "Coolmate Men T-shirt",
					Detail = "Coolmate Men T-shirt",
					Description = "Coolmate Men T-shirt"
				});

			modelBuilder.Entity<ProductInCategory>().HasData(new ProductInCategory() { CategoryId = 1, ProductId = 1 });

			var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
			var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
			modelBuilder.Entity<AppRole>().HasData(new AppRole()
			{
				Id = roleId,
				Name = "admin",
				NormalizedName = "admin",
				Description = "Adminstrator role"
			});

			var hasher = new PasswordHasher<AppUser>();
			modelBuilder.Entity<AppUser>().HasData(new AppUser()
			{
				Id = adminId,
				UserName = "admin",
				Email = "datmt@gmail.com",
				NormalizedEmail = "datmt@gmail.com",
				EmailConfirmed = true,
				PasswordHash = hasher.HashPassword(null, "1234ABC$"),
				SecurityStamp = string.Empty,
				FirstName = "Mai",
				LastName = "Tuan Dat",
				DateOfBirth = new DateTime(2000, 1, 1)
			});

			modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>()
			{
				RoleId = roleId,
				UserId = adminId
			});

		}
	}
}
