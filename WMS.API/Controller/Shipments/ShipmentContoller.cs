using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Shipments;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/shipments")]
[Authorize]
public class ShipmentController : ControllerBase
{
    private readonly IShipmentService _service;

    public ShipmentController(
        IShipmentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShipment(
        [FromBody] MakeShipmentRequestDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        await _service.CreateShipmentAsync(
            dto,
            userId);

        return Ok(
            ApiResponse<string>.Success(
                "Shipment created successfully."));
    }


    [HttpGet]
    public async Task<IActionResult> GetAllShipments(
    [FromQuery] CommonFilterDto filterDto)
    {
        var result =
            await _service.GetAllShipmentsAsync(filterDto);

        return Ok(
            ApiResponse<PagedResult<ShipmentResponseDto>>
                .Success(
                    result,
                    "Shipments fetched successfully."
                )
        );
    }

    [HttpGet("{shipmentId:int}")]
    public async Task<IActionResult> GetShipmentById(
        int shipmentId)
    {
        var shipment =
            await _service.GetShipmentByIdAsync(
                shipmentId);

        return Ok(
            ApiResponse<ShipmentDetailResponseDto>
                .Success(
                    shipment,
                    "Shipment fetched successfully."
                )
        );
    }

    [HttpPut("{shipmentId:int}/status")]
    public async Task<IActionResult> UpdateShipmentStatus(int shipmentId,[FromBody] UpdateShipmentStatusRequestDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        await _service.UpdateShipmentStatusAsync(
            shipmentId,
            dto.Status,
            userId);

        return Ok(
            ApiResponse<string>.Success(
                "Shipment status updated successfully."));
    }
}