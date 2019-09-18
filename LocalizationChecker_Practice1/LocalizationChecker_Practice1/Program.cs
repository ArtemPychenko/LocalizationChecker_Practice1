using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LocalizationChecker_Practice1
{
    public class Options
    {
        [Option('l', "languages", Separator =';', Required = true, HelpText = "Paths to language files to be processed, separated by semicolon")]
        public IEnumerable<string> LanguageFiles { get; set; }

        [Option('m', "master", Required = true, HelpText = "Path to master file.")]
        public string MasterFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output report file to be generated.")]
        public string OutputFile { get; set; }

        [Option('f', "format", Required = true, HelpText = "Report output format.  Supported formats=Text")]
        public ReportOutputFormat OutputFormat { get; set; }

        [Option('p', "path filter", Required = false, HelpText = "Path filter.")]
        public string PathFilter { get; set; }
    }

    public enum ReportOutputFormat
    {
        Text   
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser(settings =>
            {
                settings.HelpWriter = Console.Error;
                settings.CaseInsensitiveEnumValues = true;
            });

            parser.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    Console.WriteLine($"Languages = {string.Join(",", o.LanguageFiles)}, ");
                    Console.WriteLine($"Master Language = {o.MasterFile}");
                    Console.WriteLine($"Path filter = {o.PathFilter} ");
                    Console.WriteLine($"Output file = {o.OutputFile} ");
                    Console.WriteLine($"Output format = {o.OutputFormat} ");
                    Console.WriteLine($"#################################");

                    if (!System.IO.File.Exists(o.MasterFile))
                    {
                        // Ooops, master file does not exist!
                        Console.Error.WriteLine($"ERROR: Master file '{o.MasterFile}' does not exist");
                        return;
                    }

                    FileReader reader = new FileReader();
                    var masterFile = reader.ReadXMLFile(o.MasterFile);

                    List<File> languageFiles = new List<File>();
                    foreach (var path in o.LanguageFiles)
                    {
                        if (!System.IO.File.Exists(path))
                        {
                            // Ooops, language file does not exist!
                            Console.Error.WriteLine($"ERROR: Langauge file '{path}' does not exist");
                            return;
                        }

                        var file = reader.ReadXMLFile(path);
                        languageFiles.Add(file);
                    }

                    Checker checker = new Checker();
                    var result = checker.Check(masterFile, languageFiles, o.PathFilter);

                    TextReportGenerator reportGenerator = new TextReportGenerator();
                    var report = reportGenerator.Generate(result);

                    using (StreamReader streamReader = new StreamReader(report))
                    {
                        var reportContent = streamReader.ReadToEnd();

                        // 1 To create file stream (write and read)
                        using (FileStream outputFile = new FileStream(o.OutputFile, FileMode.Create, FileAccess.Write))
                        {
                            // 2 To write to file stream 
                            using (StreamWriter writer = new StreamWriter(outputFile))
                            {
                                writer.Write(reportContent);
                            }
                        }
                    }

                    Console.WriteLine("Message End !");
                });
        }
    }
}
