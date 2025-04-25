using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? UserId { get; set; }

    public int RoomId { get; set; }

    public bool IsCancelled { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual AspNetUser? User { get; set; }
}
