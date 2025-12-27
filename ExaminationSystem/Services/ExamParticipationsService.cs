using AutoMapper;
using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.ExamParticipationDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories.Implementations;

namespace ExaminationSystem.Services;

public class ExamParticipationsService
{
    ExamRepository _examRepo;
    BaseRepository<StudentCourse> _studentCourseRepo;
    ExamAttemptRepository _attemptRepo;
    BaseRepository<ExamAttemptAnswer> _attemptAnswerRepo;
    BaseRepository<Choice> _choiceRepo;
    IMapper _mapper;
    public ExamParticipationsService(IMapper mapper)
    {
        _examRepo = new ExamRepository();
        _studentCourseRepo = new BaseRepository<StudentCourse>();
        _attemptRepo = new ExamAttemptRepository();
        _choiceRepo = new BaseRepository<Choice>();
        _attemptAnswerRepo = new BaseRepository<ExamAttemptAnswer>();
        _mapper = mapper;
    }

    public async Task<TakeExamResponseDTO> TakeExamAsync(TakeExamRequestDTO dto)
    {
        var exam = await _examRepo.GetExamWithQuestionsAndChoicesAsync(dto.ExamID);

        if (exam == null)
            throw new Exception("Exam not found");

        var isStudentEnrolled = await _studentCourseRepo.AnyAsync(sc => sc.StudetnID == dto.StudentID && sc.CourseID == exam.CourseID);

        if (!isStudentEnrolled)
            throw new Exception("Student not enroll in course");

        var alreadyTaken = await _attemptRepo.AnyAsync(a => a.ExamID == dto.ExamID && a.StudentID == dto.StudentID);

        if (alreadyTaken)
            throw new Exception("Can't take the same exam again");

        var examAttempt = _mapper.Map<ExamAttempt>(dto);

        await _attemptRepo.AddAsync(examAttempt);

        examAttempt.Exam = exam;

        var takeExamResponse = _mapper.Map<TakeExamResponseDTO>(examAttempt);

        return takeExamResponse;
    }

    public async Task<SubmitExamResponseDTO> SubmitExamAsync(SubmitExamRequestDTO dto)
    {
        var attempt = await _attemptRepo.GetAttemptWithExamAsync(dto.ExamAttempitID);

        if (attempt == null)
            throw new Exception($"Not Found Exam Attempt With ID {dto.ExamAttempitID}");

        if (attempt.IsSubmitted)
            throw new Exception("You can not make submit again");

        foreach (var answer in dto.Answers)
        {
            var choice = await _choiceRepo.FindAsync(c => c.ID == answer.ChoiceID && c.QuestionID == answer.QuestionID);

            if (choice == null)
                throw new Exception("Invalid Question Or Choice");

            await _attemptAnswerRepo.AddAsync(new ExamAttemptAnswer
            {
                ExamAtteptID = attempt.ID,
                QuestionID = answer.QuestionID,
                ChoiceID = answer.ChoiceID,
                IsCorrect = choice.IsCorrect
            });
        }

        attempt.IsSubmitted = true;
        attempt.SubmittedAt = DateTime.UtcNow;

        await _attemptRepo.UpdateAsync(
             m => m.ID == attempt.ID,
             s => s.SetProperty(d => d.IsSubmitted, attempt.IsSubmitted)
                   .SetProperty(d => d.SubmittedAt, attempt.SubmittedAt)
        );

        var submitExamResponseDto = _mapper.Map<SubmitExamResponseDTO>(attempt);

        return submitExamResponseDto;

        //return new SubmitExamResponseDTO
        //{
        //    ExamAttemptID = attempt.ID,
        //    IsSubmitted = attempt.IsSubmitted,
        //    SubmittedAt = attempt.SubmittedAt.Value
        //};
    }
}
