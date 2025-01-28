using TestsAPI.Model;
using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_functions;
using AI_For_Engineering_purposes__metaheuristics_.solver;



namespace TestsAPI.Services
{
    public class TestServices
    {
        private readonly List<Test> _tests = [];
        private readonly Timer timer;
        PumaOptimization puma = new PumaOptimization();

        public TestServices()
        {
           
        }
        public Test CreateTest(string algorithmName, string functionName, List<int> parameters)
        {
            var test = new Test{AlgorithmName = algorithmName, FunctionName = functionName, Parameters = parameters };
            _tests.Add(test);
            return test;
        }
        public Test StartTest(Guid id) { 
            var test = _tests.FirstOrDefault(x => x.Id == id);
            if (test != null) test.Status = "running";
           
            var wolf = new PumaOptimization();
            double[] parameters = new double[wolf.ParamInfo.Length];
            for (int i = 0; i < wolf.ParamInfo.Length; i++)
            {
                parameters[i] = wolf.ParamInfo[i].DefaultValue;
            }
            parameters[0] = 9999;
            Solver.SolveAlgorithm(new PumaOptimization(), new Beale(), parameters);
            return test;
        }
        public Test StopTest(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);
            if (test != null) test.Status = "stopped";
            PumaOptimization.running = false;
            return test;
        }
        public Test ResumeTest(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);
            if (test != null) test.Status = "running";
            PumaOptimization.running = true;
            return test;
        }
        public Test GetStatus(Guid id)
        {
            return _tests.FirstOrDefault(t => t.Id == id);
        }
        public object GetReport(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);
            return "report";
        }
        private void RemoveOldTests(object state)
        {
            var now = DateTime.UtcNow;
            _tests.RemoveAll(t => (now - t.CreatedAt).TotalHours >= 24);
        }
    }
}
