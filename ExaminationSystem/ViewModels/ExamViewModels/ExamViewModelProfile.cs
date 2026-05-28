using ExaminationSystem.API.ViewModels.ExamViewModels;
using ExaminationSystem.BLL.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.DTOs.StudentDTOs;
using ExaminationSystem.ViewModels.ChoiceViewModel;
using ExaminationSystem.ViewModels.QuestionViewModel;

namespace ExaminationSystem.ViewModels.ExamViewModels;

public class ExamViewModelProfile : Profile
{
    public ExamViewModelProfile()
    {
        CreateMap<CreateExamManualViewModel, CreateExamManualDTO>();

        CreateMap<CreateExamAutoViewModel, CreateExamAutoDTO>();

        CreateMap<UpdateExamViewModel, UpdateExamDTO>();

        CreateMap<GetExamByIdDTO, GetExamByIdViewModel>()
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        CreateMap<ExamQuestionsDTO, ExamQuestionsViewModel>()
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

        CreateMap<GetExamChoicesInfoDTO, GetExamChoicesInfoViewModel>();

        CreateMap<GetAllExamsDTO, GetAllExamsViewModel>();

        CreateMap<UpdateExamViewModel, UpdateExamDTO>();

        CreateMap<AddingQuestionsToExamViewModel, AddingQuestionsToExamDTO>();

        CreateMap<RemoveQuestionsFromExamViewModel, RemoveQuestionsFromExamDTO>();

        CreateMap<AssignExamToStudentViewModel, AssignExamToStudentDTO>();

        CreateMap<SubmitExamRequestViewModel, SubmitExamRequestDTO>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

        CreateMap<SubmitExamAnswerViewModel, SubmitExamAnswerDTO>();

    }
}
