
using AI_For_Engineering_purposes__metaheuristics_;
using AI_For_Engineering_purposes_metaheuristics;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_functions;
using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms;

//static void printEnd(double[] fbest, double[][] xbest, TestFunction problem, Pwolf)
//{
//    double avg = fbest.Average();
//    double sumSquare = fbest.Select(val => (val - avg) * (val - avg)).Sum();
//    double sd = Math.Sqrt(sumSquare/fbest.Length);
//    double changeCoeff = sd * 100 / avg;

//    double[][] xBestT = new double[xbest[0].Length][];
//    for (int i = 0; i < xbest[0].Length; i++)
//    {
//        xBestT[i] = new double[xbest.Length];
//    }

//    for(int i = 0; i< xbest.Length; i++)
//    {
//        for(int j =0; j< xbest[i].Length; j++)
//        {
//            xBestT[j][i] = xbest[i][j];
//        }
//    }

//    double[] xAvgs = new double[xBestT.Length];
//    double[] xSds = new double[xBestT.Length];
//    double[] xChangeCoeffs = new double[xBestT.Length];

//    for (int i = 0; i < xBestT.Length; i++)
//    {
//        xAvgs[i] = xBestT[i].Average();
//        double sumSq = xBestT[i].Select(val => (val - avg) * (val - avg)).Sum();
//        xSds[i] = Math.Sqrt(sumSq / xBestT[i].Length);
//        xChangeCoeffs[i] = xSds[i] * 100 / xAvgs[i];
//    }

//    int best_index = 0;
//    for (int i = 0; i < fbest.Length; i++)
//    {
//        if (fbest[best_index] > fbest[i])
//        {
//            best_index = i;
//        }
//    }

//    string endPosition = "(";
//    foreach (var x in xbest[best_index])
//    {
//        endPosition += $"{x}, ";
//    }
//    endPosition += ")";

//    string endCoeff = "(";
//    foreach (var x in xChangeCoeffs)
//    {
//        endCoeff += $"{x}%, ";
//    }
//    endCoeff += ")";

//    string endSD = "(";
//    foreach (var x in xSds)
//    {
//        endSD += $"{x}, ";
//    }
//    endSD += ")";

//    Console.Write($"{puma.Name}, {problem.Name}, {puma.NDimension}, {puma.PF1}, {puma.PF2}, {puma.PF3}, {puma.L}, {puma.U}, {puma.Alpha}, {puma.NIterations}, {puma.Population} ");
//    Console.Write($"{endPosition},{endSD}, {endCoeff}, {fbest[best_index]}, {sd}, {changeCoeff}% \n");
//}

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using AI_For_Engineering_purposes__metaheuristics_.Interfaces;

