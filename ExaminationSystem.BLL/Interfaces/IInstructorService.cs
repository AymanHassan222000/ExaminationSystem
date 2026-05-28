using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.InstructorDTOs;

namespace ExaminationSystem.BLL.Interfaces;

public interface IInstructorService
{
    //Task<Response<bool>> AssignExamToStudentAsync(AssignExamToStudentDTO dto);
    Task<bool> InstructorIsExistAsync(int istructorID);
}
