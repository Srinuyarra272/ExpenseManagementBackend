using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Infrastructure.Services;

public class MockOcrService : IOcrService
{
    public Task<OcrResult> ParseReceiptAsync(IFormFile receiptImage)
    {
        // Mock implementation
        return Task.FromResult(new OcrResult
        {
            Amount = 150.00m,
            Date = DateTime.UtcNow,
            Merchant = "Mock Merchant"
        });
    }
}
