using global::UniversityTasksDbFirstApi.DTOs;
using global::UniversityTasksDbFirstApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace UniversityTasksDbFirstApi.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionsController : ControllerBase
    {
        private readonly SubmissionService _submissionService;

        public SubmissionsController(SubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [HttpPost]
        public async Task<ActionResult<SubmissionDto>> CreateSubmission(CreateSubmissionDto dto)
        {
            var result = await _submissionService.CreateAsync(dto);

            return result.StatusCode switch
            {
                201 => Created($"/api/submissions/{result.Data!.IdSubmission}", result.Data),
                400 => BadRequest(result.Error),
                404 => NotFound(result.Error),
                409 => Conflict(result.Error),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{idSubmission:int}/grade")]
        public async Task<ActionResult<SubmissionDto>> GradeSubmission(
            int idSubmission,
            GradeSubmissionDto dto)
        {
            var result = await _submissionService.GradeAsync(idSubmission, dto);

            return result.StatusCode switch
            {
                200 => Ok(result.Data),
                400 => BadRequest(result.Error),
                404 => NotFound(result.Error),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("{idSubmission:int}")]
        public async Task<IActionResult> DeleteSubmission(int idSubmission)
        {
            var result = await _submissionService.DeleteAsync(idSubmission);

            return result.StatusCode switch
            {
                204 => NoContent(),
                400 => BadRequest(result.Error),
                404 => NotFound(result.Error),
                _ => StatusCode(500)
            };
        }
    }
}
