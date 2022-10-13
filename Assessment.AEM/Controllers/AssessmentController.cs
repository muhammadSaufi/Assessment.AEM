using Assessment.AEM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.AEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly AssessmentService _assessmentService;
        public AssessmentController(AssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        [HttpPost]
        [Route("SavePlatformActualData")]
        public async Task<IActionResult> SavePlatformActualData(string loginusername, string loginpassword)
        {
            var response = await _assessmentService.SavePlatform(loginusername, loginpassword);
            return Ok(response);
        }
    }
}
