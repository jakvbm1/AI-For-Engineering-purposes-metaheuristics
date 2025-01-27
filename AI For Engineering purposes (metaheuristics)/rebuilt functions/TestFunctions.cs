using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.rebuilt_functions
{
    class BentCigar : IFunction
    {
        public string Name => "Bent Cigar";

        public fitnessFunction Function => bent;

        public bool IsMultiDimensional => false;

        public double[,] domain(int dimension = 2)
        {
            if (dimension != 2)
            {
                throw new Exception("Funkcja jest jedynie dwuwymiarowa");
            }

            else
            {
                return IFunction.domainGenerator(10, -10);
            }
        }

        private double bent(double[] args)
        {
            int n = args.Length;
            double sum = 0;
            for (int i = 1; i < n; i++)
                sum += args[i] * args[i];
            return args[0] * args[0] + Math.Pow(10, 6) * sum;
        }
    }

    class Rosenbrock : IFunction
    {
        public string Name => "Rosenbrock";

        public fitnessFunction Function => function;

        public bool IsMultiDimensional => true;

        private double function(double[] args)
        {
            int n = args.Length;
            double sum = 0;
            for (int i = 0; i < n - 1; i++)
                sum += 100 * Math.Pow(args[i + 1] - args[i] * args[i], 2) + Math.Pow(1 - args[i], 2);
            return sum;
        }

        public double[,] domain(int dimension = 2)
        {
            return IFunction.domainGenerator(5.12f, -5.12f, dimension);
        }
    }

    class Rastrigin : IFunction
    {

        public string Name => "Rastrigin";

        public fitnessFunction Function => function;

        public bool IsMultiDimensional => true;

        private double function(double[] args)
        {
            int n = args.Length;
            double sum = 0;
            for (int i = 0; i < n; i++)
                sum += args[i] * args[i] - 10 * Math.Cos(2 * Math.PI * args[i]);
            return 10 * n + sum;
        }

        public double[,] domain(int dimension = 2)
        {
            return IFunction.domainGenerator(5.12f, -5.12f, dimension);
        }
    }

    class Sphere : IFunction
    {

        private int nDimension;

        public string Name => "Sphere";

        public fitnessFunction Function => function;

        public bool IsMultiDimensional => true;

        private double function(double[] args)
        {
            double sum = 0;
            for (int i = 0; i < args.Length; i++)
            {
                sum += args[i] * args[i];
            }
            return sum;
        }
        public double[,] domain(int dimension = 2)
        {
            return IFunction.domainGenerator(5.12f, -5.12f, dimension);
        }
    }

    class UnknownFunction : IFunction
    {

        public string Name => "UnknownFunction";

        public fitnessFunction Function => function;

        public bool IsMultiDimensional => true;

        private double function(double[] args)
        {
            int n = args.Length;
            double sum = 0;
            for (int i = 0; i < n; i++)
                sum += Math.Abs(args[i] * Math.Sin(args[i]) + 0.1 * args[i]);
            return sum;
        }

        public double[,] domain(int dimension = 2)
        {
            return IFunction.domainGenerator(10, -10, dimension);
        }
    }

    class Eggholder : IFunction
    {
        public string Name => "Eggholder";

        public fitnessFunction Function => function;

        public bool IsMultiDimensional => false;

        private double function(double[] args)
        {
            return -(args[1] + 47) * Math.Sin(Math.Sqrt(Math.Abs((args[0] / 2) + (args[1] + 47)))) - args[0] * Math.Sin(Math.Sqrt(Math.Abs(args[0] + (args[1] + 47))));
        }

        public double[,] domain(int dimension = 2)
        {
            if (dimension != 2)
            {
                throw new Exception("Funkcja jest jedynie dwuwymiarowa");
            }

            else
            {
                return IFunction.domainGenerator(512, -512);
            }
        } 
    }

    public class Beale : IFunction
    {

        public string Name => "Beale";

        public fitnessFunction Function => function;

        public bool IsMultiDimensional => false;

        private double function(double[] args)
        {
            return Math.Pow(1.4 - args[0] + args[0] * args[1], 2) + Math.Pow(2.25 - args[0] + args[0] * args[1] * args[1], 2) + Math.Pow(2.625 - args[0] + args[0] * Math.Pow(args[1], 3), 2);
        }

        public double[,] domain(int dimension = 2)
        {
            if (dimension != 2)
            {
                throw new Exception("Funkcja jest jedynie dwuwymiarowa");
            }

            else
            {
                return IFunction.domainGenerator(4.5f, -4.5f);
            }
        }


        class BukinN6 : IFunction
        {

            public double[] LowerBoundaries => new double[] { -15, -3 };

            public double[] UpperBoundaries => new double[] { -5, 3 };

            public string Name => "Bukin function N.6";

            public fitnessFunction Function => function;

            public bool IsMultiDimensional => false;

            private double function(double[] args)
            {
                return 100 * Math.Sqrt(Math.Abs(args[1] - 0.01 * args[0] * args[0])) + 0.01 * Math.Abs(args[0] + 10);
            }

            public double[,] domain(int dimension = 2)
            {
                if (dimension != 2)
                {
                    throw new Exception("Funkcja jest jedynie dwuwymiarowa");
                }

                else
                {
                    double[,] domain = new double[2, 2];
                    domain[0, 0] = -15;
                    domain[0, 1] = -3;
                    domain[1, 0] = -5;
                    domain[1, 0] = 3;
                    return domain;
                }
            }
        }

        public class Himmelblaus : IFunction
        {
            public string Name => "Himmelblau's function";

            public fitnessFunction Function => function;

            public bool IsMultiDimensional => false;

            private double function(double[] args)
            {
                return Math.Pow(args[0] * args[0] + args[1] - 11, 2) + Math.Pow(args[0] + args[1] * args[1] - 7, 2);
            }

            public double[,] domain(int dimension = 2)
            {
                if (dimension != 2)
                {
                    throw new Exception("Funkcja jest jedynie dwuwymiarowa");
                }

                else
                {
                    return IFunction.domainGenerator(5, -5);
                }
            }
            //beale, bukin, himmelbau's,  bez eggholdera i BentCigara 

        }
    }
}
