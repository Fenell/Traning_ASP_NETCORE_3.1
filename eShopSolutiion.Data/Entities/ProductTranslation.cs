using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace eShopSolutiion.Data.Entities
{
	public class ProductTranslation
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string LanguageId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Detail { get; set; }
		public string SeoDescription { get; set; }
		public string SeoTitle { get; set; }
		public string SeoAlias { get; set; }
		public Product Product { get; set; }
		public Language Language { get; set; }
	}
}
