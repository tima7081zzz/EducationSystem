﻿using Core.Exceptions;
using DAL;
using Events;
using Events.Events;
using MediatR;

namespace Assignment.Commands;

public record AddAssignmentCommand(
    int CourseId,
    int? MaxGrade,
    string Title,
    string? Description,
    int UserId,
    DateTimeOffset Deadline) : IRequest<int>;

public class AddAssignmentCommandHandler : IRequestHandler<AddAssignmentCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRaiser _eventRaiser;

    public AddAssignmentCommandHandler(IUnitOfWork unitOfWork, IEventRaiser eventRaiser)
    {
        _unitOfWork = unitOfWork;
        _eventRaiser = eventRaiser;
    }

    public async Task<int> Handle(AddAssignmentCommand request, CancellationToken ct)
    {
        var (courseId, maxGrade, title, description, userId, dateTimeOffset) = request;

        var course = await _unitOfWork.CourseRepository.GetWithCourses(courseId, ct);
        EntityNotFoundException.ThrowIfNull(course);

        NotAllowedException.ThrowIf(course!.TeacherCourses.All(x => x.UserId != userId));

        var assignment = _unitOfWork.AssignmentRepository.Add(new DAL.Entities.Assignment
        {
            CourseId = courseId,
            MaxGrade = maxGrade,
            Title = title,
            Description = description,
            CreatorTeacherId = userId,
            Deadline = dateTimeOffset,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await _unitOfWork.SaveChanges(ct);

        await _eventRaiser.Raise(new AssignmentAddedEvent(new AssignmentAddedEventArgs(assignment.Id)), ct);

        return assignment.Id;
    }
}