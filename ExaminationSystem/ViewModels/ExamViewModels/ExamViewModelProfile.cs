using ExaminationSystem.DTOs.ExamDTOs;

namespace ExaminationSystem.ViewModels.ExamViewModels;

public class ExamViewModelProfile : Profile
{
    public ExamViewModelProfile()
    {

        CreateMap<ExamDetailsDTO, ExamDetailsViewModel>()
            .ForMember(dest => dest.ExamID,opt => opt.MapFrom(src => src.ID));

        CreateMap<UpdateExamViewModel, UpdateExamDTO>();

        CreateMap<ResponseDTO<ExamDetailsDTO>, ResponseViewModel<ExamDetailsViewModel>>()
            .ForMember(dest => dest.Data,opt => opt.MapFrom(src => src.Data));

        CreateMap<ResponseDTO<IEnumerable<ExamDetailsDTO>>, ResponseViewModel<IEnumerable<ExamDetailsViewModel>>>()
            .ForMember(dest => dest.Data,opt => opt.MapFrom(src => src.Data));


    }
}
