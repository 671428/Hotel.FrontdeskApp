using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Task
{
    public int Id { get; set; }

    public string TaskType { get; set; } = null!;

    public int RoomNumber { get; set; }

    public string TaskStatus { get; set; } = null!;

    public string? TaskNotes { get; set; }

    public DateTime? CreatedAt { get; set; }
}
