using AutoMapper;
using ExaminationSystem.DTOs.ChoiceDTOs;

namespace ExaminationSystem.ViewModels.ChoiceViewModel;

public class ChoiceViewModelProfile : Profile
{
    public ChoiceViewModelProfile()
    {
        CreateMap<CreateChoiceViewModel, CreateChoiceDTO>();

        CreateMap<ChoiceDetailsDTO, ChoiceDetailseViewModel>();

        CreateMap<UpdateChoiceViewModel, UpdateChoiceDTO>();
    }
}
