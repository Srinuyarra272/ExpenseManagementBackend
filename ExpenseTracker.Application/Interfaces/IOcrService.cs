using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Interfaces;

public interface IOcrService
{
    Task<OcrResult> ParseReceiptAsync(IFormFile receiptImage);
}

public class OcrResult
{
    public decimal? Amount { get; set; }
    public DateTime? Date { get; set; }
    public string? Merchant { get; set; }
}
