﻿using TestsAPI.Model;
using AI_For_Engineering_purposes__metaheuristics_.Interfaces;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_algorithms;
using AI_For_Engineering_purposes__metaheuristics_.rebuilt_functions;
using Microsoft.AspNetCore.Http.HttpResults;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using AI_For_Engineering_purposes__metaheuristics_;



namespace TestsAPI.Services
{
    public class TestServices
    {
        private readonly List<Test> _tests = [];
        private readonly Mutex _testsMutex = new();

        public Test CreateTest(string algorithmName, string functionName, double[] parameters, int dimensions, int iterations, string state)
        {
            var algorithm = OptimizationAlgorithms.Algorithms.FirstOrDefault(a => a.Name == algorithmName);
            var function = TestFunctions.Functions.FirstOrDefault(f => f.Name == functionName);

            if (algorithm == null || function == null) throw new Exception("Algorithm or function not found");

            algorithm.TargetIteration = iterations;
            function.Dimensions = dimensions;
            var test = new Test{ Algorithm = algorithm, Function = function, Parameters = parameters };

            _testsMutex.WaitOne();
            _tests.Add(test);
            _testsMutex.ReleaseMutex();


            if (!string.IsNullOrEmpty(state))
            {
                var f = File.CreateText(Directory.GetCurrentDirectory() + "/state/" + test.Id.ToString() + ".txt");
                f.Write(state);
                f.Close();
                algorithm.reader.LoadFromFileStateOfAlgorithm(state);
            }

            return test;
        }
        public Test StartTest(Guid id) { 
            var test = _tests.FirstOrDefault(x => x.Id == id);

            if (test == null) throw new Exception("Test not found");
            if (test.Status == TestStatus.Running || test.Status == TestStatus.Pausing) return test;

            test.Start();

            return test;
        }
        public Test StopTest(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);

            if (test == null) throw new Exception("Test not found");

            test.Pause();
            return test;
        }
        public Test GetStatus(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);

            if (test == null) throw new Exception("Test not found");

            return test;
        }
        public object GetReport(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);

            if (test == null) throw new Exception("Test not found");

            return test.Algorithm.stringReportGenerator.ReportString;
        }

        public byte[] GetPdfReport(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);

            if (test == null) return [];

            var path = Directory.GetCurrentDirectory() + "/reports/" + id.ToString() + ".pdf";
            test.Algorithm.pdfReportGenerator.GenerateReport(path);
            return File.ReadAllBytes(path);
        }

        public byte[] GetCombinedPdfReport(Guid[] ids)
        {
            var name = "combined-" + new Guid().ToString();
            var txt_path = Directory.GetCurrentDirectory() + "/reports/" + name + ".txt";
            var pdf_path = Directory.GetCurrentDirectory() + "/reports/" + name + ".pdf";

            var txt = File.CreateText(txt_path);

            foreach (var id in ids)
            {
                var test = _tests.FirstOrDefault(t => t.Id == id);

                if (test == null) return [];

                txt.WriteLine(test.Algorithm.stringReportGenerator.ReportString);
            }

            txt.Close();
            TxtToPdfReportConverter.GeneratePdfFromTxt(pdf_path, txt_path);
            return File.ReadAllBytes(pdf_path);
        }
        public byte[] GetState(Guid id)
        {
            var test = _tests.FirstOrDefault(t => t.Id == id);

            if (test == null) throw new Exception("Test not found");

            var path = Directory.GetCurrentDirectory() + "/state/" + id.ToString() + ".txt";
            test.Algorithm.writer.SaveToFileStateOfAlgorithm(path);
            return File.ReadAllBytes(path);
        }
        private void RemoveOldTests(object state)
        {
            var now = DateTime.UtcNow;
            _tests.RemoveAll(t => (now - t.CreatedAt).TotalHours >= 24);
        }
    }
}
