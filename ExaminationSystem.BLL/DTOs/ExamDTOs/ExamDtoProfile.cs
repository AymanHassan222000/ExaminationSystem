using ExaminationSystem.BLL.DTOs.ExamDTOs;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;

namespace ExaminationSystem.DTOs.ExamDTOs;

public class ExamDtoProfile : Profile
{
    public ExamDtoProfile()
    {

        // Mapping for CreateExamManualDTO
        CreateMap<CreateExamManualDTO, Exam>()
            .ForMember(dest => dest.ExamQuestions, opt => opt.MapFrom(src =>
                src.QuestionIDs.Select(qid => new ExamQuestion { QuestionID = qid }).ToList()));

        //Mapping for CreateExamAutoDTO
        CreateMap<CreateExamAutoDTO, Exam>();

        // Mapping for GetAllExamsDTO
        CreateMap<Exam, GetAllExamsDTO>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));

        // Mapping for GetExamByIdDTO
        CreateMap<Exam, GetExamByIdDTO>()
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.ExamQuestions));

        CreateMap<ExamQuestion, ExamQuestionsDTO>()
            .ForMember(dest => dest.QuestionID, opt => opt.MapFrom(src => src.QuestionID))
            .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.Head))
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Question.Choices));

        CreateMap<Choice, GetExamChoicesInfoDTO>()
            .ForMember(dest => dest.ChoiceID, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.ChoiceText, opt => opt.MapFrom(src => src.ChoiceText));

        // Mapping for UpdateExamDTO
        CreateMap<UpdateExamDTO, Exam>();

        CreateMap<AssignExamToStudentDTO, ExamAttempt>();

    }
}
