using AutoMapper.QueryableExtensions;
using ExaminationSystem.DTOs.QuestionDTOs;

namespace ExaminationSystem.Services.Implementation;

public class QuestionService : IQuestionService
{
    private readonly IBaseRepository<Question> _questionRepo;
    private readonly IBaseRepository<Instructor> _instructorRepo;
    private readonly IMapper _mapper;
    public QuestionService(IMapper mapper,
        IBaseRepository<Question> questionRepo,
        IBaseRepository<Instructor> instructorRepo)
    {
        _questionRepo = questionRepo;
        _instructorRepo = instructorRepo;
        _mapper = mapper;
    }

    public async Task<ResponseDTO<QuestionDetailsDTO>> AddQuestionAsync(CreateQuestionDTO dto)
    {
        var instructor = await _instructorRepo.GetById(dto.InstructorID).FirstOrDefaultAsync();

        if (instructor == null)
            return new FailureResponseDTO<QuestionDetailsDTO>(ErrorCode.InvalidInstructorID, $"No instructor was found with ID = {dto.InstructorID}");

        var question = _mapper.Map<Question>(dto);

        await _questionRepo.AddAsync(question);

        question.Instructor = instructor;

        var questionDetails = _mapper.Map<QuestionDetailsDTO>(question);

        return new SuccessResponseDTO<QuestionDetailsDTO>(questionDetails);
    }

    public async Task<ResponseDTO<IEnumerable<QuestionDetailsDTO>>> GetAllQuestionsAsync(int instructorID)
    {
        var questions= await _questionRepo.GetAll().Where(m => m.CreatedBy == instructorID)
                                        .ProjectTo<QuestionDetailsDTO>(_mapper.ConfigurationProvider)
                                        .OrderBy(e => e.Level)
                                        .ToListAsync();

        if (!questions.Any())
            return new FailureResponseDTO<IEnumerable<QuestionDetailsDTO>>(ErrorCode.QuestionNotFound, "Not found any questions");

        return new SuccessResponseDTO<IEnumerable<QuestionDetailsDTO>>(questions);
    }

    public async Task<ResponseDTO<QuestionDetailsDTO>> GetQuestionByIDAsync(int questionID,int instructorID)
    {
        var question = await _questionRepo.GetById(questionID).Include(m => m.Instructor).FirstOrDefaultAsync();

        if (question == null)
            return new FailureResponseDTO<QuestionDetailsDTO>(ErrorCode.InvalidQuestionID, $"No question was found with ID = {questionID}");

        if (question.CreatedBy != instructorID)
            return new FailureResponseDTO<QuestionDetailsDTO>(ErrorCode.UnAuthorized, "You can not view this question");

        var questionDetails = _mapper.Map<QuestionDetailsDTO>(question);

        return new SuccessResponseDTO<QuestionDetailsDTO>(questionDetails);
    }

    public async Task<ResponseDTO<QuestionDetailsDTO>> UpdateQuestionAsync(int questionID,int instructorID, UpdateQuestionDTO dto)
    {
        var question = await _questionRepo.GetById(questionID).FirstOrDefaultAsync();

        if (question == null)
            return new FailureResponseDTO<QuestionDetailsDTO>(ErrorCode.InvalidQuestionID, $"Not found question with ID {questionID}");

        if (question.CreatedBy != instructorID)
            return new FailureResponseDTO<QuestionDetailsDTO>(ErrorCode.UnAuthorized, "You can not modify this question");

        //var result = await _questionRepo.UpdateAsync(
        //c => c.ID == questionID,
        //s => s
        //      .SetProperty(d => d.QuestionText, dto.QuestionText)
        //      .SetProperty(d => d.Level, dto.Level)
        //      .SetProperty(d => d.UpdatedAt, DateTime.UtcNow)
        //      .SetProperty(d => d.UpdatedBy, instructorID)
        //);

        //if (result == 0)
        //    return new FailureResponseDTO<QuestionDetailsDTO>(ErrorCode.FaildeUpdateQuestion, "Update failed");

        return await GetQuestionByIDAsync(questionID, instructorID) ;
    }

    public async Task<ResponseDTO<QuestionDetailsDTO>> DeleteQuestionAsync(int questionID, int instructorID)
    {
        var question = await _questionRepo.GetById(questionID).FirstOrDefaultAsync();

        if (question == null)
            return new FailureResponseDTO<QuestionDetailsDTO>(ErrorCode.InvalidQuestionID, $"No question was found with ID = {questionID}");

        if (question.CreatedBy != instructorID)
            return new FailureResponseDTO<QuestionDetailsDTO>(ErrorCode.UnAuthorized, "You can not delete this question");

        await _questionRepo.HardDeleteAsync(question);

        return new SuccessResponseDTO<QuestionDetailsDTO>(null);
    }

}
