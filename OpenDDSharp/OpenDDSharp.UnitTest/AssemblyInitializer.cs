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
using System.IO;
using System.Diagnostics;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;
using OpenDDSharp.UnitTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public sealed class AssemblyInitializer
    {
        private const string RTPS_DISCOVERY = "RtpsDiscovery";
        private const string INFOREPO_DISCOVERY = "InfoRepo";
        private const string INFOREPO_IOR = "repo.ior";
        internal const int INFOREPO_DOMAIN = 23;
        internal const int RTPS_DOMAIN = 42;
        internal const int RTPS_OTHER_DOMAIN = 43;

        private static SupportProcessHelper _supportProcess;
        private static Process _infoProcess;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            RtpsDiscovery disc = new RtpsDiscovery(RTPS_DISCOVERY);
            ParticipantService.Instance.AddDiscovery(disc);            
            ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;
            Assert.AreEqual(RTPS_DISCOVERY, ParticipantService.Instance.DefaultDiscovery);

            InfoRepoDiscovery infoRepo = new InfoRepoDiscovery(INFOREPO_DISCOVERY, "file://" + INFOREPO_IOR);
            ParticipantService.Instance.AddDiscovery(infoRepo);
            ParticipantService.Instance.SetRepoDomain(INFOREPO_DOMAIN, INFOREPO_DISCOVERY);

            _supportProcess = new SupportProcessHelper(context);
            _infoProcess = _supportProcess.SpawnDCPSInfoRepo();
            System.Threading.Thread.Sleep(1000);

            Assert.IsFalse(ParticipantService.Instance.IsShutdown);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _supportProcess.KillProcess(_infoProcess);
            if (File.Exists(INFOREPO_IOR))
            {
                File.Delete(INFOREPO_IOR);
            }

            TransportRegistry.Instance.Release();
            ParticipantService.Instance.Shutdown();
            Assert.IsTrue(ParticipantService.Instance.IsShutdown);
        }
    }
}
