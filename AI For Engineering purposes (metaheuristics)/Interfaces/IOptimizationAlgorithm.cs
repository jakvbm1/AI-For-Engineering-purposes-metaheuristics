using AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.Interfaces
{
    public class ParamInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double UpperBoundary { get; set; }
        public double LowerBoundary { get; set; }
        public double DefaultValue { get; set; }

        public ParamInfo(string name, string description, double upperBoundary, double lowerBoundary, double defaultValue)
        {
            Name = name;
            Description = description;
            UpperBoundary = upperBoundary;
            LowerBoundary = lowerBoundary;
            DefaultValue = defaultValue;
        }
    }

    public interface IOptimizationAlgorithm
    {
        string Name { get; set; }
        ParamInfo[] ParamInfo { get; set; }
        IStateWriter writer { get; set; }
        IStateReader reader { get; set; }
        IGeneratePDFReport pdfReportGenerator { get; set; }
        public IGenerateTextReport stringReportGenerator { get; set; }
        double[] Xbest { get; set; }
        double Fbest { get; set; }
        int NumberOfEvaluationFitnessFunction {  get; set; }
        bool Running { get; set; }
        int CurrentIteration { get; set; }
        int TargetIteration { get; set; }

        void Solve(fitnessFunction f, double[,] domain, string functionName, params double[] parameters);
    }
}
