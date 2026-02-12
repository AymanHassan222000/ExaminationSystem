using ExaminationSystem.DTOs.InstructorDTOs;

namespace ExaminationSystem.Services.Implementation;

public class InstructorService : IInstructorService
{
    private readonly IBaseRepository<Course> _courseRepo;
    private readonly IBaseRepository<Exam> _examRepo;
    private readonly IBaseRepository<ExamQuestion> _examQuestionRepo;
    private readonly IBaseRepository<Question> _questionRepo;
    private readonly IBaseRepository<Student> _studentRepo;
    private readonly IBaseRepository<ExamAttempt> _examAttemptRepo;
    private readonly IMapper _mapper;
    public InstructorService(
        IBaseRepository<Course> courseRepo,
        IBaseRepository<Exam> examRepo,
        IBaseRepository<ExamQuestion> examQuestionRepo,
        IBaseRepository<Question> questionRepo,
        IBaseRepository<Student> studentRepo,
        IBaseRepository<ExamAttempt> examAttemptRepo,
        IMapper mapper)
    {
        _courseRepo = courseRepo;
        _examRepo = examRepo;
        _examQuestionRepo = examQuestionRepo;
        _questionRepo = questionRepo;
        _mapper = mapper;
        _studentRepo = studentRepo;
        _examAttemptRepo = examAttemptRepo;
    }

    public async Task<ResponseDTO<bool>> CreateExamManuallyAsync(CreateExamManualDTO dto, UserContextDTO userContext)
    {
        if (!dto.QuestionIDs.Any())
            return new FailureResponseDTO<bool>(ErrorCode.QuestionNotFound, "Exam must contain at least one question");

        var hasInvalidQuestions = await HasInvalidQuestions(dto.QuestionIDs, userContext);

        if (hasInvalidQuestions)
            return new FailureResponseDTO<bool>(ErrorCode.UnAuthorized, "One or more questions are invalid or do not belong to you");

        var exam = _mapper.Map<Exam>(dto);
        exam.CreatedBy = userContext.UserID;

        await _examRepo.AddAsync(exam);

        return new SuccessResponseDTO<bool>(true);

    }

    public async Task<ResponseDTO<bool>> CreateExamAutoAsync(CreateExamAutoDTO dto, UserContextDTO userContext)
    {
        var query = _questionRepo.GetAll();

        if (userContext.UserRole == UserRoles.Instructor)
            query = query.Where(q => q.InstructorID == userContext.UserID);

        var (sampleCount, madiumCount, hardCount) = await GetQuestionCountsByLevelAsync(query);

        var (numSample, numMedium, numHard) = CalculateDistribution(dto);

        if (numSample > sampleCount || numMedium > madiumCount || numHard > hardCount)
            return new FailureResponseDTO<bool>(ErrorCode.NotEnoughQuestions, "Not enough questions available for the requested difficulty distribution");

        var selectedSample = await GetRandomQuestionsAsync(query, QuestionLevel.Simple, numSample);
        var selectedMedium = await GetRandomQuestionsAsync(query, QuestionLevel.Medium, numMedium);
        var selectedHard = await GetRandomQuestionsAsync(query, QuestionLevel.Hard, numHard);

        var allQuestionIds = selectedSample.Concat(selectedMedium).Concat(selectedHard).ToList();

        var exam = _mapper.Map<Exam>(dto);
        exam.NumberOfQuestions = allQuestionIds.Count;
        exam.CreatedBy = userContext.UserID;

        exam.ExamQuestions = allQuestionIds.Select((qid, index) => new ExamQuestion { QuestionID = qid }).ToList();

        await _examRepo.AddAsync(exam);

        return new SuccessResponseDTO<bool>(true);
    }

    public async Task<ResponseDTO<bool>> AssignExamToStudentAsync(AssignExamToStudentDTO dto, UserContextDTO userContext)
    {
        var isStudentExist = await _studentRepo.AnyAsync(s => s.UserID == userContext.UserID);

        if (!isStudentExist) 
            return new FailureResponseDTO<bool>(ErrorCode.StudentNotFound,$"Not found student with ID {dto.StudentID}");

        var instructorId = await _examRepo.Find(e => e.ID == dto.ExamID)
                                          .Select(e => e.Course.InstructorID)
                                          .FirstOrDefaultAsync();

        if (instructorId == 0)
            return new FailureResponseDTO<bool>(ErrorCode.ExamNotFound,$"Not found exam with id {dto.ExamID}");

        if (userContext.UserRole == UserRoles.Instructor) {

            if (instructorId != userContext.UserID)
                return new FailureResponseDTO<bool>(ErrorCode.UnAuthorized,"You can't assign this exam to student");
        }

        var examAttempt = _mapper.Map<ExamAttempt>(dto);

        await _examAttemptRepo.AddAsync(examAttempt);

        return new SuccessResponseDTO<bool>(true);
    }


    #region Private Methodes
    private async Task<(int sampleCount, int mediumCount, int hardCount)> GetQuestionCountsByLevelAsync(IQueryable<Question> query)
    {
        var counts = await query.GroupBy(q => q.Level)
                                .Select(g => new
                                {
                                    Level = g.Key,
                                    Count = g.Count()
                                })
                                .ToListAsync();

        var sampleCount = counts.FirstOrDefault(x => x.Level == QuestionLevel.Simple)?.Count ?? 0;
        var mediumCount = counts.FirstOrDefault(x => x.Level == QuestionLevel.Medium)?.Count ?? 0;
        var hardCount = counts.FirstOrDefault(x => x.Level == QuestionLevel.Hard)?.Count ?? 0;

        return (sampleCount, mediumCount, hardCount);
    }

    private (int numSample, int numMedium, int numHard) CalculateDistribution(CreateExamAutoDTO dto)
    {
        var sample = dto.NumberOfSample ?? (int)Math.Ceiling(dto.NumberOfQuestions * 0.4);
        var medium = dto.NumberOfMedium ?? (int)Math.Ceiling(dto.NumberOfQuestions * 0.4);
        var hard = dto.NumberOfHard ?? (dto.NumberOfQuestions - sample - medium);

        return (sample, medium, hard);
    }

    private async Task<List<int>> GetRandomQuestionsAsync(IQueryable<Question> query,QuestionLevel level,int count)
    {
        return await query
            .Where(q => q.Level == level)
            .OrderBy(q => Guid.NewGuid())
            .Select(q => q.ID)
            .Take(count)
            .ToListAsync();
    }

    private async Task<bool> HasInvalidQuestions(IEnumerable<int> questionIDs, UserContextDTO userContext)
    {
        if (userContext.UserRole == UserRoles.Admin)
            return false;

        var haseInvalidQuestions = await _questionRepo.AnyAsync(q => questionIDs.Contains(q.ID) && q.InstructorID != userContext.UserID);

        return haseInvalidQuestions;
    }
    #endregion
}

