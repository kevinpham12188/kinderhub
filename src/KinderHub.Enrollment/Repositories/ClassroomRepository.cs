using KinderHub.Enrollment.Data;
using KinderHub.Enrollment.Models;
using KinderHub.Enrollment.Models.Enums;
using KinderHub.Enrollment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KinderHub.Enrollment.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly EnrollmentDbContext _context;

        public ClassroomRepository(EnrollmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            string normalizedName = name.Trim().ToLower();
            return await _context.Classrooms.AnyAsync(c => c.Name.ToLower() == normalizedName);
        }
        
        public async Task<Classroom> CreateClassroomAsync(Classroom classroom)
        {
            await _context.Classrooms.AddAsync(classroom);
            await _context.SaveChangesAsync();
            return classroom;
        }
        public async Task<IEnumerable<Classroom>> GetAllClassroomsAsync()
        {
            return await _context.Classrooms
                .Include(c => c.ClassroomTeachers)
                .Include(c => c.Children.Where(child => child.Status == ChildStatus.Active))
                .ToListAsync();
        }
        public async Task<Classroom?> GetClassroomByIdAsync(Guid id)
        {
            return await _context.Classrooms
                .Include(c => c.ClassroomTeachers)
                .Include(c => c.Children.Where(child => child.Status == ChildStatus.Active))
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Classroom> UpdateClassroomAsync(Classroom classroom)
        {
            _context.Classrooms.Update(classroom);
            await _context.SaveChangesAsync();
            return classroom;
        }
        public async Task DeleteClassroomAsync(Guid id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom != null)
            {
                _context.Classrooms.Remove(classroom);
                await _context.SaveChangesAsync();
            }
        }
    }
}