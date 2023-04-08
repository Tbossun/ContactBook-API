using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hateoas
{
    public class PageList<T> : List<T>
    {
        // These properties represent information about the current page and the overall paged list
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPages);

        // This constructor initializes the paged list with the specified items, count, page number, and page size
        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        // This method creates a new paged list from the specified queryable collection and page number
        public static PageList<T> Create(IQueryable<T> collection, int pageNumber)
        {
            int count = collection.Count();
            var items = collection.Skip(10 * (pageNumber - 1)).Take(10).ToList();
            return new PageList<T>(items, count, pageNumber, 10);
        }
    }
}
