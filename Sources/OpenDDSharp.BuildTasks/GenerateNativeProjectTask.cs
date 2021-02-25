﻿/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 Jose Morato

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
using System.IO;
using System.Xml;
using System.Globalization;
using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Threading;

namespace OpenDDSharp.BuildTasks
{    
    public class GenerateNativeProjectTask : Task
    {
        #region Fields
        private DTE2 _dte;
        private Solution2 _solution;
        private SolutionBuild2 _build;
        private Project _project;
        private string _solutionName;
        private string _projectName;
        private int _msbuildVersion;
        private readonly Dictionary<int, string> _platformToolsets = new Dictionary<int, string>
        {
            { 15, "v141" },
            { 16, "v142" }
        };
        private string _solutionFullPath;
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

        [Required]
        public string Configuration { get; set; }

        [Required]
        public string Platform { get; set; }

        public bool IsStandard { get; set; }

        public bool IsWrapper { get; set; }

        public bool IsLinux { get; set; }
        #endregion

        #region Methods
        public override bool Execute()
        {
            returnValue = true;

            System.Threading.Thread t = new System.Threading.Thread(ThreadProc);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return returnValue;
        }

        private void ThreadProc()
        {
            OleMessageFilter.Register(Log);

            if (!IsWrapper)
            {
                Log.LogMessage(MessageImportance.High, "Generating native IDL library...");
            }
            else
            {
                Log.LogMessage(MessageImportance.High, "Generating wrapper IDL library...");
            }

            Initialize();
            if (!returnValue)
            {
                return;
            }

            Configuration = Configuration.Replace("Linux", string.Empty);

            GenerateSolutionFile();
            if (!returnValue)
            {
                ShutDown();
                return;
            }

            GenerateProjectFile();
            if (!returnValue)
            {
                ShutDown();
                return;
            }

            CopyIdlFiles();
            if (!returnValue)
            {
                ShutDown();
                return;
            }

            SetSolutionConfiguration();
            if (!returnValue)
            {
                ShutDown();
                return;
            }

            BuildWithMSBuild();
            if (!returnValue)
            {
                ShutDown();
                return;
            }

            ShutDown();

            OleMessageFilter.Revoke();
        }

        private void Initialize()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Initializing DTE...");

                TemplatePath = Path.GetFullPath(TemplatePath);

                if (IsWrapper)
                {
                    _solutionName = OriginalProjectName + "WrapperSolution";
                    _projectName = OriginalProjectName + "Wrapper.vcxproj";
                }
                else
                {
                    _solutionName = OriginalProjectName + "NativeSolution";
                    _projectName = OriginalProjectName + "Native.vcxproj";
                }

                string fullPath = Path.Combine(IntDir, _projectName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                fullPath = Path.Combine(IntDir, _projectName + ".filters");
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                fullPath = Path.Combine(IntDir, _projectName + ".user");
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                // Get the current MSBuild version
                var msbuildProcess = System.Diagnostics.Process.GetCurrentProcess();
                _msbuildVersion = msbuildProcess.MainModule.FileVersionInfo.FileMajorPart;

                // Create the DTE instance
                Type type = Type.GetTypeFromProgID(string.Format(CultureInfo.InvariantCulture, "VisualStudio.DTE.{0}.0", _msbuildVersion));
                object obj = Activator.CreateInstance(type, true);
                _dte = (DTE2)obj;
                _dte.SuppressUI = true;
                _dte.MainWindow.Visible = false;
                _dte.UserControl = false;
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                returnValue = false;
            }
        }

        private void GenerateSolutionFile()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Generating solution file...");

                _solutionFullPath = Path.Combine(IntDir, _solutionName + ".sln");
                if (File.Exists(_solutionFullPath))
                {
                    File.Delete(_solutionFullPath);
                }

                if (!Directory.Exists(IntDir))
                {
                    Directory.CreateDirectory(IntDir);
                }

                _dte.Solution.Create(IntDir, _solutionName);
                _dte.Solution.SaveAs(_solutionFullPath);

