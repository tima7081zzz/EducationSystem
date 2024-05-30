namespace Emailing;

public class ClientParams
{
    public required string Username { get; set; }
    public required string UserPassword { get; set; }
    public required string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
}