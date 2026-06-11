using KinderHub.Enrollment.DTOs.Requests;
using KinderHub.Enrollment.DTOs.Responses;
using KinderHub.Enrollment.Exceptions;
using KinderHub.Enrollment.Helpers;
using KinderHub.Enrollment.Models;
using KinderHub.Enrollment.Models.Enums;
using KinderHub.Enrollment.Repositories.Interfaces;
using KinderHub.Enrollment.Services.Interfaces;

namespace KinderHub.Enrollment.Services
{
    public class ChildService : IChildService
    {
        private readonly IChildRepository _childRepo;
        private readonly IClassroomRepository _classroomRepo;
        private readonly IClassroomTeacherRepository _classroomTeacherRepo;
        public ChildService(IChildRepository childRepo, IClassroomRepository classroomRepo, IClassroomTeacherRepository classroomTeacherRepo)
        {
            _childRepo = childRepo;
            _classroomRepo = classroomRepo;
            _classroomTeacherRepo = classroomTeacherRepo;
        }

        public async Task<ChildResponseDto> EnrollChildAsync(EnrollChildRequestDto request)
        {
            // Validate classroom exists
            var classroom = await _classroomRepo.GetClassroomByIdAsync(request.ClassroomId);
            if(classroom == null)
            {
                throw new NotFoundException($"Classroom with ID {request.ClassroomId} not found.");
            }

            // Validate if classroom has teacher
            var classroomTeacher = await _classroomTeacherRepo.GetTeachersByClassroomIdAsync(request.ClassroomId);
            if(classroomTeacher == null || !classroomTeacher.Any())
            {
                throw new ConflictException($"Classroom with ID {request.ClassroomId} does not have an assigned teacher.");
            }

            // Calculate child's age group from Date of Birth
            var age = EnrollmentRules.CalculateAgeGroup(request.DateOfBirth);

            // Check age group matches classroom age group
            if(age != classroom.AgeGroup)
            {
                throw new AgeGroupMismatchException($"Child's age group {age} does not match classroom age group {classroom.AgeGroup}.");
            }

            // Count current children in classroom
            var currentChildren = await _childRepo.CountChildrenInClassroomAsync(request.ClassroomId);
            var maxCapacity = EnrollmentRules.CalculateMaxCapacity(classroomTeacher.Count(), classroom.AgeGroup);
            if(currentChildren >= maxCapacity)
            {
                throw new CapacityExceededException($"Classroom with ID {request.ClassroomId} has reached its maximum capacity of {maxCapacity} children.");
            }

            // Create child entity
            var child = new Child
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                EnrollmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ClassroomId = request.ClassroomId,
                ParentId = request.ParentId,
                IsPottyTrained = false,
                Status = ChildStatus.Active
            };
            await _childRepo.EnrollChildAsync(child);
            return MapToResponseDto(child);
            
        }

        public async Task<IEnumerable<ChildResponseDto>> GetChildrenAsync()
        {
            var children = await _childRepo.GetChildrenAsync();
            return children.Select(MapToResponseDto);
        }
        public async Task<ChildResponseDto> GetChildByIdAsync(Guid id)
        {
            var child = await _childRepo.GetChildByIdAsync(id);
            if(child == null)
            {
                throw new NotFoundException($"Child with ID {id} not found.");
            }
            return MapToResponseDto(child);
        }
        public async Task<ChildResponseDto> UpdateChildAsync(Guid id, UpdateChildRequestDto request)
        {
            var child = await _childRepo.GetChildByIdAsync(id);
            if(child == null)
            {
                throw new NotFoundException($"Child with ID {id} not found.");
            }

            // Update fields
            child.FirstName = request.FirstName;
            child.LastName = request.LastName;
            child.DateOfBirth = request.DateOfBirth;
            child.ParentId = request.ParentId;
            child.IsPottyTrained = request.IsPottyTrained;


            await _childRepo.UpdateChildAsync(child);
            return MapToResponseDto(child);
        }
        public async Task<IEnumerable<ChildResponseDto>> GetChildrenByClassroomIdAsync(Guid classroomId)
        {
            var children = await _childRepo.GetChildrenByClassroomIdAsync(classroomId);
            return children.Select(MapToResponseDto);
        }

        public async Task<ChildResponseDto> WithdrawChildAsync(Guid id)
        {
            var child = await _childRepo.GetChildByIdAsync(id);
            if(child == null)
            {
                throw new NotFoundException($"Child with ID {id} not found.");
            }
            if(child.Status != ChildStatus.Active)
            {
                throw new ConflictException("Child is not active and cannot be withdrawn");
            }
            var updatedChild = await _childRepo.WithdrawChildAsync(id);
            return MapToResponseDto(updatedChild);
        }

        //age mismatch flag for dashboard
        public async Task<IEnumerable<AgeMismatchResponseDto>> GetAgeMismatchesAsync()
        {
            var children = await _childRepo.GetActiveChildrenWithClassroomAsync();
            var mismatches = new List<AgeMismatchResponseDto>();

            foreach(var child in children)
            {
                var calculatedAgeGroup = EnrollmentRules.CalculateAgeGroup(child.DateOfBirth);

                var classroomAgeGroup = child.Classroom!.AgeGroup;

                if(calculatedAgeGroup == classroomAgeGroup) continue;

                if(calculatedAgeGroup == AgeGroup.Preschool && classroomAgeGroup == AgeGroup.Twaddler && !child.IsPottyTrained) continue;

                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var ageInMonths = ((today.Year - child.DateOfBirth.Year) * 12)
                        + (today.Month - child.DateOfBirth.Month);

                mismatches.Add(new AgeMismatchResponseDto
                {
                    ChildId = child.Id,
                    ChildName = $"{child.FirstName} {child.LastName}",
                    DateOfBirth = child.DateOfBirth,
                    AgeInMonths = ageInMonths,
                    CurrentAgeGroup = classroomAgeGroup.ToString(),
                    RecommendedAgeGroup = calculatedAgeGroup.ToString(),
                    CurrentClassroomId = child.ClassroomId!.Value,
                    CurrentClassroomName = child.Classroom.Name
                });       
            }
            return mismatches;
        }
    
        private static ChildResponseDto MapToResponseDto(Child child)
        {
            return new ChildResponseDto
            {
                Id = child.Id,
                FirstName = child.FirstName,
                LastName = child.LastName,
                DateOfBirth = child.DateOfBirth,
                AgeGroup = EnrollmentRules.CalculateAgeGroup(child.DateOfBirth).ToString(),
                EnrollmentDate = child.EnrollmentDate,
                Status = child.Status.ToString(),
                ClassroomId = child.ClassroomId,
                ClassroomName = child.Classroom?.Name,
                ParentId = child.ParentId,
                IsPottyTrained = child.IsPottyTrained,
                CreatedAt = child.CreatedAt
            };
        }
         
    }

}