using TestsAPI.Model;


namespace TestsAPI.Services
{
    public class TestServices
    {
        private readonly List<Test> _tests;
        private readonly Timer timer;

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
            return test;
        }
        public Test StopTest(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);
            if (test != null) test.Status = "stopped";
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
