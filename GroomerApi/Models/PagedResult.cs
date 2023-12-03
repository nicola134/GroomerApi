using DevExpress.Data.Browsing.Design;

namespace GroomerApi.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ItemFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemFrom + pageSize -1;
            TotalItemsCount= totalCount;

        }
    }

}
