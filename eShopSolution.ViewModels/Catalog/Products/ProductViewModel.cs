using System;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
	    public int Id { get; set; }
	    public decimal Price { get; set; }
	    public decimal OriginalPrice { get; set; }
	    public int Stock { get; set; }
	    public int ViewCount { get; set; }
	    public DateTime DateCreated { get; set; }
	    public string LanguageId { get; set; }
	    public string Name { get; set; }
	    public string Description { get; set; }
	    public string Detail { get; set; }
	    public string SeoDescription { get; set; }
	    public string SeoTitle { get; set; }
	    public string SeoAlias { get; set; }
    }
}
