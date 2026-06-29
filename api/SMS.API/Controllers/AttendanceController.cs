using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs;
using SMS.Application.Features.Attendance.GetAttendanceById;
using SMS.Application.Features.Attendance.SearchAttendance;
using SMS.Application.Features.Attendance.CreateAttendance;
using SMS.Application.Features.Attendance.GetAttendance;
using SMS.Application.Features.Attendance.UpdateAttendance;
using SMS.Application.Features.Attendance.DeleteAttendance;

namespace SMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly CreateAttendanceHandler _createAttendanceHandler;
    private readonly GetAttendanceHandler _getAttendanceHandler;
    private readonly SearchAttendanceHandler _searchAttendanceHandler;
    private readonly GetAttendanceByIdHandler _getAttendanceByIdHandler;
    private readonly UpdateAttendanceHandler _updateAttendanceHandler;
    private readonly DeleteAttendanceHandler _deleteAttendanceHandler;

    public AttendanceController(
        CreateAttendanceHandler createAttendanceHandler,
        GetAttendanceHandler getAttendanceHandler,
        SearchAttendanceHandler searchAttendanceHandler,
        GetAttendanceByIdHandler getAttendanceByIdHandler,
        UpdateAttendanceHandler updateAttendanceHandler,
        DeleteAttendanceHandler deleteAttendanceHandler)
    {
        _createAttendanceHandler = createAttendanceHandler;
        _getAttendanceHandler = getAttendanceHandler;
        _searchAttendanceHandler = searchAttendanceHandler;
        _getAttendanceByIdHandler = getAttendanceByIdHandler;
        _updateAttendanceHandler = updateAttendanceHandler;
        _deleteAttendanceHandler = deleteAttendanceHandler;
    }

    [HttpPost]
    public async Task<ActionResult<AttendanceResponseDto>> Create(
        [FromBody] CreateAttendanceRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateAttendanceCommand(
            request.StudentId,
            request.ClassId,
            request.Status,
            request.AttendanceDate,
            request.Remarks);

        var result = await _createAttendanceHandler.HandleAsync(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedAttendanceResponseDto>> Search(
        [FromQuery] SearchAttendanceQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _searchAttendanceHandler
            .HandleAsync(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AttendanceDetailsDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _getAttendanceByIdHandler
            .HandleAsync(id, cancellationToken);

        if (result == null)
        {
            return NotFound(new
            {
                error = "Attendance not found."
            });
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<AttendanceListItemDto>>> Get(
        [FromQuery] GetAttendanceQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _getAttendanceHandler.HandleAsync(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AttendanceResponseDto>> Update(
        Guid id,
        [FromBody] UpdateAttendanceRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAttendanceCommand(
            id,
            request.Status,
            request.AttendanceDate,
            request.Remarks);

        var result = await _updateAttendanceHandler.HandleAsync(command, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _deleteAttendanceHandler.HandleAsync(
            new DeleteAttendanceCommand(id),
            cancellationToken);

        if (!result)
        {
            return NotFound(new
            {
                error = "Attendance not found."
            });
        }

        return Ok(new
        {
            message = "Attendance deleted successfully."
        });
    }
}
