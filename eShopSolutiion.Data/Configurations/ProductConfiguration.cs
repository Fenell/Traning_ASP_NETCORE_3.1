using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolutiion.Data.Configurations
{
	public class ProductConfiguration:IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.ToTable("Product");

			builder.HasKey(a => a.Id);
			builder.Property(a => a.Id).UseIdentityColumn();

			builder.Property(a => a.Price).IsRequired();

			builder.Property(a=>a.Stock).IsRequired().HasDefaultValue(0);

			builder.Property(a => a.ViewCount).IsRequired().HasDefaultValue(0);

			builder.Property(a => a.OriginalPrice).IsRequired();

			//builder.HasMany(a => a.ProductInCategories).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
		}
	}
}
