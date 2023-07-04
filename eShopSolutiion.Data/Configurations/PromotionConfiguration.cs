using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace eShopSolutiion.Data.Configurations
{
	public class PromotionConfiguration:IEntityTypeConfiguration<Promotion>
	{
		public void Configure(EntityTypeBuilder<Promotion> builder)
		{
			builder.ToTable("Promotion");

			builder.HasKey(a => a.Id);

			builder.Property(a => a.Id).UseIdentityColumn();

			builder.Property(a=>a.Name).IsRequired().HasMaxLength(200);
		}
	}
}
