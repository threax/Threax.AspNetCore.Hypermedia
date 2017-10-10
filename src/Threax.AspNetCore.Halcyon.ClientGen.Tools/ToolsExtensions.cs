using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Threax.AspNetCore.BuiltInTools;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public static class ToolsExtensions
    {
        public static IToolRunner UseClientGenTools(this IToolRunner toolRunner)
        {
            return toolRunner.AddTool("clientgen", new ToolCommand("Generate a client library file to the specified destination.", a =>
            {
                if (a.Args.Count < 1)
                {
                    throw new NotSupportedException("You must provide a file name to use the clientgen tool.");
                }
                var file = a.Args[0];
                var extension = Path.GetExtension(file);
                switch (extension)
                {
                    case ".ts":
                        var typescriptWriter = a.Scope.ServiceProvider.GetRequiredService<TypescriptClientWriter>();
                        using (var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None)))
                        {
                            typescriptWriter.CreateClient(writer);
                        }
                        break;
                    case ".cs":
                        var csharpWriter = a.Scope.ServiceProvider.GetRequiredService<CSharpClientWriter>();
                        using (var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None)))
                        {
                            csharpWriter.CreateClient(writer);
                        }
                        break;
                    default:
                        throw new NotSupportedException($"{file} is not supported. Can only generate typescript (.ts) or c# (.cs) clients.");
                }

                return Task.FromResult(0);
            }));
        }
    }
}
