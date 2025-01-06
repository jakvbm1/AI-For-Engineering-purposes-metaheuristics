using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using AI_For_Engineering_purposes__metaheuristics_.Metaheuristics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms
{
    class Wolf
    {
        public double[] Position;
        public double Fitness;
        public Wolf(int dim)
        {
            Position = new double[dim];
        }

    }

    public class WolfTextReport: IGenerateTextReport
    {
        private string functionName;
        private int population;
        private int dimension;
        private int iteration;
        private int numberOfEvaluations;
        private double fBest;
        private double[] xBest;

        private string report;

        public WolfTextReport(string functionName, int population, int dimension, int iteration, int numberOfEvaluations, double fBest, double[] xBest)
        {
            this.functionName = functionName;
            this.population = population;
            this.dimension = dimension;
            this.iteration = iteration;
            this.numberOfEvaluations = numberOfEvaluations;
            this.fBest = fBest;
            this.xBest = xBest;

            setReport();
        }

        private void setReport()
        {
            ReportString += "Grey Wolf Optimization Algorithm \n";
            ReportString += $"{functionName} \n";
            ReportString += $"iteracje {iteration} populacja {population}  wymiary {dimension}\n";
            ReportString += $"Najlepsza wartość {fBest}\n";
            ReportString += $"Pozycja końcowa: \n";

            foreach (var x in xBest)
            {
                ReportString += $"{x}, ";
            }
        }

        public string ReportString { get => report; set => report = value; }
    }

    class WolfStateWriter : IStateWriter
    {
        private string functionName;
        private int population;
        private int dimension;
        private int iteration;
        private int numberOfEvaluations;
        private int currentIteration;
        Wolf[] wolves;
        public WolfStateWriter(int population, int dimension, int iteration, int currentIteration, int numberOfEvaluations, Wolf[] wolves, string functionName)
        {
            this.functionName = functionName;
            this.population = population;
            this.dimension = dimension;
            this.iteration = iteration;
            this.currentIteration = currentIteration;
            this.wolves = wolves;
            this.numberOfEvaluations = numberOfEvaluations;
        }

        public void SaveToFileStateOfAlgorithm(string path)
        {
            using (StreamWriter sw = new StreamWriter($"{path}/GWOState.txt"))
            {
                sw.WriteLine(functionName);
                sw.WriteLine(population);
                sw.WriteLine(dimension);
                sw.WriteLine(iteration);

                sw.WriteLine(currentIteration.ToString());
                sw.WriteLine(numberOfEvaluations.ToString());
                foreach (Wolf p in wolves)
                {
                    string line = "";
                    for (int i = 0; i < p.Position.Length; i++)
                    {
                        line += p.Position[i].ToString() + ", ";
                    }
                    line += p.Fitness.ToString();
                    sw.WriteLine(line);
                }
            }
        }
    }

    internal class GreyWolfOptimization : IOptimizationAlgorithm
    {
        private int nCall = 0;
        private double fbest;
        private double[] xbest;
        private string name = "Grey Wolf Optimization Algorithm";
        private ParamInfo[] paramInfo;

        private int currentIteration;
        private  int iterations;
        private  int population;
        private  int dimensions;
        private  Wolf[] Wolves;
        private  fitnessFunction FitnessFunction;
        private  Random rnd = new Random();
        private IGenerateTextReport textReport;

        public GreyWolfOptimization()
        {
            ParamInfo iteration = new ParamInfo("iteration", "number of iterations", 100000, 1, 10);
            ParamInfo population = new ParamInfo("population", "size of population", 100000, 1, 10);
            ParamInfo dimensions = new ParamInfo("dimensions", "number of dimensions", 100000, 1, 10);

            paramInfo = new ParamInfo[] {iteration, population, dimensions};
        }

        public string Name { get => name; set => name = value; }
        public ParamInfo[] ParamInfo { get => paramInfo; set => paramInfo = value; }
        public IStateWriter writer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IStateReader reader { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IGeneratePDFReport pdfReportGenerator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IGenerateTextReport stringReportGenerator { get => textReport; set =>textReport = value; }
        public double[] Xbest { get => xbest; set => xbest = value; }
        public double Fbest { get => fbest; set => fbest = value; }
        public int NumberOfEvaluationFitnessFunction { get => nCall; set => nCall = value; }

        public void Solve(fitnessFunction f, double[,] domain, string functionName, params double[] parameters)
        {
            iterations = (int)parameters[0];
            population = (int)parameters[1];
            dimensions = (int)parameters[2];

            Wolves = new Wolf[population];

            for (int i = 0; i < this.population; i++)
            {
                Wolves[i] = new Wolf(dimensions);
            }

            for (int i = 0; i < this.population; i++)
            {
                Wolves[i].Position = new double[dimensions];

                for (int j = 0; j < dimensions; j++)
                {
                    Wolves[i].Position[j] = domain[0, j] + rnd.NextDouble() * (domain[1, j] - domain[0, j]);
                }
            }


            for (int i = 0; i < this.population; i++)
            {
                Wolves[i].Fitness = CalculateFitnessFunction(Wolves[i].Position);
            }

            (var alpha, var beta, var delta) = GetAlphaBetaDelta();

            for (; currentIteration < iterations; currentIteration++)
            {

                double a = 2.0 - currentIteration * (2.0 / iterations);

                for (int wolfIndex = 0; wolfIndex < population; wolfIndex++)
                {
                    for (int parameterIndex = 0; parameterIndex < dimensions; parameterIndex++)
                    {
                        double X1 = GetXValue(a, alpha.Position[parameterIndex], Wolves[wolfIndex].Position[parameterIndex]);
                        double X2 = GetXValue(a, beta.Position[parameterIndex], Wolves[wolfIndex].Position[parameterIndex]);
                        double X3 = GetXValue(a, delta.Position[parameterIndex], Wolves[wolfIndex].Position[parameterIndex]);

                        double val = (X1 + X2 + X3) / 3;

                        if (val < domain[0, parameterIndex])
                            val = domain[0, parameterIndex];
                        else if (val > domain[1, parameterIndex])
                            val = domain[1, parameterIndex];

                        Wolves[wolfIndex].Position[parameterIndex] = val;
                        Wolves[wolfIndex].Fitness = CalculateFitnessFunction(Wolves[wolfIndex].Position);
                    }
                }

                (alpha, beta, delta) = GetAlphaBetaDelta();
                Xbest = alpha.Position;
                Fbest = alpha.Fitness;
                writer = new WolfStateWriter(population, dimensions, iterations, currentIteration, nCall, Wolves, functionName);
                writer.SaveToFileStateOfAlgorithm("");
            }

            NumberOfEvaluationFitnessFunction += population;
            stringReportGenerator = new WolfTextReport(functionName, population, dimensions, iterations, nCall, Fbest, xbest);
        }

        private double CalculateFitnessFunction(double[] args)
        {
            NumberOfEvaluationFitnessFunction++;
            return FitnessFunction(args);
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
    }
}
