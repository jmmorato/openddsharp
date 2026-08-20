/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using System.IO;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace OpenDDSharp.Build.Tasks;

/// <summary>
/// Prepare linux files taks.
/// </summary>
[TaskName("PrepareLinuxLibraries")]
public class PrepareLinuxLibraries : FrostingTask<BuildContext>
{
    private const string OPENDDS_VERSION = "3.34.0";
    private const string ACE_VERSION = "8.0.6";
    private const string TAO_VERSION = "4.0.6";

    private readonly string[] _aceLibraries =
    {
        @"ACE_wrappers\ace\libACE.so",
    };

    private readonly string[] _taoLibraries =
    {
        @"ACE_wrappers\TAO\tao\libTAO.so",
        @"ACE_wrappers\TAO\tao\CodecFactory\libTAO_CodecFactory.so",
        @"ACE_wrappers\TAO\tao\AnyTypeCode\libTAO_AnyTypeCode.so",
        @"ACE_wrappers\TAO\tao\BiDir_GIOP\libTAO_BiDirGIOP.so",
        @"ACE_wrappers\TAO\tao\PI\libTAO_PI.so",
        @"ACE_wrappers\TAO\tao\PortableServer\libTAO_PortableServer.so",
    };

    private readonly string[] _openddsLibraries =
    {
        @"dds\libOpenDDS_Dcps.so",
        @"dds\DCPS\RTPS\libOpenDDS_Rtps.so",
        @"dds\InfoRepo\libOpenDDS_InfoRepoLib.so",
        @"dds\DCPS\InfoRepoDiscovery\libOpenDDS_InfoRepoDiscovery.so",
        @"dds\DCPS\transport\rtps_udp\libOpenDDS_Rtps_Udp.so",
        @"dds\DCPS\transport\shmem\libOpenDDS_Shmem.so",
        @"dds\DCPS\transport\tcp\libOpenDDS_Tcp.so",
        @"dds\DCPS\transport\multicast\libOpenDDS_Multicast.so",
        @"dds\DCPS\transport\udp\libOpenDDS_Udp.so",
    };

    /// <inheritdoc/>
    public override void Run(BuildContext context)
    {
        var solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
        var buildPlatforms = new[]
        {
            "x64",
            "arm64",
        };

        foreach (var platform in buildPlatforms)
        {
            foreach (var s in _aceLibraries)
            {
                var sourceFile = Path.Combine(solutionPath, "ext", $"OpenDDS_linux-{platform}", $"{s}.{ACE_VERSION}");
                var destinationFile = Path.Combine(solutionPath, "ext", $"OpenDDS_linux-{platform}", s);

                context.Log.Information($"Copying ACE Library {sourceFile} to {destinationFile}");

                File.Copy(sourceFile, destinationFile, true);
            }

            foreach (var s in _taoLibraries)
            {
                var sourceFile = Path.Combine(solutionPath, "ext", $"OpenDDS_linux-{platform}", $"{s}.{TAO_VERSION}");
                var destinationFile = Path.Combine(solutionPath, "ext", $"OpenDDS_linux-{platform}", s);

                context.Log.Information($"Copying TAO Library {sourceFile} to {destinationFile}");

                File.Copy(sourceFile, destinationFile, true);
            }

            foreach (var s in _openddsLibraries)
            {
                var sourceFile = Path.Combine(solutionPath, "ext", $"OpenDDS_linux-{platform}", $"{s}.{OPENDDS_VERSION}");
                var destinationFile = Path.Combine(solutionPath, "ext", $"OpenDDS_linux-{platform}", s);

                context.Log.Information($"Copying DDS Library {sourceFile} to {destinationFile}");

                File.Copy(sourceFile, destinationFile, true);
            }
        }
    }
}