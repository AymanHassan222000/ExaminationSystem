using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.ResultEvaluationDTOs;
using ExaminationSystem.ViewModels.ExamViewModels;

namespace ExaminationSystem.ViewModels.ResultEvaluationViewModels;

public class ResultEvaluationViewModelProfile : Profile
{
    public ResultEvaluationViewModelProfile()
    {
        CreateMap<EvaluateExamResponseDTO, EvaluateExamResponseViewModel>();

        CreateMap<StudentExamResultDTO, StudentExamResultViewModel>()
            .ForMember(dest => dest.ExamInfo,opt => opt.MapFrom(src => src.ExamInfo));

        CreateMap<GetExamInfoDTO, GetExamInfoViewModel>();

    }
}
