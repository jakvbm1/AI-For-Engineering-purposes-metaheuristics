using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms
{
    public class Wolf
    {
        public double[] Position;
        public double Fitness;
        public Wolf(int dim)
        {
            Position = new double[dim];
        }

    }

    public class WolfPDFReport : IGeneratePDFReport
    {
        
        string functionName;
        private int population;
        private int dimension;
        private int iteration;
        private int numberOfEvaluations;
        private double fBest;
        private double[] xBest;

        public WolfPDFReport(string functionName, int population, int dimension, int iteration, int numberOfEvaluations, double fBest, double[] xBest)
        {
            this.functionName = functionName;
            this.population = population;
            this.dimension = dimension;
            this.iteration = iteration;
            this.numberOfEvaluations = numberOfEvaluations;
            this.fBest = fBest;
            this.xBest = xBest;
        }

        public void GenerateReport(string path)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Puma PDF Report";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 12, XFontStyleEx.Regular);

            gfx.DrawString("Grey Wolf Optimization Algorithm", new XFont("Arial", 16, XFontStyleEx.Bold), XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);
            gfx.DrawString($"{functionName}", font, XBrushes.Black, new XRect(40, 40, page.Width - 80, page.Height), XStringFormats.TopLeft);
            gfx.DrawString($"Iteracje: {iteration}, populacja: {population}, wymiary: {dimension}", font, XBrushes.Black, new XRect(40, 60, page.Width - 80, page.Height), XStringFormats.TopLeft);
            gfx.DrawString($"Najlepsza wartość (fBest): {fBest}", font, XBrushes.Black, new XRect(40, 80, page.Width - 80, page.Height), XStringFormats.TopLeft);
            gfx.DrawString($"Pozycja końcowa (xBest): {string.Join("; ", xBest)}", font, XBrushes.Black, new XRect(40, 100, page.Width - 80, page.Height), XStringFormats.TopLeft);

            document.Save($"{path}\\GWO_PDF_Report.pdf");
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
            ReportString += $"Iteracje: {iteration}; populacja: {population}; wymiary: {dimension} \n";
            ReportString += $"Najlepsza wartość (fBest): {fBest}\n";
            ReportString += $"Pozycja końcowa (xBest): \n";

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
            using (StreamWriter sw = new StreamWriter($"{path}\\GWOState.txt"))
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

    public class GreyWolfOptimization : IOptimizationAlgorithm
    {
        public int nCall = 0;
        public double fbest;
        public double[] xbest;
        public string name = "Grey Wolf Optimization Algorithm";
        public ParamInfo[] paramInfo;
        public bool running { get; set; } = true;
        public int currentIteration;
        public  int iterations;
        public  int population;
        public  int dimensions;
        public  Wolf[] Wolves;
        public  fitnessFunction FitnessFunction;
        public  Random rnd = new Random();

        private IGenerateTextReport textReport;
        private IGeneratePDFReport pdfReport;

        public GreyWolfOptimization()
        {
            ParamInfo iteration = new ParamInfo("iteration", "number of iterations", 100000, 1, 10);
            ParamInfo population = new ParamInfo("population", "size of population", 100000, 1, 10);
            ParamInfo dimensions = new ParamInfo("dimensions", "number of dimensions", 100000, 1, 2);

            paramInfo = new ParamInfo[] {iteration, population, dimensions};
        }

        public string Name { get => name; set => name = value; }
        public ParamInfo[] ParamInfo { get => paramInfo; set => paramInfo = value; }
        public IStateWriter writer { get; set; }
        public IStateReader reader { get; set; }
        public IGeneratePDFReport pdfReportGenerator { get => pdfReport; set => pdfReport = value; }
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
                    if (domain.GetLength(0) < 2 || domain.GetLength(1) < dimensions)
                    {
                        Console.WriteLine(domain.GetLength(0));
                        Console.WriteLine(domain.GetLength(1));
                        throw new ArgumentException("domain array must have at least 2 rows and 'dimensions' columns.");
                    }
                    Wolves[i].Position[j] = domain[0, j] + rnd.NextDouble() * (domain[1, j] - domain[0, j]);
                }
            }


            for (int i = 0; i < this.population; i++)
            {
                Wolves[i].Fitness = CalculateFitnessFunction(Wolves[i].Position, f);
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
                        Wolves[wolfIndex].Fitness = CalculateFitnessFunction(Wolves[wolfIndex].Position, f);
                    }
                }

                (alpha, beta, delta) = GetAlphaBetaDelta();
                Xbest = alpha.Position;
                Fbest = alpha.Fitness;
                writer = new WolfStateWriter(population, dimensions, iterations, currentIteration, nCall, Wolves, functionName);
                writer.SaveToFileStateOfAlgorithm("C:\\Users\\MSI\\Desktop\\");
            }

            NumberOfEvaluationFitnessFunction += population;
            stringReportGenerator = new WolfTextReport(functionName, population, dimensions, iterations, nCall, Fbest, xbest);
            pdfReportGenerator = new WolfPDFReport(functionName, population, dimensions, iterations, nCall, Fbest, xbest);
            
            Console.WriteLine(stringReportGenerator.ReportString);
            
            //pdfReportGenerator.GenerateReport("C:\\Users\\MSI\\Desktop\\AI-For-Engineering-purposes-metaheuristics");
        }

        private double CalculateFitnessFunction(double[] args, fitnessFunction FitnessFunction)
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
