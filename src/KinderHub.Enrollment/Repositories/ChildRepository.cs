using KinderHub.Enrollment.Data;
using KinderHub.Enrollment.Models;
using KinderHub.Enrollment.Models.Enums;
using KinderHub.Enrollment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KinderHub.Enrollment.Repositories
{
    public class ChildRepository : IChildRepository
    {
        private readonly EnrollmentDbContext _context;
        public ChildRepository(EnrollmentDbContext context)
        {
            _context = context;
        }

        public async Task<Child> EnrollChildAsync(Child child)
        {
            _context.Children.Add(child);
            await _context.SaveChangesAsync();
            return child;
        }
        public async Task<IEnumerable<Child>> GetChildrenAsync()
        {
            return await _context.Children.Include(c => c.Classroom).ToListAsync();
        }
        public async Task<Child?> GetChildByIdAsync(Guid id)
        {
            return await _context.Children.Include(c=> c.Classroom).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Child> UpdateChildAsync(Child child)
        {
            _context.Children.Update(child);
            await _context.SaveChangesAsync();
            return child;
        }
        public async Task<IEnumerable<Child>> GetChildrenByClassroomIdAsync(Guid classroomId)
        {
            return await _context.Children.Include(c => c.Classroom).Where(c => c.ClassroomId == classroomId && c.Status == ChildStatus.Active).ToListAsync();
        }
        public async Task<int> CountChildrenInClassroomAsync(Guid classroomId)
        {
            return await _context.Children.CountAsync(c => c.ClassroomId == classroomId && c.Status == ChildStatus.Active);
        }

        public async Task<Child> WithdrawChildAsync(Guid id)
        {
            var child = await  _context.Children.Include(c => c.Classroom).FirstOrDefaultAsync(c => c.Id == id);
            child!.Status = ChildStatus.Withdrawn;
            child.ClassroomId = null;

            await _context.SaveChangesAsync();
            return child;

        }
        public async Task<IEnumerable<Child>> GetActiveChildrenWithClassroomAsync()
        {
            return await _context.Children
                .Include(c => c.Classroom)
                .Where(c => c.Status == ChildStatus.Active && c.ClassroomId != null)
                .ToListAsync(); 
        }
    }
}