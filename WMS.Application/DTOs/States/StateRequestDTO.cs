using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.State;

public class StateRequestDTO
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}