using ExaminationSystem.API.ViewModels.CourseViewModels;
using ExaminationSystem.BLL.DTOs.CourseDTOs;
using ExaminationSystem.DTOs.CourseDTOs;

namespace ExaminationSystem.ViewModels.CourseViewModels;

public class CourseViewModelProfile : Profile
{
    public CourseViewModelProfile()
    {
        //Mapping for Add Course
        CreateMap<AddCourseViewModel, AddCourseDTO>();

        //Mapping for Enroll Student in Course
        CreateMap<EnrollStudentInCourseViewModel, EnrollStudentInCourseDTO>();

        //Mapping for Add Course Prerequisite
        CreateMap<AddCoursePrerequisiteViewModel, AddCoursePrerequisiteDTO>()
            .ForMember(dest => dest.Prerequisites, opt => opt.MapFrom(src => src.Prerequisites));

        CreateMap<PrerequisiteCourseViewModel, PrerequisiteCourseDTO>();

        //Mapping for course details
        CreateMap<CourseDetailsDTO, CourseDetailsViewModel>();

        //Mapping for update course
        CreateMap<UpdateCourseViewModel, UpdateCourseDTO>();

        //Mapping for remove course prerequiste
        CreateMap<RemoveCoursePrerequisiteViewModel, RemoveCoursePrerequisiteDTO>();
    }
}
