using ExaminationSystem.BLL.DTOs.CourseDTOs;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DTOs.InstructorDTOs;

namespace ExaminationSystem.DTOs.IntructorDTOs;

public class InstructorDtoProfile : Profile
{
    public InstructorDtoProfile()
    {
        //Mapping for enroll student to course.
        CreateMap<EnrollStudentInCourseDTO, StudentCourse>()
            .ForMember(dest => dest.StudetnID, opt => opt.MapFrom(src => src.StudentID))
            .ForMember(dest => dest.CourseID, opt => opt.MapFrom(src => src.CourseID));

        //Mapping for GetInstructorInfoDTO
        CreateMap<Instructor, GetInstructorInfoDTO>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => $"{src.User.FirstName}"))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => $"{src.User.LastName}"))
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.UserID));

        //Mapping for GetAllInstructorsDTO
        CreateMap<Instructor, InstructorResponseDTO>()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.UserID))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

    }
}
