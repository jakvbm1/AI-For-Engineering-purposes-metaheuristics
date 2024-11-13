using AI_For_Engineering_purposes__metaheuristics_;

namespace AI_For_Engineering_purposes_metaheuristics
{

    class BentCigar : TestFunction
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

    class Beale : TestFunction
    {
        public Beale() { this.NDimension = 2; }

        private int nDimension;
        public int NDimension { get => nDimension; set => nDimension = 2; }

        public double[] LowerBoundaries => new double[] { -4.5, -4.5 };

        public double[] UpperBoundaries => new double[] { 4.5, 4.5 };

        public string Name => "Beale";


        public double function(double[] args)
        {
            return Math.Pow(1.4 - args[0] + args[0] * args[1], 2) + Math.Pow(2.25 - args[0] + args[0] * args[1] * args[1], 2) + Math.Pow(2.625 - args[0] + args[0] * Math.Pow(args[1], 3), 2);
        }
    }


    class BukinN6 : TestFunction
    {
        public BukinN6() { this.NDimension = 2; }

        private int nDimension;
        public int NDimension { get => nDimension; set => nDimension = 2; }

        public double[] LowerBoundaries => new double[] { -15, -3 };

        public double[] UpperBoundaries => new double[] { -5, 3 };

        public string Name => "Bukin function N.6";


        public double function(double[] args)
        {
            return 100 * Math.Sqrt(Math.Abs(args[1] - 0.01 * args[0] * args[0])) + 0.01 * Math.Abs(args[0] + 10);
        }
        //beale, bukin, himmelbau's,  bez eggholdera i BentCigara 

    }

    class Himmelblaus : TestFunction
    {
        public Himmelblaus() { this.NDimension = 2; }

        private int nDimension;
        public int NDimension { get => nDimension; set => nDimension = 2; }

        public double[] LowerBoundaries => new double[] { -5, -5 };

        public double[] UpperBoundaries => new double[] { 5, 5 };

        public string Name => "Himmelblau's function";


        public double function(double[] args)
        {
            return Math.Pow(args[0] * args[0] + args[1] - 11, 2) + Math.Pow(args[0] + args[1] * args[1] - 7, 2);
        }
        //beale, bukin, himmelbau's,  bez eggholdera i BentCigara 

    }
}



