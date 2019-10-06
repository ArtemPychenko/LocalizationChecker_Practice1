using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LocalizationChecker.Filtering;
using LocalizationChecker.Reading;
using LocalizationChecker.Reporting;
using File = LocalizationChecker.Model.File;

namespace LocalizationChecker
{
    public enum ReportOutputFormat
    {
        Text   
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Parser parser = new Parser(settings =>
            {
                settings.HelpWriter = Console.Error;
                settings.CaseInsensitiveEnumValues = true;
            });

            ParseConsoleArguments(parser, args);
        }

        /// <summary>
        /// Read xml files, compare with main file and write results to report.
        /// </summary>
        /// <param name="options">Parsed options from command line.</param>
        private static void ProcessFile(Options options)
        {
            FileReader reader = new FileReader();
            var masterFile = reader.ReadAndFilterXmlFile(options.MasterFile);

            List<File> languageFiles = new List<File>();

            // Foreach loop runs in parallel for all non-master language files as soon as
            // processing order is not important.
            Parallel.ForEach(options.LanguageFiles, (path) =>
            {
                var file = reader.ReadAndFilterXmlFile(path);
                languageFiles.Add(file);
            });

            // Compare all non-master language files with master file.
            Checker checker = new Checker();
            var result = checker.Check(masterFile, languageFiles, options.PathFilter);

            // Generates data for final report.
            TextReportGenerator reportGenerator = new TextReportGenerator();
            var report = reportGenerator.Generate(result);

            using (StreamReader streamReader = new StreamReader(report))
            {
                var reportContent = streamReader.ReadToEnd();

                // 1 To create file stream (write and read)
                using (FileStream outputFile = new FileStream(options.OutputFile, FileMode.Create, FileAccess.Write))
                {
                    // 2 To write to file stream 
                    using (StreamWriter writer = new StreamWriter(outputFile))
                    {
                        writer.Write(reportContent);
                    }
                }
            }

            Console.WriteLine("Message End !");
        }

        /// <summary>
        /// Parse arguments from console using custom parser.
        /// </summary>
        /// <param name="parser">Custom parser to simplify parameters parsing.</param>
        /// <param name="args">Parameters provided by command line.</param>
        private static void ParseConsoleArguments(Parser parser, string[] args)
        {
            parser.ParseArguments<Options>(args).WithParsed(options =>
            {
                Console.WriteLine($"Languages = {string.Join(",", options.LanguageFiles)}, ");
                Console.WriteLine($"Master Language = {options.MasterFile}");
                Console.WriteLine($"Path filter = {options.PathFilter} ");
                Console.WriteLine($"Output file = {options.OutputFile} ");
                Console.WriteLine($"Output format = {options.OutputFormat} ");
                Console.WriteLine("#################################");

                ProcessFile(options);
            });
        }
    }
}
