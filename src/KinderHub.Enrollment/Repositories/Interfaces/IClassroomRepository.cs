using KinderHub.Enrollment.Models;

namespace KinderHub.Enrollment.Repositories.Interfaces
{
    public interface IClassroomRepository
    {
        Task<bool> NameExistsAsync(string name);
         Task<Classroom> CreateClassroomAsync(Classroom classroom);
         Task<IEnumerable<Classroom>> GetAllClassroomsAsync();
         Task<Classroom?> GetClassroomByIdAsync(Guid id);
         Task<Classroom> UpdateClassroomAsync(Classroom classroom);
         Task DeleteClassroomAsync(Guid id);       
    }
}