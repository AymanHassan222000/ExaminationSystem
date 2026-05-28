using ExaminationSystem.API.ViewModels.ChoiceViewModels;
using ExaminationSystem.DTOs.ChoiceDTOs;

namespace ExaminationSystem.ViewModels.ChoiceViewModel;

public class ChoiceViewModelProfile : Profile
{
    public ChoiceViewModelProfile()
    {
        CreateMap<AddChoiceViewModel, AddChoiceDTO>();



        //CreateMap<CreateChoiceViewModel, CreateChoiceDTO>();

        //CreateMap<ChoiceDetailsDTO, ChoiceDetailseViewModel>();

        //CreateMap<UpdateChoiceViewModel, UpdateChoiceDTO>();
    }
}
