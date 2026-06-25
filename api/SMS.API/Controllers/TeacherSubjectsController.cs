using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs;
using SMS.Application.Features.TeacherSubjects.CreateTeacherSubject;
using SMS.Application.Features.TeacherSubjects.GetTeacherSubjects;
using SMS.Application.Features.TeacherSubjects.DeleteTeacherSubject;

namespace SMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeacherSubjectsController : ControllerBase
{
    private readonly CreateTeacherSubjectHandler _createTeacherSubjectHandler;
    private readonly GetTeacherSubjectsHandler _getTeacherSubjectsHandler;
    private readonly DeleteTeacherSubjectHandler _deleteTeacherSubjectHandler;

    public TeacherSubjectsController(
        CreateTeacherSubjectHandler createTeacherSubjectHandler,
        GetTeacherSubjectsHandler getTeacherSubjectsHandler,
        DeleteTeacherSubjectHandler deleteTeacherSubjectHandler)
    {
        _createTeacherSubjectHandler = createTeacherSubjectHandler;
        _getTeacherSubjectsHandler = getTeacherSubjectsHandler;
        _deleteTeacherSubjectHandler = deleteTeacherSubjectHandler;
    }

    [HttpPost]
    public async Task<ActionResult<TeacherSubjectResponseDto>> Create(
        CreateTeacherSubjectRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTeacherSubjectCommand(
            request.TeacherId,
            request.SubjectId);

        var result = await _createTeacherSubjectHandler
            .HandleAsync(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet("teacher/{teacherId}")]
    public async Task<ActionResult<List<TeacherSubjectListItemDto>>> GetByTeacher(
        Guid teacherId,
        CancellationToken cancellationToken)
    {
        var result = await _getTeacherSubjectsHandler
            .HandleAsync(teacherId, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _deleteTeacherSubjectHandler.HandleAsync(
            new DeleteTeacherSubjectCommand(id),
            cancellationToken);

        if (!result)
        {
            return NotFound("Teacher subject not found.");
        }

        return NoContent();
    }
}
