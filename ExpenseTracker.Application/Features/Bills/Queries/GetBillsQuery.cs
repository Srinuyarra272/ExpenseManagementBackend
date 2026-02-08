using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Queries;

public class GetBillsQuery : IRequest<List<BillDto>>
{
}
