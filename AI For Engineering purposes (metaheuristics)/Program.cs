// See https://aka.ms/new-console-template for more information
using AI_For_Engineering_purposes__metaheuristics_.Metaheuristics;
using AI_For_Engineering_purposes_metaheuristics;

Console.WriteLine("Hello, World!");
var problem = new Rastrigin(2);
var optimizer = new PO(10, 100, problem.UpperBoundaries, problem.LowerBoundaries, problem.function, "rastrigin");
Console.WriteLine(optimizer.Solve());

