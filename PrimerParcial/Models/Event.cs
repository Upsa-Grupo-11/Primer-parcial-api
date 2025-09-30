using System.ComponentModel.DataAnnotations;

namespace PrimerParcial.Models;

public class Event
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(200)]
    public string Location { get; set; }

    [Required]
    public DateTime StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public bool IsOnline { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}