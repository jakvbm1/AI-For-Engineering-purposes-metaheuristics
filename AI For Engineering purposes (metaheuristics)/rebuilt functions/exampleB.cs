using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.rebuilt_functions
{
    internal class exampleB : IFunction
    {
        ObjectiveFunction of = new ObjectiveFunction();


        public fitnessFunction Function => of.FunkcjaCelu.Wartosc;

        public string Name => "example B";

        public bool IsMultiDimensional => false;

        public int Dimensions { get; set; } = 3;

        public double[,] domain()
        {
            double[,] dom = new double[,] { { -100, -100, -100 }, { 100, 100, 100 } };
            return dom;
        }
    }
}
