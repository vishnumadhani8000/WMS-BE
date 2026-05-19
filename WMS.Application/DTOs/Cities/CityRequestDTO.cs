using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.State;

public class CityRequestDTO
{

    public long StateId { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}