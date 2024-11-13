
using AI_For_Engineering_purposes__metaheuristics_;
using AI_For_Engineering_purposes__metaheuristics_.Metaheuristics;
using AI_For_Engineering_purposes_metaheuristics;

static void printEnd(double[] fbest, double[][] xbest, TestFunction problem, PO puma)
{
    double avg = fbest.Average();
    double sumSquare = fbest.Select(val => (val - avg) * (val - avg)).Sum();
    double sd = Math.Sqrt(sumSquare/fbest.Length);
    double changeCoeff = sd * 100 / avg;

    double[][] xBestT = new double[xbest[0].Length][];
    for (int i = 0; i < xbest[0].Length; i++)
    {
        xBestT[i] = new double[xbest.Length];
    }

    for(int i = 0; i< xbest.Length; i++)
    {
        for(int j =0; j< xbest[i].Length; j++)
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
        endSD += $"{x}%, ";
    }
    endSD += ")";

    Console.Write($"{puma.Name}, {problem.Name}, {puma.NDimension}, {puma.PF1}, {puma.PF2}, {puma.PF3}, {puma.L}, {puma.U}, {puma.Alpha}, {puma.NIterations}, {puma.Population} ");
    Console.Write($"{endPosition},{endSD}, {endCoeff}, {fbest[best_index]}, {sd}, {changeCoeff} \n");
}

int[] N = { 10, 20, 40, 80 };
int[] I = { 5, 10, 20, 40, 60, 80 };
var problem = new Beale();


    for (int j = 0; j < N.Length; j++)
    {
        for (int k = 0; k < I.Length; k++)
        {
        double[] fbests = new double[10];
        double[][] xbests = new double[10][];
        var optimizer = new PO(I[k], N[j], problem.UpperBoundaries, problem.LowerBoundaries, problem.function, $"{problem.Name}");
        for (int i = 0; i < 10; i++)
        {
            
            fbests[i] = optimizer.Solve();
            xbests[i] = optimizer.XBest;
        }
        printEnd(fbests, xbests, problem, optimizer);
        }
    }







