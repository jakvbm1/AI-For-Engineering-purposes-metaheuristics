using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.Interfaces
{
    interface IFunction
    {
        public fitnessFunction Function { get; }

        public float[,] domain(int dimension = 2);

        public string Name { get; }

        public bool IsMultiDimensional { get; }

        public static double[] FilledArray(int size, double value)
        {
            double[] a = new double[size];
            Array.Fill(a, value);
            return a;
        }

        public static float[,] domainGenerator( float upper, float lower, int dimension = 2) 
        {
            float[, ] domain = new float[2, dimension];
            for (int i = 0; i < dimension; i++) 
            {
                domain[0, i] = lower;
                domain[1, i] = upper;
            }
            return domain;
        }

    }
}
