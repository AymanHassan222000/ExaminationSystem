using ExaminationSystem.API.ViewModels.ResultEvaluationViewModels;
using ExaminationSystem.API.ViewModels.StudentViewModels;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.ResultEvaluationDTOs;
using ExaminationSystem.ViewModels.ExamViewModels;

namespace ExaminationSystem.ViewModels.ResultEvaluationViewModels;

public class ResultEvaluationViewModelProfile : Profile
{
    public ResultEvaluationViewModelProfile()
    {
        CreateMap<EvaluateExamResponseDTO, EvaluateExamResponseViewModel>();

        CreateMap<GetExamInfoDTO, GetExamInfoViewModel>();

        CreateMap<ExamResultSummaryDTO, ExamResultSummaryViewModel>();

        CreateMap<GetStudentInfoDTO, GetStudentInfoViewModel>();

    }
}
