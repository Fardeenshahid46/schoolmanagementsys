using SMS.Application.Features.Subjects.UpdateSubject;
using SMS.Application.Features.Subjects.DeleteSubject;
using SMS.Application.Features.Subjects.SearchSubjects;
using SMS.Application.Features.Subjects.GetSubjectById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs;
using SMS.Application.Features.Subjects.CreateSubject;
using SMS.Application.Features.Subjects.GetSubject;
namespace SMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubjectsController : ControllerBase
{
    private readonly CreateSubjectHandler _createSubjectHandler;
    private readonly GetSubjectHandler _getSubjectHandler;
    private readonly GetSubjectByIdHandler _getSubjectByIdHandler;
    private readonly DeleteSubjectHandler _deleteSubjectHandler;
    private readonly SearchSubjectHandler _searchSubjectHandler;
    private readonly UpdateSubjectHandler _updateSubjectHandler;

    public SubjectsController(
        CreateSubjectHandler createSubjectHandler,
        GetSubjectHandler getSubjectHandler,
        GetSubjectByIdHandler getSubjectByIdHandler,
        DeleteSubjectHandler deleteSubjectHandler,
        SearchSubjectHandler searchSubjectHandler,
        UpdateSubjectHandler updateSubjectHandler)    
    {
        _createSubjectHandler = createSubjectHandler;
        _getSubjectHandler=getSubjectHandler;
        _getSubjectByIdHandler=getSubjectByIdHandler;
        _deleteSubjectHandler=deleteSubjectHandler;
        _searchSubjectHandler=searchSubjectHandler;
        _updateSubjectHandler=updateSubjectHandler;
    }
    [HttpGet]
public async Task<ActionResult<List<SubjectListItemDto>>> GetAll(
    CancellationToken cancellationToken)
{
    var subjects = await _getSubjectHandler
        .HandleAsync(cancellationToken);

    return Ok(subjects);
}
[HttpGet("{id}")]
public async Task<ActionResult<SubjectDetailsDto>> GetById(
    Guid id,
    CancellationToken cancellationToken)
{
    var result = await _getSubjectByIdHandler
        .HandleAsync(id, cancellationToken);

    if (result == null)
    {
        return NotFound(new
        {
            error = "Subject not found."
        });
    }

    return Ok(result);
}
[HttpPut("{id}")]
public async Task<ActionResult<SubjectResponseDto>> Update(
    Guid id,
    UpdateSubjectRequestDto request,
    CancellationToken cancellationToken)
{
    var command = new UpdateSubjectCommand(
        id,
        request.Name,
        request.Code);

    var result = await _updateSubjectHandler
        .HandleAsync(command, cancellationToken);

    if (result == null)
    {
        return NotFound(new
        {
            error = "Subject not found."
        });
    }

    return Ok(result);
}
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(
    Guid id,
    CancellationToken cancellationToken)
{
    var result = await _deleteSubjectHandler.HandleAsync(
        new DeleteSubjectCommand(id),
        cancellationToken);

    if (!result)
    {
        return NotFound(new
        {
            error = "Subject not found."
        });
    }

    return Ok(new
    {
        message = "Subject deleted successfully."
    });
}
[HttpGet("search")]
public async Task<ActionResult<PagedSubjectResponseDto>> Search(
    [FromQuery] GetSubjectQuery query,
    CancellationToken cancellationToken)
{
    var result = await _searchSubjectHandler
        .HandleAsync(query, cancellationToken);

    return Ok(result);
}
    [HttpPost]
    public async Task<ActionResult<SubjectResponseDto>> Create(
        CreateSubjectRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateSubjectCommand(
            request.Name,
            request.Code);

        var result = await _createSubjectHandler
            .HandleAsync(command, cancellationToken);

        return Ok(result);
    }
}