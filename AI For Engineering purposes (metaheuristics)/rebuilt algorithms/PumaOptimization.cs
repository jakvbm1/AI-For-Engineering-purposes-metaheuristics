﻿using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms
{
    public class Puma
    {
        public double[] Position;
        public double Fitness;

        public Puma(int dim)
        {
            Position = new double[dim];
        }
    }

    public class PumaGeneratePdfFromTxt
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

    public class PumaPDFReport : IGeneratePDFReport
    {
        string functionName;
        double fBest;
        double[] xBest;
        double pf1, pf2, pf3, l, u;
        int a, iteration, population, dimension;

        public PumaPDFReport(string functionName, double fBest, double[] xBest, int iteration, int dimension, double[] parameters)
        {
            this.iteration = iteration;
            this.dimension = dimension;
            this.functionName = functionName;
            this.fBest = fBest;
            this.xBest = xBest;
            this.population = (int)parameters[0];
            this.pf1 = parameters[1];
            this.pf2 = parameters[2];
            this.pf3 = parameters[3];
            this.l = parameters[4];
            this.u = parameters[5];
            this.a = (int)parameters[6];
        }

        public void GenerateReport(string path)
        {
            try
            {
                using (PdfWriter writer = new PdfWriter(path))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);

                        pdf.GetDocumentInfo().SetTitle("Puma PDF Report");

                        document.Add(new Paragraph("Puma Optimization Algorithm")
                            .SetFontSize(16)
                            .SetTextAlignment(TextAlignment.CENTER));

                        document.Add(new Paragraph(functionName)
                            .SetFontSize(12)
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetMarginTop(10));

                        document.Add(new Paragraph($"Parametry: PF1 = {pf1}; PF2 = {pf2}; PF3 = {pf3}; l = {l}; u = {u}; a = {a}")
                            .SetFontSize(12));

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

                Console.WriteLine($"PDF report generated successfully at: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF: {ex.Message}");
            }
        }
    }

    public class PumaTextReport : IGenerateTextReport
    {
        string functionName;
        double fBest;
        double[] xBest;
        double pf1, pf2, pf3, l, u;
        int a, iteration, population, dimension;

        private string report;

        public PumaTextReport(string functionName, double fBest, double[] xBest, int iteration, int dimension, double[] parameters)
        {
            this.iteration = iteration;
            this.dimension = dimension;
            this.functionName = functionName;
            this.fBest = fBest;
            this.xBest = xBest;
            this.population = (int)parameters[0];
            this.pf1 = parameters[1];
            this.pf2 = parameters[2];
            this.pf3 = parameters[3];
            this.l = parameters[4];
            this.u = parameters[5];
            this.a = (int)parameters[6];
            setReport();
        }

        private void setReport()
        {
            ReportString += "Puma Optimization Algorithm \n";
            ReportString += $"{functionName} \n";
            ReportString += $"Parametry: PF1 = {pf1}; PF2 = {pf2}; PF3 = {pf3}; l = {l}; u = {u}; a = {a} \n";
            ReportString += $"Iteracje: {iteration}; populacja: {population}; wymiary: {dimension} \n";
            ReportString += $"Najlepsza wartość (fBest): {fBest} \n";
            ReportString += $"Pozycja końcowa (xBest): \n";

            foreach (var x in xBest)
            {
                ReportString += $"{x}, ";
            }

            ReportString += "\n\n";
        }

        public string ReportString { get => report; set => report = value; }
    }


    public class PumaStateWriter : IStateWriter
    {
        private int currentIteration;
        private int populationSize;
        private int dimension;
        private int iterations;

        private double pf1, pf2, pf3, l, u;
        private int a;

        private string functionName;
        private int numberOfEvaluations;
        private Puma[] population;

        public PumaStateWriter(int currentIteration, int populationSize, int dimension, int iterations, double pf1, double pf2, double pf3, double l, double u, int a, int numberOfEvaluations, Puma[] population, string functionName)
        {
            this.currentIteration = currentIteration;
            this.populationSize = populationSize;
            this.iterations = iterations;
            this.dimension = dimension;
            this.pf1 = pf1;
            this.pf2 = pf2;
            this.pf3 = pf3;
            this.l = l;
            this.u = u;
            this.a = a;
            this.numberOfEvaluations = numberOfEvaluations;
            this.population = population;
            this.functionName = functionName;
        }

        public void SaveToFileStateOfAlgorithm(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(functionName);
                sw.WriteLine(populationSize);
                sw.WriteLine(iterations);
                sw.WriteLine(dimension);


                sw.WriteLine(currentIteration.ToString());
                sw.WriteLine(numberOfEvaluations.ToString());

                sw.WriteLine(pf1);
                sw.WriteLine(pf2);
                sw.WriteLine(pf3);
                sw.WriteLine(l);
                sw.WriteLine(u);
                sw.WriteLine(a);
                foreach (Puma p in population)
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


    public class PumaStateReader : IStateReader
    {
        public Puma[] Pumas { get; private set; }
        public int Population { get; private set; }
        public int Iterations { get; private set; }
        public int Dimension { get; private set; }
        public int CurrentIteration { get; private set; }
        public int NCall { get; private set; }
        public double PF1 { get; private set; }
        public double PF2 { get; private set; }
        public double PF3 { get; private set; }
        public double L { get; private set; }
        public double U { get; private set; }
        public int A { get; private set; }
        public string FunctionName { get; private set; }

        public void LoadFromFileStateOfAlgorithm(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    FunctionName = sr.ReadLine();
                    Population = int.Parse(sr.ReadLine());
                    Iterations = int.Parse(sr.ReadLine());
                    Dimension = int.Parse(sr.ReadLine());
                    CurrentIteration = int.Parse(sr.ReadLine());
                    NCall = int.Parse(sr.ReadLine());
                    PF1 = double.Parse(sr.ReadLine());
                    PF2 = double.Parse(sr.ReadLine());
                    PF3 = double.Parse(sr.ReadLine());
                    L = double.Parse(sr.ReadLine());
                    U = double.Parse(sr.ReadLine());
                    A = int.Parse(sr.ReadLine());

                    Puma[] pumas = new Puma[Population];
                    for (int i = 0; i < Population; i++)
                    {
                        pumas[i] = new Puma(Dimension);
                        string line = sr.ReadLine();
                        string[] args = line.Split(", ");

                        pumas[i].Fitness = Double.Parse(args[args.Length - 1]);

                        pumas[i].Position = new double[Dimension];
                        for (int j = 0; j < Dimension; j++)
                        {
                            pumas[i].Position[j] = double.Parse(args[j]);
                        }

                    }
                    Pumas = pumas;
                }
            }
        }
    }




    public class PumaOptimization : IOptimizationAlgorithm
    {
        public string name = "Puma Optimization Algorithm";
        public double fbest { get; set; }
        public double[] xbest { get; set; }
        public ParamInfo[] paramInfo { get; set; }
        public int n_call = 0;
        public double pf1 { get; set; }
        public double pf2 { get; set; }
        public double pf3 { get; set; }
        public double l { get; set; }
        public double u { get; set; }
        public int a { get; set; }
        public bool Running { get; set; } = true;
        public int dimensions { get; set; }
        public int population { get; set; }

        private Random rnd = new Random();

        public IGenerateTextReport textReport;
        public IGeneratePDFReport pdfReport;

        public int CurrentIteration { get; set; }
        public int TargetIteration { get; set; }

        public PumaOptimization()
        {
            var population = new ParamInfo("population", "population of pumas", 10000, 3, 10);
            var PF1 = new ParamInfo("PF1", "parameter no. 1", 0.8, 0.2, 0.5);
            var PF2 = new ParamInfo("PF2", "parameter no. 2", 0.8, 0.2, 0.5);
            var PF3 = new ParamInfo("PF3", "parameter no. 3", 0.5, 0.1, 0.3);
            var L = new ParamInfo("L", "L parameter", 0.9, 0.5, 0.7);
            var U = new ParamInfo("U", "U parameter", 0.6, 0.2, 0.4);
            var alpha = new ParamInfo("Alpha", "Alpha parameter", 3, 1, 2);


            ParamInfo = new ParamInfo[] { population, PF1, PF2, PF3, L, U, alpha };

        }

        public string Name { get => name; set => name = value; }
        public ParamInfo[] ParamInfo { get => paramInfo; set => paramInfo = value; }
        public IStateWriter writer { get; set; }
        public IStateReader reader { get; set; } = new PumaStateReader();
        public IGeneratePDFReport pdfReportGenerator { get => pdfReport; set => pdfReport = value; }
        public IGenerateTextReport stringReportGenerator { get => textReport; set => textReport = value; }
        public double[] Xbest { get => xbest; set => xbest = value; }
        public double Fbest { get => fbest; set => fbest = value; }
        public int NumberOfEvaluationFitnessFunction { get => n_call; set => n_call = value; }
        public double Pf1 { get => pf1; set => pf1 = value; }

        public void Solve(fitnessFunction f, double[,] domain, string functionName, params double[] parameters)
        {
            reader.LoadFromFileStateOfAlgorithm(""); //sciezka do folderu
            var Pumas = new Puma[(int)parameters[0]];
            
            dimensions = domain.GetLength(1);
            xbest = new double[dimensions];
            if (((PumaStateReader)reader).Pumas != null && ((PumaStateReader)reader).Pumas.Length > 0)
            {
                population = ((PumaStateReader)reader).Population;
                TargetIteration = ((PumaStateReader)reader).Iterations;
                
                Pf1 = ((PumaStateReader)reader).PF1;
                pf2 = ((PumaStateReader)reader).PF2;
                pf3 = ((PumaStateReader)reader).PF3;
                l = ((PumaStateReader)reader).L;
                u = ((PumaStateReader)reader).U;
                a = ((PumaStateReader)reader).A;
                CurrentIteration = ((PumaStateReader)reader).CurrentIteration;
                Pumas = ((PumaStateReader)reader).Pumas;
            }
            else
            {
                population = (int)parameters[0];  
                Pf1 = parameters[1];
                pf2 = parameters[2];
                pf3 = parameters[3];
                l = parameters[4];
                u = parameters[5];
                a = (int)parameters[6];

                Pumas = new Puma[population];

                for (int i = 0; i < population; i++)
                {
                    Pumas[i] = new Puma(dimensions);
                }
                for (int i = 0; i < population; i++)
                {
                    for (int j = 0; j < dimensions; j++)
                    {
                        Pumas[i].Position[j] = domain[0, j] + rnd.NextDouble() * (domain[1, j] - domain[0, j]);
                    }
                }

                for (int i = 0; i < population; i++)
                {
                    Pumas[i].Fitness = callFunction(Pumas[i].Position, f);
                }
            }

            double[] UnSelected = { 1.0, 1.0 };
            double F3_Explore = 0.0;
            double F3_Exploit = 0.0;
            double[] Seq_Time_Explore = { 1.0, 1.0, 1.0 };
            double[] Seq_Time_Exploit = { 1.0, 1.0, 1.0 };
            double[] Seq_Cost_Explore = { 1.0, 1.0, 1.0 };
            double[] Seq_Cost_Exploit = { 1.0, 1.0, 1.0 };
            double Score_Explore = 0.0;
            double Score_Exploit = 0.0;
            List<double> PF_F3 = new List<double>();
            double Mega_Explor = 0.99;
            double Mega_Exploit = 0.99;
            double[] Costs_Explor = new double[TargetIteration];
            double[] Costs_Exploit = new double[TargetIteration];


            Random random = new Random();


            int ind = Array.IndexOf(Pumas, Pumas.OrderBy(p => p.Fitness).First());
            for (int i = 0; i < xbest.Length; i++)
            {
                xbest[i] = Pumas[ind].Position[i];
            }

            fbest = Pumas[ind].Fitness;

            bool Flag_Change = false;

            double[] InitialXBest = xbest;
            double InitialFBest = fbest;

            // Unexperienced Phase
            for (; CurrentIteration < 3; CurrentIteration++)
            {
                Puma[] Pumas_explore = Exploration(Pumas, f, domain);
                Costs_Explor[CurrentIteration] = Pumas_explore.Min(p => p.Fitness);

                Puma[] Pumas_exploit = Exploitation(Pumas, f, domain);
                Costs_Exploit[CurrentIteration] = Pumas_exploit.Min(p => p.Fitness);

                Puma[] CombinedPumas = Pumas.Concat(Pumas_explore).Concat(Pumas_exploit).ToArray();
                Pumas = CombinedPumas.OrderBy(s => s.Fitness).Take(population).ToArray();
                for (int i = 0; i < xbest.Length; i++)
                {
                    xbest[i] = Pumas[0].Position[i];
                }
                fbest = Pumas[0].Fitness;

                writer = new PumaStateWriter(CurrentIteration, population, dimensions, TargetIteration, pf1, pf2, pf3, l, u, a, n_call, Pumas, functionName);
            }

            Seq_Cost_Explore[0] = Math.Abs(InitialFBest - Costs_Explor[0]); // Eq(5)
            Seq_Cost_Exploit[0] = Math.Abs(InitialFBest - Costs_Exploit[0]);    // Eq(8)
            Seq_Cost_Explore[1] = Math.Abs(Costs_Explor[1] - Costs_Explor[0]);  // Eq(6)
            Seq_Cost_Exploit[1] = Math.Abs(Costs_Exploit[1] - Costs_Exploit[0]);    // Eq(9)
            Seq_Cost_Explore[2] = Math.Abs(Costs_Explor[2] - Costs_Explor[1]);  // Eq(7)
            Seq_Cost_Exploit[2] = Math.Abs(Costs_Exploit[2] - Costs_Exploit[1]);    // Eq(10)

            for (int i = 0; i < 3; i++)
            {
                if (Seq_Cost_Explore[i] != 0)
                    PF_F3.Add(Seq_Cost_Explore[i]);
                if (Seq_Cost_Exploit[i] != 0)
                    PF_F3.Add(Seq_Cost_Exploit[i]);
            }

            var F1_Explor = Pf1 * (Seq_Cost_Explore[0] / Seq_Time_Explore[0]);  // Eq(1)
            var F1_Exploit = Pf1 * (Seq_Cost_Exploit[0] / Seq_Time_Exploit[0]); // Eq(2)
            var F2_Explor = pf2 * ((Seq_Cost_Explore[0] + Seq_Cost_Explore[1] + Seq_Cost_Explore[2]) / (Seq_Time_Explore[0] + Seq_Time_Explore[1] + Seq_Time_Explore[2]));  // Eq(3)
            var F2_Exploit = pf2 * ((Seq_Cost_Exploit[0] + Seq_Cost_Exploit[1] + Seq_Cost_Exploit[2]) / (Seq_Time_Exploit[0] + Seq_Time_Exploit[1] + Seq_Time_Exploit[2])); // Eq(4)

            // Score calculation
            Score_Explore = (Pf1 * F1_Explor) + (pf2 * F2_Explor);  // Eq(11)
            Score_Exploit = (Pf1 * F1_Exploit) + (pf2 * F2_Exploit);    // Eq(12)

            // Experienced Phase
            for (; CurrentIteration < TargetIteration && Running; CurrentIteration++)
            {

                bool SelectFlag;
                double[] Count_select = new double[2];

                if (Score_Explore > Score_Exploit)
                {
                    // Exploration
                    SelectFlag = false;
                    Pumas = Exploration(Pumas, f, domain);
                    Count_select = UnSelected;
                    UnSelected[1] = UnSelected[1] + 1;
                    UnSelected[0] = 1;
                    F3_Explore = pf3;
                    F3_Exploit = F3_Exploit + pf3;

                    Puma TBest = Pumas.OrderBy(p => p.Fitness).First();
                    Seq_Cost_Explore[2] = Seq_Cost_Explore[1];
                    Seq_Cost_Explore[1] = Seq_Cost_Explore[0];
                    Seq_Cost_Explore[0] = Math.Abs(fbest - TBest.Fitness);

                    if (Seq_Cost_Exploit[0] != 0)
                        PF_F3.Add(Seq_Cost_Explore[0]);

                    if (TBest.Fitness < fbest)
                    {
                        for (int i = 0; i < xbest.Length; i++)
                        {
                            xbest[i] = TBest.Position[i];
                        }
                        fbest = TBest.Fitness;
                    }
                }
                else
                {
                    // Exploitation
                    SelectFlag = true;
                    Pumas = Exploitation(Pumas, f, domain);
                    Count_select = UnSelected;
                    UnSelected[0] = UnSelected[0] + 1;
                    UnSelected[1] = 1;
                    F3_Explore = F3_Explore + pf3;
                    F3_Exploit = pf3;

                    Puma TBest = Pumas.OrderBy(p => p.Fitness).First();
                    Seq_Cost_Exploit[2] = Seq_Cost_Exploit[1];
                    Seq_Cost_Exploit[1] = Seq_Cost_Exploit[0];
                    Seq_Cost_Exploit[0] = Math.Abs(fbest - TBest.Fitness);

                    if (Seq_Cost_Exploit[0] != 0)
                        PF_F3.Add(Seq_Cost_Exploit[0]);

                    if (TBest.Fitness < fbest)
                    {
                        for (int i = 0; i < xbest.Length; i++)
                        {
                            xbest[i] = TBest.Position[i];
                        }
                        fbest = TBest.Fitness;
                    }
                }

                if (Flag_Change != SelectFlag)
                {
                    Flag_Change = SelectFlag;
                    Seq_Time_Explore[2] = Seq_Time_Explore[1];
                    Seq_Time_Explore[1] = Seq_Time_Explore[0];
                    Seq_Time_Explore[0] = Count_select[0];
                    Seq_Time_Exploit[2] = Seq_Time_Exploit[1];
                    Seq_Time_Exploit[1] = Seq_Time_Exploit[0];
                    Seq_Time_Exploit[0] = Count_select[1];
                }

                F1_Explor = Pf1 * (Seq_Cost_Explore[0] / Seq_Time_Explore[0]);    // Eq(14)
                F1_Exploit = Pf1 * (Seq_Cost_Exploit[0] / Seq_Time_Exploit[0]);   // Eq(13)
                F2_Explor = pf2 * ((Seq_Cost_Explore[0] + Seq_Cost_Explore[1] + Seq_Cost_Explore[2]) / (Seq_Time_Explore[0] + Seq_Time_Explore[1] + Seq_Time_Explore[2]));    // Eq(16)
                F2_Exploit = pf2 * ((Seq_Cost_Exploit[0] + Seq_Cost_Exploit[1] + Seq_Cost_Exploit[2]) / (Seq_Time_Exploit[0] + Seq_Time_Exploit[1] + Seq_Time_Exploit[2]));   // Eq(15)

                // calculate function value Eq(17) and Eq(18)
                if (Score_Explore < Score_Exploit)
                {
                    Mega_Explor = Math.Max((Mega_Explor - 0.01), 0.01);
                    Mega_Exploit = 0.99;
                }
                else if (Score_Explore > Score_Exploit)
                {
                    Mega_Explor = 0.99;
                    Mega_Exploit = Math.Max((Mega_Exploit - 0.01), 0.01);
                }

                double lmn_Explore = 1 - Mega_Explor;   // Eq(24)
                double lmn_Exploit = 1 - Mega_Exploit;  // Eq(22)

                if (PF_F3.Any())
                {
                    Score_Explore = (Mega_Explor * F1_Explor) + (Mega_Explor * F2_Explor) + (lmn_Explore * (PF_F3.Min() * F3_Explore));  // Eq(20)
                    Score_Exploit = (Mega_Exploit * F1_Exploit) + (Mega_Exploit * F2_Exploit) + (lmn_Exploit * (PF_F3.Min() * F3_Exploit));  // Eq(19)
                }
                else
                {
                    Score_Explore = (Mega_Explor * F1_Explor) + (Mega_Explor * F2_Explor) + (lmn_Explore * F3_Explore);  // Eq(20)
                    Score_Exploit = (Mega_Exploit * F1_Exploit) + (Mega_Exploit * F2_Exploit) + (lmn_Exploit * F3_Exploit);  // Eq(19)
                }

                writer = new PumaStateWriter(CurrentIteration, population, dimensions, TargetIteration, pf1, pf2, pf3, l, u, a, n_call, Pumas, functionName);
            }
            stringReportGenerator = new PumaTextReport(functionName, fbest, xbest, TargetIteration, dimensions, parameters);
            pdfReportGenerator = new PumaPDFReport(functionName, fbest, xbest, TargetIteration, dimensions, parameters);
            Console.WriteLine(stringReportGenerator.ReportString);
        }

        private void boundaryControl(ref double[] args, double[,] domain)
        {
            for (int i = 0; i < dimensions; i++)
            {
                if (args[i] > domain[1, i])
                {
                    args[i] = domain[1, i];
                }

                else if (args[i] < domain[0, i])
                {
                    args[i] = domain[0, i];
                }
            }
        }

        private Puma[] Exploration(Puma[] Pumas, fitnessFunction fun, double[,] domain)
        {
            Pumas.OrderBy(p => p.Fitness);
            double pCR = u; //eq 28
            double PCR = 1 - pCR; //eq 29
            double p = PCR / population;
            double[] x = new double[dimensions];
            double[] y = Randn(dimensions);
            double[] z = new double[dimensions];
            double[] newArgs = new double[dimensions];

            for (int i = 0; i < population; i++)
            {

                for (int j = 0; j < dimensions; j++)
                {
                    x[j] = Pumas[i].Position[j];
                }
                int[] help_arr = Enumerable.Range(0, population - 1).ToArray();
                var A = help_arr.OrderBy(z => rnd.Next()).ToList();
                A.Remove(i);
                int a = A[0];
                int b = A[1];
                int c = A[2];
                int d = A[3];
                int e = A[4];
                int f = A[5];
                double G = 2 * rnd.NextDouble() - 1; //eq 26
                if (rnd.NextDouble() < 0.5)
                {
                    for (int j = 0; j < dimensions; j++)
                    {
                        y[j] *= (domain[1, j] - domain[0, j]) + domain[0, j]; //Eq 25
                    }
                }
                else
                {
                    for (int j = 0; j < dimensions; j++)
                    {
                        y[j] = Pumas[a].Position[j] + G * (Pumas[a].Position[j] - Pumas[b].Position[j]) + G * (((Pumas[a].Position[j] - Pumas[b].Position[j]) - (Pumas[c].Position[j] - Pumas[d].Position[j])) + ((Pumas[c].Position[j] - Pumas[d].Position[j]) - (Pumas[e].Position[j] - Pumas[f].Position[j]))); //Eq 25
                    }
                }
                boundaryControl(ref y, domain);
                double j0 = rnd.Next(dimensions);
                for (int j = 0; j < dimensions; j++)
                {
                    if (j == j0 || rnd.NextDouble() < pCR)
                        z[j] = y[j];
                    else
                        z[j] = x[j];
                }
                newArgs = z;
                double newFit = callFunction(newArgs, fun);

                if (newFit < Pumas[i].Fitness)
                {
                    Pumas[i].Position = newArgs;
                    Pumas[i].Fitness = newFit;
                }
            }

            return Pumas;
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
        private double callFunction(double[] args, fitnessFunction f)
        {
            n_call++;
            return f(args);
        }

        private Puma[] Exploitation(Puma[] Pumas, fitnessFunction fun, double[,] domain)
        {
            for (int i = 0; i < population; i++)
            {
                double[] beta = Randn(dimensions);
                double[] w = Randn(dimensions); // eq 37
                double[] v = Randn(dimensions); // eq 38
                double[] F1 = Randn(dimensions);
                double[] F2 = new double[dimensions];
                double R = 2 * rnd.NextDouble() - 1; // eq 34
                double[] mBest = new double[dimensions];
                double[] S1 = Randn(dimensions);
                double rand9 = rnd.NextDouble() * 2 - 1;
                double[] S2 = new double[dimensions];
                double[] Xattack = new double[dimensions];

                for (int j = 0; j < dimensions; j++)
                {
                    F1[j] *= Math.Exp(2 - (CurrentIteration + 1) * (2 / TargetIteration)); // eq 35
                    F2[j] = w[j] * v[j] * v[j] * Math.Cos(2 * rnd.NextDouble() * w[j]); // eq 36
                    var sum = 0.0;
                    for (int k = 0; k < population; k++)
                    {
                        sum += Pumas[k].Position[j];
                    }
                    mBest[j] = (sum / population) / population;
                    S1[j] += rand9;
                    S2[j] = F1[j] * R * Pumas[i].Position[j] + F2[j] * (1 - R) * xbest[j];
                    Xattack[j] = S2[j] / S1[j];
                }

                // eq 32
                double[] newArgs = new double[dimensions];
                if (rnd.NextDouble() <= 0.5)
                {
                    if (rnd.NextDouble() <= l)
                    {
                        for (int j = 0; j < dimensions; j++)
                            newArgs[j] = xbest[j] + 2 * rnd.NextDouble() * Math.Exp(beta[j]) * (Pumas[rnd.Next(population)].Position[j] - Pumas[i].Position[j]);
                    }
                    else
                    {
                        for (int j = 0; j < dimensions; j++)
                            newArgs[j] = 2 * rnd.NextDouble() * Xattack[j] - xbest[j];
                    }
                }
                else
                {
                    int r1 = (int)Math.Round((population - 1) * rnd.NextDouble());
                    for (int j = 0; j < dimensions; j++)
                        newArgs[j] = mBest[j] * Pumas[r1].Position[j] - Math.Pow(-1, rnd.Next(2) * Pumas[i].Position[j] / (1 + a * rnd.NextDouble()));
                }

                boundaryControl(ref newArgs, domain);

                double newFit = callFunction(newArgs, fun);

                if (newFit < Pumas[i].Fitness)
                {
                    Pumas[i].Position = newArgs;
                    Pumas[i].Fitness = newFit;
                }
            }
            return Pumas;
        }



    }
}