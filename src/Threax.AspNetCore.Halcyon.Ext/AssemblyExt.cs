using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public static class AssemblyExt
    {
        /// <summary>
        /// Compute the md5 hash of the target assembly. This will look at the assembly's file and compute the hash.
        /// It will be returned as a string of hex characters.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static String ComputeMd5(this Assembly assembly)
        {
            byte[] hashBytes;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(assembly.Location))
                {
                    hashBytes = md5.ComputeHash(stream);
                }
            }

            var sb = new StringBuilder(hashBytes.Length);
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Compute the md5 for the given assembly and any .dll files found. You can modify 
        /// this search pattern and how the directory is searched. Including more files will 
        /// take longer.
        /// </summary>
        /// <param name="assembly">The assembly to search from.</param>
        /// <param name="searchPattern">The search pattern, by default looks for dlls, (*.dll)</param>
        /// <param name="searchOption">The directories to check, by default this is TopDirectoryOnly to only look for dlls on the same level as the target assembly.</param>
        /// <param name="take">The maximum number of files to hash. Default: 1000.</param>
        /// <returns></returns>
        public static String ComputeMd5ForAllNearby(this Assembly assembly, String searchPattern = "*.dll", SearchOption searchOption = SearchOption.TopDirectoryOnly, int take = 1000)
        {
            using (var md5 = MD5.Create())
            {
                byte[] finalBytes;
                var searchPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (var stream = new MemoryStream())
                {
                    foreach (var file in Directory.GetFiles(searchPath, searchPattern, searchOption).Take(take))
                    {
                        using (var assemblyStream = File.OpenRead(file))
                        {
                            var hashBytes = md5.ComputeHash(assemblyStream);
                            stream.Write(hashBytes, 0, hashBytes.Length);
                        }
                    }
                    stream.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    finalBytes = md5.ComputeHash(stream);
                }

                var sb = new StringBuilder(finalBytes.Length);
                foreach (var b in finalBytes)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
