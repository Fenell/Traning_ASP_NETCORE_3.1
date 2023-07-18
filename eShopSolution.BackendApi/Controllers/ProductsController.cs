using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _publicProductService;
		private readonly IProductService _manageProductService;

		public ProductsController(IProductService publicProductService, IProductService manageProductService)
		{
			_publicProductService = publicProductService;
			_manageProductService = manageProductService;
		}

		//http://localhost:port/(controller name)
		//Ít dùng đến trong thực tế
		//[HttpGet("{languageId}")]
		//public async Task<IActionResult> Get(string languageId)
		//{
		//	var product = await _publicProductService.GetAll(languageId);
		//	return Ok(product);
		//}

		//http://localhost:port/produtc?/pageIndex=1&pageSize=10&CategoryId=
		[HttpGet("{languageId}")]
		public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
		{
			var product = await _publicProductService.GetAllByCategoryId(languageId, request);
			return Ok(product);
		}

		[HttpGet("{productId}/{languageId}")]
		public async Task<IActionResult> GetById(int productId, string languageId)
		{
			var product = await _manageProductService.GetById(productId, languageId);
			if (product == null)
			{
				return BadRequest("Cannot find product"); //400
			}
			return Ok(product);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var productId = await _manageProductService.Create(request);
			if (productId == 0)
			{
				return BadRequest();
			}

			var product = await _manageProductService.GetById(productId, request.LanguageId);

			return Created(nameof(GetById), product);

			//Lỗi: No route matches the supplied values.
			//return CreatedAtAction(actionName: nameof(GetById), routeValues:new { id = productId },value: product);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var affectedResult = await _manageProductService.Update(request);
			if (affectedResult == 0)
			{
				return BadRequest();
			}
			return Ok();
		}

		[HttpDelete("{productId}")]
		public async Task<IActionResult> Delete(int productId)
		{
			var affectedResult = await _manageProductService.Delete(productId);
			if (affectedResult == 0)
			{
				return BadRequest(); //400
			}
			return Ok();
		}

		// Update 1 phần có thể dùng HttpPatch
		[HttpPatch("{productId}/{newPrice}")]
		public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
		{
			var isSuccessful = await _manageProductService.UpdatePrice(productId, newPrice);

			if (isSuccessful)
			{
				return Ok();
			}
			return BadRequest();
		}

		[HttpGet("{productId}/images")]
		public async Task<IActionResult> GetImage(int productId)
		{
			var images = await _manageProductService.GetListImages(productId);

			return Ok(images);
		}

		[HttpPost("{productId}/images")]
		public async Task<IActionResult> CreateImage(int productId, [FromForm]ProductImageCreateRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var imageId = await _manageProductService.AddImage(productId, request);

			if (imageId == 0)
			{
				return BadRequest();
			}
			var image = _manageProductService.GetImageById(imageId);

			return Created(nameof(GetById), image);
		}

		[HttpPut("{productId}/images/{imageId}")]
		public async Task<IActionResult> UpdateImage(int imageId, [FromForm]ProductImageUpdateRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var result = await _manageProductService.UpdateImages(imageId, request);

			if (result == 0)
			{
				return BadRequest();
			}

			return Ok();
		}

		[HttpDelete("{productId}/images/{imageId}")]
		public async Task<IActionResult> DeleteImage(int imageId)
		{
			var result = await _manageProductService.DeleteImage(imageId);

			if (result == 0)
			{
				return BadRequest();
			}
			return Ok();
		}

	}
}
