using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingDemo.Repositories.File
{
    public interface IFileRepository
    {
        bool FileExists(string filePath);
        FileStream GetFileStream(string filePath);
    }
}
