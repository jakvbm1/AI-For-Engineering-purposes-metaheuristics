    
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
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
/*
static void printEndPuma(double[] fbest, double[][] xbest, IFunction problem, IOptimizationAlgorithm puma)
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
       
        double[] parameters =  new double[algorithm.ParamInfo.Length];
        for (int i = 0; i < algorithm.ParamInfo.Length; i++) {
            parameters[i] = algorithm.ParamInfo[i].DefaultValue;
        
        }
        
        
        


             for (int i = 0; i < 10; i++)
            {
            algorithm.Solve(prblm.Function, prblm.domain(), prblm.Name, parameters);
            fbests[i] = algorithm.Fbest;
            xbests[i] = algorithm.Xbest;
        }
            
        
        printEndPuma(fbests, xbests, prblm, algorithm);
        }
    }*/






static async Task solve_algorithm(IOptimizationAlgorithm algorithm, IFunction prblm, double[] parameters)
{
    

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

    double[] fbests = new double[10];
    double[][] xbests = new double[10][];
    
    
    for (int i = 0; i < 10; i++)
    {
        await WaitWhile();
        algorithm.Solve(prblm.Function, prblm.domain(), prblm.Name, parameters);
        fbests[i] = algorithm.Fbest;
        xbests[i] = algorithm.Xbest;
       
        

    }
  

}
var puma = new PumaOptimization();
double[] parameters = new double[puma.ParamInfo.Length];
for (int i = 0; i < puma.ParamInfo.Length; i++) {
    parameters[i] = puma.ParamInfo[i].DefaultValue;
}
solve_algorithm(new PumaOptimization(), new Beale(), parameters);




