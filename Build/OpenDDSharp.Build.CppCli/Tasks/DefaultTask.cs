using Cake.Frosting;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("Default")]
    [IsDependentOn(typeof(CleanTask))]
    [IsDependentOn(typeof(BuildTask))]
    [IsDependentOn(typeof(TestTask))]
    public class DefaultTask : FrostingTask
    {
    }
}
