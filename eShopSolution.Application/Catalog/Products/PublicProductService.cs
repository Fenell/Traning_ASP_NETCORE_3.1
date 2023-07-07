using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolutiion.Data.EF;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Products.Public;
using eShopSolution.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.Products
{
	public class PublicProductService : IPublicProductService
	{
		private readonly EShopDbContext _context;

		public PublicProductService(EShopDbContext context)
		{
			_context = context;
		}

		public async Task<List<ProductViewModel>> GetAll()
		{
			var query = from p in _context.Products
						join pt in _context.ProductTranslations on p.Id equals pt.ProductId
						join pic in _context.ProductInCategories on p.Id equals pic.ProductId
						join c in _context.Categories on pic.CategoryId equals c.Id
						select new { p, pt, pic };

			var data = await query.Select(a => new ProductViewModel()
			{
				Id = a.p.Id,
				Name = a.pt.Name,
				DateCreated = a.p.DateCreated,
				Description = a.pt.Description,
				Detail = a.pt.Detail,
				LanguageId = a.pt.LanguageId,
				OriginalPrice = a.p.OriginalPrice,
				Price = a.p.Price,
				SeoAlias = a.pt.SeoAlias,
				SeoDescription = a.pt.SeoDescription,
				SeoTitle = a.pt.SeoTitle,
				Stock = a.p.Stock,
				ViewCount = a.p.ViewCount
			}).ToListAsync();

			return data;

		}

		public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
		{
			// 1.Select join
			var query = from p in _context.Products
						join pt in _context.ProductTranslations on p.Id equals pt.ProductId
						join pic in _context.ProductInCategories on p.Id equals pic.ProductId
						join c in _context.Categories on pic.CategoryId equals c.Id
						select new { p, pt, pic };

			//2. filter
			if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
			{
				query = query.Where(p => p.pic.CategoryId == request.CategoryId);
			}

			//3. paging (phân trang)
			int totalRow = await query.CountAsync();
			var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
				.Take(request.PageSize)
				.Select(a => new ProductViewModel()
				{
					Id = a.p.Id,
					Name = a.pt.Name,
					DateCreated = a.p.DateCreated,
					Description = a.pt.Description,
					Detail = a.pt.Detail,
					LanguageId = a.pt.LanguageId,
					OriginalPrice = a.p.OriginalPrice,
					Price = a.p.Price,
					SeoAlias = a.pt.SeoAlias,
					SeoDescription = a.pt.SeoDescription,
					SeoTitle = a.pt.SeoTitle,
					Stock = a.p.Stock,
					ViewCount = a.p.ViewCount
				}).ToListAsync();

			//4.select and projection
			var pageResult = new PagedResult<ProductViewModel>()
			{
				TotalRecode = totalRow,
				Items = data
			};

			return pageResult;
		}
	}
}
