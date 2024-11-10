using AI_For_Engineering_purposes_metaheuristics;
using System.Globalization;

namespace AI_For_Engineering_purposes__metaheuristics_.Metaheuristics
{
    class PO : IOptimizationAlgorithm
    {

        //zmienne pomocnicze do "nadzorowania pracy algorytmu
        private string name = "Puma Optimizer";
        private int nOfCalls = 0;
        private long evaluationTime = 0;
        private int currentIteration = 0;
        private double[] fBestHistory; 
        private double fBest;
        private double[] xBest;
        private string problemName;


        //zmienne do opdalenia algorytmu z dana funkcja
        private int nDimensions;
        private int nIterations;
        private int population;
        private double[] upperBoundaries;
        private double[] lowerBoundaries;
        private Func<double[], double> f;
        private Puma[] Pumas;
        private Random rnd = new Random();
        //parametry algorytmu do "dostrojenia"
        double PF1, PF2, PF3, U, L, Alpha;

        public PO(int nIterations, int population, double[] upperBoundaries, double[] lowerBoundaries, Func<double[], double> f, string problemName = "unknown", double PF1 = 0.5, double PF2 = 0.5, double PF3 = 0.3, double U = 0.4, double L = 0.7, double Alpha = 2)
        {
            this.problemName = problemName;
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
            this.Pumas = new Puma[population];
        }

        public string Name { get => name; set => name = value;}
        public double[] XBest { get => xBest; set => xBest = value; }
        public double FBest { get => fBest; set => fBest = value; }
        public int NumberOfEvaluationFitnessFunction { get => nOfCalls; set => nOfCalls = value; }

