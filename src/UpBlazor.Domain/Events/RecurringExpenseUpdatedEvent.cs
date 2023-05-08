using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class RecurringExpenseUpdatedEvent : INotification
{
    public required RecurringExpense RecurringExpense { get; set; }
}