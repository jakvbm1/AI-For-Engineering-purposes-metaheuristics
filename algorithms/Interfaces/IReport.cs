using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_For_Engineering_purposes__metaheuristics_.Interfaces
{
    public interface IGeneratePDFReport
    {
        void GenerateReport(string path);
    }

   public interface IGenerateTextReport
    {
        string ReportString { get; set; }
    }
}
