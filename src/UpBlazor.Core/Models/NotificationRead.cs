using System;

namespace UpBlazor.Core.Models;

public class NotificationRead
{
    public string Id => UserId + NotificationId;
    public string UserId { get; set; }
    public Guid NotificationId { get; set; }
}