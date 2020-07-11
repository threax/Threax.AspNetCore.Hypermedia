using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public static class AssemblyExt
    {
        /// <summary>
        /// Compute the md5 hash of the target assembly. This will look at the assembly's file and compute the hash.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static String ComputeMd5(this Assembly assembly)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(assembly.Location))
                {
                    return Convert.ToBase64String(md5.ComputeHash(stream));
                }
            }
        }
    }
}
