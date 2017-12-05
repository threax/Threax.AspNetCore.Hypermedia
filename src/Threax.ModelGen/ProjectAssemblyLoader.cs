using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Threax.ModelGen
{
    /// <summary>
    /// This class will scan for and load the assembly for the project we are trying to generate code for.
    /// </summary>
    /// <remarks>
    /// Based on http://www.natemcmaster.com/blog/2016/12/26/project-evalutation-cli-tool/
    /// </remarks>
    class ProjectAssemblyLoader
    {
        public static Assembly LoadProjectAssembly(String appDir)
        {
            //Find the target project
            var projectFile = Directory.EnumerateFiles(
                appDir,
                "*.*proj")
                .Where(f => !f.EndsWith(".xproj")) // filter xproj files, which are MSBuild meta-projects
                .FirstOrDefault();

            var targetFileName = Path.GetFileName(projectFile) + ".dotnet-threax-modelgen.targets";
            var projectExtPath = Path.Combine(Path.GetDirectoryName(projectFile), "obj");
            var targetFile = Path.Combine(projectExtPath, targetFileName);

            //Write a target file that will gather up the desired values
            File.WriteAllText(targetFile,
@"<Project>
      <Target Name=""_GetThreaxModelgenInfo"">
         <ItemGroup>
            <_DotNetNamesOutput Include=""OutputPath: $(OutputPath)"" />
            <_DotNetNamesOutput Include=""TargetFileName: $(TargetFileName)"" />
         </ItemGroup>
         <WriteLinesToFile File=""$(_ThreaxModelgenNamesFile)"" Lines=""@(_DotNetNamesOutput)"" Overwrite=""true"" />
      </Target>
  </Project>");

            //Compile the project
            var tmpFile = Path.GetTempFileName();
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"msbuild \"{projectFile}\" /t:_GetThreaxModelgenInfo /nologo \"/p:_ThreaxModelgenNamesFile={tmpFile}\""
            };
            var process = Process.Start(psi);
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException("Invoking MSBuild target failed");
            }

            //Read the results from the temp file
            var lines = File.ReadAllLines(tmpFile);
            File.Delete(tmpFile);

            var properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in lines)
            {
                var idx = line.IndexOf(':');
                if (idx <= 0) continue;
                var name = line.Substring(0, idx)?.Trim();
                var value = line.Substring(idx + 1)?.Trim();
                properties.Add(name, value);
            }

            var targetAssembly = Path.GetFullPath(Path.Combine(appDir, properties["OutputPath"], properties["TargetFileName"]));
            var additionalSearchPath = Path.GetDirectoryName(targetAssembly);

            if (!File.Exists(targetAssembly))
            {
                throw new FileNotFoundException($"Cannot find project assembly {targetAssembly}.");
            }

            //Try to resolve any other assemblies from the loaded path, this keeps the editor working when it uses something from another dll.
            AppDomain.CurrentDomain.AssemblyResolve += (s, a) =>
            {
                try
                {
                    var assemblyVersion = "";
                    var assemblyName = a.Name;
                    var commaIndex = assemblyName.IndexOf(',');
                    if (commaIndex != -1)
                    {
                        assemblyName = assemblyName.Substring(0, commaIndex);
                        var endVersionIndex = a.Name.IndexOf(',', ++commaIndex);
                        if(endVersionIndex != -1)
                        {
                            assemblyVersion = a.Name.Substring(commaIndex, endVersionIndex - commaIndex).Replace("Version=", "").Trim();
                        }
                    }
                    assemblyName += ".dll";

                    if (a.RequestingAssembly != null)
                    {
                        var sameDirSearch = Path.Combine(Path.GetDirectoryName(a.RequestingAssembly.Location), assemblyName);
                        if (File.Exists(sameDirSearch))
                        {
                            return Assembly.LoadFile(sameDirSearch);
                        }
                    }

                    var additionalSearch = Path.Combine(additionalSearchPath, assemblyName);
                    if (File.Exists(additionalSearch))
                    {
                        return Assembly.LoadFile(additionalSearch);
                    }

                    //Try to search nuget files, this is really hairy and likely to break in a strong wind
                    var assemblyFolder = Path.GetFileNameWithoutExtension(assemblyName);
                    var nugetPath = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget/packages", assemblyFolder));
                    var versionedPath = Path.Combine(nugetPath, assemblyVersion);
                    if (!Directory.Exists(versionedPath))
                    {
                        //Try again without last .0
                        if (versionedPath.EndsWith(".0"))
                        {
                            versionedPath = versionedPath.Substring(0, versionedPath.Length - 2);
                        }
                    }

                    if (Directory.Exists(versionedPath))
                    {
                        //Find netstandard version, hardcoded to netstandard2.0 for now
                        nugetPath = versionedPath;
                        var netStandardPath = Path.Combine(nugetPath, "lib/netstandard2.0", assemblyName);
                        if (File.Exists(netStandardPath))
                        {
                            return Assembly.LoadFile(netStandardPath);
                        }

                        //Can't find that version, see if there is only 1 dll in this folder
                        var dllFiles = Directory.GetFiles(nugetPath, assemblyName, SearchOption.AllDirectories);
                        if(dllFiles.Length == 1)
                        {
                            return Assembly.LoadFile(dllFiles[0]);
                        }
                    }
                }
                catch (Exception) { }

                return null;
            };

            return Assembly.LoadFile(targetAssembly);
        }
    }
}
