using ExaminationSystem.BLL.DTOs.QuestionDTOs;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DTOs.ChoiceDTOs;

namespace ExaminationSystem.BLL.DTOs.ChoiceDTOs;

public class ChoiceDtoProfile : Profile
{
    public ChoiceDtoProfile()
    {
        CreateMap<AddChoiceDTO, Choice>()
            .ForMember(dest => dest.ChoiceText, opt => opt.MapFrom(src => src.Text));

        CreateMap<UpdateQuestionChoiceDTO, Choice>()
            .ForMember(dest => dest.ChoiceText, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ChoiceID));

        CreateMap<Choice, GetChoicesInfoForEvaluation>()
            .ForMember(dest => dest.ChoiceID,opt => opt.MapFrom(src => src.ID));
    }
}
