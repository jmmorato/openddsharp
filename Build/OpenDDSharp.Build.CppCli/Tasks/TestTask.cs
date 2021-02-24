using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.VSTest;
using Cake.Frosting;
using System.IO;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("Test")]    
    public class TestTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var path = $"../../Tests/OpenDDSharp.UnitTest/bin/{context.BuildPlatform}/{context.BuildConfiguration}/";
            var file = "OpenDDSharp.UnitTest.dll";
            var testAdapterPath = Path.Combine(BuildContext.OPENDDSHARP_SOLUTION_FOLDER, "packages/MSTest.TestAdapter.2.1.2/build/_common");
            var settingsFile = Path.Combine(Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER), "CodeCoverage.runsettings");
            var platform = context.BuildPlatform == PlatformTarget.x64 ? VSTestPlatform.x64 : VSTestPlatform.x86;

            context.VSTest(path + file, new VSTestSettings
            {
                ToolPath = context.Tools.Resolve("vstest.console.exe"),
                TestAdapterPath = Path.GetFullPath(testAdapterPath),
                FrameworkVersion = VSTestFrameworkVersion.NET45,
                EnableCodeCoverage = true,
                PlatformArchitecture = platform,
                WorkingDirectory = Path.GetFullPath(path),
                EnvironmentVariables =
                {
                    { nameof(BuildContext.DDS_ROOT), Path.GetFullPath(BuildContext.DDS_ROOT).TrimEnd('\\') },
                    { nameof(BuildContext.ACE_ROOT), Path.GetFullPath(BuildContext.ACE_ROOT).TrimEnd('\\') },
                    { nameof(BuildContext.TAO_ROOT), Path.GetFullPath(BuildContext.TAO_ROOT).TrimEnd('\\') },
                    { nameof(BuildContext.MPC_ROOT), Path.GetFullPath(BuildContext.MPC_ROOT).TrimEnd('\\') },
                },
                SettingsFile = settingsFile,
            });
        }
    }
}
