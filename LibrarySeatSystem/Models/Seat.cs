using System.ComponentModel.DataAnnotations;

namespace LibrarySeatSystem.Models;

public class Seat
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string SeatNumber { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }

    public int Floor { get; set; } = 1;

    public string Area { get; set; } = "自习区";

    public bool IsActive { get; set; } = true;

    public List<Reservation> Reservations { get; set; } = new();
}
