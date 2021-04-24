using Cake.Common.Tools.NuGet;
using Cake.Frosting;
using Cake.Common.Tools.NuGet.Pack;
using System.IO;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("PackNuGet")]    
    public class PackNuGet : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            string solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            string path = Path.Combine(solutionPath, "Sources", "OpenDDSharp", "OpenDDSharp.nuspec");
            Cake.Core.IO.FilePath filePath = new Cake.Core.IO.FilePath(path);

            context.NuGetPack(filePath, new NuGetPackSettings());

            path = Path.Combine(solutionPath, "Sources", "OpenDDSharp.IdlGenerator", "OpenDDSharp.IdlGenerator.nuspec");
            filePath = new Cake.Core.IO.FilePath(path);

            context.NuGetPack(filePath, new NuGetPackSettings());

            string releaseFolder = Path.Combine(solutionPath, "Release");
            if (!Directory.Exists(releaseFolder))
            {
                Directory.CreateDirectory(releaseFolder);
            }

            foreach (var file in Directory.GetFiles(releaseFolder, "*.nupkg"))
            {
                File.Delete(file);
            }

            foreach (var file in Directory.GetFiles(".", "*.nupkg"))
            {
                FileInfo fi = new FileInfo(file);
                File.Move(file, Path.Combine(releaseFolder, fi.Name));
            }
        }
    }
}
