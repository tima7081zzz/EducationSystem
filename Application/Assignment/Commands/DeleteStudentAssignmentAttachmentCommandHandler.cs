using BlobStorage;
using Core.Exceptions;
using DAL;
using MediatR;

namespace Assignment.Commands;

public record DeleteStudentAssignmentAttachmentCommand(int UserId, int AttachmentId) : IRequest;

public class DeleteStudentAssignmentAttachmentCommandHandler : IRequestHandler<DeleteStudentAssignmentAttachmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageManager _blobStorageManager;

    public DeleteStudentAssignmentAttachmentCommandHandler(IUnitOfWork unitOfWork, IBlobStorageManager blobStorageManager)
    {
        _unitOfWork = unitOfWork;
        _blobStorageManager = blobStorageManager;
    }

    public async Task Handle(DeleteStudentAssignmentAttachmentCommand request, CancellationToken ct)
    {
        var (userId, attachmentId) = request;
        
        var attachment = await _unitOfWork.StudentAssignmentAttachmentRepository.Get(attachmentId, ct);
        
        EntityNotFoundException.ThrowIf(attachment is null || attachment.StudentUserId != userId);

        await _blobStorageManager.Delete(attachment!.BlobName, ct);
        await _unitOfWork.StudentAssignmentAttachmentRepository.Delete(attachment.Id, ct);
    }
}