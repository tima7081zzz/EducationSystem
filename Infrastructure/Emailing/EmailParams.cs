namespace Emailing;

public class EmailParams
{
    public required string FromName { get; set; }
    public required string FromEmail { get; set; }
    public required string Subject { get; set; }
    public string? HtmlBody { get; set; }
    public string? TextBody { get; set; }
    public List<string> ToList { get; set; } = [];
}