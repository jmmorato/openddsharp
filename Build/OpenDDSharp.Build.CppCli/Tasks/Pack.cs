using Cake.Frosting;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("Pack")]    
    [IsDependentOn(typeof(PackNuGet))]
    [IsDependentOn(typeof(PackVsix))]
    public class Pack : FrostingTask
    {
    }
}
