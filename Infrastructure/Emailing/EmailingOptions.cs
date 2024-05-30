namespace Emailing;

public class EmailingOptions
{
    public const string SectionName = "Emailing";
    public required NotificationSender NotificationSender { get; set; }
}

public class NotificationSender
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string UserPassword { get; set; }
    public required string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
}