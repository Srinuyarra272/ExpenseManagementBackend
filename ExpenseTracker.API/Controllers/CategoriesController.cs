using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _repository;

    public CategoriesController(ICategoryRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var categories = await _repository.GetAllAsync();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create([FromBody] Category category)
    {
        category.Id = Guid.NewGuid().ToString();
        await _repository.AddAsync(category);
        return CreatedAtAction(nameof(GetAll), new { id = category.Id }, category);
    }

    [HttpPost("seed-missing")]
    public async Task<ActionResult> SeedMissing()
    {
        var existingCategories = await _repository.GetAllAsync();
        var existingNames = existingCategories.Select(c => c.Name).ToHashSet();

        var defaultCategories = new List<Category>
        {
            new Category { Id = Guid.NewGuid().ToString(), Name = "Food", Icon = "restaurant", Color = "#EF4444", Type = TransactionType.Expense, UserId = "" },
            new Category { Id = Guid.NewGuid().ToString(), Name = "Salary", Icon = "payments", Color = "#10B981", Type = TransactionType.Income, UserId = "" },
            new Category { Id = Guid.NewGuid().ToString(), Name = "Rent", Icon = "home", Color = "#3B82F6", Type = TransactionType.Expense, UserId = "" },
            new Category { Id = Guid.NewGuid().ToString(), Name = "Shopping", Icon = "shopping_cart", Color = "#F59E0B", Type = TransactionType.Expense, UserId = "" },
            new Category { Id = Guid.NewGuid().ToString(), Name = "Transport", Icon = "directions_car", Color = "#8B5CF6", Type = TransactionType.Expense, UserId = "" },
            new Category { Id = Guid.NewGuid().ToString(), Name = "Entertainment", Icon = "movie", Color = "#EC4899", Type = TransactionType.Expense, UserId = "" },
            new Category { Id = Guid.NewGuid().ToString(), Name = "Health", Icon = "medical_services", Color = "#14B8A6", Type = TransactionType.Expense, UserId = "" },
            new Category { Id = Guid.NewGuid().ToString(), Name = "Utilities", Icon = "bolt", Color = "#06B6D4", Type = TransactionType.Expense, UserId = "" }
        };

        var missingCategories = defaultCategories.Where(c => !existingNames.Contains(c.Name)).ToList();

        foreach (var category in missingCategories)
        {
            await _repository.AddAsync(category);
        }

        return Ok(new { added = missingCategories.Count, categories = missingCategories.Select(c => c.Name) });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}

