namespace Lim_BE_Assignment.Dtos
{
    public class PageResponse<T>
    {
        /// <summary>
        /// Item 총수
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 페이지
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 페이지 사이즈
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 최대 페이지
        /// </summary>
        public int MaxPage => Convert.ToInt32(Math.Ceiling(Total * 1.0 / Math.Max(1, PageSize)));
        /// <summary>
        /// 페이징 데이터
        /// </summary>
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
