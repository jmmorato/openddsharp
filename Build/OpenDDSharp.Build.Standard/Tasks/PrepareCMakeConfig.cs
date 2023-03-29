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
using System.IO;
using System.Linq;
using System.Text;
using Cake.Frosting;

namespace OpenDDSharp.Build.Standard.Tasks
{
    /// <summary>
    /// Prepare cmake config files taks.
    /// </summary>
    [TaskName("PrepareCMakeConfig")]
    public class PrepareCMakeConfig : FrostingTask<BuildContext>
    {
        private readonly string[] _variablesToRemove = new string[]
        {
            "OPENDDS_MPC",
            "OPENDDS_ACE",
            "OPENDDS_TAO",
        };

        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            var solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            var path = Path.Combine(solutionPath, "ext");

            foreach (var file in Directory.GetFiles(path, "config.cmake", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(file);
                var stringBuilder = new StringBuilder();
                foreach (var line in lines)
                {
                    if (line.StartsWith("#") || _variablesToRemove.Any(line.Contains))
                    {
                        continue;
                    }
                    stringBuilder.AppendLine(line);
                }
                File.WriteAllText(file, stringBuilder.ToString());
            }
        }
    }
}
