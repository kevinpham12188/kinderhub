using KinderHub.Enrollment.DTOs.Requests;
using KinderHub.Enrollment.DTOs.Responses;

namespace KinderHub.Enrollment.Services.Interfaces
{
    public interface IClassroomTeacherService
    {
         Task<ClassroomTeacherResponseDto> AssignTeacherToClassroomAsync(Guid classroomId, AssignTeacherRequestDto request);
         Task<IEnumerable<ClassroomTeacherResponseDto>> GetTeachersByClassroomIdAsync(Guid classroomId);
         Task RemoveTeacherFromClassroomAsync(Guid classroomId, Guid teacherId);
    }
}