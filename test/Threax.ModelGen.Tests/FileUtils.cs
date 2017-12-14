using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Threax.ModelGen.Tests
{
    static class FileUtils
    {
        private static String TestFileDirectory => Path.Combine(Directory.GetCurrentDirectory(), "../../../TestFiles");

        public static void WriteTestFile(Type testType, String filename, String contents)
        {
            filename = Path.Combine(TestFileDirectory, testType.Name, filename);

            var folder = Path.GetDirectoryName(filename);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (var writer = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None)))
            {
                writer.Write(contents);
            }
        }

        public static String ReadTestFile(Type testType, String filename)
        {
            filename = Path.Combine(TestFileDirectory, testType.Name, filename);

            using (var reader = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.None)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
