using Core.Exceptions;
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
        try
        {
            var course = await _mediator.Send(new GetCourseQuery(UserId, id), ct);
            return Ok(course);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("user-courses")]
    public async Task<IActionResult> GetForUser(CancellationToken ct)
    {
        var courses = await _mediator.Send(new GetUserCoursesQuery(UserId), ct);
        
        return Ok(new GetUserCoursesResponseModel
        {
            UserCourses = courses
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AddCourseRequestModel requestModel, CancellationToken ct)
    {
        try
        {
            var command = new AddCourseCommand(requestModel.Name, requestModel.Description, requestModel.Category, UserId);
        
            var courseId = await _mediator.Send(command, ct);
        
            return Ok(courseId);
        }
        catch (WrongOperationException)
        {
            return BadRequest("name can not be empty");
        }
    }
    
    [HttpPost("{publicId}/join")]
    public async Task<IActionResult> Join(string publicId, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new JoinCourseCommand(publicId, UserId), ct);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{id:int}/users")]
    public async Task<IActionResult> GetCourseUsers(int id, CancellationToken ct)
    {
        try
        {
            var courseUsers = await _mediator.Send(new GetCourseUsersQuery(UserId, id), ct);
            return Ok(courseUsers);
        }
        catch (EntityNotFoundException) 
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new DeleteCourseCommand(UserId, id), ct);
            return Ok();
        }
        catch (WrongOperationException)
        {
            return BadRequest("name can not be empty");
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpDelete("student-course/{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new DeleteStudentCommand(UserId, id), ct);
            return Ok();
        }
        catch (WrongOperationException)
        {
            return BadRequest("name can not be empty");
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("user-course/{id:int}/toggle-notifications")]
    public async Task<IActionResult> ToggleUserCourseNotification(int id, CancellationToken ct)
    {
        await _mediator.Send(new ToggleCourseNotificationsCommand(UserId, id), ct);
        return Ok();
    }
}