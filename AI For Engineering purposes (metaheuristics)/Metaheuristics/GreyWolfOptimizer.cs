using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using AI_For_Engineering_purposes_metaheuristics;

namespace AI_For_Engineering_purposes__metaheuristics_.Metaheuristics
{


    class GreyWolfOptimizer : IOA
    {
        private int currentIteration;
        private readonly int targetIterations;
        private readonly int population;
        private readonly int dimensions;
        private readonly Wolf[] Wolves;
        private readonly Func<double[], double> FitnessFunction;
        private readonly double[] upperBoundaries;
        private readonly double[] lowerBoundaries;
        private readonly Random rnd = new Random();
        public int NumberOfEvaluationFitnessFunction { get; set; }
        public long Time { get; private set; }
        public string Name { get; set; } = "Grey Wolf Optimizer";
        public double[] XBest { get; set; }
        public double FBest { get; set; }

        public int NDimension { get => dimensions; }
        public int NIterations { get => targetIterations; }
        public int Population { get => population; }
        public GreyWolfOptimizer(Func<double[], double> fitnessFunction, int population, int targetIterations, double[] upperBoundaries, double[] lowerBoundaries)
        {
            this.FitnessFunction = fitnessFunction;
            this.population = population;
            this.targetIterations = targetIterations;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;
            this.dimensions = upperBoundaries.Length;
            this.Time = 0;
            this.currentIteration = 0;
            this.NumberOfEvaluationFitnessFunction = 0;
            this.Wolves = new Wolf[this.population];

            for(int i=0; i < this.population; i++)
            {
                Wolves[i] = new Wolf(dimensions);
            }

            for (int i = 0; i < this.population; i++)
            {
                Wolves[i].Position = new double[dimensions];

                for (int j = 0; j < dimensions; j++)
                {
                    Wolves[i].Position[j] = lowerBoundaries[j] + rnd.NextDouble() * (upperBoundaries[j] - lowerBoundaries[j]);
                }
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < this.population; i++)
            {
                Wolves[i].Fitness = CalculateFitnessFunction(Wolves[i].Position);
            }
            this.Time += watch.ElapsedMilliseconds;
        }

        public double Solve()
        {
            (var alpha, var beta, var delta) = GetAlphaBetaDelta();

            for (; currentIteration < targetIterations; currentIteration++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                double a = 2.0 - currentIteration * (2.0 / targetIterations);

                for (int wolfIndex = 0; wolfIndex < population; wolfIndex++)
                {
                    for (int parameterIndex = 0; parameterIndex < dimensions; parameterIndex++)
                    {
                        double X1 = GetXValue(a, alpha.Position[parameterIndex], Wolves[wolfIndex].Position[parameterIndex]);
                        double X2 = GetXValue(a, beta.Position[parameterIndex], Wolves[wolfIndex].Position[parameterIndex]);
                        double X3 = GetXValue(a, delta.Position[parameterIndex], Wolves[wolfIndex].Position[parameterIndex]);

                        double val = (X1 + X2 + X3) / 3;

                        if (val < lowerBoundaries[parameterIndex])
                            val = lowerBoundaries[parameterIndex];
                        else if (val > upperBoundaries[parameterIndex])
                            val = upperBoundaries[parameterIndex];

                        Wolves[wolfIndex].Position[parameterIndex] = val;
                        Wolves[wolfIndex].Fitness = CalculateFitnessFunction(Wolves[wolfIndex].Position);
                    }
                }

                (alpha, beta, delta) = GetAlphaBetaDelta();
                XBest = alpha.Position;
                FBest = alpha.Fitness;

                watch.Stop();
                this.Time += watch.ElapsedMilliseconds;
                
            }

            NumberOfEvaluationFitnessFunction += population;
            return FBest;
        }

        private (Wolf, Wolf, Wolf) GetAlphaBetaDelta()
        {
            Wolf alpha = Wolves[0], beta = Wolves[0], delta = Wolves[0];

            foreach (var wolf in Wolves)
            {
                if (wolf.Fitness < alpha.Fitness)
                {
                    delta = beta;
                    beta = alpha;
                    alpha = wolf;
                }
                else if (wolf.Fitness < beta.Fitness)
                {
                    delta = beta;
                    beta = wolf;
                }
                else if (wolf.Fitness < delta.Fitness)
                {
                    delta = wolf;
                }
            }

            return (alpha, beta, delta);
        }

        private double GetXValue(double a, double posP, double pos)
        {
            Random rnd = new Random();
            double r1 = rnd.NextDouble();
            double r2 = rnd.NextDouble();

            double A1 = 2.0 * a * r1 - a; //Equation (3.3)
            double C1 = 2.0 * r2; //Equation (3.4)

            double D = Math.Abs(C1 * posP - pos); //Equation (3.5)-part 1
            return posP - A1 * D; //Equation (3.6)-part 1
        }

        public void SaveToFileStateOfAlghoritm()
        {
            bool shouldExit = false;

            ConsoleCancelEventHandler preventExit = (sender, e) =>
            {
                shouldExit = true;
                e.Cancel = true;
            };

            Console.CancelKeyPress += preventExit;

            SaveLoadableState();

            if (shouldExit) System.Environment.Exit(0);
            Console.CancelKeyPress -= preventExit;
        }

        public void SaveLoadableState()
        {
            var file = File.CreateText("GWO_state.txt");

            file.WriteLine("Dimensions; Population; TargetIterations; CurrentIteration; NumberOfEvaluationFitnessFunction; Time;");
            file.WriteLine($"{dimensions}; {population}; {targetIterations}; {currentIteration}; {NumberOfEvaluationFitnessFunction}; {Time};");

            file.WriteLine();

            for (int i = 0; i < dimensions; i++)
            {
                file.Write($"x{i}; ");
            }

            file.WriteLine();

            for (int i = 0; i < population; i++)
            {
                foreach (var x in Wolves[i].Position)
                {
                    file.Write($"{x}; ");
                }
                file.WriteLine();
            }

            file.Close();
        }

        public void SaveResult()
        {
            var resultFile = File.CreateText("GWO_result.txt");
            resultFile.WriteLine($"{NumberOfEvaluationFitnessFunction} [liczba wywołań funkcji celu]");
            resultFile.Write($"{FBest} ");

            foreach (var x in XBest)
            {
                resultFile.Write($"{x} ");
            }

            resultFile.WriteLine("[najlepszy osobnik wraz z wartością funkcji celu]");
            resultFile.WriteLine($"{population} [populacja]");
            resultFile.WriteLine($"{targetIterations} [liczba iteracji]");

            resultFile.Close();
        }

        private double CalculateFitnessFunction(double[] args)
        {
            NumberOfEvaluationFitnessFunction++;
            return FitnessFunction(args);
        }
    }

    class Wolf
    {
        public double[] Position;
        public double Fitness;
        public Wolf(int dim)
        {
            Position = new double[dim];
        }

    }
}