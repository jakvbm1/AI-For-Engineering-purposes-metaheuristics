
using AI_For_Engineering_purposes__metaheuristics_;
using AI_For_Engineering_purposes__metaheuristics_.Metaheuristics;
using AI_For_Engineering_purposes_metaheuristics;

//static void printEnd(double[] fbest, double[][] xbest, TestFunction problem, PO puma)
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

static void printEndPuma(double[] fbest, double[][] xbest, TestFunction problem, PO puma)
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
        worksheet.Cells[row, 3].Value = puma.NDimension;
        worksheet.Cells[row, 4].Value = puma.PF1;
        worksheet.Cells[row, 5].Value = puma.PF2;
        worksheet.Cells[row, 6].Value = puma.PF3;
        worksheet.Cells[row, 7].Value = puma.L;
        worksheet.Cells[row, 8].Value = puma.U;
        worksheet.Cells[row, 9].Value = puma.Alpha;
        worksheet.Cells[row, 10].Value = puma.NIterations;
        worksheet.Cells[row, 11].Value = puma.Population;
        worksheet.Cells[row, 12].Value = endPosition;
        worksheet.Cells[row, 13].Value = endSD;
        worksheet.Cells[row, 14].Value = endCoeff;
        worksheet.Cells[row, 15].Value = fbest[best_index];
        worksheet.Cells[row, 16].Value = sd;
        worksheet.Cells[row, 17].Value = changeCoeff;

        package.Save();
    }

    Console.Write($"{puma.Name}, {problem.Name}, {puma.NDimension}, {puma.PF1}, {puma.PF2}, {puma.PF3}, {puma.L}, {puma.U}, {puma.Alpha}, {puma.NIterations}, {puma.Population} ");
    Console.Write($"{endPosition},{endSD}, {endCoeff}, {fbest[best_index]}, {sd}, {changeCoeff}% \n");
}

static void printEndWolf(double[] fbest, double[][] xbest, TestFunction problem, GreyWolfOptimizer puma)
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
        worksheet.Cells[row, 1].Value = puma.Name;
        worksheet.Cells[row, 2].Value = problem.Name;
        worksheet.Cells[row, 3].Value = puma.NDimension;
        worksheet.Cells[row, 4].Value = puma.NIterations;
        worksheet.Cells[row, 5].Value = puma.Population;
        worksheet.Cells[row, 6].Value = endPosition;
        worksheet.Cells[row, 7].Value = endSD;
        worksheet.Cells[row, 8].Value = endCoeff;
        worksheet.Cells[row, 9].Value = fbest[best_index];
        worksheet.Cells[row, 10].Value = sd;
        worksheet.Cells[row, 11].Value = changeCoeff;

        package.Save();
    }

    Console.Write($"{puma.Name}, {problem.Name}, {puma.NDimension},  {puma.NIterations}, {puma.Population} ");
    Console.Write($"{endPosition},{endSD}, {endCoeff}, {fbest[best_index]}, {sd}, {changeCoeff}% \n");
}


int[] N = { 10, 20, 40, 80 };
int[] I = { 5, 10, 20, 40, 60, 80 };
int[] D = { 2, 5, 10, 30};



//for (int h = 0; h < D.Length; h++)
//{
    var prblm = new Beale();
    //var optimizer = new PO(100, 100, prblm.UpperBoundaries, prblm.LowerBoundaries, prblm.function, $"{prblm.Name}");

    for (int j = 0; j < N.Length; j++)
    {
        for (int k = 0; k < I.Length; k++)
        {
            double[] fbests = new double[10];
            double[][] xbests = new double[10][];
            var optimizer = new PO(I[k], N[j], prblm.UpperBoundaries, prblm.LowerBoundaries, prblm.function, $"{prblm.Name}");
            //var optimizer = new GreyWolfOptimizer(prblm.function, N[j], I[k], prblm.UpperBoundaries, prblm.LowerBoundaries);
            for (int i = 0; i < 10; i++)
            {
                optimizer = new PO(I[k], N[j], prblm.UpperBoundaries, prblm.LowerBoundaries, prblm.function, $"{prblm.Name}");
                fbests[i] = optimizer.Solve();
                xbests[i] = optimizer.XBest;
            }
            printEndPuma(fbests, xbests, prblm, optimizer);
        }
    }
   // }

    double[] pf1 = { 0.2, 0.5, 0.8 };
    double[] pf2 = { 0.2, 0.5, 0.8 };
    double[] pf3 = { 0.1, 0.3, 0.5 };
    double[] U = { 0.2, 0.4, 0.6 };
    double[] L = { 0.5, 0.7, 0.9 };
    int[] ALPHA = { 1, 2, 3 };
    var problem =new Beale();
    foreach (var p1 in pf1)
    {
        foreach (var p2 in pf2)
        {
            foreach (var p3 in pf3)
            {
                foreach (var u in U)
                {
                    foreach (var l in L)
                    {
                        foreach (var a in ALPHA)
                        {
                            double[] fbests = new double[10];
                            double[][] xbests = new double[10][];
                            var optimizer = new PO(I[1], N[1], problem.UpperBoundaries, problem.LowerBoundaries, problem.function, $"{problem.Name}", p1, p2, p3, u, l, a);
                            for (int i = 0; i < 10; i++)
                            {
                                optimizer = new PO(I[1], N[1], problem.UpperBoundaries, problem.LowerBoundaries, problem.function, $"{problem.Name}", p1, p2, p3, u, l, a);
                                fbests[i] = optimizer.Solve();
                                xbests[i] = optimizer.XBest;
                            }
                            printEndPuma(fbests, xbests, problem, optimizer);
                        }
                    }
                }
            }
        }
    }





