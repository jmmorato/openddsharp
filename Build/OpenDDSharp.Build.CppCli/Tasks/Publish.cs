using Cake.Frosting;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("Publish")]    
    [IsDependentOn(typeof(PublishNuGet))]
    [IsDependentOn(typeof(PublishVsix))]
    public class Publish : FrostingTask
    {
    }
}
