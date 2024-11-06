using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_
{
    interface TestFunction
    {
        int NDimension { get; set; }
        double[] LowerBoundaries { get; }
        double[] UpperBoundaries { get; }

        string Name {  get; }
        double function(double[] args);

        public static double[] FilledArray(int size, double value)
        {
            double[] a = new double[size];
            Array.Fill(a, value);
            return a;
        }
    }
}
