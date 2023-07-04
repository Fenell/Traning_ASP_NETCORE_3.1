using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolutiion.Data.Configurations
{
	public class ContactConfiguration:IEntityTypeConfiguration<Contact>
	{
		public void Configure(EntityTypeBuilder<Contact> builder)
		{
			builder.ToTable("Contact");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).UseIdentityColumn();

			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

			builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(11);

			builder.Property(x => x.Message).IsRequired();
		}
	}
}
