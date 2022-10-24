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
using Cake.Frosting;

namespace OpenDDSharp.Build.Standard.Tasks
{
    /// <summary>
    /// Prepare linux files taks.
    /// </summary>
    [TaskName("PrepareLinuxLibraries")]
    public class PrepareLinuxLibraries : FrostingTask<BuildContext>
    {
        private const string OPENDDS_VERSION = "3.22.0";
        private const string ACE_VERSION = "6.5.18";
        private const string TAO_VERSION = "2.5.18";

        private readonly string[] _acelibraries = new string[]
        {
            @"ext\OpenDDS_Linux\ACE_TAO\ACE\ace\libACE.so",
        };

        private readonly string[] _taoLibraries = new string[]
        {
            @"ext\OpenDDS_Linux\ACE_TAO\TAO\tao\libTAO.so",
            @"ext\OpenDDS_Linux\ACE_TAO\TAO\tao\CodecFactory\libTAO_CodecFactory.so",
            @"ext\OpenDDS_Linux\ACE_TAO\TAO\tao\AnyTypeCode\libTAO_AnyTypeCode.so",
            @"ext\OpenDDS_Linux\ACE_TAO\TAO\tao\BiDir_GIOP\libTAO_BiDirGIOP.so",
            @"ext\OpenDDS_Linux\ACE_TAO\TAO\tao\PI\libTAO_PI.so",
            @"ext\OpenDDS_Linux\ACE_TAO\TAO\tao\PortableServer\libTAO_PortableServer.so",
        };

        private readonly string[] _openddsLibraries = new string[]
        {
            @"ext\OpenDDS_Linux\dds\libOpenDDS_Dcps.so",
            @"ext\OpenDDS_Linux\dds\DCPS\RTPS\libOpenDDS_Rtps.so",
            @"ext\OpenDDS_Linux\dds\InfoRepo\libOpenDDS_InfoRepoLib.so",
            @"ext\OpenDDS_Linux\dds\DCPS\InfoRepoDiscovery\libOpenDDS_InfoRepoDiscovery.so",
            @"ext\OpenDDS_Linux\dds\DCPS\transport\rtps_udp\libOpenDDS_Rtps_Udp.so",
            @"ext\OpenDDS_Linux\dds\DCPS\transport\shmem\libOpenDDS_Shmem.so",
            @"ext\OpenDDS_Linux\dds\DCPS\transport\tcp\libOpenDDS_Tcp.so",
            @"ext\OpenDDS_Linux\dds\DCPS\transport\multicast\libOpenDDS_Multicast.so",
            @"ext\OpenDDS_Linux\dds\DCPS\transport\udp\libOpenDDS_Udp.so",
        };

        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            string solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);

            foreach (string s in _acelibraries)
            {
                string sourceFile = Path.Combine(solutionPath, $"{s}.{ACE_VERSION}");
                string destinationFile = Path.Combine(solutionPath, s);
                File.Copy(sourceFile, destinationFile, true);
            }

            foreach (string s in _taoLibraries)
            {
                string sourceFile = Path.Combine(solutionPath, $"{s}.{TAO_VERSION}");
                string destinationFile = Path.Combine(solutionPath, s);
                File.Copy(sourceFile, destinationFile, true);
            }

            foreach (string s in _openddsLibraries)
            {
                string sourceFile = Path.Combine(solutionPath, $"{s}.{OPENDDS_VERSION}");
                string destinationFile = Path.Combine(solutionPath, s);
                File.Copy(sourceFile, destinationFile, true);
            }
        }
    }
}
