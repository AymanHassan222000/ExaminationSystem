using ExaminationSystem.DTOs.IntructorDTOs;

namespace ExaminationSystem.ViewModels.InstructorViewModels;

public class InstructorViewModelProfile : Profile
{
    public InstructorViewModelProfile()
    {
        CreateMap<GetInstructorInfoDTO, GetInstructorInfoViewModel>();
    }
}
