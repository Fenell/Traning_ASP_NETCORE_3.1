using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Enums;

namespace eShopSolutiion.Data.Entities
{
	public class Promotion
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public bool ApplyForAll { get; set; }
		public int? DiscountPercent { get; set; }
		public decimal? DiscountAmount { get; set; }
		public string ProductIds { get; set; }
		public string ProductCategoryIds { get; set; }
		public Status Status { get; set; }
	}
}