        private Puma[] Exploitation(Puma[] Pumas)
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
                        sum += Pumas[k].Position[j];
                    }
                    mBest[j] = (sum / population) / population;
                    S1[j] += rand9;
                    S2[j] = F1[j] * R * Pumas[i].Position[j] + F2[j] * (1 - R) * xBest[j];
                    Xattack[j] = S2[j] / S1[j];
                }

                // eq 32
                double[] newArgs = new double[nDimensions];
                if (rnd.NextDouble() <= 0.5)
                {
                    if (rnd.NextDouble() <= L)
                    {
                        for (int j = 0; j < nDimensions; j++)
                            newArgs[j] = xBest[j] + 2 * rnd.NextDouble() * Math.Exp(beta[j]) * (Pumas[rnd.Next(population)].Position[j] - Pumas[i].Position[j]);
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
                        newArgs[j] = mBest[j] * Pumas[r1].Position[j] - Math.Pow(-1, rnd.Next(2) * Pumas[i].Position[j] / (1 + Alpha * rnd.NextDouble()));
                }

                boundaryControl(newArgs);

                double newFit = callFunction(newArgs);

                if (newFit < Pumas[i].Fitness)
                {
                    Pumas[i].Position = newArgs;
                    Pumas[i].Fitness = newFit;
                }
            }
            return Pumas;
        }

        // tutaj pracuj tymku
        private Puma[] Exploration(Puma[] Pumas)
        {
            
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
        private double callFunction(double[] args)
        {
            nOfCalls++;
            return f(args);
        }

        public double Solve()
        {
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
            double[] Costs_Explor = new double[nIterations];
            double[] Costs_Exploit = new double[nIterations];

            var watch = System.Diagnostics.Stopwatch.StartNew();
            initializePopulation();

            int ind = Array.IndexOf(Pumas, Pumas.OrderBy(p => p.Fitness).First());
            XBest = Pumas[ind].Position;
            FBest = Pumas[ind].Fitness;

            bool Flag_Change = false;

            double[] InitialXBest = XBest;
            double InitialFBest = FBest;

            // Unexperienced Phase
            for (; currentIteration < 3; currentIteration++)
            {
                Puma[] Pumas_explore = Exploration(Pumas);
                Costs_Explor[currentIteration] = Pumas_explore.Min(p => p.Fitness);

                Puma[] Pumas_exploit = Exploitation(Pumas);
                Costs_Exploit[currentIteration] = Pumas_exploit.Min(p => p.Fitness);

                Puma[] CombinedPumas = Pumas.Concat(Pumas_explore).Concat(Pumas_exploit).ToArray();
                Pumas = CombinedPumas.OrderBy(s => s.Fitness).Take(population).ToArray();
                XBest = Pumas[0].Position;
                FBest = Pumas[0].Fitness;
                fBestHistory[currentIteration] = FBest;
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

            var F1_Explor = PF1 * (Seq_Cost_Explore[0] / Seq_Time_Explore[0]);  // Eq(1)
            var F1_Exploit = PF1 * (Seq_Cost_Exploit[0] / Seq_Time_Exploit[0]); // Eq(2)
            var F2_Explor = PF2 * ((Seq_Cost_Explore[0] + Seq_Cost_Explore[1] + Seq_Cost_Explore[2]) / (Seq_Time_Explore[0] + Seq_Time_Explore[1] + Seq_Time_Explore[2]));  // Eq(3)
            var F2_Exploit = PF2 * ((Seq_Cost_Exploit[0] + Seq_Cost_Exploit[1] + Seq_Cost_Exploit[2]) / (Seq_Time_Exploit[0] + Seq_Time_Exploit[1] + Seq_Time_Exploit[2])); // Eq(4)

            // Score calculation
            Score_Explore = (PF1 * F1_Explor) + (PF2 * F2_Explor);  // Eq(11)
            Score_Exploit = (PF1 * F1_Exploit) + (PF2 * F2_Exploit);    // Eq(12)

            // Experienced Phase
            for (; currentIteration < nIterations; currentIteration++)
            {
                bool SelectFlag;
                double[] Count_select = new double[2];

                if (Score_Explore > Score_Exploit)
                {
                    // Exploration
                    SelectFlag = false;
                    Pumas = Exploration(Pumas);
                    Count_select = UnSelected;
                    UnSelected[1] = UnSelected[1] + 1;
                    UnSelected[0] = 1;
                    F3_Explore = PF3;
                    F3_Exploit = F3_Exploit + PF3;

                    Puma TBest = Pumas.OrderBy(p => p.Fitness).First();
                    Seq_Cost_Explore[2] = Seq_Cost_Explore[1];
                    Seq_Cost_Explore[1] = Seq_Cost_Explore[0];
                    Seq_Cost_Explore[0] = Math.Abs(FBest - TBest.Fitness);

                    if (Seq_Cost_Exploit[0] != 0)
                        PF_F3.Add(Seq_Cost_Explore[0]);

                    if (TBest.Fitness < FBest)
                    {
                        XBest = TBest.Position;
                        FBest = TBest.Fitness;
                    }
                }
                else
                {
                    // Exploitation
                    SelectFlag = true;
                    Pumas = Exploitation(Pumas);
                    Count_select = UnSelected;
                    UnSelected[0] = UnSelected[0] + 1;
                    UnSelected[1] = 1;
                    F3_Explore = F3_Explore + PF3;
                    F3_Exploit = PF3;

                    Puma TBest = Pumas.OrderBy(p => p.Fitness).First();
                    Seq_Cost_Exploit[2] = Seq_Cost_Exploit[1];
                    Seq_Cost_Exploit[1] = Seq_Cost_Exploit[0];
                    Seq_Cost_Exploit[0] = Math.Abs(FBest - TBest.Fitness);

                    if (Seq_Cost_Exploit[0] != 0)
                        PF_F3.Add(Seq_Cost_Exploit[0]);

                    if (TBest.Fitness < FBest)
                    {
                        XBest = TBest.Position;
                        FBest = TBest.Fitness;
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

                F1_Explor = PF1 * (Seq_Cost_Explore[0] / Seq_Time_Explore[0]);    // Eq(14)
                F1_Exploit = PF1 * (Seq_Cost_Exploit[0] / Seq_Time_Exploit[0]);   // Eq(13)
                F2_Explor = PF2 * ((Seq_Cost_Explore[0] + Seq_Cost_Explore[1] + Seq_Cost_Explore[2]) / (Seq_Time_Explore[0] + Seq_Time_Explore[1] + Seq_Time_Explore[2]));    // Eq(16)
                F2_Exploit = PF2 * ((Seq_Cost_Exploit[0] + Seq_Cost_Exploit[1] + Seq_Cost_Exploit[2]) / (Seq_Time_Exploit[0] + Seq_Time_Exploit[1] + Seq_Time_Exploit[2]));   // Eq(15)

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

                Score_Explore = (Mega_Explor * F1_Explor) + (Mega_Explor * F2_Explor) + (lmn_Explore * (PF_F3.Min() * F3_Explore));  // Eq(20)
                Score_Exploit = (Mega_Exploit * F1_Exploit) + (Mega_Exploit * F2_Exploit) + (lmn_Exploit * (PF_F3.Min() * F3_Exploit));  // Eq(19)

                fBestHistory[currentIteration] = FBest;
            }
            watch.Stop();
            evaluationTime =  watch.ElapsedMilliseconds / 1000;
            return FBest;
        }

        private void initializePopulation()
        {
            Random random = new Random();

            for (int i = 0; i < population; i++)
            {
                Pumas[i].Position = new double[nDimensions];

                for (int j = 0; j < nDimensions; j++)
                {
                    Pumas[i].Position[j] = lowerBoundaries[j] + rnd.NextDouble() * (upperBoundaries[j] - lowerBoundaries[j]);
                }
            }

            for (int i = 0; i < population; i++)
            {
                Pumas[i].Fitness = callFunction(Pumas[i].Position);
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

        private void saveResults()
        {
            
            Directory.CreateDirectory($"/{problemName}");
            Directory.CreateDirectory($"/I{nIterations}P{population}");

            string endstate = $"iterations {nIterations}, population {population}, number of calls {nOfCalls}, time {evaluationTime} \n";
            endstate += FBest.ToString("n", CultureInfo.GetCultureInfo("pl")) + '\n';
            foreach (var pos in XBest)
            {
                endstate += $"{pos.ToString("n", CultureInfo.GetCultureInfo("pl"))}\n";
            }
            File.WriteAllText($"{problemName}/I{nIterations}P{population}/endstate.txt", endstate);

            string progress = "";
            foreach(var best in fBestHistory)
            {
                progress += $"{best.ToString("n", CultureInfo.GetCultureInfo("pl"))}\n";
            }

            File.WriteAllText($"{problemName}/I{nIterations}P{population}/progress.txt", progress);

        }
    }

    class Puma
    {
        public required double[] Position;
        public double Fitness;
    }
}
