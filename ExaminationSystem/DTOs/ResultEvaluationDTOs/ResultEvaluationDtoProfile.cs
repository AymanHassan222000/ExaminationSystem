using AutoMapper;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.Models;

namespace ExaminationSystem.DTOs.ResultEvaluationDTOs;

public class ResultEvaluationDtoProfile : Profile
{
    public ResultEvaluationDtoProfile()
    {
        CreateMap<ExamResult, EvaluateExamResponseDTO>();

        //CreateMap<ExamResult, StudentExamResultDTO>();

        CreateMap<ExamAttempt, StudentExamResultDTO>()
            .ForMember(dest => dest.ExamInfo, opt => opt.MapFrom(src => src.Exam))
            .ForMember(dest => dest.TotalQuestions, opt => opt.MapFrom(src => src.ExamResult.TotalQuestions))
            .ForMember(dest => dest.CorrectAnswers, opt => opt.MapFrom(src => src.ExamResult.CorrectAnswers))
            .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.ExamResult.Percentage))
            .ForMember(dest => dest.IsPassed, opt => opt.MapFrom(src => src.ExamResult.IsPassed));

        CreateMap<Exam, GetExamInfoDTO>()
            .ForMember(dest => dest.ExamID, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.ExamTitle, opt => opt.MapFrom(src => src.Title)) ;
    }
}
