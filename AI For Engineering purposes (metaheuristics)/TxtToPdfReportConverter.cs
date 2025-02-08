using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace AI_For_Engineering_purposes__metaheuristics_
{
    public class TxtToPdfReportConverter
    {
        public static void GeneratePdfFromTxt(string pdfFilePath, string txtFilePath)
        {
            try
            {
                if (!File.Exists(txtFilePath))
                {
                    Console.WriteLine("Error: The specified TXT file does not exist.");
                    return;
                }

                string[] lines = File.ReadAllLines(txtFilePath);

                using (PdfWriter writer = new PdfWriter(pdfFilePath))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);

                        int index = 1;
                        bool newReport = true;

                        foreach (var line in lines)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                newReport = true;
                                document.Add(new Paragraph("\n"));
                            }
                            else
                            {
                                if (newReport)
                                {
                                    document.Add(new Paragraph($"Raport nr. {index++}")
                                        .SetFontSize(14));
                                    newReport = false;
                                }

                                document.Add(new Paragraph(line));
                            }
                        }

                        document.Close();
                    }
                }

                Console.WriteLine("PDF report generated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF: {ex.Message}");
            }
        }
    }
}
