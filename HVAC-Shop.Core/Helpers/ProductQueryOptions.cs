namespace HVAC_Shop.Core.Helpers
{
    public class ProductQueryOptions
    {
        public string? SortBy { get; set; } = "name";
        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
    }
}
