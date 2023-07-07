using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolutiion.Data.Configurations
{
	public class ProductImageConfiguration:IEntityTypeConfiguration<ProductImage>
	{
		public void Configure(EntityTypeBuilder<ProductImage> builder)
		{
			builder.ToTable("ProductImage");
			builder.HasKey(a => a.Id);
			builder.Property(a => a.Id).UseIdentityColumn();

			builder.Property(a => a.ImagePath).IsRequired().HasMaxLength(100);
			builder.Property(a => a.Caption).HasMaxLength(200);

			builder.HasOne(a=>a.Product).WithMany(x=>x.ProductImages).HasForeignKey(a => a.ProductId);
		}
	}
}
