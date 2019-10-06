using System.IO;
using LocalizationChecker.Model;

namespace LocalizationChecker.Reporting
{
    /// <summary>
    /// Generates the data for final report.
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Prepares the data for final report.
        /// </summary>
        /// <param name="result">Result of comparing language files with master file.</param>
        /// <returns>Memory stream to write to the report file.</returns>
        Stream Generate(ParentResult result);
    }
}
