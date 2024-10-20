namespace AI_For_Engineering_purposes_metaheuristics
{
    public class TestFunctions
    {
        public static double[] BentCigarLowerBoundaries(int dimensions)
        {
            return FilledArray(dimensions, -10);
        }

        public static double[] BentCigarUpperBoundaries(int dimensions)
        {
            return FilledArray(dimensions, 10);
        }

        public static double BentCigarFunction(double[] x)
        {
            int n = x.Length;
            double sum = 0;
            for (int i = 1; i < n; i++)
                sum += x[i] * x[i];
            return x[0] * x[0] + Math.Pow(10, 6) * sum;
        }

        public static double[] RosenbrockLowerBoundaries(int dimensions)
        {
            return FilledArray(dimensions, -5);
        }

        public static double[] RosenbrockUpperBoundaries(int dimensions)
        {
            return FilledArray(dimensions, 10);
        }

        public static double RosenbrockFunction(double[] x)
        {
            int n = x.Length;
            double sum = 0;
            for (int i = 0; i < n - 1; i++)
                sum += 100 * Math.Pow(x[i + 1] - x[i] * x[i], 2) + Math.Pow(1 - x[i], 2);
            return sum;
        }

        public static double[] RastriginLowerBoundaries(int dimensions)
        {
            return FilledArray(dimensions, -5.12);
        }

        public static double[] RastriginUpperBoundaries(int dimensions)
        {
            return FilledArray(dimensions, 5.12);
        }

        public static double RastriginFunction(double[] x)
        {
            int n = x.Length;
            double sum = 0;
            for (int i = 0; i < n; i++)
                sum += x[i] * x[i] - 10 * Math.Cos(2 * Math.PI * x[i]);
            return 10 * n + sum;
        }

        public static double[] SphereLowerBoundaries(int dimensions)
        {
            return FilledArray(dimensions, -10000);
        }

        public static double[] SphereUpperBoundaries(int dimensions)
        {
            return FilledArray(dimensions, 10000);
        }

        public static double SphereFunction(double[] x)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i] * x[i];
            }
            return sum;
        }

        public static double[] UnknownFunctionLowerBoundaries(int dimensions)
        {
            return FilledArray(dimensions, -10);
        }

        public static double[] UnknownFunctionUpperBoundaries(int dimensions)
        {
            return FilledArray(dimensions, 10);
        }

        public static double UnknownFunction(double[] x)
        {
            int n = x.Length;
            double sum = 0;
            for (int i = 0; i < n; i++)
                sum += Math.Abs(x[i] * Math.Sin(x[i]) + 0.1 * x[i]);
            return sum;
        }

        public static double[] EggholderLowerBoundaries()
        {
            return new double[] { -512, -512 };
        }

        public static double[] EggholderUpperBoundaries()
        {
            return new double[] { 512, 512 };
        }

        public static double EggholderFunction(double[] x)
        {
            return -(x[1] + 47) * Math.Sin(Math.Sqrt(Math.Abs((x[0] / 2) + (x[1] + 47)))) - x[0] * Math.Sin(Math.Sqrt(Math.Abs(x[0] + (x[1] + 47))));
        }

        public static double[] FilledArray(int size, double value)
        {
            double[] a = new double[size];
            Array.Fill(a, value);
            return a;
        }

            return new FitnessFunctionType[]
            {
                new FitnessFunctionType
                {
                    Name = "Unknown Function",
                    MinCoordinates = FilledArray(dimensions, -10),
                    MaxCoordinates = FilledArray(dimensions, 10),
                    Dimensions = dimensions,
                    Fn = (double[] x) => {
                        
                    }
                },
                new FitnessFunctionType
                {
                    Name = "Eggholder function",
                    MinCoordinates = new double[] {-512, -512},
                    MaxCoordinates = new double[] {512, 512},
                    Dimensions = 2,
                    Fn = (double[] x) => {
                        return -(x[1] + 47) * Math.Sin(Math.Sqrt(Math.Abs((x[0]/2) + (x[1] + 47)))) -x[0]* Math.Sin(Math.Sqrt(Math.Abs(x[0] + (x[1] + 47))));
                    }
                },
                TSFDE_fractional_boundary_fitness_function.GetFitnessFunction(dimensions)
            };
        }

        
    }
}



