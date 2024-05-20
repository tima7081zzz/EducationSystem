﻿using BlobStorage;
using BlobStorage.Models;
using Core.Exceptions;
using DAL;
using DAL.Entities;
using MediatR;

namespace Assignment.Commands;

public record AddAssignmentStudentAttachmentCommand(int AssignmentId, int UserId, BlobFileBase BlobFileBase) : IRequest<Uri>;

public class AddAssignmentStudentAttachmentCommandHandler : IRequestHandler<AddAssignmentStudentAttachmentCommand, Uri>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageManager _blobManager; 

    public AddAssignmentStudentAttachmentCommandHandler(IUnitOfWork unitOfWork, IBlobStorageManager blobManager)
    {
        _unitOfWork = unitOfWork;
        _blobManager = blobManager;
    }

    public async Task<Uri> Handle(AddAssignmentStudentAttachmentCommand request, CancellationToken ct)
    {
        await ValidateOperation(request, ct);

        var blobInfo = await _blobManager.Upload(request.BlobFileBase, ct);

        await using var transaction = _unitOfWork.BeginTransaction();

        var studentAssignment = _unitOfWork.StudentAssignmentRepository.Add(new StudentAssignment
        {
            UserId = request.UserId,
            AssignmentId = request.AssignmentId,
            Status = StudentCourseTaskStatus.NotSubmitted,
        });

        _unitOfWork.StudentAssignmentAttachmentRepository.Add(new StudentAssignmentAttachment
        {
            StudentAssignmentId = studentAssignment.Id,
            StudentUserId = request.UserId,
            BlobName = blobInfo.BlobName,
        });

        await _unitOfWork.SaveChanges(ct);
        await transaction.CommitAsync(ct);
        
        return blobInfo.Uri;
    }

    private async Task ValidateOperation(AddAssignmentStudentAttachmentCommand request, CancellationToken ct)
    {
        var assignment = await _unitOfWork.AssignmentRepository.Get(request.AssignmentId, ct);
        if (assignment is null)
        {
            throw new EntityNotFoundException();
        }

        if (assignment.CreatorTeacherId == request.UserId)
        {
            throw new WrongOperationException();
        }

        var studentCourse = await _unitOfWork.StudentCourseRepository.Get(request.UserId, assignment.CourseId, ct);
        if (studentCourse is null)
        {
            throw new WrongOperationException();
        }
    }
}