using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class IncomeDeletedEvent : INotification
{
    public required Income Income { get; set; }
}