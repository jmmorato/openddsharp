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
using System.IO;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Git;
using Cake.Common;
using Cake.Common.Tools.VSWhere;
using Cake.Common.IO;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("SetupThirdParty")]    
    public sealed class SetupThirdPartyTask : FrostingTask<BuildContext>
    {
        private readonly DirectoryPath _clonePath = new DirectoryPath(System.IO.Path.GetFullPath(BuildContext.DDS_ROOT));
        private string _versionTag;

        public override bool ShouldRun(BuildContext context)
        {
            _versionTag = "DDS-" + context.OpenDdsVersion;

            if (context.IgnoreThirdPartySetup)
            {
                return false;
            }

            if (!Directory.Exists(BuildContext.DDS_ROOT))
            {
                return true;
            }
                        
            string currentHead = context.GitDescribe(_clonePath);
            if (_versionTag != currentHead)
            {
                return true;
            }

            return false;
        }

        public override void Run(BuildContext context)
        {
            if (!Directory.Exists(BuildContext.THIRD_PARTY_FOLDER))
            {
                context.Log.Information("Creating third folder folder...");
                Directory.CreateDirectory(BuildContext.THIRD_PARTY_FOLDER);
            }

            if (Directory.Exists(BuildContext.DDS_ROOT))
            {
                context.Log.Information("Cleaning OpenDDS folder...");
                DeleteDirectory(BuildContext.DDS_ROOT);
            }            
            
            context.Log.Information("Cloning OpenDDS repository...");
            context.GitClone("git://github.com/objectcomputing/OpenDDS.git", _clonePath);
            
            if (!ExistsVersionTag(context, _clonePath, _versionTag))
            {
                throw new Exception($"Couldn't find the OpenDDS version tag: {_versionTag}.");
            }

            context.Log.Information("Checkout OpenDDS version v{0}", context.OpenDdsVersion);
            context.GitCheckout(_clonePath, "tags/" + _versionTag);

            context.Log.Information("Apply required OpenDDSharp patches to OpenDDS...");
            foreach (string patchPath in Directory.GetFiles(BuildContext.PATCHES_FOLDER, "*.patch"))
            {
                DirectoryPath patchDirectory = new DirectoryPath(patchPath);
                int exitCode = context.StartProcess("git", new ProcessSettings
                {
                    Arguments = "apply " + patchDirectory.FullPath,
                    WorkingDirectory = BuildContext.DDS_ROOT,
                });
                if (exitCode != 0)
                {
                    throw new Exception($"Patch {patchPath} couldn't be applied. Exit code: {exitCode}");
                }
            }

            context.Log.Information("Call OpenDDS configure script");

            var configurePath = System.IO.Path.Combine(_clonePath.FullPath, "configure.cmd");
            var vsPath = context.VSWhereLatest(new Cake.Common.Tools.VSWhere.Latest.VSWhereLatestSettings
            {
                Version = "[\"15.0\", \"17.0\"]"
            });
            var arguments = "/c \"" + vsPath.FullPath + "\\Common7\\Tools\\VsDevCmd.bat\" && " + configurePath + " -v --ace-github-latest --no-test --no-debug --optimize";
            var exit = context.StartProcess("cmd.exe", new ProcessSettings
            {
                Arguments = arguments,
                WorkingDirectory = BuildContext.DDS_ROOT,
            });
            if (exit != 0)
            {
                throw new Exception($"Error calling the OpenDDS configure script. Exit code: {exit}");
            }

            context.Log.Information("Create a copy of the original bin/lib folders");
            context.CreateDirectory(System.IO.Path.Combine(_clonePath.FullPath, "original_bin"));
            context.CreateDirectory(System.IO.Path.Combine(_clonePath.FullPath, "original_lib"));
            context.CreateDirectory(System.IO.Path.Combine(_clonePath.FullPath, @"ACE_TAO\ACE\original_bin"));
            context.CreateDirectory(System.IO.Path.Combine(_clonePath.FullPath, @"ACE_TAO\ACE\original_lib"));
            context.CopyFiles(System.IO.Path.Combine(_clonePath.FullPath, @"bin\*"), System.IO.Path.Combine(_clonePath.FullPath, "original_bin"));
            context.CopyFiles(System.IO.Path.Combine(_clonePath.FullPath, @"lib\*"), System.IO.Path.Combine(_clonePath.FullPath, "original_lib"));
            context.CopyFiles(System.IO.Path.Combine(_clonePath.FullPath, @"ACE_TAO\ACE\bin\*"), System.IO.Path.Combine(_clonePath.FullPath, @"ACE_TAO\ACE\original_bin"));
            context.CopyFiles(System.IO.Path.Combine(_clonePath.FullPath, @"ACE_TAO\ACE\lib\*"), System.IO.Path.Combine(_clonePath.FullPath, @"ACE_TAO\ACE\original_lib"));            
        }

        private bool ExistsVersionTag(BuildContext context, DirectoryPath clonePath, string versionTag)
        {
            var tags = context.GitTags(clonePath);
            foreach (var tag in tags)
            {
                if (tag.FriendlyName == versionTag)
                {
                    return true;
                }
            }

            return false;
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
    }
}
