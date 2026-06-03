using KinderHub.Enrollment.DTOs.Requests;
using KinderHub.Enrollment.DTOs.Responses;

namespace KinderHub.Enrollment.Services.Interfaces
{
    public interface IClassroomService
    {
         Task<ClassroomResponseDto> CreateClassroomAsync(CreateClassroomRequestDto classroomRequestDto); 
         Task<IEnumerable<ClassroomDetailResponseDto>> GetAllClassroomsAsync();
         Task<ClassroomDetailResponseDto> GetClassroomByIdAsync(Guid id);
         Task<ClassroomDetailResponseDto> UpdateClassroomAsync(Guid id, UpdateClassroomRequestDto classroomRequestDto);
         Task DeleteClassroomAsync(Guid id);
    } 
}