using KinderHub.Enrollment.Models;

namespace KinderHub.Enrollment.Repositories.Interfaces
{
    public interface IChildRepository
    {
         Task<Child> EnrollChildAsync(Child child);
         Task<IEnumerable<Child>> GetChildrenAsync();
         Task<Child?> GetChildByIdAsync(Guid id);
         Task<Child> UpdateChildAsync(Child child);
         Task<IEnumerable<Child>> GetChildrenByClassroomIdAsync(Guid classroomId);
         Task<int> CountChildrenInClassroomAsync(Guid classroomId);
         Task<Child> WithdrawChildAsync(Guid id);
         Task<IEnumerable<Child>> GetActiveChildrenWithClassroomAsync();
    }
}