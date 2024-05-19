using Assignment.Commands;
using BlobStorage;
using Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Models.RequestModels;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssignmentController : BaseController
{
    private readonly IMediator _mediator;

    private const int MaxFileSizeMb = 10;
    private const int BytesInMb = 1048576;

    public AssignmentController(IMediator mediator)
    {
        _mediator = mediator;
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

    [HttpPost("/{assignmentId:int}/upload-attachment")]
    public async Task<IActionResult> Upload(int assignmentId, IFormFile file, CancellationToken ct)
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
    
    [HttpPost("/{assignmentId:int}/submit")]
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