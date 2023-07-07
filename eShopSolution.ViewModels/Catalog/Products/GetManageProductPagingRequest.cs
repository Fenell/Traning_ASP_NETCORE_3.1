﻿using System.Collections.Generic;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.ViewModels.Catalog.Products
{
	public class GetManageProductPagingRequest:PagingRequestBase
	{
		public string Keyword { get; set; }
		public List<int> CategoryIds { get; set; }
	}
}
