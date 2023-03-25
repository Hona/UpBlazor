using System;

namespace UpBlazor.Domain.Entities;

public class NotificationRead
{
    public string Id => UserId + NotificationId;
    public string UserId { get; set; }
    public Guid NotificationId { get; set; }
}