using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace eShopSolutiion.Data.Entities
{
	public class AppUser : IdentityUser<Guid>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }

		public ICollection<Cart> Carts { get; set; }
		public ICollection<Order> Orders { get; set; }
		public ICollection<Transaction> Transactions { get; set; }
	}
}
