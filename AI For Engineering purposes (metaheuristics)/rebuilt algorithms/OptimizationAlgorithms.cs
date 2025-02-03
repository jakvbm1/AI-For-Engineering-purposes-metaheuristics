using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms
{
    public class OptimizationAlgorithms
    {
        public static IOptimizationAlgorithm[] Algorithms
        {
            get => [
                new GreyWolfOptimization(),
                new PumaOptimization(),
            ];
        }
    }
}
