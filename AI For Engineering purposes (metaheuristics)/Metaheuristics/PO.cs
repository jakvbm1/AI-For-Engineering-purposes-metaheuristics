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
        private string name = "Puma optimization";
        private int nOfCalls = 0;
        private int evaluationTime = 0;
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
        private double[,] args;
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
        }

        public string Name { get => name; set => name = value;}
        public double[] XBest { get => xBest; set => xBest = value; }
        public double FBest { get => fBest; set => fBest = value; }
        public int NumberOfEvaluationFitnessFunction { get => nOfCalls; set => nOfCalls = value; }

        //jak chcecie wywolac funkcje to uzywajcie tego callFunction zeby automatycznie tez liczyc ilosc wywolan funkcji 
        private double callFunction(double[] args)
        {
            nOfCalls++;
            return f(args);
        }

        public double Solve()
        {
            initializePopulation();
            
            return FBest;
        }

        private void initializePopulation()
        {
            Random random = new Random();
            for(int i=0; i<population; i++)
            {
                for(int j=0; j<nDimensions; j++)
                    args[i, j] = random.NextDouble() * (upperBoundaries[j] - lowerBoundaries[j]) + lowerBoundaries[j];
            }
        }

        //czasem te przeksztalcenia z metaheurystyk lubia wywalac poza zakres przeszukiwany wiec proponuje by puszczac taka funkcje na koncu iteracji by poprawic takie przypadki
        private void boundaryControl()
        {
            for(int i=0; i<population; i++)
            {
                for(int j = 0; j<nDimensions; j++)
                {
                    if (args[i,j] > upperBoundaries[j])
                    {
                        args[i, j] = upperBoundaries[j];
                    }

                    else if (args[i, j] < lowerBoundaries[j])
                    {
                        args[i, j] = lowerBoundaries[j];
                    }
                }
            }
        }
    }
}
