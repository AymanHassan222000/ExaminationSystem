using AutoMapper;
using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.ExamParticipationDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.ViewModels.ChoiceViewModel;
using ExaminationSystem.ViewModels.QuestionViewModel;

namespace ExaminationSystem.ViewModels.ExamParticipationViewModels;

public class ExamParticipationsViewModelProfile : Profile
{
    public ExamParticipationsViewModelProfile()
    {
        CreateMap<TakeExamRequestViewModel, TakeExamRequestDTO>();

        CreateMap<TakeExamResponseDTO, TakeExamResponseViewModel>()
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        CreateMap<ExamQuestionsDTO, ExamQuestionsViewModel>()
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

        CreateMap<QuestionChoicesDTO, QuestionChoicesViewModel>();

        CreateMap<SubmitExamRequestViewModel, SubmitExamRequestDTO>()
            .ForMember(dest => dest.Answers,opt => opt.MapFrom(src => src.Answers));

        CreateMap<SubmitExamAnswerViewModel, SubmitExamAnswerDTO>();

        CreateMap<SubmitExamResponseDTO, SubmitExamResponseViewModel>();
    }
}
