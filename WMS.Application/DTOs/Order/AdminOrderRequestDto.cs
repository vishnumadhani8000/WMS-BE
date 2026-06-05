using WMS.Domain.Common;

public class AdminOrderRequestDto : CommonFilterDto{
    public bool? OnlyPending  {get;set;}
    public bool? onlypendingandaccepted  {get;set;}
    
    public int? StateId { get; set; }
    public int? CityId { get; set; }
}
    