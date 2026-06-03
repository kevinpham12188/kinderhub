using KinderHub.Enrollment.DTOs.Requests;
using KinderHub.Enrollment.DTOs.Responses;
using KinderHub.Enrollment.Exceptions;
using KinderHub.Enrollment.Helpers;
using KinderHub.Enrollment.Repositories.Interfaces;
using KinderHub.Enrollment.Services.Interfaces;

namespace KinderHub.Enrollment.Services
{
    public class ClassroomTeacherService : IClassroomTeacherService
    {
        private readonly IClassroomTeacherRepository _repository;
        private readonly IClassroomRepository _classroomRepository;
        public ClassroomTeacherService(IClassroomTeacherRepository repository, IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
            _repository = repository;
        }

        public async Task<ClassroomTeacherResponseDto> AssignTeacherToClassroomAsync(Guid classroomId, AssignTeacherRequestDto request)
        {
            // Check if classroom exists
            var classroom = await _classroomRepository.GetClassroomByIdAsync(classroomId);
            if (classroom == null)
            {
                throw new NotFoundException("Classroom not found");
            }

            // Check if teacher is already assigned to this classroom
            bool alreadyAssigned = await _repository.IsTeacherAssignedToClassroomAsync(classroomId, request.TeacherId);
            if (alreadyAssigned)
            {
                throw new ConflictException("Teacher is already assigned to this classroom");
            }

            // Check if teacher is assigned to another classroom (if needed based on business rules)
            bool assignedToAnotherClassroom = await _repository.IsTeacherAssignedToAnyClassroomAsync(request.TeacherId);
            if (assignedToAnotherClassroom)
            {
                throw new ConflictException("Teacher is already assigned to another classroom");
            }

            // Assign teacher to classroom
            var classroomTeacher = await _repository.AssignTeacherToClassroomAsync(classroomId, request.TeacherId);
            return new ClassroomTeacherResponseDto
            {
                Id = classroomTeacher.Id,
                TeacherId = classroomTeacher.TeacherId,
                ClassroomId = classroomTeacher.ClassroomId,
                CreatedAt = classroomTeacher.CreatedAt
            };
        }
        public async Task<IEnumerable<ClassroomTeacherResponseDto>> GetTeachersByClassroomIdAsync(Guid classroomId)
        {
            // Check if classroom exists
            var classroom = await _classroomRepository.GetClassroomByIdAsync(classroomId);
            if (classroom == null)
            {
                throw new NotFoundException("Classroom not found");
            }
            var teachers = await _repository.GetTeachersByClassroomIdAsync(classroomId);
            return teachers.Select(ct => new ClassroomTeacherResponseDto
            {
                Id = ct.Id,
                TeacherId = ct.TeacherId,
                ClassroomId = ct.ClassroomId,
                CreatedAt = ct.CreatedAt
            });           
        }

        public async Task RemoveTeacherFromClassroomAsync(Guid classroomId, Guid teacherId)
        {
            //Check if classroom exists
            var classroom = await _classroomRepository.GetClassroomByIdAsync(classroomId);
            if (classroom == null)
            {
                throw new NotFoundException("Classroom not found");
            }

            //Check if teacher is assigned to this classroom
            bool isAssigned = await _repository.IsTeacherAssignedToClassroomAsync(classroomId, teacherId);
            if (!isAssigned)            {
                throw new NotFoundException("Teacher is not assigned to this classroom");
            }
            // Get current teacher count after removal
            var currentTeacherCount = classroom.ClassroomTeachers.Count - 1; // Subtract 1 for the teacher being removed
            var currentEnrollment = classroom.Children.Count;
            var maxCapacity = EnrollmentRules.CalculateMaxCapacity(currentTeacherCount, classroom.AgeGroup);
            if (currentEnrollment > maxCapacity)
            {
                throw new ConflictException("Cannot remove teacher as it would put the classroom over capacity");
            }
            await _repository.RemoveTeacherFromClassroomAsync(classroomId, teacherId);
        }  
    }
}