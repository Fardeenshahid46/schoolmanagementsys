using SMS.Application.Features.Students.SearchStudents;
using SMS.Application.Features.Students.DeleteStudent;
using SMS.Application.Features.Students.UpdateStudent;
using SMS.Application.Features.Students.GetStudentById;
using SMS.Application.Features.Students.GetStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs;
using SMS.Application.Features.Students.CreateStudent;

namespace SMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
 private readonly CreateStudentHandler _createStudentHandler;
 private readonly GetStudentHandler _getStudentHandler;
 private readonly GetStudentByIdHandler _getStudentByIdHandler;
 private readonly UpdateStudentHandler _updateStudentHandler;
 private readonly DeleteStudentHandler _deleteStudentHandler;
 private readonly SearchStudentHandler _searchStudentHandler;
public StudentsController(
    CreateStudentHandler createStudentHandler,
    GetStudentHandler getStudentHandler,
    GetStudentByIdHandler getStudentByIdHandler,
    UpdateStudentHandler updateStudentHandler,
    DeleteStudentHandler deleteStudentHandler,
    SearchStudentHandler searchStudentHandler)
{
    _createStudentHandler = createStudentHandler;
    _getStudentHandler = getStudentHandler;
    _getStudentByIdHandler = getStudentByIdHandler;
    _updateStudentHandler = updateStudentHandler;
    _deleteStudentHandler = deleteStudentHandler;
    _searchStudentHandler = searchStudentHandler;
} 
[HttpGet("search")]
public async Task<ActionResult<PagedStudentResponseDto>> Search(
    [FromQuery] GetStudentQuery query,
    CancellationToken cancellationToken)
{
    var result = await _searchStudentHandler
        .HandleAsync(query, cancellationToken);

    return Ok(result);
}

[HttpDelete("{id}")]
public async Task<IActionResult> Delete(
    Guid id,
    CancellationToken cancellationToken)
{
    var command = new DeleteStudentCommand(id);


    var result = await _deleteStudentHandler
        .HandleAsync(command, cancellationToken);


    if (!result)
    {
        return NotFound(new
        {
            error = "Student not found."
        });
    }


    return Ok(new
    {
        message = "Student deleted successfully."
    });
}
[HttpPut("{id}")]
public async Task<IActionResult> Update(
    Guid id,
    UpdateStudentRequestDto request,
    CancellationToken cancellationToken)
{
    var command = new UpdateStudentCommand(
        id,
        request.FirstName,
        request.LastName,
        request.Gender,
        request.DateOfBirth,
        request.GuardianName,
        request.GuardianPhone,
        request.Address
    );


    var result = await _updateStudentHandler
        .HandleAsync(command, cancellationToken);


    if (!result)
    {
        return NotFound(new
        {
            error = "Student not found."
        });
    }


    return Ok(new
    {
        message = "Student updated successfully."
    });
}
[HttpGet("{id}")]
public async Task<ActionResult<StudentDetailsDto>> GetById(
    Guid id,
    CancellationToken cancellationToken)
{
    var result = await _getStudentByIdHandler.HandleAsync(
        id,
        cancellationToken);


    if(result == null)
    {
        return NotFound(new
        {
            error = "Student not found."
        });
    }


    return Ok(result);
}
[HttpGet]
public async Task<ActionResult<List<StudentListItemDto>>> GetAll(
    CancellationToken cancellationToken)
{
    var students = await _getStudentHandler
        .HandleAsync(cancellationToken);

    return Ok(students);
}
    [HttpPost]
    public async Task<ActionResult<StudentResponseDto>> Create(
        CreateStudentRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateStudentCommand(
            request.FirstName,
            request.LastName,
            request.Gender,
            request.DateOfBirth,
            request.AdmissionNumber,
            request.GuardianName,
            request.GuardianPhone,
            request.Address);

        var result = await _createStudentHandler.HandleAsync(
            command,
            cancellationToken);

        return Ok(result);
    }
}