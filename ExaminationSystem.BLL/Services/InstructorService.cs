using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.InstructorDTOs;
using ExaminationSystem.Helpers.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BLL.Services;

public class InstructorService : IInstructorService
{
    private readonly IGeneralRepository<Exam> _examRepo;
    private readonly IGeneralRepository<Student> _studentRepo;
    private readonly IGeneralRepository<ExamAttempt> _examAttemptRepo;
    private readonly IGeneralRepository<Instructor> _instructorRepo;
    public InstructorService(
        IGeneralRepository<Exam> examRepo,
        IGeneralRepository<ExamQuestion> examQuestionRepo,
        IGeneralRepository<Student> studentRepo,
        IGeneralRepository<ExamAttempt> examAttemptRepo,
        IGeneralRepository<Instructor> instructorRepo)
    {
        _examRepo = examRepo;
        _studentRepo = studentRepo;
        _examAttemptRepo = examAttemptRepo;
        _instructorRepo = instructorRepo;
    }


    public async Task<bool> InstructorIsExistAsync(int istructorId) =>
        await _instructorRepo.AnyAsync(i => i.ID == istructorId);

}

