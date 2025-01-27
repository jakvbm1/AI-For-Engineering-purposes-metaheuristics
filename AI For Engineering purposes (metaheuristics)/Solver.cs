using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.solver
{
    public class Solver
    {
        public static void SolveAlgorithm(IOptimizationAlgorithm algorithm, IFunction prblm, double[] parameters)
        {
            algorithm.Solve(prblm.Function, prblm.domain(), prblm.Name, parameters);
            double fbest = algorithm.Fbest;
            double[] xbests = algorithm.Xbest;


        }

    }
}
