using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string folderName);
}
