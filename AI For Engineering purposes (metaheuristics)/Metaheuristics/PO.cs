using AI_For_Engineering_purposes_metaheuristics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.Metaheuristics
{
    class PO : IOptimizationAlgorithm
    {

        //zmienne pomocnicze do "nadzorowania pracy algorytmu
        private string name = "Puma Optimizer";
        private int nOfCalls = 0;
        private int evaluationTime = 0;
        private int currentIteration = 0;
        private double[] fBestHistory; 
        private double fBest;
        private double[] xBest;


        //zmienne do opdalenia algorytmu z dana funkcja
        private int nDimensions;
        private int nIterations;
        private int population;
        private double[] upperBoundaries;
        private double[] lowerBoundaries;
        private Func<double[], double> f;
        private double[,] args; // tablica argumentow (w artykule Sol.X)
        private double[] fit; // tablica wartosci (w artykule Sol.Cost)
        private Random rnd = new Random();
        //parametry algorytmu do "dostrojenia"
        double PF1, PF2, PF3, U, L, Alpha;

        public PO(int nIterations, int population, double[] upperBoundaries, double[] lowerBoundaries, Func<double[], double> f, double PF1 = 0.5, double PF2 = 0.5, double PF3 = 0.3, double U = 0.4, double L = 0.7, double Alpha = 2)
        {
            this.nIterations = nIterations;
            this.population = population;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;
            this.f = f;
            this.PF1 = PF1;
            this.PF2 = PF2;
            this.PF3 = PF3;
            this.U = U;
            this.L = L;
            this.Alpha = Alpha;
            this.nDimensions = upperBoundaries.Length;
            this.xBest = new double[nDimensions];
            this.fBestHistory = new double[nIterations];
            this.args = new double[population, nDimensions];
            this.fit = new double[population];
        }

        public string Name { get => name; set => name = value;}
        public double[] XBest { get => xBest; set => xBest = value; }
        public double FBest { get => fBest; set => fBest = value; }
        public int NumberOfEvaluationFitnessFunction { get => nOfCalls; set => nOfCalls = value; }

        private void Exploitation()
        {
            for (int i = 0; i < population; i++)
            { 
                double[] beta = Randn(nDimensions);
                double[] w = Randn(nDimensions); // eq 37
                double[] v = Randn(nDimensions); // eq 38
                double[] F1 = Randn(nDimensions);
                double[] F2 = new double[nDimensions];
                double R = 2 * rnd.NextDouble() - 1; // eq 34
                double[] mBest = new double[nDimensions];
                double[] S1 = Randn(nDimensions);
                double rand9 = rnd.NextDouble() * 2 - 1;
                double[] S2 = new double[nDimensions];
                double[] Xattack = new double[nDimensions];
                for (int j = 0; j < nDimensions; j++)
                {
                    F1[j] *= Math.Exp(2 - (currentIteration + 1) * (2 / nIterations)); // eq 35
                    F2[j] = w[j] * v[j] * v[j] * Math.Cos(2 * rnd.NextDouble() * w[j]); // eq 36
                    var sum = 0.0;
                    for (int k = 0; k < population; k++)
                    {
                        sum += args[k, j];
                    }
                    mBest[j] = (sum / population) / population;
                    S1[j] += rand9;
                    S2[j] = F1[j] * R * args[i, j] + F2[j] * (1 - R) * xBest[j];
                    Xattack[j] = S2[j] / S1[j];
                }

                // eq 32
                double[] newArgs = new double[nDimensions];
                if (rnd.NextDouble() <= 0.5)
                {
                    if (rnd.NextDouble() <= L)
                    {
                        for (int j = 0; j < nDimensions; j++)
                            newArgs[j] = xBest[j] + 2 * rnd.NextDouble() * Math.Exp(beta[j]) * (args[rnd.Next(population), j] - args[i, j]);
                    }
                    else
                    {
                        for (int j = 0; j < nDimensions; j++)
                            newArgs[j] = 2 * rnd.NextDouble() * Xattack[j] - xBest[j];
                    }
                }
                else
                {
                    int r1 = (int)Math.Round((population - 1) * rnd.NextDouble());
                    for (int j = 0; j < nDimensions; j++)
                        newArgs[j] = mBest[j] * args[r1, j] - Math.Pow(-1, rnd.Next(2) * args[i, j] / (1 + Alpha * rnd.NextDouble()));
                }

                boundaryControl(newArgs);

                double newFit = callFunction(newArgs);

                if (newFit < fit[i])
                    fit[i] = newFit;
            }
        }

        //Gaussian distribution
        public double[] Randn(int dim)
        {
            Random rand = new Random();
            return Enumerable.Range(0, dim).Select(_ =>
                Math.Sqrt(-2.0 * Math.Log(rand.NextDouble())) * Math.Cos(2.0 * Math.PI * rand.NextDouble())
            ).ToArray();
        }

        //jak chcecie wywolac funkcje to uzywajcie tego callFunction zeby automatycznie tez liczyc ilosc wywolan funkcji 
        private double callFunction(double[] args)
        {
            nOfCalls++;
            return f(args);
        }

        public double Solve()
        {
            double[] unSelected = { 1.0, 1.0 };
            double F3_Explore = 0.0;
            double F3_Exploit = 0.0;
            double[] Seq_Time_Explore = { 1.0, 1.0, 1.0 };
            double[] Seq_Time_Exploit = { 1.0, 1.0, 1.0 };
            double[] Seq_Cost_Explore = { 1.0, 1.0, 1.0 };
            double[] Seq_Cost_Exploit = { 1.0, 1.0, 1.0 };
            double Score_Explore = 0.0;
            double Score_Exploit = 0.0;
            double[] PF = { 0.5, 0.5, 0.3 };
            List<double> PF_F3 = new List<double>();
            double Mega_Explor = 0.99;
            double Mega_Exploit = 0.99;

            initializePopulation();

            int ind = Array.IndexOf(fit, fit.Min());
            for (int i = 0; i < nDimensions; i++)
            {
                XBest[i] = args[ind, i];
            }
            FBest = fit[ind];

            bool flagChange = false;

            // Unexperienced Phase
            for (; currentIteration < 3; currentIteration++)
            {

            }

            return FBest;
        }

        private void initializePopulation()
        {
            Random random = new Random();
            for(int i=0; i<population; i++)
            {
                double[] arg = new double[nDimensions];
                for(int j=0; j<nDimensions; j++)
                {
                    args[i, j] = random.NextDouble() * (upperBoundaries[j] - lowerBoundaries[j]) + lowerBoundaries[j];
                    arg[j] = args[i, j];
                }
                fit[i] = callFunction(arg);
            }
        }

        //czasem te przeksztalcenia z metaheurystyk lubia wywalac poza zakres przeszukiwany wiec proponuje by puszczac taka funkcje na koncu iteracji by poprawic takie przypadki
        private void boundaryControl(double[] args)
        {
            for (int i = 0; i < nDimensions; i++)
            {
                if (args[i] > upperBoundaries[i])
                {
                    args[i] = upperBoundaries[i];
                }

                else if (args[i] < lowerBoundaries[i])
                {
                    args[i] = lowerBoundaries[i];
                }
            }
        }
    }
}
