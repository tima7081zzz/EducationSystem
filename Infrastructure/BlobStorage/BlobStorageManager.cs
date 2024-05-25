using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorage.Exceptions;
using BlobStorage.Models;
using Core.Helpers;
using Microsoft.Extensions.Options;
using BlobInfo = BlobStorage.Models.BlobInfo;

namespace BlobStorage;

public interface IBlobStorageManager
{
    Task<BlobInfo> Upload(BlobFileBase file, CancellationToken ct);
    Task<bool> Delete(string blobName, CancellationToken ct);
}

public class BlobStorageManager : IBlobStorageManager
{
    private readonly BlobContainerClient _containerClient;

    public BlobStorageManager(IOptionsSnapshot<BlobOptions> blobOptions)
    {
        var blobServiceClient = new BlobServiceClient(blobOptions.Value.BlobConnectionString);
        _containerClient =
            blobServiceClient.GetBlobContainerClient(blobOptions.Value.StudentAssignmentAttachmentsContainerName);
    }

    public async Task<BlobInfo> Upload(BlobFileBase file, CancellationToken ct)
    {
        var uniqueId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        var fileName = $"{uniqueId}{file.FileName}";
        var blobName = SanitizeBlobName(fileName);

        if (string.IsNullOrEmpty(blobName))
        {
            throw new InvalidBlobNameException("Blob name is empty after sanitizing");
        }

        var blobClient = _containerClient.GetBlobClient(blobName);
        var headers = new BlobHttpHeaders {ContentType = file.ContentType};
        await blobClient.UploadAsync(file.InputStream, headers, cancellationToken: ct);

        return new BlobInfo
        {
            BlobName = blobClient.Name,
            Uri = blobClient.Uri,
        };
    }

    public async Task<bool> Delete(string blobName, CancellationToken ct)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        return await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
    }

    private static string SanitizeBlobName(string fileName)
    {
        return string.IsNullOrEmpty(fileName) ? fileName : new string(fileName.Where(char.IsLetterOrDigit).ToArray());
    }
}