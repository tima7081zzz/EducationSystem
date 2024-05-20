namespace DAL;

public class DbOptions
{
    public const string SectionName = "Db";
    public required string ConnectionString { get; set; }
}