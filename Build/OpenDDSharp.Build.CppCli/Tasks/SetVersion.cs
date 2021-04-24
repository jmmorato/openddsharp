using Cake.Frosting;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("SetVersion")]
    [IsDependentOn(typeof(SetVersionAssemblyInfo))]
    [IsDependentOn(typeof(SetVersionNuspec))]
    [IsDependentOn(typeof(SetVersionVsix))]
    [IsDependentOn(typeof(SetVersionProjectTemplate))]
    public class SetVersion : FrostingTask
    {
    }
}
