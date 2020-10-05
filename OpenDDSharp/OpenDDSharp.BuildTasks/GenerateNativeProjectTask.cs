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
using EnvDTE100;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace OpenDDSharp.BuildTasks
{    
    public class GenerateNativeProjectTask : Task
    {
        #region Fields
        private DTE2 _dte;
        private Solution4 _solution;
        private Project _project;
        private string _solutionName;
        private string _projectName;
        private int _msbuildVersion;
        private readonly Dictionary<int, string> _platformToolsets = new Dictionary<int, string>
        {
            { 15, "v141" },
            { 16, "v142" }
        };
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
        #endregion

        #region Methods
        public override bool Execute()
        {
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

            GenerateSolutionFile();
            GenerateProjectFile();
            CopyIdlFiles();
            BuildWithMSBuild();
            ShutDown();

            return true;
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
                        System.Threading.Thread.Sleep(150);

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
            string fullPath = Path.Combine(IntDir, _solutionName + ".sln");
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            int retry = 100;
            bool success = false;
            while (!success && retry > 0)
            {
                try
                {
                    _dte.Solution.Create(IntDir, _solutionName);

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
                        System.Threading.Thread.Sleep(150);

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
                    _solution = (Solution4)_dte.Solution;
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
                        System.Threading.Thread.Sleep(150);

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
                        System.Threading.Thread.Sleep(150);

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
                    _solution.AddFromTemplate(TemplatePath, IntDir, _projectName, false);

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
                        System.Threading.Thread.Sleep(150);

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
                        System.Threading.Thread.Sleep(150);

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
            List<string> platforms = new List<string>();

            if (Platform == "Win32" || Platform == "x86")
            {
                platforms.Add("x86");
            }
            else if (Platform == "x64")
            {
                platforms.Add("x64");
            }
            else if (Platform == "AnyCPU")
            {
                platforms.Add("x86");
                platforms.Add("x64");
            }

            foreach (string platform in platforms)
            {
                string solutionConfiguration = string.Format("{0}|{1}", Configuration, platform);

                int retry = 100;
                bool success = false;

                while (!success && retry > 0)
                {
                    try
                    {
                        _solution.SolutionBuild.BuildProject(solutionConfiguration, _project.FullName, true);
                        if (_solution.SolutionBuild.LastBuildInfo > 0)
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
                        success = true;
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
                            System.Threading.Thread.Sleep(150);
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
        }

        private void ShutDown()
        {
            try
            {                
                _dte.Quit();
            }
            catch { }
        }
        #endregion
    }
}
