using Assignment.Commands;
using Assignment.Queries;
using BlobStorage.Models;
using Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Web.Models.RequestModels;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AssignmentController : BaseController
{
    private readonly IMediator _mediator;

    private const int MaxFileSizeMb = 10;
    private const int BytesInMb = 1048576;

    public AssignmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        try
        {
            var assignment = await _mediator.Send(new GetAssignmentQuery(UserId, id), ct);
            return Ok(assignment);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddAssignmentRequestModel requestModel, CancellationToken ct)
    {
        try
        {
            var command = new AddAssignmentCommand(requestModel.CourseId, requestModel.MaxGrade, requestModel.Title,
                requestModel.Description, UserId, requestModel.Deadline);

            await _mediator.Send(command, ct);

            return Ok();
        }
        catch (WrongOperationException)
        {
            return Forbid();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{assignmentId:int}/upload-attachment")]
    public async Task<IActionResult> Upload(int assignmentId, [FromForm] IFormFile file, CancellationToken ct)
    {
        if (file.Length is < 1 or > MaxFileSizeMb * BytesInMb)
        {
            return BadRequest();
        }
        
        try
        {
            var blobFileBase = new BlobFileBase
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                InputStream = file.OpenReadStream(),
            };

            var command = new AddAssignmentStudentAttachmentCommand(assignmentId, UserId, blobFileBase);
            var uri = await _mediator.Send(command, ct);

            return Ok(uri.AbsoluteUri);
        }
        catch (WrongOperationException)
        {
            return Forbid();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpPost("{assignmentId:int}/submit")]
    public async Task<IActionResult> Submit(int assignmentId, CancellationToken ct)
    {
        try
        {
            var command = new SubmitAssignmentCommand(UserId, assignmentId);
            await _mediator.Send(command, ct);

            return Ok();
        }
        catch (WrongOperationException)
        {
            return Forbid();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
}