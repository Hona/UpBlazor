using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class ExpenseDeletedEvent : INotification
{
    public required Expense Expense { get; set; }
}