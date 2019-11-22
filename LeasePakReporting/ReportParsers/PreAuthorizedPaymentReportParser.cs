using System;
using System.Collections.Generic;
using System.IO;
using ReportingDemo.LeasePakReporting.Factories;
using ReportingDemo.LeasePakReporting.Reports;

namespace ReportingDemo.LeasePakReporting.ReportParsers
{
    public class PreAuthorizedPaymentReportParser : AbstractLeasePakReportParser, ILeasePakReportParser
    {
        private const string LeftMostColumnName = "G/L";

        public PreAuthorizedPaymentReportParser() : base(Constants.LeasePakReporting.Report.PreAuthorizedPayments)
        {
        }

        public ILeasePakReport Parse(DateTime reportDate, string portfolio)
        {
            var report = LeasePakReportFactory.Create<PreAuthorizedPaymentReport>(reportDate, portfolio);

            using (var reader = new StreamReader(report.GetFileStream()))
            {
                // Skip over the header lines that LeasePak stamps on every report to get to report headers
                SkipReportHeaderData(reader);

                // column heading separator of a bunch of "=" characters, spaces where the columns begin and end
                var columnIndicesLine = reader.ReadLine();

                var columnIndices = new List<int>();
                for (var charIndex = 0; charIndex < columnIndicesLine.Length; charIndex++)
                {
                    // Empty spaces where the columns begin and end
                    if (string.IsNullOrWhiteSpace(columnIndicesLine[charIndex].ToString())) 
                    {
                        columnIndices.Add(charIndex);
                    }
                }
                report.SetColumnSplitIndices(columnIndices);

                var dataLines = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("NO ITEMS WERE FOUND"))
                    {
                        // There was no data, exit the parsing
                        return null;
                    }

                    if (line.Equals("\f"))
                    {
                        // Hit the end of a page, need to skip headers again
                        SkipReportHeaderData(reader);
                        // Need to skip over the column heading separator since we already have the column indices
                        reader.ReadLine();
                    }

                    dataLines.Add(line);
                }

                report.AddDataRows(dataLines);
                return report;
            }
        }

        public void SkipReportHeaderData(StreamReader reader)
        {
            string text;
            do
            {
                if (reader.EndOfStream)
                {
                    throw new InvalidOperationException($"End of file reached and the header line was not found. Report not in correct format.");
                }
                text = reader.ReadLine();

                // Look for the left-most column header, it precedes all of the payments in the report
            } while (!text.TrimStart().StartsWith(GetFirstColumnName()));
        }

        public string GetFirstColumnName()
        {
            return LeftMostColumnName;
        }
    }
}