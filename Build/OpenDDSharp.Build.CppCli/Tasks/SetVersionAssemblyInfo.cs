using Cake.Frosting;
using Cake.FileHelpers;
using Cake.Common.IO;
using Cake.Core.IO;
using System;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("SetVersionAssemblyInfo")]    
    public class SetVersionAssemblyInfo : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            
            string version = $"{context.MajorVersion}.{context.MinorVersion}.{context.GetBuildRevisionVersion()}";
            DirectoryPath path = context.MakeAbsolute(context.Directory(BuildContext.OPENDDSHARP_SOLUTION_FOLDER));

            context.ReplaceRegexInFiles($"{path}/**/AssemblyInfo.cs", "(?<=AssemblyVersion\\(\")(.+?)(?=\"\\))", version);
            context.ReplaceRegexInFiles($"{path}/**/AssemblyInfo.cs", "(?<=AssemblyFileVersion\\(\")(.+?)(?=\"\\))", version);
            
            context.ReplaceRegexInFiles($"{path}/**/AssemblyInfo.h", "(?<=File_Version\\s)(.+?)(?=\\s)", version.Replace('.', ','));
            context.ReplaceRegexInFiles($"{path}/**/AssemblyInfo.h", "(?<=File_Version_Str\\s\")(.+?)(?=\")", version);
            context.ReplaceRegexInFiles($"{path}/**/AssemblyInfo.h", "(?<=Assembly_Version\\sL\")(.+?)(?=\")", version);
        }
    }
}
