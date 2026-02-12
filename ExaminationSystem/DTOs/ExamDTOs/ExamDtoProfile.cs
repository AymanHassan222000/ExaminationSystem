namespace ExaminationSystem.DTOs.ExamDTOs;

public class ExamDtoProfile : Profile
{
    public ExamDtoProfile()
    {
        CreateMap<Exam, ExamDetailsDTO>()
            .ForMember(dest => dest.CourseInfo, opt => opt.MapFrom(src => src.Course));
    }
}
