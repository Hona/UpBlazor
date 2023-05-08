using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class SavingsPlanCreatedEvent : INotification
{
    public required SavingsPlan SavingsPlan { get; set; }
}