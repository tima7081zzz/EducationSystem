using MediatR;
using Microsoft.AspNetCore.Mvc;
using Settings.Commands;
using Settings.Queries;
using Web.Attributes;
using Web.Models.RequestModels;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SettingsController : BaseController
{
    private readonly IMediator _mediator;

    public SettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var userSettings = await _mediator.Send(new GetUserSettingsQuery(UserId), ct);
        return Ok(userSettings);
    }

    [HttpPut("notifications")]
    public async Task<IActionResult> Update(UserNotificationsRequestModel model, CancellationToken ct)
    {
        await _mediator.Send(new UpdateUserSettingsCommand(UserId, new UserNotificationsModel
        {
            IsEnabled = model.IsEnabled,
            NewAssignmentEnabled = model.NewAssignmentEnabled,
            DeadlineReminderEnabled = model.DeadlineReminderEnabled,
            GradingAssignmentEnabled = model.GradingAssignmentEnabled,
            AssignmentSubmittedEnabled = model.AssignmentSubmittedEnabled,
        }), ct);

        return Ok();
    }
}