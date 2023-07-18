using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;


namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService
	{
		Task<int> Create(ProductCreateRequest request);

		Task<int> Update(ProductUpdateRequest request);

		Task<int> Delete(int productId);

		Task<ProductViewModel> GetById(int productId, string languageId);

		Task<bool> UpdatePrice(int productId, decimal newPrice);

		Task<bool> UpdateStock(int productId, int addedQuantity);

		Task AddViewCount(int productId);

		Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);
		Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);

		Task<int> AddImage(int productId, ProductImageCreateRequest request);

		Task<int> DeleteImage(int imageId);

		Task<int> UpdateImages(int imageId, ProductImageUpdateRequest request);

		Task<ProductImageViewModel> GetImageById(int imageId);

		Task<List<ProductImageViewModel>> GetListImages(int productId);

	}
}
