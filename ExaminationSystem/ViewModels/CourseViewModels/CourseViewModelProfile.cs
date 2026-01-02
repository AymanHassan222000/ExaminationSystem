using AutoMapper;
using ExaminationSystem.DTOs.CourseDTOs;

namespace ExaminationSystem.ViewModels.CourseViewModels;

public class CourseViewModelProfile : Profile
{
    public CourseViewModelProfile()
    {
        CreateMap<CourseDetailsDTO, CourseDetailsViewModel>();

        CreateMap<CreateCourseViewModel, CreateCourseDTO>();

        CreateMap<UpdateCourseViewModel, UpdateCourseDTO>();


    }
}
