using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class IncomeUpdatedEvent : INotification
{
    public required Income Income { get; set; }
}