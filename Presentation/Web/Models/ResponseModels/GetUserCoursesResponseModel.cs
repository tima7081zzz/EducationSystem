using Course.Queries;

namespace Web.Models.ResponseModels;

public class GetUserCoursesResponseModel
{
    public IEnumerable<UserCourseModel> UserCourses { get; set; } = [];
}