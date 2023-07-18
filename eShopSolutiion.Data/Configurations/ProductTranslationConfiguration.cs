using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
	public class ProductTranslationConfiguration:IEntityTypeConfiguration<ProductTranslation>
	{
		public void Configure(EntityTypeBuilder<ProductTranslation> builder)
		{
			builder.ToTable("ProductTranslation");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).UseIdentityColumn();

			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

			builder.Property(x => x.SeoAlias).IsRequired().HasMaxLength(200);

			builder.Property(x => x.Detail).HasMaxLength(500);

			builder.Property(x => x.LanguageId).IsUnicode(false).IsRequired().HasMaxLength(5);

			builder.HasOne(x => x.Language).WithMany(x => x.ProductTranslations).HasForeignKey(x => x.LanguageId);

			builder.HasOne(x => x.Product).WithMany(x => x.ProductTranslations).HasForeignKey(x => x.ProductId);
		}
	}
}
