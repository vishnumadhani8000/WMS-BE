namespace WMS.Domain.Common;

public class CommonFilterDto
{
    public string? Search { get; set; }
    public int     PageNumber { get; set; } = 1;
    public int     PageSize   { get; set; } = 10;
    public string? SortBy     { get; set; }         
    public bool    Ascending  { get; set; } = true;
}