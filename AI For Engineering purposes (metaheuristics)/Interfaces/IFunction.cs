using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.Interfaces
{
    public interface IFunction
    {
        public fitnessFunction Function { get; }

        public double[,] domain();

        public string Name { get; }

        public bool IsMultiDimensional { get; }

        public int Dimensions { set; get; }

        public static double[] FilledArray(int size, double value)
        {
            double[] a = new double[size];
            Array.Fill(a, value);
            return a;
        }

        public static double[,] domainGenerator( float upper, float lower, int dimension = 2) 
        {
            double [, ] domain = new double[2, dimension];
            for (int i = 0; i < dimension; i++) 
            {
                domain[0, i] = lower;
                domain[1, i] = upper;
            }
            return domain;
        }

    }
}
