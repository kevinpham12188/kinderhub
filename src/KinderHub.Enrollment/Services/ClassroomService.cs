using KinderHub.Enrollment.DTOs.Requests;
using KinderHub.Enrollment.DTOs.Responses;
using KinderHub.Enrollment.Exceptions;
using KinderHub.Enrollment.Helpers;
using KinderHub.Enrollment.Models;
using KinderHub.Enrollment.Repositories.Interfaces;
using KinderHub.Enrollment.Services.Interfaces;

namespace KinderHub.Enrollment.Services
{
    public class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;
        public ClassroomService(IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
        }

        public async Task<ClassroomResponseDto> CreateClassroomAsync(CreateClassroomRequestDto classroomRequestDto)
        {
            bool nameExists = await _classroomRepository.NameExistsAsync(classroomRequestDto.Name);
            if (nameExists)
            {
                throw new ConflictException("A classroom with the same name already exists");
            }
            Classroom classroom = new Classroom
            {
                Id = Guid.NewGuid(),
                Name = classroomRequestDto.Name.Trim(),
                AgeGroup = classroomRequestDto.AgeGroup
            };
            Classroom createdClassroom = await _classroomRepository.CreateClassroomAsync(classroom);
            return new ClassroomResponseDto
            {
                Id = createdClassroom.Id,
                Name = createdClassroom.Name,
                AgeGroup = createdClassroom.AgeGroup.ToString(),
                MaxCapacityPerTeacher = EnrollmentRules.CalculateMaxCapacity(1, createdClassroom.AgeGroup),
                CreatedAt = createdClassroom.CreatedAt
            };
        } 
        public async Task<IEnumerable<ClassroomDetailResponseDto>> GetAllClassroomsAsync()
        {
            var classrooms = await _classroomRepository.GetAllClassroomsAsync();
            
            return classrooms.Select(c => 
            {
                var teacherCount = c.ClassroomTeachers.Count;
                var currentEnrollment = c.Children.Count;
                var maxCapacity = EnrollmentRules.CalculateMaxCapacity(teacherCount, c.AgeGroup);
                return new ClassroomDetailResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    AgeGroup = c.AgeGroup.ToString(),
                    TeacherCount = teacherCount,
                    CurrentEnrollment = c.Children.Count,
                    MaxCapacity = maxCapacity,
                    CreatedAt = c.CreatedAt
                };
            });   
        }
        public async Task<ClassroomDetailResponseDto> GetClassroomByIdAsync(Guid id)
        {
            var classroom = await _classroomRepository.GetClassroomByIdAsync(id);
            if (classroom == null)
            {
                throw new NotFoundException("Classroom not found");
            }
            var teacherCount = classroom.ClassroomTeachers.Count;
            var currentEnrollment = classroom.Children.Count;
            var maxCapacity = EnrollmentRules.CalculateMaxCapacity(teacherCount, classroom.AgeGroup);
            return new ClassroomDetailResponseDto
            {
                Id = classroom.Id,
                Name = classroom.Name,
                AgeGroup = classroom.AgeGroup.ToString(),
                TeacherCount = teacherCount,
                CurrentEnrollment = currentEnrollment,
                MaxCapacity = maxCapacity,
                CreatedAt = classroom.CreatedAt
            };
        }
        public async Task<ClassroomDetailResponseDto> UpdateClassroomAsync(Guid id, UpdateClassroomRequestDto classroomRequestDto)
        {
            var classroom = await _classroomRepository.GetClassroomByIdAsync(id);
            if(classroom == null)
            {
                throw new NotFoundException("Classroom not found");
            }
            bool nameExists = await _classroomRepository.NameExistsAsync(classroomRequestDto.Name);
            if(nameExists && classroom.Name.ToLower() != classroomRequestDto.Name.Trim().ToLower())
            {
                throw new ConflictException("A classroom with the same name already exists");
            }
            classroom.Name = classroomRequestDto.Name.Trim();
            await _classroomRepository.UpdateClassroomAsync(classroom);
            return new ClassroomDetailResponseDto
            {
                Id = classroom.Id,
                Name = classroom.Name,
                AgeGroup = classroom.AgeGroup.ToString(),
                TeacherCount = classroom.ClassroomTeachers.Count,
                CurrentEnrollment = classroom.Children.Count,
                MaxCapacity = EnrollmentRules.CalculateMaxCapacity(classroom.ClassroomTeachers.Count, classroom.AgeGroup),
                CreatedAt = classroom.CreatedAt
            };
        }
        public async Task DeleteClassroomAsync(Guid id)
        {
            var classroom = await _classroomRepository.GetClassroomByIdAsync(id);
            if (classroom == null)
            {
                throw new NotFoundException("Classroom not found");
            }
            if(classroom.Children.Any())
            {
                throw new ConflictException("Cannot delete classroom with active children enrolled");
            }
            await _classroomRepository.DeleteClassroomAsync(id);
        }
    }
}