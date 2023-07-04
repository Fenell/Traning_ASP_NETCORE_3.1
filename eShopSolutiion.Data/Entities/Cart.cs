using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace eShopSolutiion.Data.Entities
{
	public class Cart
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }

		public Product Product { get; set; }
	}
}
