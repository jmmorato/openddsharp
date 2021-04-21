using Cake.Common.Tools.MSBuild;
using Cake.Frosting;
using System.IO;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("PackVsix")]    
    public class PackVsix : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            string solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            string projectFolder = Path.Combine(solutionPath, "Sources", "OpenDDSharp.IdlTemplates");
            string projectPath = Path.Combine(projectFolder, "OpenDDSharp.IdlTemplates.csproj");
            context.MSBuild(projectPath, new MSBuildSettings
            {
                Configuration = "Release",
                PlatformTarget = PlatformTarget.MSIL,
                Targets = { "Clean", "Build" },
                MaxCpuCount = 0,
                WorkingDirectory = projectFolder,
            });

            string releaseFolder = Path.Combine(solutionPath, "Release");
            if (!Directory.Exists(releaseFolder))
            {
                Directory.CreateDirectory(releaseFolder);
            }

            foreach(var file in Directory.GetFiles(releaseFolder, "*.vsix"))
            {
                File.Delete(file);
            }

            foreach (var file in Directory.GetFiles(Path.Combine(projectFolder, "bin", "Release"), "*.vsix"))
            {
                FileInfo fi = new FileInfo(file);
                File.Move(file, Path.Combine(releaseFolder, fi.Name));
            }
        }
    }
}
