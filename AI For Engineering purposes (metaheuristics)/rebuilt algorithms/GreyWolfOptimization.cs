using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
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

    public class WolfGeneratePdfFromTxt
    {
        public void GeneratePdfFromTxt(string pdfFilePath, string txtFilePath)
        {
            try
            {
                if (!File.Exists(txtFilePath))
                {
                    Console.WriteLine("Error: The specified TXT file does not exist.");
                    return;
                }

                string[] lines = File.ReadAllLines(txtFilePath);

                using (PdfWriter writer = new PdfWriter(pdfFilePath))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);

                        int index = 1;
                        bool newReport = true;

                        foreach (var line in lines)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                newReport = true;
                                document.Add(new Paragraph("\n"));
                            }
                            else
                            {
                                if (newReport)
                                {
                                    document.Add(new Paragraph($"Raport nr. {index++}")
                                        .SetFontSize(14));
                                    newReport = false;
                                }

                                document.Add(new Paragraph(line));
                            }
                        }

                        document.Close();
                    }
                }

                Console.WriteLine("PDF report generated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF: {ex.Message}");
            }
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
            string filePath = Path.Combine(path, "GWO_PDF_Report.pdf");

            try
            {
                using (PdfWriter writer = new PdfWriter(filePath))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);

                        pdf.GetDocumentInfo().SetTitle("Puma PDF Report");

                        document.Add(new Paragraph("Grey Wolf Optimization Algorithm")
                            .SetFontSize(16)
                            .SetTextAlignment(TextAlignment.CENTER));

                        document.Add(new Paragraph(functionName)
                            .SetFontSize(12)
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetMarginTop(10));

                        document.Add(new Paragraph($"Iteracje: {iteration}, populacja: {population}, wymiary: {dimension}")
                            .SetFontSize(12));

                        document.Add(new Paragraph($"Najlepsza wartość (fBest): {fBest}")
                            .SetFontSize(12));

                        document.Add(new Paragraph("Pozycja końcowa (xBest):")
                            .SetFontSize(12));

                        document.Add(new Paragraph(string.Join("; ", xBest))
                            .SetFontSize(12));

                        document.Close();
                    }
                }

                Console.WriteLine($"PDF report generated successfully at: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF: {ex.Message}");
            }
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

            ReportString += "\n\n";
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
        private int CurrentIteration;
        Wolf[] wolves;
        public WolfStateWriter(int population, int dimension, int iteration, int CurrentIteration, int numberOfEvaluations, Wolf[] wolves, string functionName)
        {
            this.functionName = functionName;
            this.population = population;
            this.dimension = dimension;
            this.iteration = iteration;
            this.CurrentIteration = CurrentIteration;
            this.numberOfEvaluations = numberOfEvaluations;
            this.wolves = wolves;
        }

        public void SaveToFileStateOfAlgorithm(string path)
        {
            using (StreamWriter sw = new StreamWriter($"{path}\\GWOState.txt"))
            {
                sw.WriteLine(functionName);
                sw.WriteLine(population);
                sw.WriteLine(dimension);
                sw.WriteLine(iteration);

                sw.WriteLine(CurrentIteration.ToString());
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


    class WolfStateReader : IStateReader
    {
        public Wolf[] Wolves { get; private set; }
        public int Population { get; private set; }
        public int Dimension { get; private set; }
        public int Iterations { get; private set; }
        public int CurrentIteration { get; private set; }
        public int NCall { get; private set; }
        public string FunctionName { get; private set; }
        public void LoadFromFileStateOfAlgorithm(string path)
        {
         if(File.Exists(path+"/GWOState.txt"))
            {
                using (StreamReader sr = new StreamReader(path + "/GWOState.txt"))
                {
                    FunctionName = sr.ReadLine();
                    Population = int.Parse(sr.ReadLine());
                    Dimension = int.Parse(sr.ReadLine());
                    Iterations = int.Parse(sr.ReadLine());
                    CurrentIteration = int.Parse(sr.ReadLine());
                    NCall = int.Parse(sr.ReadLine());
                    Wolf[] wolves = new Wolf[Population];
                    for (int i = 0; i < Population; i++)
                    {
                        wolves[i] = new Wolf(Dimension);
                        string line = sr.ReadLine();
                        string[] args = line.Split(", ");

                        wolves[i].Fitness = Double.Parse(args[args.Length - 1]);
                        wolves[i].Position = new double[Dimension];
                        for (int j = 0; j<Dimension; j++)
                        {
                            wolves[i].Position[j] = double.Parse(args[j]);
                        }
                    }
                    Wolves = wolves;
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
        public ParamInfo[] paramInfo = [];
        public bool Running { get; set; } = true;
        public  int population;
        public  int dimensions;
        public  Wolf[] Wolves;
        public  fitnessFunction FitnessFunction;
        public  Random rnd = new Random();

        private IGenerateTextReport textReport;
        private IGeneratePDFReport pdfReport;

        public int CurrentIteration { get; set; } = 0;
        public int TargetIteration { get; set; } = 10;

        public string Name { get => name; set => name = value; }
        public ParamInfo[] ParamInfo { get => paramInfo; set => paramInfo = value; }
        public IStateWriter writer { get; set; }
        public IStateReader reader { get; set; }
        public IGeneratePDFReport pdfReportGenerator { get => pdfReport; set => pdfReport = value; }
        public IGenerateTextReport stringReportGenerator { get => textReport; set =>textReport = value; }
        public double[] Xbest { get => xbest; set => xbest = value; }
        public double Fbest { get => fbest; set => fbest = value; }
        public int NumberOfEvaluationFitnessFunction { get => nCall; set => nCall = value; }

        public GreyWolfOptimization()
        {
            ParamInfo population = new ParamInfo("population", "size of population", 100000, 1, 10);

            paramInfo = new ParamInfo[] { population };
        }

        public void Solve(fitnessFunction f, double[,] domain, string functionName, params double[] parameters)
        {
            reader = new WolfStateReader();
            dimensions = domain.GetLength(1);
            reader.LoadFromFileStateOfAlgorithm(""); //tutaj by trzeba bylo wprowadzic sciezke do folderu gdzie zapisujemy te stany

            if(((WolfStateReader)reader).Wolves != null && ((WolfStateReader)reader).Wolves.Length > 0)
            {
                population = ((WolfStateReader)reader).Population;
                TargetIteration = ((WolfStateReader)reader).Iterations;
                Wolves = ((WolfStateReader)reader).Wolves;
            }
            else 
            {
                population = (int)parameters[0];

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
            }
            
            //up until here
            (var alpha, var beta, var delta) = GetAlphaBetaDelta();

            for (; CurrentIteration < TargetIteration && Running; CurrentIteration++)
            {

                double a = 2.0 - CurrentIteration * (2.0 / TargetIteration);

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
                writer = new WolfStateWriter(population, dimensions, TargetIteration, CurrentIteration, nCall, Wolves, functionName);
                writer.SaveToFileStateOfAlgorithm(""); //tutaj tez path do tego folderu
            }

            NumberOfEvaluationFitnessFunction += population;
            stringReportGenerator = new WolfTextReport(functionName, population, dimensions, TargetIteration, nCall, Fbest, xbest);
            pdfReportGenerator = new WolfPDFReport(functionName, population, dimensions, TargetIteration, nCall, Fbest, xbest);
            
            Console.WriteLine(stringReportGenerator.ReportString);
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
