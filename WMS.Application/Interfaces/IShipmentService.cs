using WMS.Application.DTOs.Shipments;
using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Application.Interfaces;

public interface IShipmentService
{
    Task CreateShipmentAsync(MakeShipmentRequestDto dto,int userId);

    Task<PagedResult<ShipmentResponseDto>>GetAllShipmentsAsync(CommonFilterDto filterDto);

    Task<ShipmentDetailResponseDto>GetShipmentByIdAsync(int shipmentId);
    Task UpdateShipmentStatusAsync(int shipmentId,ShipmentStatus status,int userId);
}