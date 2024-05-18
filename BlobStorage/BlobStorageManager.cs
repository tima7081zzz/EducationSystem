using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorage.Exceptions;
using Core.Helpers;
using Microsoft.Extensions.Options;

namespace BlobStorage;

public interface IBlobStorageManager
{
    Task<BlobInfo> Upload(BlobFileBase file, CancellationToken ct);
}

public class BlobStorageManager : IBlobStorageManager
{
    private readonly BlobContainerClient _containerClient;

    public BlobStorageManager(BlobContainerType containerType, IOptionsSnapshot<BlobOptions> blobOptions)
    {
        var containerName = EnumUtils.GetEnumValueDescription(containerType);

        var blobServiceClient = new BlobServiceClient(new Uri(blobOptions.Value.BlobConnectionString));
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
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

    private static string SanitizeBlobName(string fileName)
    {
        return string.IsNullOrEmpty(fileName) ? fileName : new string(fileName.Where(char.IsLetterOrDigit).ToArray());
    }
}