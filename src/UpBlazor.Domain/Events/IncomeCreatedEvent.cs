using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class IncomeCreatedEvent : INotification
{
    public required Income Income { get; set; }
}