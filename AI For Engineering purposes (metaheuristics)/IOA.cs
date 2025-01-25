using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes_metaheuristics
{
    public interface IOA
    {
        // Nazwa algorytmu
        string Name { get; set; }

        // Metoda rozpoczynaj ąca rozwi ą zywanie zagadnienia poszukiwania
        // minimum funkcji celu .
        // Na pocz ątku sprawdza , czy w lokalizacji jest plik ze stanem algorytmu
        // w odpowiednim formacie . Jeśli taki plik istnieje , wznawia obliczenia
        // dla tego stanu ,
        // W przeciwnym razie zaczyna obliczenia od pocz ą tku .
        // Zwraca warto ść funkcji celu dla znalezionego rozwi ą zania
        // ( najlepszego osobnika )
        double Solve();

        // Właś ciow ść zwracaj ąca tablic ę z najlepszym osobnikiem
        double[] XBest { get; set; }

        // Właś ciwo ść zwracaj ąca warto ść funkcji dopasowania
        // dla najlepszego osobnika
        double FBest { get; set; }

        // Właś ciwo ść zwracaj ąca liczb ę wywo łań funkcji dopasowania
        int NumberOfEvaluationFitnessFunction { get; set; }
    }
}
