namespace ExaminationSystem.BLL.Interfaces;

public interface IStudentService
{
    Task<bool> IsStudentExist(int studentID);
}
