using AutoMapper;
using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.ViewModels.ChoiceViewModel;
using ExaminationSystem.ViewModels.ExamParticipationViewModels;
using ExaminationSystem.ViewModels.QuestionViewModel;

namespace ExaminationSystem.DTOs.ExamParticipationDTOs;

public class ExamParticipationsDtoProfile : Profile
{
    public ExamParticipationsDtoProfile()
    {
        CreateMap<TakeExamRequestDTO, ExamAttempt>()
            .ForMember(dest => dest.IsSubmitted, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<ExamAttempt, TakeExamResponseDTO>()
            .ForMember(dest => dest.ExamAttempitID, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Exam.ExamQuestions));

        CreateMap<ExamQuestion, ExamQuestionsDTO>()
            .ForMember(dest => dest.QuestionID, opt => opt.MapFrom(src => src.Question.ID))
            .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.QuestionText))
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Question.Choices));

        CreateMap<Choice, QuestionChoicesDTO>()
            .ForMember(dest => dest.ChoiceID, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.ChoiceText, opt => opt.MapFrom(src => src.ChoiceText));

        CreateMap<ExamAttempt, ExamAttemptAnswer>()
            .ForMember(dest => dest.ExamAtteptID, opt => opt.MapFrom(src => src.ID));

        CreateMap<ExamAttempt, SubmitExamResponseDTO>()
            .ForMember(dest => dest.ExamAttemptID, opt => opt.MapFrom(src => src.ID));
            //.ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(src => src.SubmittedAt.Value));
    }
}
