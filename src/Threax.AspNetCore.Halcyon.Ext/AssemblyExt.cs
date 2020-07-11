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
    }
}
