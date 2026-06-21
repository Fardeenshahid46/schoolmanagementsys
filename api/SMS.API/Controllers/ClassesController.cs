using SMS.Application.Features.Classes.SearchClasses;
using SMS.Application.Features.Classes.DeleteClass;
using SMS.Application.Features.Classes.UpdateClass;
using SMS.Application.Features.Classes.GetClassById;
using SMS.Application.Features.Classes.GetClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs;
using SMS.Application.Features.Classes.CreateClass;

namespace SMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassesController : ControllerBase
{
    private readonly CreateClassHandler _createClassHandler;
    private readonly GetClassHandler _getClassHandler;
    private readonly GetClassByIdHandler _getClassByIdHandler;
    private readonly UpdateClassHandler _updateClassHandler;
    private readonly DeleteClassHandler _deleteClassHandler;
    private readonly SearchClassHandler _searchClassHandler;

    public ClassesController(
        CreateClassHandler createClassHandler,
        GetClassHandler getClassHandler,
        GetClassByIdHandler getClassByIdHandler,
        UpdateClassHandler updateClassHandler,
        DeleteClassHandler deleteClassHandler,
        SearchClassHandler searchClassHandler)
    {
        _createClassHandler = createClassHandler;
        _getClassHandler = getClassHandler;
        _getClassByIdHandler=getClassByIdHandler;
        _updateClassHandler=updateClassHandler;
        _deleteClassHandler=deleteClassHandler;
        _searchClassHandler=searchClassHandler;
    }
[HttpGet]
public async Task<ActionResult<List<ClassListItemDto>>> GetAll(
    CancellationToken cancellationToken)
{
    var classes = await _getClassHandler
        .HandleAsync(cancellationToken);

    return Ok(classes);
}
[HttpGet("search")]
public async Task<ActionResult<PagedClassResponseDto>> Search(
    [FromQuery] GetClassQuery query,
    CancellationToken cancellationToken)
{
    var result = await _searchClassHandler
        .HandleAsync(query, cancellationToken);

    return Ok(result);
}
[HttpGet("{id}")]
public async Task<ActionResult<ClassDetailsDto>> GetById(
    Guid id,
    CancellationToken cancellationToken)
{
    var result = await _getClassByIdHandler
        .HandleAsync(id, cancellationToken);

    if (result == null)
    {
        return NotFound(new
        {
            error = "Class not found."
        });
    }

    return Ok(result);
}
[HttpPut("{id}")]
public async Task<IActionResult> Update(
    Guid id,
    UpdateClassRequestDto request,
    CancellationToken cancellationToken)
{
    var command = new UpdateClassCommand(
        id,
        request.Name,
        request.Section);

    var result = await _updateClassHandler
        .HandleAsync(command, cancellationToken);

    if (!result)
    {
        return NotFound(new
        {
            error = "Class not found."
        });
    }

    return Ok(new
    {
        message = "Class updated successfully."
    });
}
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(
    Guid id,
    CancellationToken cancellationToken)
{
    var command = new DeleteClassCommand(id);

    var result = await _deleteClassHandler
        .HandleAsync(command, cancellationToken);

    if (!result)
    {
        return NotFound(new
        {
            error = "Class not found."
        });
    }

    return Ok(new
    {
        message = "Class deleted successfully."
    });
}
    [HttpPost]
    public async Task<ActionResult<ClassResponseDto>> Create(
        CreateClassRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateClassCommand(
            request.Name,
            request.Section);

        var result = await _createClassHandler.HandleAsync(
            command,
            cancellationToken);

        return Ok(result);
    }
}