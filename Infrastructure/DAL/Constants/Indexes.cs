namespace DAL.Constants;

public static class Indexes
{
    public const string UserEmail = "IX_User_Email";
    public const string TeacherCourseTeacherUserId = "IX_TeacherCourse_TeacherUserId";
    public const string StudentCourseUserIdCourseId = "IX_StudentCourse_UserId_CourseId";
    public const string StudentAssignmentUserIdAssignmentId = "IX_StudentAssignment_UserId_AssignmentId";
    public const string CoursePublicId = "IX_Course_PublicId";
    public const string AssignmentCourseId = "IX_Assignment_CourseId";
    public const string UserNotificationSettingsUserId = "IX_UserNotificationSettings_UserId";
    public const string RecommendedCourseUserIdCourseId = "IX_RecommendedCourse_UserId_CourseId";
}