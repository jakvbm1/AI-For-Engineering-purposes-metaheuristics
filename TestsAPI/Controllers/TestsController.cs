using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_functions;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestsAPI.Model;
using TestsAPI.Services;

namespace TestsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly TestServices _testService;

        public TestsController(TestServices testService)
        {
            _testService = testService;
        }

        [HttpGet("algorithms")]
        public IActionResult GetAlgorithms()
        {
            var responseAlgorithms = OptimizationAlgorithms
                .Algorithms
                .Select(a => new ResponseAlgorithm { Name = a.Name, ParamInfo =  a.ParamInfo });

            return Ok(responseAlgorithms);
        }

        [HttpGet("functions")]
        public IActionResult GetFunctions()
        {
            var responseFunctions = TestFunctions
                .Functions
                .Select(f => new ResponseFunction { Name = f.Name, IsMultiDimensional = f.IsMultiDimensional, DefaultDimensions = f.Dimensions });

            return Ok(responseFunctions);
        }

        [HttpPost("create")]
        public IActionResult CreateTest([FromBody] CreateTestBody body)
        {
            var newTest = _testService.CreateTest(body.AlgorithmName, body.FunctionName, body.Parameters, body.Dimensions, body.Iterations);
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

            return Ok(new { test.Id, test.Status, test.Algorithm.CurrentIteration, test.Algorithm.Xbest, test.Algorithm.Fbest });
        }

        [HttpGet("report/{id}")]
        public IActionResult GetReport(Guid id)
        {
            var report = _testService.GetReport(id);
            if (report == null) return NotFound("Test nie istnieje.");

            return Ok(report);
        }

        [HttpGet("pdf-report/{id}")]
        public IActionResult GetPdfReport(Guid id)
        {
            var report = _testService.GetPdfReport(id);
            if (report == null) return NotFound("Test nie istnieje.");

            return File(report, "application/pdf", "report.pdf");
        }
    }

    public class ResponseFunction
    {
        public string Name { get; set; }
        public bool IsMultiDimensional { get; set; }
        public int DefaultDimensions { get; set; }
    }

    public class ResponseAlgorithm
    {
        public string Name { get; set; }
        public ParamInfo[] ParamInfo { get; set; }
    }

    public class CreateTestBody
    {
        public string AlgorithmName { get; set; }
        public string FunctionName { get; set; }
        public double[] Parameters { get; set; }
        public int Dimensions { get; set; }
        public int Iterations { get; set; }
    }
}
