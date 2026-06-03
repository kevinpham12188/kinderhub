using KinderHub.Enrollment.Data;
using KinderHub.Enrollment.Models;
using KinderHub.Enrollment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KinderHub.Enrollment.Repositories
{
    public class ClassroomTeacherRepository : IClassroomTeacherRepository
    {
        private readonly EnrollmentDbContext _context;

        public ClassroomTeacherRepository(EnrollmentDbContext context)
        {
            _context = context;
        }

        public async Task<ClassroomTeacher> AssignTeacherToClassroomAsync(Guid classroomId, Guid teacherId)
        {
            var classroomTeacher = new ClassroomTeacher
            {
                Id = Guid.NewGuid(),
                ClassroomId = classroomId,
                TeacherId = teacherId
            };
            await _context.ClassroomTeachers.AddAsync(classroomTeacher);
            await _context.SaveChangesAsync();
            return classroomTeacher;
        }
        public async Task<IEnumerable<ClassroomTeacher>> GetTeachersByClassroomIdAsync(Guid classroomId)
        {
            return await _context.ClassroomTeachers.Where(ct => ct.ClassroomId == classroomId).ToListAsync();
        }
        public async Task RemoveTeacherFromClassroomAsync(Guid classroomId, Guid teacherId)
        {
            var classroomTeacher = await GetClassroomTeacherAsync(classroomId, teacherId);
            if (classroomTeacher != null)
            {
                _context.ClassroomTeachers.Remove(classroomTeacher);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsTeacherAssignedToClassroomAsync(Guid classroomId, Guid teacherId)
        {
            return await _context.ClassroomTeachers.AnyAsync(ct => ct.ClassroomId == classroomId && ct.TeacherId == teacherId);
        }
        public async Task<bool> IsTeacherAssignedToAnyClassroomAsync(Guid teacherId)
        {
            return await _context.ClassroomTeachers.AnyAsync(ct => ct.TeacherId == teacherId);
        }
        public async Task<ClassroomTeacher?> GetClassroomTeacherAsync(Guid classroomId, Guid teacherId)
        {
            return await _context.ClassroomTeachers.FirstOrDefaultAsync(ct => ct.ClassroomId == classroomId && ct.TeacherId == teacherId);
        }
    }
}