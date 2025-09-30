namespace BackendApi.Models;

public class Event
{
    public int Id { get; set; }                 // PK (Identity)
    public string Title { get; set; } = default!;
    public string Location { get; set; } = default!;
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public bool IsOnline { get; set; }
    public string? Notes { get; set; }
}