using Course.Commands;
using Course.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Web.Models.RequestModels;
using Web.Models.ResponseModels;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CourseController : BaseController
{
    private readonly IMediator _mediator;

    public CourseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var course = await _mediator.Send(new GetCourseQuery(UserId, id), ct);
        return Ok(course);
    }

    [HttpGet("user-courses")]
    public async Task<IActionResult> GetForUser(CancellationToken ct)
    {
        var courses = await _mediator.Send(new GetUserCoursesQuery(UserId), ct);
        return Ok(new GetUserCoursesResponseModel {UserCourses = courses});
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddCourseRequestModel requestModel, CancellationToken ct)
    {
        var command = new AddCourseCommand(requestModel.Name, requestModel.Description, requestModel.Category, UserId);
        var courseId = await _mediator.Send(command, ct);

        return Ok(courseId);
    }

    [HttpPost("{publicId}/join")]
    public async Task<IActionResult> Join(string publicId, CancellationToken ct)
    {
        await _mediator.Send(new JoinCourseCommand(publicId, UserId), ct);
        return Ok();
    }

    [HttpGet("{id:int}/users")]
    public async Task<IActionResult> GetCourseUsers(int id, CancellationToken ct)
    {
        var courseUsers = await _mediator.Send(new GetCourseUsersQuery(UserId, id), ct);
        return Ok(courseUsers);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteCourseCommand(UserId, id), ct);
        return Ok();
    }

    [HttpDelete("student-course/{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteStudentCommand(UserId, id), ct);
        return Ok();
    }

    [HttpPut("user-course/{id:int}/toggle-notifications")]
    public async Task<IActionResult> ToggleUserCourseNotification(int id, CancellationToken ct)
    {
        await _mediator.Send(new ToggleCourseNotificationsCommand(UserId, id), ct);
        return Ok();
    }
}