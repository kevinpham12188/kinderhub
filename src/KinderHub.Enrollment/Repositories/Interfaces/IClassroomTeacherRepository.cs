using KinderHub.Enrollment.Models;

namespace KinderHub.Enrollment.Repositories.Interfaces
{
    public interface IClassroomTeacherRepository
    {
         Task<ClassroomTeacher> AssignTeacherToClassroomAsync(Guid classroomId, Guid teacherId);
         Task<IEnumerable<ClassroomTeacher>> GetTeachersByClassroomIdAsync(Guid classroomId);
         Task RemoveTeacherFromClassroomAsync(Guid classroomId, Guid teacherId);
         Task<bool> IsTeacherAssignedToClassroomAsync(Guid classroomId, Guid teacherId);
         Task<bool> IsTeacherAssignedToAnyClassroomAsync(Guid teacherId);
         Task<ClassroomTeacher?> GetClassroomTeacherAsync(Guid classroomId, Guid teacherId);
    }
}