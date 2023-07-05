using System;
using System.Collections.Generic;
using System.Text;
using eShopSolutiion.Data.Enums;

namespace eShopSolutiion.Data.Entities
{
	public class Category
	{
		public int Id { get; set; }
		public int SortOrder { get; set; }
		public bool IsShowOnHome { get; set; }
		public int? ParentId { get; set; }
		public Status Status { get; set; }

		public ICollection<ProductInCategory> ProductInCategories { get; set; }

		public ICollection<CategoryTranslation> CategoryTranslations { get; set; }
	}
}
