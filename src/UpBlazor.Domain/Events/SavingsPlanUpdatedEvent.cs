using MediatR;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Domain.Events;

public class SavingsPlanUpdatedEvent : INotification
{
    public required SavingsPlan SavingsPlan { get; set; }
}