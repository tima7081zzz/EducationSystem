using Core.Exceptions;
using Course.Commands;
using Course.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.RequestModels;
using Web.Models.ResponseModels;

namespace Web.Controllers;

[Route("course")]
[Authorize]
public class CourseController : BaseController
{
    private readonly IMediator _mediator;

    public CourseController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var course = await _mediator.Send(new GetCourseQuery(UserId, id));
            return Ok(course);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("user-courses")]
    public async Task<IActionResult> GetForUser()
    {
        var courses = await _mediator.Send(new GetUserCoursesQuery(UserId));
        
        return Ok(new GetUserCoursesResponseModel
        {
            UserCourses = courses
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AddCourseRequestModel requestModel)
    {
        var command = new AddCourseCommand(requestModel.Name, requestModel.Description, requestModel.Category, UserId);
        
        var courseId = await _mediator.Send(command);
        
        return Ok(courseId);
    }
    
    [HttpPost("{publicId}/join")]
    public async Task<IActionResult> Join(string publicId)
    {
        try
        {
            await _mediator.Send(new JoinCourseCommand(publicId, UserId));
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
}