using ExaminationSystem.DAL.Models;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.StudentDTO;

namespace ExaminationSystem.DTOs.ResultEvaluationDTOs;

public class ResultEvaluationDtoProfile : Profile
{
    public ResultEvaluationDtoProfile()
    {
        CreateMap<ExamResult, EvaluateExamResponseDTO>();

        CreateMap<ExamAttempt, StudentExamResultDTO>()
            .ForMember(dest => dest.ExamInfo, opt => opt.MapFrom(src => src.Exam))
            .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.ExamResult.Percentage))
            .ForMember(dest => dest.IsPassed, opt => opt.MapFrom(src => src.ExamResult.IsPassed));

        CreateMap<Exam, GetExamInfoDTO>()
            .ForMember(dest => dest.ExamID, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.ExamTitle, opt => opt.MapFrom(src => src.Title)) ;

        CreateMap<ExamAttempt, ExamResultSummaryDTO>()
            .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.StudentID))
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
            .ForMember(dest => dest.ExamDegree, opt => opt.MapFrom(src => src.ExamResult.ExamDegree))
            .ForMember(dest => dest.StudentScore, opt => opt.MapFrom(src => src.ExamResult.StudentScore))
            .ForMember(dest => dest.Persentage, opt => opt.MapFrom(src => src.ExamResult.Percentage))
            .ForMember(dest => dest.IsPassed, opt => opt.MapFrom(src => src.ExamResult.IsPassed))
            .ForMember(dest => dest.ExamDate, opt => opt.MapFrom(src => src.Exam.Date));

    }
}
