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
using Cake.Common;
using Cake.Common.Tools.VSWhere;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using OpenDDSharp.Build.Exceptions;

namespace OpenDDSharp.Build.Tasks
{
    /// <summary>
    /// Setup third party required projects.
    /// </summary>
    [TaskName("SetupThirdParty")]
    public sealed class SetupThirdPartyTask : FrostingTask<BuildContext>
    {
        private const string OPENDDS_GIT_REPOSITORY = "https://github.com/OpenDDS/OpenDDS.git";
        private DirectoryPath _clonePath;
        private string _versionTag;

        /// <inheritdoc/>
        public override bool ShouldRun(BuildContext context)
        {
            _clonePath = new DirectoryPath(System.IO.Path.GetFullPath(context.DdsRoot));
            _versionTag = "DDS-" + context.OpenDdsVersion;

            if (context.IgnoreThirdPartySetup)
            {
                return false;
            }

            if (!Directory.Exists(_clonePath.FullPath) || !Directory.EnumerateFileSystemEntries(_clonePath.FullPath).Any())
            {
                return true;
            }

            string currentHead = GitDescribe(context);
            if (_versionTag != currentHead)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            if (!Directory.Exists(BuildContext.THIRD_PARTY_FOLDER))
            {
                context.Log.Information("Creating third-party folder...");
                Directory.CreateDirectory(BuildContext.THIRD_PARTY_FOLDER);
            }

            if (Directory.Exists(context.DdsRoot))
            {
                context.Log.Information("Cleaning OpenDDS folder...");
                DeleteDirectory(context.DdsRoot);
            }
            Directory.CreateDirectory(context.DdsRoot);

            context.Log.Information($"Cloning OpenDDS repository on {_clonePath.FullPath}...");
            Git(context, $"clone -q {OPENDDS_GIT_REPOSITORY} {_clonePath.FullPath}");

            context.Log.Information("Checkout OpenDDS version v{0}", context.OpenDdsVersion);
            Git(context, "fetch");
            Git(context, "fetch --tags");
            Git(context, $"checkout tags/{_versionTag}");

            context.Log.Information("Apply required OpenDDSharp patches to OpenDDS...");
            if (!Directory.Exists(BuildContext.PATCHES_FOLDER))
            {
                return;
            }
            var patches = Directory.EnumerateFiles(BuildContext.PATCHES_FOLDER, "*.patch");
            var patchPaths = patches as string[] ?? patches.ToArray();
            if (!patchPaths.Any())
            {
                return;
            }
            foreach (var patchPath in patchPaths)
            {
                var patchDirectory = new DirectoryPath(patchPath);
                if (BuildContext.IsLinux)
                {
                    var linuxPath = System.IO.Path.GetFullPath(patchDirectory.FullPath);

                    context.Log.Information($"Apply {linuxPath} in {context.DdsRoot}...");
                    Git(context, "apply --whitespace=fix --ignore-space-change --ignore-whitespace " + linuxPath);
                }
                else
                {
                    Git(context, "apply " + patchDirectory.FullPath);
                }
            }

            context.Log.Information("Call OpenDDS configure script");

            if (BuildContext.IsLinux || BuildContext.IsOSX)
            {
                var configurePath = System.IO.Path.Combine(_clonePath.FullPath, "configure");
                var arguments = " -v --doc-group3 --no-test --no-debug --optimize --install-origin-relative --prefix=/usr/lib";
                if (BuildContext.IsOSX)
                {
                    arguments += " --std=c++11";
                }
                context.Log.Information(arguments);

                var exit = context.StartProcess(configurePath, new ProcessSettings
                {
                    Arguments = arguments,
                    WorkingDirectory = context.DdsRoot,
                });

                if (exit != 0)
                {
                    throw new BuildException($"Error calling the OpenDDS configure script. Exit code: {exit}");
                }
            }
            else
            {
                var configurePath = System.IO.Path.Combine(_clonePath.FullPath, "configure.cmd");
                var version = "[\"15.0\", \"17.0\"]";
                if (context.VisualStudioVersion == Cake.Common.Tools.MSBuild.MSBuildToolVersion.VS2022)
                {
                    version = "[\"15.0\", \"18.0\"]";
                }
                var vsPath = context.VSWhereLatest(new Cake.Common.Tools.VSWhere.Latest.VSWhereLatestSettings
                {
                    Version = version,
                });

                var vcvar = $"\\VC\\Auxiliary\\Build\\vcvarsall.bat\" {context.BuildPlatform} -vcvars_ver=14.39";
                var vsdev = "\\Common7\\Tools\\VsDevCmd.bat\"";
                var arguments = " /c \"" + vsPath.FullPath + vcvar + " && " + configurePath + " -v --doc-group3 --no-test";
                if (context.BuildConfiguration == "Release")
                {
                    arguments += " --no-debug --optimize";
                }
                else
                {
                    arguments += " --debug --no-optimize";
                }
                context.Log.Information(configurePath + arguments);

                var exit = context.StartProcess("cmd.exe", new ProcessSettings
                {
                    Arguments = arguments,
                    WorkingDirectory = context.DdsRoot,
                });
                if (exit != 0)
                {
                    throw new BuildException($"Error calling the OpenDDS configure script. Exit code: {exit}");
                }
            }
        }

        private static void DeleteDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            var files = Directory.GetFiles(directoryPath);
            var directories = Directory.GetDirectories(directoryPath);

            foreach (var file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var dir in directories)
            {
                DeleteDirectory(dir);
            }

            File.SetAttributes(directoryPath, FileAttributes.Normal);

            Directory.Delete(directoryPath, false);
        }

        private void Git(BuildContext context, string arguments)
        {
            var exit = context.StartProcess("git", new ProcessSettings
            {
                Arguments = arguments,
                WorkingDirectory = _clonePath.FullPath,
            });

            if (exit != 0)
            {
                throw new BuildException($"Error calling 'git {arguments}'. Exit code: {exit}");
            }
        }

        private string GitDescribe(BuildContext context)
        {
            var process = context.StartAndReturnProcess("git", new ProcessSettings
            {
                Arguments = "describe",
                WorkingDirectory = _clonePath.FullPath,
                RedirectStandardOutput = true,
            });

            process.WaitForExit();

            var exit = process.GetExitCode();
            if (exit == 0)
            {
                try
                {
                    var output = process.GetStandardOutput();
                    if (output.Any())
                    {
                        return output.First();
                    }
                }
                catch
                {
                    // Expected exception when not output received
                }
            }
            else
            {
                throw new BuildException($"Error calling 'git describe'. Working directory: {_clonePath.FullPath} Exit code: {exit}");
            }

            return string.Empty;
        }
    }
}
