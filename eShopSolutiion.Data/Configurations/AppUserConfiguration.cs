using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
	public class AppUserConfiguration:IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			builder.ToTable("AppUser");

			builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);

			builder.Property(x=>x.LastName).IsRequired().HasMaxLength(100);

			builder.Property(x => x.DateOfBirth).IsRequired();
		}
	}
}
