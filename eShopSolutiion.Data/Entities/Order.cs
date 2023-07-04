using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eShopSolutiion.Data.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }
		public Guid UserId { get; set; }
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipEmail { get; set; }
		public string ShipPhone { get; set; }
		public OrderStatus Status { get; set; }

		public ICollection<OrderDetail> OrderDetails { get; set; }
	}
}
