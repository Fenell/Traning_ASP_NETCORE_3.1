using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolutiion.Data.Configurations
{
	public class TransactionConfiguration:IEntityTypeConfiguration<Transaction>
	{
		public void Configure(EntityTypeBuilder<Transaction> builder)
		{
			builder.ToTable("Transaction");

			builder.HasKey(t => t.Id);

			builder.Property(t => t.Id).UseIdentityColumn();

			builder.HasOne(x => x.AppUser).WithMany(a => a.Transactions).HasForeignKey(x => x.UserId);
		}
	}
}
