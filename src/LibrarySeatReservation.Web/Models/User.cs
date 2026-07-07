using System.ComponentModel.DataAnnotations;

namespace LibrarySeatReservation.Web.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }

    public List<Reservation> Reservations { get; set; } = new();
}
