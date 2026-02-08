using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;
using MongoDB.Driver;

namespace ExpenseTracker.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly ExpenseTrackerContext _context;

    public BillRepository(ExpenseTrackerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(string userId)
    {
        return await _context.Bills.Find(b => b.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Bill>> GetUpcomingAsync(string userId, DateTime fromDate, DateTime toDate)
    {
        return await _context.Bills.Find(b => 
            b.UserId == userId && 
            b.IsActive && 
            !b.IsPaid &&
            b.DueDate >= fromDate && 
            b.DueDate <= toDate
        ).ToListAsync();
    }

    public async Task<Bill?> GetAsync(string id)
    {
        return await _context.Bills.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Bill bill)
    {
        await _context.Bills.InsertOneAsync(bill);
    }

    public async Task UpdateAsync(Bill bill)
    {
        await _context.Bills.ReplaceOneAsync(b => b.Id == bill.Id, bill);
    }

    public async Task DeleteAsync(string id)
    {
        await _context.Bills.DeleteOneAsync(b => b.Id == id);
    }
}
