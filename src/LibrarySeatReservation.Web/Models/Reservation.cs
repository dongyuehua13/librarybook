using System.ComponentModel.DataAnnotations;

namespace LibrarySeatReservation.Web.Models;

public class Reservation
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int SeatId { get; set; }
    public Seat Seat { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "已预约";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
