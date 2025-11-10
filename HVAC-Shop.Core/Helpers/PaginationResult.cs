namespace HVAC_Shop.Core.Helpers
{
    public class PaginationResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
