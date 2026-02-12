using ExaminationSystem.DTOs.ChoiceDTOs;

namespace ExaminationSystem.Services.Interfaces;

public interface IChoiceService
{
    Task<ResponseDTO<ChoiceDetailsDTO>> AddChoiceAsync(CreateChoiceDTO dto, int instructorID);
    Task<IEnumerable<ChoiceDetailsDTO>> GetAllChoicesAsync(int instructorID);
    Task<ChoiceDetailsDTO> GetChoiceByIDAsync(int choiceID, int instructorID);
    Task<ChoiceDetailsDTO> UpdateChoiceAsync(int choiceID, int instructorID, UpdateChoiceDTO dto);

    Task DeleteChoiceAsync(int id, int instructorID);
}
