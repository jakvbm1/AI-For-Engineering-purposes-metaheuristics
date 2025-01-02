using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.Interfaces
{
    class ParamInfo
    {
        string Name { get; set; }
        string Description { get; set; }
        double UpperBoundary { get; set; }
        double LowerBoundary { get; set; }
        double DefaultValue { get; set; }

        public ParamInfo(string name, string description, double upperBoundary, double lowerBoundary, double defaultValue)
        {
            Name = name;
            Description = description;
            UpperBoundary = upperBoundary;
            LowerBoundary = lowerBoundary;
            DefaultValue = defaultValue;
        }
    }

    internal interface IOptimizationAlgorithm
    {
        string Name { get; set; }
        ParamInfo[] ParamInfo { get; set; }
        IStateWriter writer { get; set; }
        IStateReader reader { get; set; }
        IGeneratePDFReport pdfReportGenerator { get; set; }
        IGenerateTextReport stringReportGenerator { get; set; }
        double[] Xbest { get; set; }
        double Fbest { get; set; }
        int NumberOfEvaluationFitnessFunction {  get; set; }

        void Solve(fitnessFunction f, double[,] domain, params double[] parameters);
    }
}