static void printEndPuma(double[] fbest, double[][] xbest, IFunction problem, PumaOptimization puma)
{
    double avg = fbest.Average();
    double sumSquare = fbest.Select(val => (val - avg) * (val - avg)).Sum();
    double sd = Math.Sqrt(sumSquare / fbest.Length);
    double changeCoeff = sd * 100 / avg;

    double[][] xBestT = new double[xbest[0].Length][];
    for (int i = 0; i < xbest[0].Length; i++)
    {
        xBestT[i] = new double[xbest.Length];
    }

    for (int i = 0; i < xbest.Length; i++)
    {
        for (int j = 0; j < xbest[i].Length; j++)
        {
            xBestT[j][i] = xbest[i][j];
        }
    }

    double[] xAvgs = new double[xBestT.Length];
    double[] xSds = new double[xBestT.Length];
    double[] xChangeCoeffs = new double[xBestT.Length];

    for (int i = 0; i < xBestT.Length; i++)
    {
        xAvgs[i] = xBestT[i].Average();
        double sumSq = xBestT[i].Select(val => (val - avg) * (val - avg)).Sum();
        xSds[i] = Math.Sqrt(sumSq / xBestT[i].Length);
        xChangeCoeffs[i] = xSds[i] * 100 / xAvgs[i];
    }

    int best_index = 0;
    for (int i = 0; i < fbest.Length; i++)
    {
        if (fbest[best_index] > fbest[i])
        {
            best_index = i;
        }
    }

    string endPosition = "(";
    foreach (var x in xbest[best_index])
    {
        endPosition += $"{x}, ";
    }
    endPosition += ")";

    string endCoeff = "(";
    foreach (var x in xChangeCoeffs)
    {
        endCoeff += $"{x}%, ";
    }
    endCoeff += ")";

    string endSD = "(";
    foreach (var x in xSds)
    {
        endSD += $"{x}, ";
    }
    endSD += ")";
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    FileInfo fileInfo = new FileInfo("Results.xlsx");
    using (ExcelPackage package = new ExcelPackage(fileInfo))
    {
        ExcelWorksheet worksheet;
        if (package.Workbook.Worksheets.Count == 0)
        {
            worksheet = package.Workbook.Worksheets.Add("Results");

            // Add headers
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Problem";
            worksheet.Cells[1, 3].Value = "NDimension";
            worksheet.Cells[1, 4].Value = "PF1";
            worksheet.Cells[1, 5].Value = "PF2";
            worksheet.Cells[1, 6].Value = "PF3";
            worksheet.Cells[1, 7].Value = "L";
            worksheet.Cells[1, 8].Value = "U";
            worksheet.Cells[1, 9].Value = "Alpha";
            worksheet.Cells[1, 10].Value = "NIterations";
            worksheet.Cells[1, 11].Value = "Population";
            worksheet.Cells[1, 12].Value = "End Position";
            worksheet.Cells[1, 13].Value = "End SD";
            worksheet.Cells[1, 14].Value = "End Coeff";
            worksheet.Cells[1, 15].Value = "Best F";
            worksheet.Cells[1, 16].Value = "SD";
            worksheet.Cells[1, 17].Value = "Change Coeff";
        }
        else
        {
            worksheet = package.Workbook.Worksheets[0];
        }

        int row = worksheet.Dimension.End.Row + 1;

        // Add data
        worksheet.Cells[row, 1].Value = puma.Name;
        worksheet.Cells[row, 2].Value = problem.Name;
        worksheet.Cells[row, 3].Value = puma.dimensions;
        worksheet.Cells[row, 4].Value = puma.pf1;
        worksheet.Cells[row, 5].Value = puma.pf2;
        worksheet.Cells[row, 6].Value = puma.pf3;
        worksheet.Cells[row, 7].Value = puma.l;
        worksheet.Cells[row, 8].Value = puma.u;
        worksheet.Cells[row, 9].Value = puma.a;
        worksheet.Cells[row, 10].Value = puma.iterations;
        worksheet.Cells[row, 11].Value = puma.population;
        worksheet.Cells[row, 12].Value = endPosition;
        worksheet.Cells[row, 13].Value = endSD;
        worksheet.Cells[row, 14].Value = endCoeff;
        worksheet.Cells[row, 15].Value = fbest[best_index];
        worksheet.Cells[row, 16].Value = sd;
        worksheet.Cells[row, 17].Value = changeCoeff;

        package.Save();
    }

   

    Console.Write($"{puma.Name}, {problem.Name}, {puma.dimensions}, {puma.pf1}, {puma.pf2}, {puma.pf3}, {puma.l}, {puma.u}, {puma.a}, {puma.iterations}, {puma.population} ");
    Console.Write($"{endPosition},{endSD}, {endCoeff}, {fbest[best_index]}, {sd}, {changeCoeff}% \n");
}

static void printEndWolf(double[] fbest, double[][] xbest, IFunction problem, GreyWolfOptimization wolf  )
{
    double avg = fbest.Average();
    double sumSquare = fbest.Select(val => (val - avg) * (val - avg)).Sum();
    double sd = Math.Sqrt(sumSquare / fbest.Length);
    double changeCoeff = sd * 100 / avg;

    double[][] xBestT = new double[xbest[0].Length][];
    for (int i = 0; i < xbest[0].Length; i++)
    {
        xBestT[i] = new double[xbest.Length];
    }

    for (int i = 0; i < xbest.Length; i++)
    {
        for (int j = 0; j < xbest[i].Length; j++)
        {
            xBestT[j][i] = xbest[i][j];
        }
    }

    double[] xAvgs = new double[xBestT.Length];
    double[] xSds = new double[xBestT.Length];
    double[] xChangeCoeffs = new double[xBestT.Length];

    for (int i = 0; i < xBestT.Length; i++)
    {
        xAvgs[i] = xBestT[i].Average();
        double sumSq = xBestT[i].Select(val => (val - avg) * (val - avg)).Sum();
        xSds[i] = Math.Sqrt(sumSq / xBestT[i].Length);
        xChangeCoeffs[i] = xSds[i] * 100 / xAvgs[i];
    }

    int best_index = 0;
    for (int i = 0; i < fbest.Length; i++)
    {
        if (fbest[best_index] > fbest[i])
        {
            best_index = i;
        }
    }

    string endPosition = "(";
    foreach (var x in xbest[best_index])
    {
        endPosition += $"{x}, ";
    }
    endPosition += ")";

    string endCoeff = "(";
    foreach (var x in xChangeCoeffs)
    {
        endCoeff += $"{x}%, ";
    }
    endCoeff += ")";

    string endSD = "(";
    foreach (var x in xSds)
    {
        endSD += $"{x}, ";
    }
    endSD += ")";
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    FileInfo fileInfo = new FileInfo("Results.xlsx");
    using (ExcelPackage package = new ExcelPackage(fileInfo))
    {
        ExcelWorksheet worksheet;
        if (package.Workbook.Worksheets.Count == 1)
        {
            worksheet = package.Workbook.Worksheets.Add("ResultsGWO");

            // Add headers
            worksheet.Cells[1, 1].Value = "Algorithm";
            worksheet.Cells[1, 2].Value = "Problem";
            worksheet.Cells[1, 3].Value = "NDimension";
            worksheet.Cells[1, 4].Value = "NIterations";
            worksheet.Cells[1, 5].Value = "Population";
            worksheet.Cells[1, 6].Value = "End Position";
            worksheet.Cells[1, 7].Value = "End SD";
            worksheet.Cells[1, 8].Value = "End Coeff";
            worksheet.Cells[1, 9].Value = "Best F";
            worksheet.Cells[1, 10].Value = "SD";
            worksheet.Cells[1, 11].Value = "Change Coeff";
        }
        else
        {
            worksheet = package.Workbook.Worksheets[1];
        }

        int row = worksheet.Dimension.End.Row + 1;

        // Add data
        worksheet.Cells[row, 1].Value  = wolf.Name;
        worksheet.Cells[row, 2].Value = problem.Name;
        worksheet.Cells[row, 3].Value  = wolf.dimensions;
        worksheet.Cells[row, 4].Value = wolf.iterations;
        worksheet.Cells[row, 5].Value = wolf.population;
        worksheet.Cells[row, 6].Value = endPosition;
        worksheet.Cells[row, 7].Value = endSD;
        worksheet.Cells[row, 8].Value = endCoeff;
        worksheet.Cells[row, 9].Value = fbest[best_index];
        worksheet.Cells[row, 10].Value = sd;
        worksheet.Cells[row, 11].Value = changeCoeff;

        package.Save();
    }

    Console.Write($"{wolf.Name}, {problem.Name}, {wolf.dimensions},{ wolf.iterations}, {wolf.population} ");
    Console.Write($"{endPosition},{endSD}, {endCoeff}, {fbest[best_index]}, {sd}, {changeCoeff}% \n");
}


