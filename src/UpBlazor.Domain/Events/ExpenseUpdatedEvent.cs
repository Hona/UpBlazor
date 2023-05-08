using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class ExpenseUpdatedEvent : INotification
{
    public required Expense Expense { get; set; }
}