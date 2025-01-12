using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.Interfaces
{
    interface IFunction
    {
        public fitnessFunction Function { get; }

        public float[,] domain { get; set; }

        public string Name { get; }

        public bool IsMultiDimensional { get; }

    }
}
