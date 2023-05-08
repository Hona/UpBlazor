using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class RecurringExpenseCreatedEvent : INotification
{
    public required RecurringExpense RecurringExpense { get; set; }
}