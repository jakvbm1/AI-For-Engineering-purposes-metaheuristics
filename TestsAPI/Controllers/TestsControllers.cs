using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestsAPI.Model;
using TestsAPI.Services;

namespace TestsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsControllers : ControllerBase
    {
        private readonly TestServices _testService;

        public TestsControllers(TestServices testService)
        {
            _testService = testService;
        }

        [HttpPost("create")]
        public IActionResult CreateTest([FromBody] Test test)
        {
            var newTest = _testService.CreateTest(test.AlgorithmName, test.FunctionName, test.Parameters);
            return Ok(new { newTest.Id });
        }

        [HttpPost("start/{id}")]
        public IActionResult StartTest(Guid id)
        {
            var test = _testService.StartTest(id);
            if (test == null) return NotFound("Test nie istnieje.");

            return Ok(new { message = "Test uruchomiony.", test.Status });
        }

        [HttpPost("stop/{id}")]
        public IActionResult StopTest(Guid id)
        {
            var test = _testService.StopTest(id);
            if (test == null) return NotFound("Test nie istnieje.");

            return Ok(new { message = "Test zatrzymany.", test.Status });
        }

        [HttpGet("status/{id}")]
        public IActionResult GetStatus(Guid id)
        {
            var test = _testService.GetStatus(id);
            if (test == null) return NotFound("Test nie istnieje.");

            return Ok(new { test.Id, test.Status });
        }

        [HttpGet("report/{id}")]
        public IActionResult GetReport(Guid id)
        {
            var report = _testService.GetReport(id);
            if (report == null) return NotFound("Test nie istnieje.");

            return Ok(report);
        }
    }
}
