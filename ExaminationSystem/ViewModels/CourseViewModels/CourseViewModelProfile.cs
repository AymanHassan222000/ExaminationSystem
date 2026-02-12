using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.DTOs.ResponseDTOs;
using ExaminationSystem.ViewModels.ResponseViewModels;

namespace ExaminationSystem.ViewModels.CourseViewModels;

public class CourseViewModelProfile : Profile
{
    public CourseViewModelProfile()
    {
        CreateMap<CourseDetailsDTO, CourseDetailsViewModel>();

        CreateMap<CreateCourseViewModel, CreateCourseDTO>();

        CreateMap<UpdateCourseViewModel, UpdateCourseDTO>();

        CreateMap<ResponseDTO<CourseDetailsDTO>,ResponseViewModel<CourseDetailsViewModel>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

        CreateMap<ResponseDTO<IEnumerable<CourseDetailsDTO>>,ResponseViewModel<IEnumerable<CourseDetailsViewModel>>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

    }
}
