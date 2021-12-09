using System;

namespace UpBlazor.Core.Models;

public class Notification
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
}