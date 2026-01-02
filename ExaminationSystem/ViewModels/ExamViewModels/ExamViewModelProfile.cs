using AutoMapper;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.InstructorDTOs;
using ExaminationSystem.ViewModels.InstructorViewModels;

namespace ExaminationSystem.ViewModels.ExamViewModels;

public class ExamViewModelProfile : Profile
{
    public ExamViewModelProfile()
    {
        CreateMap<CreateExamViewModel,CreateExamDTO>();

        CreateMap<ExamDetailsDTO, ExamDetailsViewModel>()
            .ForMember(dest => dest.ExamID,opt => opt.MapFrom(src => src.ID));

        CreateMap<UpdateExamViewModel, UpdateExamDTO>();

    }
}
