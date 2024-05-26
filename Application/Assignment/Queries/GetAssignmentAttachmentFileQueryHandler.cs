using BlobStorage;
using Core.Exceptions;
using DAL;
using MediatR;

namespace Assignment.Queries;

public record GetAssignmentAttachmentFileQuery(int UserId, int AssignmentAttachmentId) : IRequest<AttachmentFileModel>;

public class GetAssignmentAttachmentFileQueryHandler : IRequestHandler<GetAssignmentAttachmentFileQuery, AttachmentFileModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageManager _blobStorageManager;

    public GetAssignmentAttachmentFileQueryHandler(IUnitOfWork unitOfWork, IBlobStorageManager blobStorageManager)
    {
        _unitOfWork = unitOfWork;
        _blobStorageManager = blobStorageManager;
    }

    public async Task<AttachmentFileModel> Handle(GetAssignmentAttachmentFileQuery request, CancellationToken ct)
    {
        var assignmentAttachment =
            await _unitOfWork.StudentAssignmentAttachmentRepository.GetWithAssignment(request.AssignmentAttachmentId, ct);
        EntityNotFoundException.ThrowIfNull(assignmentAttachment);

        if (assignmentAttachment!.StudentUserId != request.UserId)
        {
            var assignmentId = assignmentAttachment.StudentAssignment.AssignmentId;
            var isTeacher = await _unitOfWork.AssignmentRepository.IsTeacherForAssignment(request.UserId, assignmentId, ct);
            
            WrongOperationException.ThrowIf(!isTeacher);
        }

        var blobRaw = await _blobStorageManager.Download(assignmentAttachment.BlobName, ct);
        EntityNotFoundException.ThrowIfNull(blobRaw);

        return new AttachmentFileModel
        {
            BinaryData = blobRaw!.BinaryData,
            ContentType = blobRaw.ContentType,
            FileName = assignmentAttachment.FileName,
        };
    }
}

public class AttachmentFileModel
{
    public byte[] BinaryData { get; set; } = [];
    public required string ContentType { get; set; }
    public required string FileName { get; set; }
}

