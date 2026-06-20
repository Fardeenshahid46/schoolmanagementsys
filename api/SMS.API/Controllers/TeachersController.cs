using SMS.Application.Features.Teachers.SearchTeachers;
using SMS.Application.Features.Teachers.DeleteTeacher;
using SMS.Application.Features.Teachers.UpdateTeacher;
using SMS.Application.Features.Teachers.GetTeacherById;
using SMS.Application.Features.Teachers.GetTeacher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs;
using SMS.Application.Features.Teachers.CreateTeacher;

namespace SMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeachersController : ControllerBase
{
    private readonly CreateTeacherHandler _createTeacherHandler;
    private readonly GetTeacherHandler _getTeacherHandler;
    private readonly GetTeacherByIdHandler _getTeacherByIdHandler;
    private readonly UpdateTeacherHandler _updateTeacherHandler;
    private readonly DeleteTeacherHandler _deleteTeacherHandler;
    private readonly SearchTeacherHandler _searchTeacherHandler;

    public TeachersController(
        CreateTeacherHandler createTeacherHandler,
        GetTeacherHandler getTeacherHandler,
        GetTeacherByIdHandler getTeacherByIdHandler,
        UpdateTeacherHandler updateTeacherHandler,
        DeleteTeacherHandler deleteTeacherHandler,
        SearchTeacherHandler searchTeacherHandler)
    {
        _createTeacherHandler = createTeacherHandler;
           _getTeacherHandler = getTeacherHandler;
         _getTeacherByIdHandler = getTeacherByIdHandler;
         _updateTeacherHandler = updateTeacherHandler;
         _deleteTeacherHandler = deleteTeacherHandler;
         _searchTeacherHandler = searchTeacherHandler;
    }
[HttpGet]
public async Task<ActionResult<List<TeacherListItemDto>>> GetAll(
    CancellationToken cancellationToken)
{
    var teachers = await _getTeacherHandler
        .HandleAsync(cancellationToken);

    return Ok(teachers);
}
[HttpGet("search")]
public async Task<ActionResult<PagedTeacherResponseDto>> Search(
    [FromQuery] GetTeacherQuery query,
    CancellationToken cancellationToken)
{
    var result = await _searchTeacherHandler
        .HandleAsync(query, cancellationToken);

    return Ok(result);
}
[HttpGet("{id}")]
public async Task<ActionResult<TeacherDetailsDto>> GetById(
    Guid id,
    CancellationToken cancellationToken)
{
    var result = await _getTeacherByIdHandler.HandleAsync(
        id,
        cancellationToken);

    if (result == null)
    {
        return NotFound(new
        {
            error = "Teacher not found."
        });
    }

    return Ok(result);
}
[HttpPut("{id}")]
public async Task<IActionResult> Update(
    Guid id,
    UpdateTeacherRequestDto request,
    CancellationToken cancellationToken)
{
    var command = new UpdateTeacherCommand(
        id,
        request.FirstName,
        request.LastName,
        request.Email,
        request.PhoneNumber,
        request.Qualification,
        request.Address);

    var result = await _updateTeacherHandler
        .HandleAsync(command, cancellationToken);

    if (!result)
    {
        return NotFound(new
        {
            error = "Teacher not found."
        });
    }

    return Ok(new
    {
        message = "Teacher updated successfully."
    });
}
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(
    Guid id,
    CancellationToken cancellationToken)
{
    var command = new DeleteTeacherCommand(id);

    var result = await _deleteTeacherHandler
        .HandleAsync(command, cancellationToken);

    if (!result)
    {
        return NotFound(new
        {
            error = "Teacher not found."
        });
    }

    return Ok(new
    {
        message = "Teacher deleted successfully."
    });
}
    [HttpPost]
    public async Task<ActionResult<TeacherResponseDto>> Create(
        CreateTeacherRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTeacherCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.Qualification,
            request.Address);

        var result = await _createTeacherHandler.HandleAsync(
            command,
            cancellationToken);

        return Ok(result);
    }
}