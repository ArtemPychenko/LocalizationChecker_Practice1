using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationChecker_Practice1
{
    public interface IReportGenerator
    {
        Stream Generate(ParentResult result);
    }
}
