using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class PagedResultBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecode { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecode / PageSize;
                return (int) Math.Ceiling(pageCount);
            }
        }
    }
}
