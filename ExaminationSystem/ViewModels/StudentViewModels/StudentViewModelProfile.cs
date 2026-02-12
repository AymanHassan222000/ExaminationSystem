using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.DTOs.StudentDTO;
using ExaminationSystem.DTOs.StudentDTOs;
using ExaminationSystem.ViewModels.ChoiceViewModel;
using ExaminationSystem.ViewModels.QuestionViewModel;
using ExaminationSystem.ViewModels.StudentViewModels;

namespace ExaminationSystem.ViewModels.StudentViewModel;

public class StudentViewModelProfile : Profile
{
    public StudentViewModelProfile()
    {
        CreateMap<EnrollInCourseRequestViewModel, EnrollInCourseRequestDTO>();

        CreateMap<TakeExamRequestViewModel, TakeExamRequestDTO>();

        CreateMap<TakeExamResponseDTO, TakeExamResponseViewModel>()
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        CreateMap<ExamQuestionsDTO, ExamQuestionsViewModel>()
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

        CreateMap<QuestionChoicesDTO, QuestionChoicesViewModel>();

        CreateMap<SubmitExamRequestViewModel, SubmitExamRequestDTO>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

        CreateMap<SubmitExamAnswerViewModel, SubmitExamAnswerDTO>();

        CreateMap<SubmitExamResponseDTO, SubmitExamResponseViewModel>();

    }
}
