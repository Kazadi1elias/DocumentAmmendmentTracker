using DocumentAmendmentTracker.Web.Options;
using Microsoft.Extensions.Options;

namespace DocumentAmendmentTracker.Web.Services;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageOptions _options;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(IOptions<FileStorageOptions> options, ILogger<FileStorageService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public void ValidateFile(string fileName, long sizeBytes)
    {
        if (sizeBytes <= 0)
        {
            throw new InvalidOperationException($"'{fileName}' is empty.");
        }

        if (sizeBytes > _options.MaxFileSizeBytes)
        {
            var maxMb = _options.MaxFileSizeBytes / (1024 * 1024);
            throw new InvalidOperationException($"'{fileName}' exceeds the maximum allowed size of {maxMb} MB.");
        }

        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(extension) ||
            !_options.AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"'{fileName}' has an unsupported file type. Allowed types: {string.Join(", ", _options.AllowedExtensions)}");
        }
    }

    public string BuildStoredPath(int recordId, int amendmentId, string fileName)
    {
        var safeName = Path.GetFileName(fileName);
        var uniqueName = $"{Guid.NewGuid():N}_{safeName}";
        return Path.Combine(_options.RootPath, recordId.ToString(), amendmentId.ToString(), uniqueName);
    }

    public async Task SaveFileAsync(Stream content, string storedPath, CancellationToken cancellationToken = default)
    {
        var directory = Path.GetDirectoryName(storedPath)
            ?? throw new InvalidOperationException($"Could not determine directory for '{storedPath}'.");

        var attempt = 0;
        while (true)
        {
            attempt++;
            try
            {
                Directory.CreateDirectory(directory);

                await using var fileStream = new FileStream(
                    storedPath, FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 81920, useAsync: true);

                await content.CopyToAsync(fileStream, cancellationToken);
                return;
            }
            catch (IOException ex) when (attempt < _options.CopyRetryCount)
            {
                _logger.LogWarning(ex,
                    "Attempt {Attempt} to write attachment to network share failed for {Path}. Retrying.",
                    attempt, storedPath);
                await Task.Delay(_options.CopyRetryDelayMilliseconds * attempt, cancellationToken);
            }
        }
    }

    public void TryDeleteFile(string storedPath)
    {
        try
        {
            if (File.Exists(storedPath))
            {
                File.Delete(storedPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to clean up orphaned file at {Path} after a rolled-back save.", storedPath);
        }
    }
}
