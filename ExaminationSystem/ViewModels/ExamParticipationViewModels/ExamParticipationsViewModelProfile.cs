using AutoMapper;
using ExaminationSystem.DTOs.ExamParticipationDTOs;

namespace ExaminationSystem.ViewModels.ExamParticipationViewModels;

public class ExamParticipationsViewModelProfile : Profile
{
    public ExamParticipationsViewModelProfile()
    {
        CreateMap<TakeExamRequestViewModel, TakeExamRequestDTO>();

        CreateMap<TakeExamResponseDTO, TakeExamResponseViewModel>();

        CreateMap<SubmitExamRequestViewModel, SubmitExamRequestDTO>()
            .ForMember(dest => dest.Answers,opt => opt.MapFrom(src => src.Answers));

        CreateMap<SubmitExamAnswerViewModel, SubmitExamAnswerDTO>();

        CreateMap<SubmitExamResponseDTO, SubmitExamResponseViewModel>();
    }
}
