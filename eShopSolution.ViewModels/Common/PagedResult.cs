using System.Collections.Generic;

namespace eShopSolution.ViewModels.Common
{
	public class PagedResult<T>
	{
		public List<T> Items { get; set; }

		public int TotalRecode { get; set; }
	}
}
