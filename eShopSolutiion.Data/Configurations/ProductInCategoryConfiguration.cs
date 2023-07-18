using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
	public class ProductInCategoryConfiguration:IEntityTypeConfiguration<ProductInCategory>
	{
		public void Configure(EntityTypeBuilder<ProductInCategory> builder)
		{
			builder.ToTable("ProductInCategory");
			builder.HasKey(x => new{x.CategoryId, x.ProductId});

			builder.HasOne(a => a.Product).WithMany(x => x.ProductInCategories).HasForeignKey(a=>a.ProductId);

			builder.HasOne(a => a.Category).WithMany(x => x.ProductInCategories).HasForeignKey(a => a.CategoryId);
		}
	}
}
