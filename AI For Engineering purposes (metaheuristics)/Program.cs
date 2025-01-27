using AI_For_Engineering_purposes__metaheuristics_;
using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_functions;
using AI_For_Engineering_purposes__metaheuristics_.solver;


using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace AI_For_Engineering_purposes__metaheuristics_.main
{
    public class Program
    {
       
        

        
        public static void Main()
        {
            var wolf = new PumaOptimization();
            double[] parameters = new double[wolf.ParamInfo.Length];

            for (int i = 0; i < wolf.ParamInfo.Length; i++)
            {
                parameters[i] = wolf.ParamInfo[i].DefaultValue;
            }
            

            Solver.SolveAlgorithm(new PumaOptimization(), new Beale(), parameters);

        }
    }
}
