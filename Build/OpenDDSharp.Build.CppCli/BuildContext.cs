﻿/*********************************************************************
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
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Frosting;
using System;
using System.IO;

namespace OpenDDSharp.Build.CppCli
{
    public class BuildContext : FrostingContext
    {
        #region Constants
        public const string DEFAULT_OPENDDS_VERSION = "3.16";
        public const string DEFAULT_PERL_PATH = "C:/Strawberry/perl/bin";
        public const string DEFAULT_CONFIGURATION = "Release";
        public const PlatformTarget DEFAULT_PLATFORM = PlatformTarget.x64;

        public const string THIRD_PARTY_FOLDER = "../../ext/";
        public const string DDS_ROOT = THIRD_PARTY_FOLDER + "OpenDDS/";
        public const string ACE_ROOT = DDS_ROOT + "ACE_TAO/ACE/";
        public const string TAO_ROOT = DDS_ROOT + "ACE_TAO/TAO/";
        public const string MPC_ROOT = ACE_ROOT + "MPC/";
        public const string PATCHES_FOLDER = "../../Patches/";
        public const string OPENDDSHARP_SOLUTION_FOLDER = "../../";
        public const string OPENDDSHARP_SOLUTION_FILE = OPENDDSHARP_SOLUTION_FOLDER + "OpenDDSharp.CppCli.sln";
        public const string OPENDDS_SOLUTION_FILE = DDS_ROOT + "/DDS_TAOv2.sln";
        #endregion

        #region Properties
        public string MajorVersion { get; internal set; }
        public string MinorVersion { get; internal set; }
        public string RunNumber { get; internal set; }
        public bool IsDevelop { get; internal set; }
        public string OpenDdsVersion { get; internal set; }
        public string PerlPath { get; internal set; }
        public bool IgnoreThirdPartySetup { get; internal set; }
        public string BuildConfiguration { get; internal set; }
        public MSBuildToolVersion VisualStudioVersion { get; internal set; }
        public PlatformTarget BuildPlatform { get; internal set; }
        public string NugetApiKey { get; internal set; }
        public string VsMarketplaceToken { get; internal set; }
        #endregion

        #region Constructors
        public BuildContext(ICakeContext context) : base(context)
        {
            if (context.Arguments.HasArgument(nameof(IgnoreThirdPartySetup)))
            {
                IgnoreThirdPartySetup = bool.Parse(context.Arguments.GetArgument(nameof(IgnoreThirdPartySetup)));
            }
            else
            {
                IgnoreThirdPartySetup = false;
            }

            if (context.Arguments.HasArgument(nameof(IsDevelop)))
            {
                IsDevelop = bool.Parse(context.Arguments.GetArgument(nameof(IsDevelop)));
            }
            else
            {
                IsDevelop = true;
            }

            if (context.Arguments.HasArgument(nameof(MajorVersion)))
            {
                MajorVersion = context.Arguments.GetArgument(nameof(MajorVersion));
            }
            else
            {
                MajorVersion = "1";
            }

            if (context.Arguments.HasArgument(nameof(MinorVersion)))
            {
                MinorVersion = context.Arguments.GetArgument(nameof(MinorVersion));
            }
            else
            {
                MinorVersion = "0";
            }

            if (context.Arguments.HasArgument(nameof(RunNumber)))
            {
                RunNumber = context.Arguments.GetArgument(nameof(RunNumber));
            }
            else
            {
                RunNumber = "1";
            }

            if (context.Arguments.HasArgument(nameof(OpenDdsVersion)))
            {
                OpenDdsVersion = context.Arguments.GetArgument(nameof(OpenDdsVersion));
            }
            else
            {
                OpenDdsVersion = DEFAULT_OPENDDS_VERSION;
            }

            if (context.Arguments.HasArgument(nameof(PerlPath)))
            {
                PerlPath = context.Arguments.GetArgument(nameof(PerlPath));
            }
            else
            {
                PerlPath = DEFAULT_PERL_PATH;
            }

            if (context.Arguments.HasArgument(nameof(BuildConfiguration)))
            {
                BuildConfiguration = context.Arguments.GetArgument(nameof(BuildConfiguration));
            }
            else
            {
                BuildConfiguration = DEFAULT_CONFIGURATION;
            }

            if (context.Arguments.HasArgument(nameof(BuildPlatform)))
            {
                BuildPlatform = Enum.Parse<PlatformTarget>(context.Arguments.GetArgument(nameof(BuildPlatform)));
            }
            else
            {
                BuildPlatform = DEFAULT_PLATFORM;
            }

            if (context.Arguments.HasArgument(nameof(VisualStudioVersion)))
            {
                VisualStudioVersion = Enum.Parse<MSBuildToolVersion>(context.Arguments.GetArgument(nameof(VisualStudioVersion)));
            }
            else
            {
                VisualStudioVersion = MSBuildToolVersion.VS2019;
            }

            if (context.Arguments.HasArgument(nameof(NugetApiKey)))
            {
                NugetApiKey = context.Arguments.GetArgument(nameof(NugetApiKey));
            }
            else
            {
                NugetApiKey = string.Empty;
            }

            if (context.Arguments.HasArgument(nameof(VsMarketplaceToken)))
            {
                VsMarketplaceToken = context.Arguments.GetArgument(nameof(VsMarketplaceToken));
            }
            else
            {
                VsMarketplaceToken = string.Empty;
            }

            var ddsPath = Path.GetFullPath(DDS_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var acePath = Path.GetFullPath(ACE_ROOT).TrimEnd(Path.DirectorySeparatorChar);            
            var ddsBin = Path.Combine(ddsPath, $"bin");
            var ddsLib = Path.Combine(ddsPath, $"lib");
            var aceBin = Path.Combine(acePath, $"bin");
            var aceLib = Path.Combine(acePath, $"lib");
            var ddsBinPlatform = Path.Combine(ddsPath, $"bin_{BuildPlatform}");
            var ddsLibPlatform = Path.Combine(ddsPath, $"lib_{BuildPlatform}");
            var aceBinPlatform = Path.Combine(acePath, $"bin_{BuildPlatform}");
            var aceLibPlatform = Path.Combine(acePath, $"lib_{BuildPlatform}");
            var perlPath = Path.GetFullPath(PerlPath);
            string path = $"{perlPath};{ddsBinPlatform};{ddsLibPlatform};{aceBinPlatform};{aceLibPlatform};{ddsBin};{ddsLib};{aceBin};{aceLib};";
            System.Environment.SetEnvironmentVariable("Path", path + Environment.GetEnvironmentVariable("Path"));
            System.Environment.SetEnvironmentVariable("DDS_ROOT", ddsPath);            
        }
        #endregion

        #region Methods
        public string GetBuildRevisionVersion()
        {
            string year = DateTime.Now.ToString("yy");
            string dayOfYear = DateTime.Now.DayOfYear.ToString();
            int runNumber = int.Parse(RunNumber) % 65534;
            return $"{year}{dayOfYear}.{runNumber}";
        }
        #endregion
    }
}
