using AutoMapper;
using ExaminationSystem.DTOs.StudentDTO;

namespace ExaminationSystem.ViewModels.StudentViewModel;

public class StudentViewModelProfile : Profile
{
    public StudentViewModelProfile()
    {
        CreateMap<AssignStudentToCourseRequestViewModel, AssignStudentToCourseRequestDTO>();
    }
}
