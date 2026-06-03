using KinderHub.Enrollment.DTOs.Requests;
using KinderHub.Enrollment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KinderHub.Enrollment.Controllers
{
    [ApiController]
    [Route("api/classrooms")]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomService _classroomService;
        public ClassroomController(IClassroomService classroomService)
        {
            _classroomService = classroomService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateClassroomAsync([FromBody] CreateClassroomRequestDto request)
        {
            var result = await _classroomService.CreateClassroomAsync(request);
            return Created(string.Empty, result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAllClassroomsAsync()
        {
            var result = await _classroomService.GetAllClassroomsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetClassroomByIdAsync(Guid id)
        {
            var result = await _classroomService.GetClassroomByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClassroomAsync(Guid id, [FromBody] UpdateClassroomRequestDto request)
        {            
            var result = await _classroomService.UpdateClassroomAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClassroomAsync(Guid id)
        {
            await _classroomService.DeleteClassroomAsync(id);
            return NoContent();
        }
    }
}