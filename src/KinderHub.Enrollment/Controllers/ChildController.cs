using KinderHub.Enrollment.DTOs.Requests;
using KinderHub.Enrollment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KinderHub.Enrollment.Controllers
{
    [ApiController]
    [Route("api/children")]
    public class ChildController : ControllerBase
    {
        private readonly IChildService _childService;

        public ChildController(IChildService childService)
        {
            _childService = childService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnrollChildAsync([FromBody] EnrollChildRequestDto request)
        {
            var result = await _childService.EnrollChildAsync(request);
            return Created(string.Empty, result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetChildrenAsync()
        {
            var result = await _childService.GetChildrenAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher,Parent")]
        public async Task<IActionResult> GetChildByIdAsync(Guid id)
        {
            var result = await _childService.GetChildByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateChildAsync(Guid id, [FromBody] UpdateChildRequestDto request)
        {
            var result = await _childService.UpdateChildAsync(id, request);
            return Ok(result);
        }
    }
}