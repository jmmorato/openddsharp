﻿/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;
using OpenDDSharp.UnitTest.Helpers;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// The assembly initializer class.
    /// </summary>
    [TestClass]
    public static class AssemblyInitializer
    {
        private const string RTPS_DISCOVERY = "RtpsDiscovery";
        private const string INFOREPO_DISCOVERY = "InfoRepo";
        private const string INFOREPO_IOR = "repo.ior";
        internal const int INFOREPO_DOMAIN = 23;
        internal const int RTPS_DOMAIN = 42;
        internal const int RTPS_OTHER_DOMAIN = 43;

        private static SupportProcessHelper _supportProcess;
        private static Process _infoProcess;

        /// <summary>
        /// Gets the <see cref="DomainParticipantFactory" /> singleton instance.
        /// </summary>
        public static DomainParticipantFactory Factory { get; private set; }

        /// <summary>
        /// The assembly initializer method.
        /// </summary>
        /// <param name="context">The received <see cref="TestContext"/>.</param>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Ace.Init();

            var disc = new RtpsDiscovery(RTPS_DISCOVERY)
            {
                ResendPeriod = new TimeValue
                {
                    Seconds = 2,
                },
                SedpMulticast = false,
            };
            Assert.AreEqual(RTPS_DISCOVERY, disc.Key);
            ParticipantService.Instance.AddDiscovery(disc);
            ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;
            Assert.AreEqual(RTPS_DISCOVERY, ParticipantService.Instance.DefaultDiscovery);
            ParticipantService.Instance.SetRepoDomain(RTPS_DOMAIN, RTPS_DISCOVERY);
            ParticipantService.Instance.SetRepoDomain(RTPS_OTHER_DOMAIN, RTPS_DISCOVERY);

            var infoRepo = new InfoRepoDiscovery(INFOREPO_DISCOVERY, "corbaloc::localhost:12345/DCPSInfoRepo");
            ParticipantService.Instance.AddDiscovery(infoRepo);
            infoRepo.BitTransportIp = "localhost";
            infoRepo.BitTransportPort = 0;
            ParticipantService.Instance.SetRepoDomain(INFOREPO_DOMAIN, INFOREPO_DISCOVERY);

            _supportProcess = new SupportProcessHelper(context);
            _infoProcess = _supportProcess.SpawnDCPSInfoRepo();
            System.Threading.Thread.Sleep(1000);

            Factory = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSPendingTimeout", "3");

            Assert.IsFalse(TransportRegistry.Instance.Released);
            Assert.IsFalse(ParticipantService.Instance.IsShutdown);
        }

        /// <summary>
        /// The assembly clean-up method.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (_infoProcess != null)
            {
                _supportProcess.KillProcess(_infoProcess);
            }
            if (File.Exists(INFOREPO_IOR))
            {
                File.Delete(INFOREPO_IOR);
            }

            TransportRegistry.Instance.Release();
            Assert.IsTrue(TransportRegistry.Instance.Released);
            TransportRegistry.Close();
            var ret = ParticipantService.Instance.Shutdown();
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.IsTrue(ParticipantService.Instance.IsShutdown);

            Ace.Fini();
        }
    }
}
