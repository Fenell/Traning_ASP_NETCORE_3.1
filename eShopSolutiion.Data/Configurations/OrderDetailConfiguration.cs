using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
	public class OrderDetailConfiguration:IEntityTypeConfiguration<OrderDetail>
	{
		public void Configure(EntityTypeBuilder<OrderDetail> builder)
		{
			builder.ToTable("OrderDetail");
			builder.HasKey(a => a.Id);

			builder.HasOne(a => a.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId);
			builder.HasOne(a => a.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductId);
		}
	}
}
