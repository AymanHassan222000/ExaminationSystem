using ExaminationSystem.DTOs.InstructorDTOs;
using ExaminationSystem.DTOs.IntructorDTOs;

namespace ExaminationSystem.ViewModels.InstructorViewModels;

public class InstructorViewModelProfile : Profile
{
    public InstructorViewModelProfile()
    {
        CreateMap<GetInstructorInfoDTO, GetInstructorInfoViewModel>();

        CreateMap<CreateExamManualViewModel, CreateExamManualDTO>();

        CreateMap<ResponseDTO<bool>, ResponseViewModel<bool>>();

        CreateMap<CreateExamAutoViewModel, CreateExamAutoDTO>();

        CreateMap<AssignExamToStudentViewModel, AssignExamToStudentDTO>();
    }
}
