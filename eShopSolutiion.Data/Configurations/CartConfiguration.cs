using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
	public class CartConfiguration:IEntityTypeConfiguration<Cart>
	{
		public void Configure(EntityTypeBuilder<Cart> builder)
		{
			builder.ToTable("Cart");
			builder.HasKey(a => a.Id);
			builder.Property(a => a.Id).UseIdentityColumn();

			builder.HasOne(a => a.Product).WithMany(x => x.Carts).HasForeignKey(a => a.ProductId);

			builder.HasOne(a => a.AppUser).WithMany(x => x.Carts).HasForeignKey(a => a.UserId);
		}
	}
}
