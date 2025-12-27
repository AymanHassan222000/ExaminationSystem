using AutoMapper;
using ExaminationSystem.Models;

namespace ExaminationSystem.DTOs.IntructorDTOs;

public class InstructorDtoProfile : Profile
{
    public InstructorDtoProfile()
    {
        CreateMap<Instructor, GetInstructorInfoDTO>()
            .ForMember(d => d.FullName, s => s.MapFrom(m => $"{m.FirstName} {m.LastName}"));

    }
}
