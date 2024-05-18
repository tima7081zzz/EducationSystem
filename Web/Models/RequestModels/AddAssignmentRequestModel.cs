namespace Web.Models.RequestModels;

public class AddAssignmentRequestModel
{
    public int CourseId { get; set; }
    public int? MaxGrade { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset Deadline { get; set; }
}