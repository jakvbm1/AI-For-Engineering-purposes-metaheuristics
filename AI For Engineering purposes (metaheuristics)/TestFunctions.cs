using AI_For_Engineering_purposes__metaheuristics_;

namespace AI_For_Engineering_purposes_metaheuristics
{

    class BentCigar: TestFunction
    {
        public BentCigar(int nd) { this.NDimension = nd; }

        private int nDimension;
        public int NDimension { get => nDimension; set => nDimension = value; }

        public double[] LowerBoundaries => TestFunction.FilledArray(NDimension, -10);

        public double[] UpperBoundaries => TestFunction.FilledArray(NDimension, 10);

        public string Name => "Bent Cigar";


        public double function(double[] args)
        {
            int n = args.Length;
            double sum = 0;
            for (int i = 1; i < n; i++)
                sum += args[i] * args[i];
            return args[0] * args[0] + Math.Pow(10, 6) * sum;
        }
    }

    class Rosenbrock : TestFunction
    {
        public Rosenbrock(int nd) { this.NDimension = nd; }

        private int nDimension;
        public int NDimension { get => nDimension; set => nDimension = value; }

        public double[] LowerBoundaries => TestFunction.FilledArray(NDimension, -5.12);

        public double[] UpperBoundaries => TestFunction.FilledArray(NDimension, 5.12);

        public string Name => "Rosenbrock";


        public double function(double[] args)
        {
            int n = args.Length;
            double sum = 0;
            for (int i = 0; i < n - 1; i++)
                sum += 100 * Math.Pow(args[i + 1] - args[i] * args[i], 2) + Math.Pow(1 - args[i], 2);
            return sum;
        }
    }

    class Rastrigin : TestFunction
    {
        public Rastrigin(int nd) { this.NDimension = nd; }

        private int nDimension;
        public int NDimension { get => nDimension; set => nDimension = value; }

        public double[] LowerBoundaries => TestFunction.FilledArray(NDimension, -5.12);

        public double[] UpperBoundaries => TestFunction.FilledArray(NDimension, 5.12);

        public string Name => "Rastrigin";


        public double function(double[] args)
        {
            int n = args.Length;
            double sum = 0;
            for (int i = 0; i < n; i++)
                sum += args[i] * args[i] - 10 * Math.Cos(2 * Math.PI * args[i]);
            return 10 * n + sum;
        }
    }

    class Sphere : TestFunction
    {
        public Sphere(int nd) { this.NDimension = nd; }

        private int nDimension;
        public int NDimension { get => nDimension; set => nDimension = value; }

        public double[] LowerBoundaries => TestFunction.FilledArray(NDimension, -10000);

        public double[] UpperBoundaries => TestFunction.FilledArray(NDimension, 10000);

        public string Name => "Sphere";


        public double function(double[] args)
        {
            double sum = 0;
            for (int i = 0; i < args.Length; i++)
            {
                sum += args[i] * args[i];
            }
            return sum;
        }
    }

    class UnknownFunction : TestFunction
    {
        public UnknownFunction(int nd) { this.NDimension = nd; }

        private int nDimension;
        public int NDimension { get => nDimension; set => nDimension = value; }

        public double[] LowerBoundaries => TestFunction.FilledArray(NDimension, -10);

        public double[] UpperBoundaries => TestFunction.FilledArray(NDimension, 10);

        public string Name => "UnknownFunction";


        public double function(double[] args)
        {
            int n = args.Length;
            double sum = 0;
            for (int i = 0; i < n; i++)
                sum += Math.Abs(args[i] * Math.Sin(args[i]) + 0.1 * args[i]);
            return sum;
        }
    }

    class Eggholder : TestFunction
    {
        public Eggholder() { this.NDimension = 2; }

        private int nDimension;
        public int NDimension { get => nDimension; set => nDimension = 2; }

        public double[] LowerBoundaries => new double[] { -512, -512 };

        public double[] UpperBoundaries => new double[] { 512, 512 };

        public string Name => "Eggholder";


        public double function(double[] args)
        {
            return -(args[1] + 47) * Math.Sin(Math.Sqrt(Math.Abs((args[0] / 2) + (args[1] + 47)))) - args[0] * Math.Sin(Math.Sqrt(Math.Abs(args[0] + (args[1] + 47))));
        }
    }


}



