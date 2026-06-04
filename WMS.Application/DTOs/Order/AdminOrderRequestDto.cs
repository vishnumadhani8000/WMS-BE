using WMS.Domain.Common;

public class AdminOrderRequestDto : CommonFilterDto{
    public bool? OnlyPending  {get;set;}
    public bool? onlypendingandaccepted  {get;set;}
    
    public long? StateId { get; set; }
    public long? CityId { get; set; }
}
    