using Cake.Frosting;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("Clean")]
    [IsDependentOn(typeof(CleanOpenDDSharpTask))]
    public class CleanTask : FrostingTask
    {
    }
}
