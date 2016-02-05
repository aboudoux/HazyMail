using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HazymailTest
{
    public class TempFile : IDisposable
    {
        public string FilePath { get; private set; }

        public TempFile(string extention)
        {
            FilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Guid.NewGuid() + extention);
        }

        public void Dispose()
        {
            try {
                File.Delete(FilePath);
            }
            catch {
            }
        }
    }
}
