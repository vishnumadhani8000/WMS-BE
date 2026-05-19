using WMS.Domain.Common;

namespace WMS.Application.DTOs.State;

public class CityFilterRequestDTO : CommonFilterDto
{
    public long stateId { get; set; }

}