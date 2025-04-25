using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int NumberOfBeds { get; set; }

    public string RoomSize { get; set; } = null!;

    public string Beds { get; set; } = null!;

    public int RoomNumber { get; set; }

    public bool IsSelected { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
