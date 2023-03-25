using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class RecurringExpenseDeletedEvent : INotification
{
    public required RecurringExpense RecurringExpense { get; set; }
}