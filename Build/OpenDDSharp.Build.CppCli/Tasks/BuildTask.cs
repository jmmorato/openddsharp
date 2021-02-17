using Cake.Frosting;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("Build")]    
    [IsDependentOn(typeof(BuildOpenDDSharpTask))]
    public class BuildTask : FrostingTask
    {
       
    }
}
