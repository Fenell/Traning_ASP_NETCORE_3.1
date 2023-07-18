using System;
using System.Collections.Generic;
using System.Text;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.ViewModels.System.User
{
	public class GetUserPagingRequest:PagingRequestBase
	{
		public string Keyword { get; set; }
	}
}
