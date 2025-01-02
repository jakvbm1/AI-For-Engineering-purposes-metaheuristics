using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms
{
    internal class PumaOptimization : IOptimizationAlgorithm
    {
        private string name = "Puma Optimization Algorithm";
        private double fbest;
        private double[] xbest;
        private ParamInfo[] paramInfo;
        private int n_call = 0;

        public string Name { get => name; set => name = value; }
        public ParamInfo[] ParamInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IStateWriter writer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IStateReader reader { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IGeneratePDFReport pdfReportGenerator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IGenerateTextReport stringReportGenerator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double[] Xbest { get => xbest; set => xbest = value; }
        public double Fbest { get => fbest; set => fbest = value; }
        public int NumberOfEvaluationFitnessFunction { get => n_call; set => n_call = value; }

        public void Solve(fitnessFunction f, double[,] domain, params double[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
