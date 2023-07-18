﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using eShopSolutiion.Data.EF;
using eShopSolutiion.Data.Entities;
using eShopSolution.Application.Common;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.Products
{
	public class ProductService : IProductService
	{
		private readonly EShopDbContext _context;
		private readonly IStorageService _storageService;

		public ProductService(EShopDbContext context, IStorageService storageService)
		{
			_context = context;
			_storageService = storageService;
		}

		public async Task<int> Create(ProductCreateRequest request)
		{
			var product = new Product()
			{
				Price = request.Price,
				OriginalPrice = request.OriginalPrice,
				Stock = request.Stock,
				ViewCount = 0,
				DateCreated = DateTime.Now,
				ProductTranslations = new List<ProductTranslation>()
				{
					new ProductTranslation()
					{
						Name = request.Name,
						Description = request.Description,
						Detail = request.Detail,
						SeoDescription = request.SeoDescription,
						SeoAlias = request.SeoAlias,
						SeoTitle = request.SeoTitle,
						LanguageId = request.LanguageId,
					}
				}
			};
			//Save image
			if (request.ThumbnailImage != null)
			{
				product.ProductImages = new List<ProductImage>()
				{
					new ProductImage()
					{
						Caption = "Thumbnail image",
						DateCreated = DateTime.Now,
						FileSize = request.ThumbnailImage.Length,
						ImagePath = await SaveFile(request.ThumbnailImage),
						IsDefault = true,
						SortOrder = 1
					}
				};
			}
			_context.Products.Add(product);
			await _context.SaveChangesAsync();
			return product.Id;
		}

		public async Task<int> Update(ProductUpdateRequest request)
		{
			var product = await _context.Products.FindAsync(request.Id);
			var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
			if (product == null || productTranslations == null)
			{
				throw new EShopException($"Can not find a product with id: {request.Id}");
			}
			productTranslations.Name = request.Name;
			productTranslations.SeoAlias = request.SeoAlias;
			productTranslations.SeoDescription = request.SeoDescription;
			productTranslations.SeoTitle = request.SeoTitle;
			productTranslations.Description = request.Description;
			productTranslations.Detail = request.Detail;
			if (request.ThumbnailImage != null)
			{
				var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(a => a.IsDefault == true && a.ProductId == request.Id);
				if (thumbnailImage != null)
				{
					thumbnailImage.FileSize = request.ThumbnailImage.Length;
					thumbnailImage.ImagePath = await SaveFile(request.ThumbnailImage);
					_context.ProductImages.Update(thumbnailImage);
				}
			}

			return await _context.SaveChangesAsync();
		}

		public async Task<int> Delete(int productId)
		{
			var product = await _context.Products.FindAsync(productId);

			if (product == null)
			{
				throw new EShopException($"Cannot find a product: {productId}");
			}

			var images = _context.ProductImages.Where(a => a.ProductId == productId);
			foreach (var image in images)
			{
				await _storageService.DeleteFileAsync(image.ImagePath);
			}
			_context.Products.Remove(product);
			return await _context.SaveChangesAsync();
		}

		public async Task<bool> UpdatePrice(int productId, decimal newPrice)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
			{
				throw new EShopException($"Cannot find a product with id: {productId}");
			}
			product.Price = newPrice;

			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> UpdateStock(int productId, int addedQuantity)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
			{
				throw new EShopException($"Cannot find a product with id: {productId}");
			}
			product.Stock += addedQuantity;

			return await _context.SaveChangesAsync() > 0;
		}

		public async Task AddViewCount(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			product.ViewCount++;
			await _context.SaveChangesAsync();
		}

		public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
		{
			// 1.Select join
			var query = from p in _context.Products
						join pt in _context.ProductTranslations on p.Id equals pt.ProductId
						join pic in _context.ProductInCategories on p.Id equals pic.ProductId
						join c in _context.Categories on pic.CategoryId equals c.Id
						select new { p, pt, pic };


			//2. filter
			if (!string.IsNullOrEmpty(request.Keyword))
			{
				query = query.Where(x => x.pt.Name.Contains(request.Keyword));
			}

			if (request.CategoryIds.Count > 0)
			{
				query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
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

		public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
		{
			// 1.Select join
			var query = from p in _context.Products
				join pt in _context.ProductTranslations on p.Id equals pt.ProductId
				join pic in _context.ProductInCategories on p.Id equals pic.ProductId
				join c in _context.Categories on pic.CategoryId equals c.Id
				where pt.LanguageId == languageId
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

		public async Task<ProductViewModel> GetById(int productId, string languageId)
		{
			var product = await _context.Products.FindAsync(productId);
			var productTranslation =
				await _context.ProductTranslations.FirstOrDefaultAsync(a =>
					a.ProductId == productId && a.LanguageId == languageId);

			var productViewModel = new ProductViewModel
			{
				Id = product.Id,
				DateCreated = product.DateCreated,
				Description = productTranslation != null ? productTranslation.Description : null,
				LanguageId = productTranslation != null ? productTranslation.LanguageId : null,
				Detail = productTranslation != null ? productTranslation.Detail : null,
				Name = productTranslation != null ? productTranslation.Name : null,
				OriginalPrice = product.OriginalPrice,
				Price = product.Price,
				SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
				SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
				SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
				Stock = product.Stock,
				ViewCount = product.ViewCount
			};

			return productViewModel;
		}

		public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
		{
			var productImage = new ProductImage()
			{
				Caption = request.Caption,
				DateCreated = DateTime.Now,
				IsDefault = request.IsDefault,
				ProductId = productId,
				SortOrder = request.SortOrder
			};

			if (request.ImageFile != null)
			{
				productImage.ImagePath = await SaveFile(request.ImageFile);
				productImage.FileSize = request.ImageFile.Length;
			}

			_context.ProductImages.Add(productImage);
			await _context.SaveChangesAsync();

			return productImage.Id;
		}

		public async Task<int> DeleteImage(int imageId)
		{
			var productImage = _context.ProductImages.FirstOrDefault(x => x.Id == imageId);
			if (productImage == null)
			{
				throw new EShopException($"Cannot find a product with id:{imageId}");
			}

			await _storageService.DeleteFileAsync(productImage.ImagePath);
			_context.ProductImages.Remove(productImage);

			return await _context.SaveChangesAsync();
		}

		public async Task<int> UpdateImages(int imageId, ProductImageUpdateRequest request)
		{
			var productImage = await _context.ProductImages.FindAsync(imageId);
			if (productImage == null)
			{
				throw new EShopException($"Cannot find image with id:{imageId}");
			}

			if (request.ImageFile != null)
			{
				productImage.ImagePath = await SaveFile(request.ImageFile);
				productImage.FileSize = request.ImageFile.Length;
			}

			_context.ProductImages.Update(productImage);

			return await _context.SaveChangesAsync();
		}

		public async Task<ProductImageViewModel> GetImageById(int imageId)
		{
			var productImage = await _context.ProductImages.FindAsync(imageId);
			if (productImage == null)
			{
				throw new Exception($"Cannot find image with id: {imageId}");
			}

			var viewModel = new ProductImageViewModel()
			{
				Id = productImage.Id,
				ProductId = productImage.ProductId,
				Caption = productImage.Caption,
				DateCreated = productImage.DateCreated,
				IsDefault = productImage.IsDefault,
				SortOrder = productImage.SortOrder,
				ImagePath = productImage.ImagePath,
				FileSize = productImage.FileSize
			};

			return viewModel;
		}

		public async Task<List<ProductImageViewModel>> GetListImages(int productId)
		{
			var lstImages = await _context.ProductImages.Where(c => c.ProductId == productId)
				.Select(c => new ProductImageViewModel()
				{
					Id = c.Id,
					ProductId = c.ProductId,
					Caption = c.Caption,
					DateCreated = c.DateCreated,
					SortOrder = c.SortOrder,
					IsDefault = c.IsDefault,
					ImagePath = c.ImagePath,
					FileSize = c.FileSize
				}).ToListAsync();

			return lstImages;
		}

		private async Task<string> SaveFile(IFormFile file)
		{
			var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
			var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
			await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
			return fileName;
		}

	}
}
