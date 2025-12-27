using AutoMapper;
using ExaminationSystem.DTOs.QuestionDTOs;

namespace ExaminationSystem.ViewModels.QuestionViewModel;

public class QuestionViewModelProfile : Profile
{
    public QuestionViewModelProfile()
    {
        CreateMap<CreateQuestionViewModel, CreateQuestionDTO>();

        CreateMap<QuestionDetailsDTO, QuestionDetailsViewModel>();

        CreateMap<UpdateQuestionViewModel, UpdateQuestionDTO>();
    }
}
