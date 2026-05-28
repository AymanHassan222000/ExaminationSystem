using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.BLL.Services;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.Helpers.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExaminationSystem.BLL;

public static class BLLDependencyInjection
{
    public static IServiceCollection AddBLLDependencies(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<IExamQuestionService, ExamQuestionService>();
        services.AddScoped<IInstructorService, InstructorService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IResultEvaluationService, ResultEvaluationService>();
        services.AddScoped<IChoiceService, ChoiceService>();

        //services.AddScoped<IExamResultService, ExamResultService>();


        return services;
    }
}
