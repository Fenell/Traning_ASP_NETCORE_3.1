using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
	public class AppConfigConfiguration:IEntityTypeConfiguration<AppConfig>
	{
		public void Configure(EntityTypeBuilder<AppConfig> builder)
		{
			builder.ToTable("AppConfig");

			builder.HasKey(x => x.Key);

			builder.Property(x=>x.Value).IsRequired();
		}
	}
}
