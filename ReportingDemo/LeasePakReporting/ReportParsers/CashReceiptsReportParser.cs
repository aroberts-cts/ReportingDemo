using System;
using System.Collections.Generic;
using System.IO;
using ReportingDemo.LeasePakReporting.Factories;
using ReportingDemo.LeasePakReporting.Reports;

namespace ReportingDemo.LeasePakReporting.ReportParsers
{
    public class CashReceiptsReportParser : AbstractLeasePakReportParser, ILeasePakReportParser
    {
        // The extra space is required for exact matching
        private const string LeftMostColumnName = "LEASE ";

        public CashReceiptsReportParser() : base(Constants.LeasePakReporting.Report.CashReceipts)
        {
        }

        public ILeasePakReport Parse(DateTime reportDate, string portfolio)
        {
            var report = LeasePakReportFactory.Create<CashReceiptsReport>(reportDate, portfolio);

            using (var reader = new StreamReader(report.GetFileStream()))
            {
                // Skip over the header lines that LeasePak stamps on every report to get to report headers
                SkipReportHeaderData(reader);

                // column heading separator of a bunch of "=" characters, spaces where the columns begin and end
                var columnIndicesLine = reader.ReadLine();

                var columnIndices = new List<int>();
                for (var charIndex = 0; charIndex < columnIndicesLine.Length; charIndex++)
                {
                    // Empty spaces where the columns begin and end, but need to guard against multiple whitespaces in a row
                    if (string.IsNullOrWhiteSpace(columnIndicesLine[charIndex].ToString()) && !string.IsNullOrWhiteSpace(columnIndicesLine[charIndex - 1].ToString()))
                    {
                        columnIndices.Add(charIndex);
                    }
                }
                report.SetColumnSplitIndices(columnIndices);

                var dataLines = new List<string>();
                var previousLine = string.Empty;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.TrimStart().StartsWith("NO ITEMS WERE FOUND"))
                    {
                        // There was no data, exit the parsing
                        return null;
                    }

                    // Skip a line with whitespace up to the effective date, not a real line of data
                    if (string.IsNullOrWhiteSpace(line) || string.IsNullOrWhiteSpace(line.Substring(0, columnIndices[2]))) 
                    {
                        if (line.Equals("\f"))
                        {
                            // Hit the end of a page, need to skip headers again
                            SkipReportHeaderData(reader);
                            // Need to skip over the column heading separator since we already have the column indices
                            reader.ReadLine();
                        }
                        continue;
                    }

                    // A summary line at the bottom of each page. Should be skipped.
                    if (line.StartsWith("NO. OF LEASES"))
                    {
                        continue;
                    }

                    // If there is only whitespace up to received date, just need to fill in some data LeasePak leaves out for brevity
                    if (string.IsNullOrWhiteSpace(line.Substring(0, columnIndices[1])))
                    {
                        line = previousLine.Substring(0, columnIndices[1]) + " " + line.TrimStart();
                    }

                    dataLines.Add(line);
                    previousLine = line;
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
                text = reader.ReadLine();

                // Look for the left-most column header, it precedes all of the payments in the report
            } while (!text.TrimStart().StartsWith(GetFirstColumnName()) && !reader.EndOfStream);
        }

        public string GetFirstColumnName()
        {
            return LeftMostColumnName;
        }
    }
}