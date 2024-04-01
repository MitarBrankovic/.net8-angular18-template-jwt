using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TemplateBackend.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Bebac.Application.Common.Interfaces;

namespace TemplateBackend.Infrastructure.Services;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<BlobService> _logger;

    public BlobService(
        BlobServiceClient blobServiceClient,
        ILogger<BlobService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    private async Task<BlobContainerClient> GetBlobContainerClient(string containerName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        if (!await containerClient.ExistsAsync(cancellationToken: cancellationToken))
        {
            return null;
        }
        return containerClient;
    }

    public async Task<string> GetFileAsync(string fileName, string containerName, string folderName, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = await GetBlobContainerClient(containerName, cancellationToken);
            if (containerClient == null)
            {
                return null;
            }
            string blobName = $"{folderName}/{fileName}";
            var blobClient = containerClient.GetBlobClient(blobName);
            if (!await blobClient.ExistsAsync(cancellationToken: cancellationToken))
            {
                return null;
            }
            var blobUri = blobClient.Uri.ToString();
            return blobUri;

            // STRING BASE 64
            //var blobInfo = await blobClient.DownloadAsync(cancellationToken);
            //using (var ms = new MemoryStream())
            //{
            //    await blobInfo.Value.Content.CopyToAsync(ms, cancellationToken);
            //    byte[] byteArray = ms.ToArray();
            //    return Convert.ToBase64String(byteArray);
            //}
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<UploadFileResponse> UploadAsync(string containerName, string folderName, UploadRequest file, CancellationToken cancellationToken = default)
    {
        var containerClient = await GetBlobContainerClient(containerName, cancellationToken);
        if (containerClient == null)
        {
            return null;
        }
        string blobName = $"{folderName}/{file.FileName}";
        var fileClient = containerClient.GetBlobClient(blobName);

        if (await fileClient.ExistsAsync(cancellationToken: cancellationToken))
        {
            return null;
        }
        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            }
        };
        await fileClient.UploadAsync(file.File, options, cancellationToken);

        return new UploadFileResponse
        {
            Name = fileClient.Name,
            ContentType = file.ContentType,
            Uri = fileClient.Uri.AbsoluteUri
        };
    }

    public async Task<UploadFileResponse> UploadProfilePictureAsync(string profilePicture, string profilePictureFileName, string folderName, CancellationToken cancellationToken = default)
    {
        var imageBytes = Convert.FromBase64String(profilePicture);

        var imageStream = new MemoryStream();
        imageStream.Write(imageBytes);
        imageStream.Position = 0;

        var extension = Path.GetExtension(profilePictureFileName).TrimStart('.');

        return await UploadAsync("TemplateBackend", folderName, new UploadRequest(imageStream, profilePictureFileName, extension));
    }
}
