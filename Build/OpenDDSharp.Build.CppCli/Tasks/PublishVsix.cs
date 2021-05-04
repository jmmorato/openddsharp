/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using Cake.Common;
using Cake.Common.Tools.VSWhere;
using Cake.Frosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("PublishVsix")]    
    public class PublishVsix : FrostingTask<BuildContext>
    {
        public override bool ShouldRun(BuildContext context)
        {
            return !context.IsDevelop;
        }

        public override void Run(BuildContext context)
        {
            var vsPath = context.VSWhereLatest(new Cake.Common.Tools.VSWhere.Latest.VSWhereLatestSettings
            {
                Version = "[\"15.0\", \"17.0\"]"
            });
            var vsixPublisherPath = Path.Combine(vsPath.FullPath, "VSSDK", "VisualStudioIntegration", "Tools", "Bin", "VsixPublisher.exe");

            var solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            var releasePath = Path.Combine(solutionPath, "Release");
            var vsixPath = Path.Combine(releasePath, "OpenDDSharp.IdlTemplates.vsix");
            var publishParameters = $"publish -payload \"{vsixPath}\" -publishManifest \"vs-publish.json\" -ignoreWarnings \"VSIXValidatorWarning01,VSIXValidatorWarning02\" -personalAccessToken \"{context.VsMarketplaceToken}\"";

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("    \"$schema\": \"http://json.schemastore.org/vsix-publish\",");
            builder.AppendLine("    \"categories\": [ \"Visual Studio Extensions\" ],");
            builder.AppendLine("    \"identity\": {");
            builder.AppendLine("        \"internalName\": \"openddsharptemplates\"");
            builder.AppendLine("    },");
            builder.AppendLine("    \"overview\": \"../README.md\",");
            builder.AppendLine("    \"priceCategory\": \"free\",");
            builder.AppendLine("    \"publisher\": \"jmmorato\",");
            builder.AppendLine("    \"private\": false,");
            builder.AppendLine("    \"qna\": true,");
            builder.AppendLine("    \"repo\": \"https://github.com/jmmorato/openddsharp\"");
            builder.AppendLine("}");

            File.WriteAllText(Path.Combine(releasePath, "vs-publish.json"), builder.ToString());

            int exitCode = context.StartProcess(vsixPublisherPath, new Cake.Core.IO.ProcessSettings
            {
                Arguments = publishParameters,
                WorkingDirectory = releasePath,
            });
            if (exitCode != 0)
            {
                throw new Exception($"Program {vsixPublisherPath} couldn't be executed. Exit code: {exitCode}");
            }
        }
    }
}
