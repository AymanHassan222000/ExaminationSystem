using AutoMapper;
using ExaminationSystem.DTOs.InstructorDTOs;
using ExaminationSystem.DTOs.IntructorDTOs;

namespace ExaminationSystem.ViewModels.InstructorViewModels;

public class InstructorViewModelProfile : Profile
{
    public InstructorViewModelProfile()
    {
        CreateMap<GetInstructorInfoDTO, GetInstructorInfoViewModel>();

        CreateMap<AssignExamToCourseViewModel, AssignExamToCourseDTO>();

        CreateMap<AssignQuestionsToExamViewModel, AssignQuestionsToExamDTO>();

    }
}
