namespace BackendApi.Models;

public enum TicketSeverity { Low, Medium, High, Critical }
public enum TicketStatus { Open, InProgress, Resolved, Closed }

public class SupportTicket
{
    public int Id { get; set; }
    public string Subject { get; set; } = default!;
    public string RequesterEmail { get; set; } = default!;
    public string? Description { get; set; }
    public TicketSeverity Severity { get; set; } = TicketSeverity.Low;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }
    public string? AssignedTo { get; set; }
}