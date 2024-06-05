using DAL;
using DAL.Entities;
using MediatR;

namespace CourseRecommendations.Commands;

public record RunRecommendingCoursesCommand : IRequest;

public class RecommendCoursesCommandHandler : IRequestHandler<RunRecommendingCoursesCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RecommendCoursesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RunRecommendingCoursesCommand request, CancellationToken ct)
    {
        var users = await _unitOfWork.UserRepository.GetAll(ct);
        var courses = await _unitOfWork.CourseRepository.GetAll(ct);

        foreach (var user in users)
        {
            var recommendations = new List<RecommendedCourse>();

            foreach (var course in courses)
            {
                var topsisScore = await CalculateTopsisRecommendationRate(user, course, ct);

                recommendations.Add(new RecommendedCourse
                {
                    UserId = user.Id,
                    CourseId = course.Id,
                    TopsisScore = topsisScore
                });
            }

            await _unitOfWork.RecommendedCourseRepository.Add(recommendations, ct);
        }

        await _unitOfWork.SaveChanges(ct);
    }

    private async Task<double> CalculateTopsisRecommendationRate(User user, Course course, CancellationToken ct)
    {
        double[] weights = [0.25, 0.25, 0.35, 0.15];

        // Step 1: Normalize criteria
        var normalizedCriteria = await NormalizeCriteria(user, course, ct);

        // Step 2: Calculate weighted normalized decision matrix
        var weightedNormalizedCriteria = new double[weights.Length];
        for (var i = 0; i < weights.Length; i++)
        {
            weightedNormalizedCriteria[i] = normalizedCriteria[i] * weights[i];
        }

        // Step 3: Determine ideal and negative-ideal solutions
        double[] idealSolution = [1, 1, 1, 1]; // Ideal for all criteria
        double[] negativeIdealSolution = [0, 0, 0, 0]; // Negative ideal for all criteria

        // Step 4: Calculate separation measures
        var idealSeparation = CalculateSeparation(weightedNormalizedCriteria, idealSolution);
        var negativeIdealSeparation = CalculateSeparation(weightedNormalizedCriteria, negativeIdealSolution);

        // Step 5: Calculate relative closeness to ideal solution
        var topsisScore = negativeIdealSeparation / (idealSeparation + negativeIdealSeparation);

        return topsisScore;
    }

    private async Task<double[]> NormalizeCriteria(User user, Course course, CancellationToken ct)
    {
        var assignmentSubmissionRate = await CalculateCourseSubmissionRate(course, ct);
        var interestInCategory = await CalculateInterestInCategory(user, course.Category, ct);
        var userSimilarity = await CalculateUserSimilarity(user, course, ct);
        var coursePopularity = await CalculateCoursePopularity(course, ct);

        return [assignmentSubmissionRate, interestInCategory, userSimilarity, coursePopularity];
    }

    private static double CalculateSeparation(double[] criteria, double[] idealSolution)
    {
        var sumOfSquares = criteria.Select((t, i) => Math.Pow(t - idealSolution[i], 2)).Sum();
        return Math.Sqrt(sumOfSquares);
    }

    private async Task<double> CalculateCourseSubmissionRate(Course course, CancellationToken ct)
    {
        var totalAssignments = await _unitOfWork.StudentAssignmentRepository.GetByCourse(course.Id, ct);
        var onTimeSubmissions = totalAssignments.Count(x => x.Status is not StudentCourseTaskStatus.NotSubmitted);

        return totalAssignments.Count == 0 ? 0 : (double)onTimeSubmissions / totalAssignments.Count;
    }

    private async Task<double> CalculateInterestInCategory(User user, string? category, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return 0;
        }
        
        var totalCourses = await _unitOfWork.StudentCourseRepository.GetForUser(user.Id, ct);
        var categoryCourses = totalCourses.Count(e => e.UserId == user.Id && e.Course.Category == category);

        return totalCourses.Count == 0 ? 0 : (double)categoryCourses / totalCourses.Count;
    }

    private async Task<double> CalculateUserSimilarity(User user, Course course, CancellationToken ct)
    {
        var studentCoursesUsers = await _unitOfWork.StudentCourseRepository.GetCommonUsers(user.Id, course.Id, ct);

        var courseUsers = await _unitOfWork.StudentCourseRepository.GetByCourse(course.Id, ct);

        return studentCoursesUsers.Count == 0 ? 0 : (double) studentCoursesUsers.Count / courseUsers.Count;
    }

    private async Task<double> CalculateCoursePopularity(Course course, CancellationToken ct)
    {
        var totalUsers = await _unitOfWork.UserRepository.Count(ct);
        var courseUsers = await _unitOfWork.StudentCourseRepository.GetByCourse(course.Id, ct);

        return totalUsers == 0 ? 0 : (double) courseUsers.Count / totalUsers;
    }
}