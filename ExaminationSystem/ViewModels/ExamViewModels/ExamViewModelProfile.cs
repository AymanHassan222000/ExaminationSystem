using AutoMapper;
using ExaminationSystem.DTOs.ExamDTOs;

namespace ExaminationSystem.ViewModels.ExamViewModels;

public class ExamViewModelProfile : Profile
{
    public ExamViewModelProfile()
    {
        CreateMap<CreateExamViewModel,CreateExamDTO>();

        CreateMap<ExamDetailsDTO, ExamDetailsViewModel>();

        CreateMap<UpdateExamViewModel, UpdateExamDTO>();

        CreateMap<AssignQuestionsToExamViewModel, AssignQuestionsToExamDTO>();
    }
}
