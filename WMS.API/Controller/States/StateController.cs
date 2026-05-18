using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.State;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StateController : ControllerBase
{
    private readonly IStateService _stateService;

    // TODO: replace with JWT claim
    private const long CurrentUserId = 1;

    public StateController(IStateService stateService)
    {
        _stateService = stateService;
    }

    // GET api/state?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize   = 10)
    {
        var result = await _stateService.GetAllAsync(pageNumber, pageSize);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // GET api/state/5
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _stateService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    // POST api/state
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StateRequestDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _stateService.CreateAsync(dto, CurrentUserId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // PUT api/state/5
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] StateRequestDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _stateService.UpdateAsync(id, dto, CurrentUserId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // DELETE api/state/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _stateService.DeleteAsync(id, CurrentUserId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}