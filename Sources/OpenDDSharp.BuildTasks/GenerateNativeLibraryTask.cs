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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace OpenDDSharp.BuildTasks
{
    /// <summary>
    /// Build task to generate the native IDL library.
    /// </summary>
    public class GenerateNativeLibraryTask : Task
    {
        #region Fields
        private string _projectName;
        private bool returnValue;
        #endregion

        #region Properties
        [Required]
        public string OriginalProjectName { get; set; }

        [Required]
        public string IntDir { get; set; }

        [Required]
        public string TemplatePath { get; set; }

        [Required]
        public ITaskItem[] IdlFiles { get; set; }

        public bool IsLinux { get; set; }

        public bool IsOSX { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>Returns <c>true</c> if success, otherwise <c>false</c>.</returns>
        public override bool Execute()
        {
            returnValue = DoExecute();


            return returnValue;
        }

        private bool DoExecute()
        {
            Log.LogMessage(MessageImportance.High, "Generating native IDL library...");

            Initialize();

            return true;
        }

        private void Initialize()
        {
            TemplatePath = Path.GetFullPath(TemplatePath);

            _projectName = OriginalProjectName + "Wrapper";

            string fullPath = Path.Combine(IntDir, _projectName);
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath);
            }

            Directory.CreateDirectory(fullPath);

            CopyIdlFiles();

            List<string> idlFiles = new ();
            List<string> srcFiles = new ();
            foreach (ITaskItem s in IdlFiles)
            {
                string filename = s.GetMetadata("Filename");
                idlFiles.Add(filename + ".idl");
                srcFiles.Add(filename + "TypeSupport.h");
                srcFiles.Add(filename + "TypeSupport.cpp");
            }

            var cmakeInput = Path.Combine(TemplatePath, "CMakeListsTemplate.txt");
            var cmakeOutput = Path.Combine(IntDir, "CMakeLists.txt");
            var marshalInput = Path.Combine(TemplatePath, "marshal.h");
            var marshalOutput = Path.Combine(IntDir, "marshal.h");

            File.Copy(marshalInput, marshalOutput, true);

            using StreamReader reader = new (cmakeInput);
            using StreamWriter writer = new (cmakeOutput);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    line = string.Empty;
                }

                line = line.Replace("${PROJECT_NAME}", _projectName);
                line = line.Replace("${WRAPPER_FILES}", string.Join(" ", srcFiles));
                line = line.Replace("${IDL_FILES}", string.Join(" ", idlFiles));
                writer.WriteLine(line);
            }

            writer.Flush();

            writer.Close();
            reader.Close();
        }

        private void CopyIdlFiles()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Copying IDL files...");

                foreach (ITaskItem s in IdlFiles)
                {
                    string identity = s.GetMetadata("Filename") + s.GetMetadata("Extension");
                    string inputPath = s.GetMetadata("FullPath");
                    string outputPath = Path.Combine(IntDir, identity);

                    using (StreamReader reader = new (inputPath))
                    using (StreamWriter writer = new (outputPath))
                    {
                        writer.WriteLine("#include <tao/orb.idl>");

                        while (!reader.EndOfStream)
                        {
                            writer.WriteLine(reader.ReadLine());
                        }

                        writer.Flush();

                        writer.Close();
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                returnValue = false;
            }
        }
        #endregion
    }
}
