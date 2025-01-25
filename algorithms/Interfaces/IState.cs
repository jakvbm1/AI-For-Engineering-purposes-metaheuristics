using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.Interfaces
{
    public delegate double fitnessFunction(params double[] args);

    public interface IStateWriter
    {
        void SaveToFileStateOfAlgorithm(string path);
    }

    public interface IStateReader
    {
        void LoadFromFileStateOfAlgorithm(string path);
    }
}
