using TestsAPI.Model;
using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_functions;



namespace TestsAPI.Services
{
    public class TestServices
    {
        private readonly List<Test> _tests = [];

        public Test CreateTest(string algorithmName, string functionName, double[] parameters, int dimensions, int iterations)
        {
            var algorithm = OptimizationAlgorithms.Algorithms.FirstOrDefault(a => a.Name == algorithmName);
            var function = TestFunctions.Functions.FirstOrDefault(f => f.Name == functionName);

            if (algorithm == null || function == null) throw new Exception("Algorithm or function not found");

            algorithm.TargetIteration = iterations;
            function.Dimensions = dimensions;
            var test = new Test{ Algorithm = algorithm, Function = function, Parameters = parameters };
            _tests.Add(test);
            return test;
        }
        public Test StartTest(Guid id) { 
            var test = _tests.FirstOrDefault(x => x.Id == id);

            if (test == null) throw new Exception("Test not found");
            if (test.Status == TestStatus.Running || test.Status == TestStatus.Pausing) throw new Exception("Test is already running");

            test.Start();

            return test;
        }
        public Test StopTest(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);

            if (test == null) throw new Exception("Test not found");

            test.Pause();
            return test;
        }
        public Test GetStatus(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);

            if (test == null) throw new Exception("Test not found");

            return test;
        }
        public object GetReport(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);

            if (test == null) throw new Exception("Test not found");

            return test.Algorithm.stringReportGenerator.ReportString;
        }

        public byte[] GetPdfReport(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);

            if (test == null) throw new Exception("Test not found");

            var path = Directory.GetCurrentDirectory() + "/reports/" + id.ToString() + ".pdf";
            test.Algorithm.pdfReportGenerator.GenerateReport(path);
            return File.ReadAllBytes(path);
        }
        private void RemoveOldTests(object state)
        {
            var now = DateTime.UtcNow;
            _tests.RemoveAll(t => (now - t.CreatedAt).TotalHours >= 24);
        }
    }
}
