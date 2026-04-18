namespace Lim_BE_Assignment.Dtos
{
    public class PageResponse<T>
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int MaxPage => Convert.ToInt32(Math.Ceiling(Total * 1.0 / Math.Max(1, PageSize)));
        public List<T> Items { get; set; } = new List<T>();

        public PageResponse()
        {
            
        }

        public PageResponse(int page, int pageSize, int total, IEnumerable<T> items)
        {
            this.Items.AddRange(items);
            this.Page = page;
            this.PageSize = pageSize;
            this.Total = total; 
        }
    }
}
