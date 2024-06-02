using Assignment.Commands;
using Assignment.Queries;
using BlobStorage.Models;
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
        var assignment = await _mediator.Send(new GetAssignmentQuery(UserId, id), ct);
        return Ok(assignment);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddAssignmentRequestModel requestModel, CancellationToken ct)
    {
        var command = new AddAssignmentCommand(requestModel.CourseId, requestModel.MaxGrade, requestModel.Title,
            requestModel.Description, UserId, requestModel.Deadline);

        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpPost("{assignmentId:int}/upload-attachment")]
    public async Task<IActionResult> Upload(int assignmentId, [FromForm] IFormFile file, CancellationToken ct)
    {
        if (file.Length is < 1 or > MaxFileSizeMb * BytesInMb)
        {
            return BadRequest();
        }
        
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
    
    [HttpPost("{id:int}/submit")]
    public async Task<IActionResult> Submit(int id, SubmitAssignmentRequestModel model, CancellationToken ct)
    {
        var command = new SubmitAssignmentCommand(UserId, id, model.Comment);
        await _mediator.Send(command, ct);

        return Ok();
    }
    
    [HttpPost("{assignmentId:int}/unsubmit")]
    public async Task<IActionResult> Unsubmit(int assignmentId, CancellationToken ct)
    {
        var command = new UnsubmitAssignmentCommand(UserId, assignmentId);
        await _mediator.Send(command, ct);

        return Ok();
    }
    
    [HttpDelete("attachment/{attachmentId:int}")]
    public async Task<IActionResult> DeleteAttachment(int attachmentId, CancellationToken ct)
    {
        var command = new DeleteStudentAssignmentAttachmentCommand(UserId, attachmentId);
        await _mediator.Send(command, ct);

        return Ok();
    }
    
    [HttpPost("{id:int}/student-user/{studentUserId:int}/grade")]
    public async Task<IActionResult> Grade(int id, int studentUserId, GradeAssignmentRequestModel model, CancellationToken ct)
    {
        var command = new GradeAssignmentCommand(UserId, studentUserId, id, model.Grade, model.GradingComment);
        await _mediator.Send(command, ct);

        return Ok();
    }
    
    [HttpGet("{id:int}/overview")]
    public async Task<IActionResult> Overview(int id, CancellationToken ct)
    {
        var overview = await _mediator.Send(new GetAssignmentOverviewQuery(UserId, id), ct);
        return Ok(overview);
    }
    
    [HttpGet("{id:int}/student-user/{studentUserId:int}/teacher-preview")]
    public async Task<IActionResult> TeacherPreview(int id, int studentUserId, CancellationToken ct)
    {
        var preview = await _mediator.Send(new GetAssignmentTeacherPreviewQuery(UserId, studentUserId, id), ct);
        return Ok(preview);
    }

    [HttpGet("attachment/{attachmentId:int}/file")]
    public async Task<IActionResult> GetAttachmentFile(int attachmentId, CancellationToken ct)
    {
        var attachmentFileModel = await _mediator.Send(new GetAssignmentAttachmentFileQuery(UserId, attachmentId), ct);
        return File(attachmentFileModel.BinaryData, attachmentFileModel.ContentType, attachmentFileModel.FileName);
    }
}