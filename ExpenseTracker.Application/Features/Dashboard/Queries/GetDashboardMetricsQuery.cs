using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Dashboard.Queries;

public class GetDashboardMetricsQuery : IRequest<DashboardMetricsDto>
{
    public string UserId { get; set; } = string.Empty; // For future auth filtering
    public int? Month { get; set; }
    public int? Year { get; set; }
}
