using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestsAPI.Model
{
    public class Test
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public IOptimizationAlgorithm Algorithm { get; set; } = default!;
        public IFunction Function { get; set; } = default!;
        public double[] Parameters { get; set; } = [];
        public TestStatus Status { get; private set; } = TestStatus.Created;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        Task? SolveTask { get; set; } = null;

        public void Start()
        {
            Algorithm.Running = true;
            Status = TestStatus.Running;

            SolveTask = Task.Run(() =>
            {
                Algorithm.Solve(Function.Function, Function.domain(), Function.Name, Parameters);

                if (Algorithm.Running)
                {
                    Status = TestStatus.Finished;
                }
                else
                {
                    Status = TestStatus.Paused;
                }

                SolveTask = null;
            });
        }

        public void Pause()
        {
            Algorithm.Running = false;
            Status = TestStatus.Pausing;
        }

        public void Dispose()
        {
            SolveTask?.Dispose();
            SolveTask = null;
        }
    }

    public enum TestStatus
    {
        Created = 1,
        Running = 2,
        Pausing = 3,
        Paused = 4,
        Finished = 5
    }
}
