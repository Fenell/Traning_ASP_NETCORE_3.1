using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
	public class OrderConfiguration:IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.ToTable("Order");

			builder.HasKey(a => a.Id);

			builder.Property(a=>a.ShipEmail).IsRequired().IsUnicode(false).HasMaxLength(50);

			builder.Property(a => a.ShipPhone).IsRequired().HasMaxLength(11);

			builder.Property(a => a.ShipAddress).IsRequired().HasMaxLength(200);

			builder.Property(a => a.ShipName).IsRequired().HasMaxLength(200);

			builder.HasOne(a => a.AppUser).WithMany(x => x.Orders).HasForeignKey(a => a.UserId);
		}
	}
}
