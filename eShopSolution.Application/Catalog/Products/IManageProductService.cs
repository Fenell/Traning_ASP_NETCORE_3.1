using System.Collections.Generic;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Products.Public;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;


namespace eShopSolution.Application.Catalog.Products
{
	public interface IManageProductService
	{
		Task<int> Create(ProductCreateRequest request);

		Task<int> Update(ProductUpdateRequest request);

		Task<int> Delete(int productId);

		Task<bool> UpdatePrice(int productId, decimal newPrice);

		Task<bool> UpdateStock(int productId, int addedQuantity);

		Task AddViewCount(int productId);


		Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

		Task<int> AddImages(int productId, List<IFormFile> files);

		Task<int> DeleteImages(int productId);

		Task<int> UpdateImages(int imageId, string caption, bool isDefault);

		Task<List<ProductImageViewModel>> GetListImage(int productId);
	}
}
