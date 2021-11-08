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

namespace OpenDDSharp.Build.Standard
{
    /// <summary>
    /// Build context class to be shared between tasks.
    /// </summary>
    public class BuildContext : FrostingContext
    {
        #region Constants
        internal const string DEFAULT_OPENDDS_VERSION = "3.18.1";

        private const string DEFAULT_PERL_FOLDER = "C:/Strawberry/perl/bin";

        internal const string DEFAULT_CONFIGURATION = "Release";

        internal const PlatformTarget DEFAULT_PLATFORM = PlatformTarget.x64;

        internal const string THIRD_PARTY_FOLDER = "../../ext/";

        internal const string PATCHES_FOLDER = "../../Patches/";

        internal const string OPENDDSHARP_SOLUTION_FOLDER = "../../";

        internal const string OPENDDSHARP_SOLUTION_FILE = OPENDDSHARP_SOLUTION_FOLDER + "OpenDDSharp.Standard.sln";
        #endregion

        #region Properties
        /// <summary>
        /// Gets the OpenDDSharp major version.
        /// </summary>
        public string MajorVersion { get; internal set; }

        /// <summary>
        /// Gets the OpenDDSharp minor version.
        /// </summary>
        public string MinorVersion { get; internal set; }

        /// <summary>
        /// Gets the current run number.
        /// </summary>
        public string RunNumber { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether is a build in develop branch.
        /// </summary>
        public bool IsDevelop { get; internal set; }

        /// <summary>
        /// Gets the OpenDDS version to use during the build.
        /// </summary>
        public string OpenDdsVersion { get; internal set; }

        /// <summary>
        /// Gets the Perl installation path.
        /// </summary>
        public string PerlPath { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether ignore the third-party setup.
        /// </summary>
        public bool IgnoreThirdPartySetup { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether ignore the third-party build.
        /// </summary>
        public bool IgnoreThirdPartyBuild { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether cleanup OpenDDS temporal files.
        /// </summary>
        public bool CleanupTemporalFiles { get; internal set; }

        /// <summary>
        /// Gets the build configuration to use.
        /// </summary>
        public string BuildConfiguration { get; internal set; }

        /// <summary>
        /// Gets the Visual Studio version used to build the project.
        /// </summary>
        public MSBuildToolVersion VisualStudioVersion { get; internal set; }

        /// <summary>
        /// Gets the build platform to use.
        /// </summary>
        public PlatformTarget BuildPlatform { get; internal set; }

        /// <summary>
        /// Gets the NugetApiKey.
        /// </summary>
        public string NugetApiKey { get; internal set; }

        internal string DdsRoot { get; private set; }

        internal string AceRoot { get; private set; }

        internal string TaoRoot { get; private set; }

        internal string MpcRoot { get; private set; }

        internal string OpenDdsSolutionFile { get; private set; }

        internal bool IsLinuxBuild
        {
            get
            {
                return BuildConfiguration.StartsWith("Linux");
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildContext"/> class.
        /// </summary>
        /// <param name="context">The context parameter.</param>
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

            if (context.Arguments.HasArgument(nameof(CleanupTemporalFiles)))
            {
                CleanupTemporalFiles = bool.Parse(context.Arguments.GetArgument(nameof(CleanupTemporalFiles)));
            }
            else
            {
                CleanupTemporalFiles = false;
            }

            if (context.Arguments.HasArgument(nameof(IgnoreThirdPartyBuild)))
            {
                IgnoreThirdPartyBuild = bool.Parse(context.Arguments.GetArgument(nameof(IgnoreThirdPartyBuild)));
            }
            else
            {
                IgnoreThirdPartyBuild = false;
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
                PerlPath = DEFAULT_PERL_FOLDER;
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

            if (!BuildConfiguration.StartsWith("Linux"))
            {
                DdsRoot = THIRD_PARTY_FOLDER + "OpenDDS/";
            }
            else
            {
                DdsRoot = THIRD_PARTY_FOLDER + "OpenDDS_Linux/";
            }

            AceRoot = DdsRoot + "ACE_TAO/ACE/";
            TaoRoot = DdsRoot + "ACE_TAO/TAO/";
            MpcRoot = AceRoot + "MPC/";
            OpenDdsSolutionFile = DdsRoot + "/DDS_TAOv2.sln";

            var ddsPath = Path.GetFullPath(DdsRoot).TrimEnd(Path.DirectorySeparatorChar);
            var acePath = Path.GetFullPath(AceRoot).TrimEnd(Path.DirectorySeparatorChar);
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
        internal static string ToWslPath(string windowPath)
        {
            var drive = "/mnt/" + windowPath[0].ToString().ToLowerInvariant();
            var ret = drive + windowPath[2..];
            ret = ret.Replace("\\", "/");

            return ret;
        }

        internal string GetBuildRevisionVersion()
        {
            string year = DateTime.Now.ToString("yy");
            string dayOfYear = DateTime.Now.DayOfYear.ToString();
            int runNumber = int.Parse(RunNumber) % 65534;
            return $"{year}{dayOfYear}.{runNumber}";
        }
        #endregion
    }
}
