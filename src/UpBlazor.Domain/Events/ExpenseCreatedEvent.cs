using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class ExpenseCreatedEvent : INotification
{
    public required Expense Expense { get; set; }
}