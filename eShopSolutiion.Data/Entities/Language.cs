using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolutiion.Data.Entities
{
	public class Language
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public bool IdDefault { get; set; }

		public ICollection<CategoryTranslation> CategoryTranslations { get; set; }
		public ICollection<ProductTranslation> ProductTranslations { get; set; }
	}
}
