using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs;
using SMS.Application.Features.StudentClasses.CreateStudentClass;
using SMS.Application.Features.StudentClasses.GetStudentClasses;
using SMS.Application.Features.StudentClasses.DeleteStudentClass;
using SMS.Application.Features.StudentClasses.GetStudentClassById;
using SMS.Application.Features.StudentClasses.SearchStudentClasses;

namespace SMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentClassesController : ControllerBase
{
    private readonly CreateStudentClassHandler _createStudentClassHandler;
    private readonly GetStudentClassesHandler _getStudentClassesHandler;
    private readonly DeleteStudentClassHandler _deleteStudentClassHandler;
    private readonly GetStudentClassByIdHandler _getStudentClassByIdHandler;
    private readonly SearchStudentClassHandler _searchStudentClassHandler;

    public StudentClassesController(
        CreateStudentClassHandler createStudentClassHandler,
        GetStudentClassesHandler getStudentClassesHandler,
        DeleteStudentClassHandler deleteStudentClassHandler,
        GetStudentClassByIdHandler getStudentClassByIdHandler,
        SearchStudentClassHandler searchStudentClassHandler)
    {
        _createStudentClassHandler = createStudentClassHandler;
        _getStudentClassesHandler = getStudentClassesHandler;
        _deleteStudentClassHandler = deleteStudentClassHandler;
        _getStudentClassByIdHandler = getStudentClassByIdHandler;
        _searchStudentClassHandler = searchStudentClassHandler;
    }

[HttpPost]
public async Task<ActionResult<StudentClassResponseDto>> Create(
    [FromBody] CreateStudentClassRequestDto request,
    CancellationToken cancellationToken)
{
    var command = new CreateStudentClassCommand(
        request.StudentId,
        request.ClassId);

    var result = await _createStudentClassHandler
        .HandleAsync(command, cancellationToken);

    return Ok(result);
}
    [HttpGet("search")]
    public async Task<ActionResult<PagedStudentClassResponseDto>> Search(
        [FromQuery] GetStudentClassQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _searchStudentClassHandler
            .HandleAsync(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<List<StudentClassListItemDto>>> GetByStudent(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var result = await _getStudentClassesHandler
            .HandleAsync(studentId, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentClassDetailsDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _getStudentClassByIdHandler.HandleAsync(
            id,
            cancellationToken);

        if (result == null)
        {
            return NotFound(new
            {
                error = "Student class assignment not found."
            });
        }

        return Ok(result);
    }
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(
    Guid id,
    CancellationToken cancellationToken)
{
    var result = await _deleteStudentClassHandler.HandleAsync(
        new DeleteStudentClassCommand(id),
        cancellationToken);

    if (!result)
    {
        return NotFound("Student class assignment not found.");
    }

    return Ok(new
    {
        message = "Student class deleted successfully."
    });
}
}
