namespace Web.Models.RequestModels;

public class AddCourseRequestModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
}