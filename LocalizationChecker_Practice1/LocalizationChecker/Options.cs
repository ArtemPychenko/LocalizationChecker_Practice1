using System.Collections.Generic;
using CommandLine;

namespace LocalizationChecker
{
    public class Options
    {
        [Option('l', "languages", Separator = ';', Required = true, HelpText = "Paths to language files to be processed, separated by semicolon")]
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
}
