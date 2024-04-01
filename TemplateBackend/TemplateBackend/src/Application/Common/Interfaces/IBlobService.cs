using TemplateBackend.Application.Common.Models;

namespace Bebac.Application.Common.Interfaces;
public interface IBlobService
{
    Task<UploadFileResponse> UploadAsync(string containerName, string folderName, UploadRequest file, CancellationToken cancellationToken = default);
    Task<string> GetFileAsync(string fileName, string containerName, string folderName, CancellationToken cancellationToken = default);

    Task<UploadFileResponse> UploadProfilePictureAsync(string profilePicture, string profilePictureFileName, string folderName, CancellationToken cancellationToken = default);
}
