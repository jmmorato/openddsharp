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
using System.Runtime.InteropServices;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Frosting;

namespace OpenDDSharp.Build
{
    /// <summary>
    /// Build context class to be shared between tasks.
    /// </summary>
    public class BuildContext : FrostingContext
    {
        #region Constants
        private const string DEFAULT_OPENDDS_VERSION = "3.31";

        private const string DEFAULT_PERL_FOLDER = "C:/Strawberry/perl/bin";

        private const string DEFAULT_CONFIGURATION = "Release";

        private const PlatformTarget DEFAULT_PLATFORM = PlatformTarget.x64;

        private const string DEFAULT_VS_EDITION = "Enterprise";

        internal const string THIRD_PARTY_FOLDER = "../../ext/";

        internal const string PATCHES_FOLDER = "../../Patches/";

        internal const string NATIVE_FOLDER = "./../../Native/";

        internal const string OPENDDSHARP_SOLUTION_FOLDER = "../../";

        internal const string OPENDDSHARP_SOLUTION_FILE = OPENDDSHARP_SOLUTION_FOLDER + "OpenDDSharp.sln";
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
        public string PatchVersion { get; internal set; }

        /// <summary>
        /// Gets the current build number.
        /// </summary>
        public string BuildNumber { get; internal set; }

        /// <summary>
        /// Gets the current pre-release tag.
        /// </summary>
        public string PreReleaseTag { get; internal set; }

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
        /// Gets the Visual Studio edition used to build the project.
        /// </summary>
        public string VisualStudioEdition { get; internal set; }

        /// <summary>
        /// Gets the build platform to use.
        /// </summary>
        public PlatformTarget BuildPlatform { get; internal set; }

        /// <summary>
        /// Gets the NugetApiKey.
        /// </summary>
        public string NugetApiKey { get; internal set; }

        /// <summary>
        /// Gets the current branch name.
        /// </summary>
        public string BranchName { get; internal set; }

        internal static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        internal static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        internal static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        internal static bool IsARM64 => RuntimeInformation.OSArchitecture == Architecture.Arm64;

        internal string DdsRoot { get; private set; }

        internal string AceRoot { get; private set; }

        internal string TaoRoot { get; private set; }

        internal string MpcRoot { get; private set; }

        internal string OpenDdsSolutionFile { get; private set; }

        internal bool IsDevelop => BranchName != "main";

        internal string RunTime
        {
            get
            {
                var runtime = string.Empty;
                if (IsWindows)
                {
                    if (BuildPlatform == PlatformTarget.x64)
                    {
                        runtime = "win-x64";
                    }
                    else
                    {
                        runtime = "win-x86";
                    }
                }
                else if (IsLinux)
                {
                    runtime = "linux-x64";

                    if (BuildPlatform == PlatformTarget.ARM64)
                    {
                        runtime = "linux-arm64";
                    }
                }
                else if (IsOSX)
                {
                    runtime = "osx-x64";

                    if (BuildPlatform == PlatformTarget.ARM64)
                    {
                        runtime = "osx-arm64";
                    }
                }

                return runtime;
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
            foreach (var arg in context.Arguments.GetArguments())
            {
                Console.WriteLine(arg.Key + ": " + arg.Value);
            }
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

            if (context.Arguments.HasArgument(nameof(BranchName)))
            {
                BranchName = context.Arguments.GetArgument(nameof(BranchName));
            }
            else
            {
                BranchName = string.Empty;
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

            if (context.Arguments.HasArgument(nameof(PatchVersion)))
            {
                PatchVersion = context.Arguments.GetArgument(nameof(PatchVersion));
            }
            else
            {
                PatchVersion = "0";
            }

            if (context.Arguments.HasArgument(nameof(BuildNumber)))
            {
                BuildNumber = context.Arguments.GetArgument(nameof(BuildNumber));
            }
            else
            {
                BuildNumber = "0";
            }

            if (context.Arguments.HasArgument(nameof(PreReleaseTag)))
            {
                PreReleaseTag = context.Arguments.GetArgument(nameof(PreReleaseTag));
            }
            else
            {
                PreReleaseTag = string.Empty;
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
                VisualStudioVersion = MSBuildToolVersion.VS2022;
            }

            if (context.Arguments.HasArgument(nameof(VisualStudioEdition)))
            {
                VisualStudioEdition = context.Arguments.GetArgument(nameof(VisualStudioEdition));
            }
            else
            {
                VisualStudioEdition = DEFAULT_VS_EDITION;
            }

            if (context.Arguments.HasArgument(nameof(NugetApiKey)))
            {
                NugetApiKey = context.Arguments.GetArgument(nameof(NugetApiKey));
            }
            else
            {
                NugetApiKey = string.Empty;
            }

            DdsRoot = $"{THIRD_PARTY_FOLDER}OpenDDS_{BuildPlatform}/";
            if (!IsWindows)
            {
                DdsRoot = THIRD_PARTY_FOLDER + $"OpenDDS_{RunTime}/";
            }

            AceRoot = DdsRoot + "ACE_wrappers/";
            TaoRoot = DdsRoot + "ACE_wrappers/TAO/";
            MpcRoot = AceRoot + "MPC/";
            OpenDdsSolutionFile = DdsRoot + "/DDS_TAOv2.sln";

            var ddsPath = Path.GetFullPath(DdsRoot).TrimEnd(Path.DirectorySeparatorChar);
            var acePath = Path.GetFullPath(AceRoot).TrimEnd(Path.DirectorySeparatorChar);
            var ddsBin = Path.Combine(ddsPath, "bin");
            var ddsLib = Path.Combine(ddsPath, "lib");
            var aceBin = Path.Combine(acePath, "bin");
            var aceLib = Path.Combine(acePath, "lib");
            var perlPath = Path.GetFullPath(PerlPath);
            var path = $"{perlPath};{ddsBin};{ddsLib};{aceBin};{aceLib};";
            System.Environment.SetEnvironmentVariable("Path", path + Environment.GetEnvironmentVariable("Path"));
            System.Environment.SetEnvironmentVariable("DDS_ROOT", ddsPath);
        }
        #endregion
    }
}
