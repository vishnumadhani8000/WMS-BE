using WMS.Application.DTOs.Shipments;
using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Application.Interfaces;

public interface IShipmentService
{
    Task CreateShipmentAsync(MakeShipmentRequestDto dto,long userId);

    Task<PagedResult<ShipmentResponseDto>>GetAllShipmentsAsync(CommonFilterDto filterDto);

    Task<ShipmentDetailResponseDto>GetShipmentByIdAsync(long shipmentId);
    Task UpdateShipmentStatusAsync(long shipmentId,ShipmentStatus status,long userId);
}