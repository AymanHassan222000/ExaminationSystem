using AutoMapper;
using ExaminationSystem.Models;

namespace ExaminationSystem.DTOs.ChoiceDTOs;

public class ChoiceDtoProfile : Profile
{
    public ChoiceDtoProfile()
    {
        CreateMap<CreateChoiceDTO, Choice>();
    }
}
