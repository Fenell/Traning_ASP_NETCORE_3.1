using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Configurations;
using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace eShopSolutiion.Data.EF
{
	public class EShopDbContext : DbContext
	{
		protected EShopDbContext()
		{
		}

		public EShopDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<AppConfig> AppConfigs { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductInCategory> ProductInCategories { get; set; }
		public DbSet<Promotion> Promotions { get; set; }
		public DbSet<Transaction> Transactions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new AppConfigConfiguration());
			modelBuilder.ApplyConfiguration(new CartConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryConfiguration());
			modelBuilder.ApplyConfiguration(new ContactConfiguration());
			modelBuilder.ApplyConfiguration(new OrderConfiguration());
			modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
			modelBuilder.ApplyConfiguration(new ProductConfiguration());
			modelBuilder.ApplyConfiguration(new ProductInCategoryConfiguration());
			modelBuilder.ApplyConfiguration(new PromotionConfiguration());
			modelBuilder.ApplyConfiguration(new TransactionConfiguration());

		}
	}
}
