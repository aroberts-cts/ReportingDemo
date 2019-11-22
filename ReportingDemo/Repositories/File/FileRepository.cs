using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ReportingDemo.Repositories.File
{
    public class FileRepository : IFileRepository
    {
        public bool FileExists(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }

        public FileStream GetFileStream(string filePath)
        {
            return new FileStream(filePath, FileMode.Open);
        }
    }
}