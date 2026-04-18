namespace Lim_BE_Assignment.Dtos
{
    public class PageResponse<T>
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int MaxPage { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }
}