                _solution = _dte.Solution as Solution2;
                _build = _solution.SolutionBuild as SolutionBuild2;
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                returnValue = false;
            }
        }

        private void GenerateProjectFile()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Generating project file...");

                AddProjectTemplate();
                GetCreatedProject();

                foreach (ITaskItem s in IdlFiles)
                {
                    string filename = s.GetMetadata("Filename");

                    if (!IsWrapper)
                    {
                        _project.ProjectItems.AddFromFile(filename + "C.h");
                        _project.ProjectItems.AddFromFile(filename + "IDL_Export.h");
                        _project.ProjectItems.AddFromFile(filename + "S.h");
                        _project.ProjectItems.AddFromFile(filename + "TypeSupportC.h");
                        _project.ProjectItems.AddFromFile(filename + "TypeSupportImpl.h");
                        _project.ProjectItems.AddFromFile(filename + "TypeSupportS.h");

                        _project.ProjectItems.AddFromFile(filename + "C.cpp");
                        _project.ProjectItems.AddFromFile(filename + "S.cpp");
                        _project.ProjectItems.AddFromFile(filename + "TypeSupportC.cpp");
                        _project.ProjectItems.AddFromFile(filename + "TypeSupportImpl.cpp");
                        _project.ProjectItems.AddFromFile(filename + "TypeSupportS.cpp");

                        _project.ProjectItems.AddFromFile(filename + "C.inl");
                        _project.ProjectItems.AddFromFile(filename + "TypeSupportC.inl");
                    }
                    else
                    {
                        _project.ProjectItems.AddFromFile(filename + "TypeSupport.h");
                        _project.ProjectItems.AddFromFile(filename + "TypeSupport.cpp");
                    }
                }

                _project.Save();

                if (!IsLinux)
                {
                    // No really elegant but I couldn't cast the project to VCProject because the "Interface not registered" and M$ says that it is not a bug :S
                    // https://developercommunity.visualstudio.com/content/problem/568/systeminvalidcastexception-unable-to-cast-com-obje.html
                    XmlDocument doc = new XmlDocument();
                    doc.Load(_project.FullName);
                    XmlNode root = doc.DocumentElement;
                    XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                    ns.AddNamespace("msbld", "http://schemas.microsoft.com/developer/msbuild/2003");

                    XmlNodeList nodes = root.SelectNodes("//msbld:PreprocessorDefinitions", ns);
                    foreach (XmlNode node in nodes)
                    {
                        foreach (ITaskItem s in IdlFiles)
                        {
                            string fileName = s.GetMetadata("Filename");
                            node.InnerXml = string.Format("{0}IDL_BUILD_DLL;{1}", fileName.ToUpper(), node.InnerXml);
                        }
                    }

                    if (IsWrapper)
                    {
                        nodes = root.SelectNodes("//msbld:AdditionalDependencies", ns);
                        foreach (XmlNode node in nodes)
                        {
                            node.InnerXml = string.Format("{0}Native.lib;{1}", OriginalProjectName, node.InnerXml);
                        }
                    }

                    if (_platformToolsets.ContainsKey(_msbuildVersion))
                    {
                        nodes = root.SelectNodes("//msbld:PlatformToolset", ns);
                        foreach (XmlNode node in nodes)
                        {
                            node.InnerXml = _platformToolsets[_msbuildVersion];
                        }
                    }

                    doc.Save(_project.FullName);
                    _project.Save();
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                returnValue = false;
            }
        }

        private void SetSolutionConfiguration()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Setting solution configuration...");

                string platform = "x64";
                if (Platform == "Win32" || Platform == "x86" || Platform == "AnyCPU")
                {
                    platform = "x86";
                }

                bool found = false;
                foreach (SolutionConfiguration2 sc in _build.SolutionConfigurations)
                {
                    if (sc.Name == Configuration && sc.PlatformName == platform)
                    {
                        sc.Activate();
                        found = true;
                    }
                }

                if (!found)
                {
                    Log.LogError("Solution configuration not found: {0}|{1}", Configuration, platform);
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                returnValue = false;
            }
        }

        private void AddProjectTemplate()
        {
            _solution.AddFromTemplate(TemplatePath, IntDir, OriginalProjectName, false);
        }

        private void GetCreatedProject()
        {
            _project = _solution.Projects.Item(1);

            if (_project == null)
            {
                throw new ApplicationException("The project couldn't be created.");
            }
        }

        private void CopyIdlFiles()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Copying IDL files...");

                foreach (ITaskItem s in IdlFiles)
                {
                    string identity = IsStandard ? s.GetMetadata("Filename") + s.GetMetadata("Extension") : s.GetMetadata("Identity");
                    string inputPath = s.GetMetadata("FullPath");
                    string outputPath = Path.Combine(IntDir, identity);

                    using (StreamReader reader = new StreamReader(inputPath))
                    using (StreamWriter writer = new StreamWriter(outputPath))
                    {
                        writer.WriteLine("#include <tao/orb.idl> // Workaround for the error C2961: inconsistent explicit instantiations, a previous explicit instantiation did not specify '__declspec(dllimport)'");
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

        private void BuildWithMSBuild()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Building solution...");

                string platform = "x64";
                if (Platform == "Win32" || Platform == "x86" || Platform == "AnyCPU")
                {
                    platform = "x86";
                }

                _build.Build(true);

                CheckBuildInfo(platform);
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                returnValue = false;
            }
        }

        private void CheckBuildInfo(string platform)
        {
            int result = int.MaxValue;
            if (_build.BuildState == vsBuildState.vsBuildStateDone)
            {
                result = _build.LastBuildInfo;
                Log.LogMessage(MessageImportance.High, "Build result: {0}", result);
            }
            else
            {
                Log.LogMessage(MessageImportance.High, "Unexpected build state: {0}", _build.BuildState);                
            }               

            if (result > 0 && result < int.MaxValue)
            {
                Log.LogMessage(MessageImportance.High, "Build result: {0}", result);

                string projectName = Path.GetFileNameWithoutExtension(_project.FullName);
                string cppPlatform = platform;
                if (platform == "x86")
                {
                    cppPlatform = "Win32";
                }
                string logFile = Path.Combine(IntDir, "obj", cppPlatform, Configuration, projectName + ".log");
                if (File.Exists(logFile))
                {
                    var logText = File.ReadAllText(logFile);
                    Log.LogError("The project {0} failed to build. Last build log: ", _project.FullName, logText);
                }
                else
                {
                    Log.LogError("The project {0} failed to build. No log file found in: ", _project.FullName, logFile);
                }

                returnValue = false;
            }
        }

        private void ShutDown()
        {
            try
            {
                _dte.Solution.Close(true);
                _dte.Quit();
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }
        }
        #endregion
    }
}
