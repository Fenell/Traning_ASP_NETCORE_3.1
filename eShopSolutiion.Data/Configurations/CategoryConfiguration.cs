using eShopSolutiion.Data.Entities;
using eShopSolutiion.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
	public class CategoryConfiguration:IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable("Category");

			builder.HasKey(c => c.Id);

			builder.Property(a => a.Status).HasDefaultValue(Status.Active);

			//builder.HasMany(a => a.ProductInCategories).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId);
		}
	}
}