int[] N = { 10, 20, 40, 80 };
int[] I = { 5, 10, 20, 40, 60, 80 };
int[] D = { 2, 5, 10, 30};




    var prblm = new Beale();
  

    for (int j = 0; j < N.Length; j++)
    {
        for (int k = 0; k < I.Length; k++)
        {
            double[] fbests = new double[10];
            double[][] xbests = new double[10][];
        var algorithm = new PumaOptimization();
        double[] parameters = { N[j], I[k], D[0], algorithm.pf1, algorithm.pf2, algorithm.pf3, algorithm.l, algorithm.u, algorithm.a };
        
        
        


             for (int i = 0; i < 10; i++)
            {
            algorithm.Solve(prblm.Function, prblm.domain(), prblm.Name, parameters);
            fbests[i] = algorithm.Fbest;
            xbests[i] = algorithm.Xbest;
        }
            printEndPuma(fbests, xbests, prblm, algorithm);
        }
    }


double[] pf1 = { 0.2, 0.5, 0.8 };
double[] pf2 = { 0.2, 0.5, 0.8 };
double[] pf3 = { 0.1, 0.3, 0.5 };
double[] U = { 0.2, 0.4, 0.6 };
double[] L = { 0.5, 0.7, 0.9 };
int[] ALPHA = { 1, 2, 3 };
var problem = new Beale();




void solve_alogrithm(IOptimizationAlgorithm algorithm, double[] P1 = null, double[]P2=null, double[] P3 = null , double[]P4 = null, double[]P5 = null, double[] P6=null)
{
    P1 ??= new Double[] { 1 };
    P2 ??= new Double[] { 1 };
    P3 ??= new Double[] { 1 };
    P4 ??= new Double[] { 1 };
    P5 ??= new Double[] { 1 };
    P6 ??= new Double[] { 1 };

    bool is_running = true;
    
    int[] N = { 10, 20, 40, 80 };
    int[] I = { 5, 10, 20, 40, 60, 80 };
    int[] D = { 2, 5, 10, 30 };

    async Task WaitWhile()
    {
        while (!is_running)
        {
            await Task.Delay(100);
        }

    }
     var problem = new Beale();

    double[] fbests = new double[10];
    double[][] xbests = new double[10][];
   // algorithm.Solve(problem.Function, problem.domain() as double[], problem.Name, [N[0], I[0]]);                       


    


}




/*for (int j = 0; j < pf1.Length; j++)
{
    for (int k = 0; k < pf2.Length; k++)
    {
        for (int l = 0; l < pf3.Length; l++)
        {
            for (int m = 0; m < U.Length; m++)
            {
                for (int n = 0; n < L.Length; n++)
                {
                    for (int o = 0; o < ALPHA.Length; o++)
                    {
                        double[] fbests = new double[10];
                        double[][] xbests = new double[10][];
                        var optimizer = new PO(I[1], N[1], problem.UpperBoundaries, problem.LowerBoundaries, problem.function, $"{problem.Name}", pf1[j], pf2[k], pf3[l], U[m], L[n], ALPHA[o]);
                        for (int i = 0; i < 10; i++)
                        {
                            optimizer = new PO(I[1], N[1], problem.UpperBoundaries, problem.LowerBoundaries, problem.function, $"{problem.Name}", pf1[j], pf2[k], pf3[l], U[m], L[n], ALPHA[o]);
                            fbests[i] = optimizer.Solve();
                            xbests[i] = optimizer.XBest;
                        }
                        printEndPuma(fbests, xbests, problem, optimizer);
                    }
                }

            }
        }
    }

}*/


