/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using System.IO;
using System.Linq;
using System.Text;
using Cake.Frosting;

namespace OpenDDSharp.Build.Tasks
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
            "OPENDDS_RAPIDJSON",
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
                    if (line.StartsWith('#') || _variablesToRemove.Any(line.Contains))
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
