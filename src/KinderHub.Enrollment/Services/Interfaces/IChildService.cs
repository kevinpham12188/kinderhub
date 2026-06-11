using KinderHub.Enrollment.DTOs.Requests;
using KinderHub.Enrollment.DTOs.Responses;

namespace KinderHub.Enrollment.Services.Interfaces
{
    public interface IChildService
    {
         Task<ChildResponseDto> EnrollChildAsync(EnrollChildRequestDto request);
         Task<IEnumerable<ChildResponseDto>> GetChildrenAsync();
         Task<ChildResponseDto> GetChildByIdAsync(Guid id);
         Task<ChildResponseDto> UpdateChildAsync(Guid id, UpdateChildRequestDto request);
         Task<IEnumerable<ChildResponseDto>> GetChildrenByClassroomIdAsync(Guid classroomId);
         Task<ChildResponseDto> WithdrawChildAsync(Guid id);

        //age mismatch flag for dashboard
         Task<IEnumerable<AgeMismatchResponseDto>> GetAgeMismatchesAsync();
    }
}