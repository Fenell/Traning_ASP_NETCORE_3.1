﻿using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
	public class AppRoleConfiguration:IEntityTypeConfiguration<AppRole>
	{
		public void Configure(EntityTypeBuilder<AppRole> builder)
		{
			builder.ToTable("AppRole");

			builder.Property(x => x.Description).HasMaxLength(200).IsRequired();


		}
	}
}
