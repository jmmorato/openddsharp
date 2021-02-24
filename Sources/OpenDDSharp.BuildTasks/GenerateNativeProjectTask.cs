/*********************************************************************
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
        string _solutionFullPath;
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
            System.Threading.Thread t = new System.Threading.Thread(ThreadProc);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return true;
        }

        private void ThreadProc()
        {
            OleMessageFilter.Register();

            if (!IsWrapper)
            {
                Log.LogMessage(MessageImportance.High, "Generating native IDL library...");
            }
            else
            {
                Log.LogMessage(MessageImportance.High, "Generating wrapper IDL library...");
            }

            Initialize();

#if DEBUG
            Log.LogMessage(MessageImportance.High, "TemplatePath: " + TemplatePath);
            Log.LogMessage(MessageImportance.High, "IntDir: " + IntDir);
            Log.LogMessage(MessageImportance.High, "_projectName: " + _projectName);
#endif
            Configuration = Configuration.Replace("Linux", string.Empty);
            GenerateSolutionFile();
            GenerateProjectFile();
            SetSolutionConfiguration();
            CopyIdlFiles();
            BuildWithMSBuild();
            ShutDown();

            OleMessageFilter.Revoke();
        }

        private void Initialize()
        {
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

            InitializeDTE();
        }

        private void InitializeDTE()
        {
            // Get the current MSBuild version
            var msbuildProcess = System.Diagnostics.Process.GetCurrentProcess();
            _msbuildVersion = msbuildProcess.MainModule.FileVersionInfo.FileMajorPart;

            int retry = 100;
            bool success = false;
            while (!success && retry > 0)
            {
                try
                {
                    // Create the DTE instance
                    Type type = Type.GetTypeFromProgID(string.Format(CultureInfo.InvariantCulture, "VisualStudio.DTE.{0}.0", _msbuildVersion));
                    object obj = Activator.CreateInstance(type, true);
                    _dte = (DTE2)obj;
                    _dte.SuppressUI = true;
                    _dte.MainWindow.Visible = false;
                    _dte.UserControl = false;

                    success = true;
                }
#if DEBUG
                catch (Exception ex)
#else
                catch
#endif
                {
                    success = false;
                    retry--;

                    if (retry > 0)
                    {
                        System.Threading.Thread.Sleep(1000);

#if DEBUG
                        Log.LogMessage(MessageImportance.High, "Exception {0}: {1}", msbuildProcess.MainModule.FileVersionInfo.FileMajorPart, ex.ToString());
#endif
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void GenerateSolutionFile()
        {
            _solutionFullPath = Path.Combine(IntDir, _solutionName + ".sln");
            if (File.Exists(_solutionFullPath))
            {
                File.Delete(_solutionFullPath);
            }

            if (!Directory.Exists(IntDir))
            {
                Directory.CreateDirectory(IntDir);
            }

            int retry = 100;
            bool success = false;
            while (!success && retry > 0)
            {
                try
                {
                    _dte.Solution.Create(IntDir, _solutionName);
                    _dte.Solution.SaveAs(_solutionFullPath);

                    success = true;
                }
#if DEBUG
                catch (Exception ex)
#else
                catch
#endif
                {
                    success = false;
                    retry--;

                    if (retry > 0)
                    {
                        System.Threading.Thread.Sleep(1000);

#if DEBUG
                        Log.LogMessage(MessageImportance.High, "Exception: " + ex.ToString());
#endif
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            retry = 100;
            success = false;
            while (!success && retry > 0)
            {
                try
                {
                    _solution = _dte.Solution as Solution2;
                    _build = _solution.SolutionBuild as SolutionBuild2;
                    
                    success = true;
                }
#if DEBUG
                catch (Exception ex)
#else
                catch
#endif
                {
                    success = false;
                    retry--;

                    if (retry > 0)
                    {
                        System.Threading.Thread.Sleep(1000);

#if DEBUG
                        Log.LogMessage(MessageImportance.High, "Exception: " + ex.ToString());
#endif
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void GenerateProjectFile()
        {
            AddProjectTemplate();
            GetCreatedProject();

            int retry = 100;
            bool success = false;

            while (!success && retry > 0)
            {
                try
                {
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
                    
                    success = _project.Saved;

                }
#if DEBUG
                catch (Exception ex)
#else
                catch
#endif
                {
                    success = false;
                    retry--;

                    if (retry > 0)
                    {
                        System.Threading.Thread.Sleep(1000);

#if DEBUG
                        Log.LogMessage(MessageImportance.High, "Exception: " + ex.ToString());
#endif
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void SetSolutionConfiguration()
        {
            string platform = "x64";
            if (Platform == "Win32" || Platform == "x86" || Platform == "AnyCPU")
            {
                platform = "x86";
            }

            int retry = 100;
            bool success = false;
            while (!success && retry > 0)
            {
                try
                {
                    foreach (SolutionConfiguration2 sc in _build.SolutionConfigurations)
                    {
                        if (sc.Name == Configuration && sc.PlatformName == platform)
                        {
                            sc.Activate();
                            success = true;
                        }
                    }
                }
#if DEBUG
                catch (Exception ex)
#else
                catch
#endif
                {
                    success = false;
                    retry--;

                    if (retry > 0)
                    {
                        System.Threading.Thread.Sleep(1000);

#if DEBUG
                        Log.LogMessage(MessageImportance.High, "Exception: " + ex.ToString());
#endif
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void AddProjectTemplate()
        {
            int retry = 100;
            bool success = false;

            while (!success && retry > 0)
            {
                try
                {
                    _solution.AddFromTemplate(TemplatePath, IntDir, OriginalProjectName, false);

                    success = true;
                }
#if DEBUG
                catch (Exception ex)
#else
                catch
#endif
                {
                    success = false;
                    retry--;

                    if (retry > 0)
                    {
                        System.Threading.Thread.Sleep(1000);

#if DEBUG
                        Log.LogMessage(MessageImportance.High, "Exception: " + ex.ToString());
#endif
                    }
                    else
                    {
                        throw;
                    }
                }
            }            
        }

        private void GetCreatedProject()
        {
            int retry = 100;
            bool success = false;
            while (!success && retry > 0)
            {
                try
                {
                    _project = _solution.Projects.Item(1);                    

                    success = true;
                }
#if DEBUG
                catch (Exception ex)
#else
                catch
#endif
                {
                    success = false;
                    retry--;

                    if (retry > 0)
                    {
                        System.Threading.Thread.Sleep(1000);

#if DEBUG
                        Log.LogMessage(MessageImportance.High, "Exception: " + ex.ToString());
#endif
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (_project == null)
            {
                throw new ApplicationException("The project couldn't be created.");
            }
        }

        private void CopyIdlFiles()
        {            
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

        private void BuildWithMSBuild()
        {
            string platform = "x64";
            if (Platform == "Win32" || Platform == "x86" || Platform == "AnyCPU")
            {
                platform = "x86";
            }
            string solutionConfiguration = string.Format("{0}|{1}", Configuration, platform);

            int retry = 100;
            bool success = false;
            while (!success && retry > 0)
            {
                try
                {
                    _build.Build(true);
                    success = true;

                    CheckBuildInfo(platform);
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
#if DEBUG
                catch (Exception ex)
#else
                catch
#endif
                {
                    success = false;
                    retry--;

                    if (retry > 0)
                    {
                        System.Threading.Thread.Sleep(1000);
#if DEBUG
                        Log.LogMessage(MessageImportance.High, "Exception: " + ex.ToString());
#endif
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void CheckBuildInfo(string platform)
        {
            int retry = 100;
            bool success = false;
            int result = int.MaxValue;
            while (!success && retry > 0)
            {
                try
                {
                    if (_build.BuildState == vsBuildState.vsBuildStateDone)
                    {
                        result = _build.LastBuildInfo;
                        Log.LogMessage(MessageImportance.High, "BUILD RESULT: {0}", result);
                        success = true;
                    }
                    else
                    {
                        Log.LogMessage(MessageImportance.High, "BUILD STATE NOT DONE: {0}", _build.BuildState);
                        retry--;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
#if DEBUG
                catch (Exception ex)
#else
                catch
#endif
                {
                    success = false;
                    retry--;

                    if (retry > 0)
                    {
                        System.Threading.Thread.Sleep(1000);
#if DEBUG
                        Log.LogMessage(MessageImportance.High, "Exception: " + ex.ToString());
#endif
                    }
                    else
                    {
                        Log.LogMessage(MessageImportance.High, "LASTBUILDINFO cannot be retrieved: ");
                    }
                }
            }

            if (result > 0 && result < int.MaxValue)
            {
                string projectName = Path.GetFileNameWithoutExtension(_project.FullName);
                string cppPlatform = platform;
                if (platform == "x86")
                {
                    cppPlatform = "Win32";
                }
                string logFile = Path.Combine(IntDir, "obj", cppPlatform, Configuration, projectName + ".log");
                Log.LogMessage(MessageImportance.High, File.ReadAllText(logFile));

                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The project {0} failed to build.", _project.FullName));
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
