using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eShopSolutiion.Data.Entities
{
	[Table("Product")]
	public class Product
	{
		public int Id { get; set; }
		public decimal Price { get; set; }
		public decimal OriginalPrice { get; set; }
		public int Stock { get; set; }
		public int ViewCount { get; set; }
		public DateTime DateCreated { get; set; }
		public string SeoAlias { get; set; }

		public ICollection<Cart> Carts { get; set; }
		public ICollection<OrderDetail> OrderDetails { get; set; }
		public ICollection<ProductInCategory> ProductInCategories { get; set; }
	}
}
