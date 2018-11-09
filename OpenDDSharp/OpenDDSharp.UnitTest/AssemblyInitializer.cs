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
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public sealed class AssemblyInitializer
    {
        private const string RTPS_DISCOVERY = "RtpsDiscovery";

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            RtpsDiscovery disc = new RtpsDiscovery(RTPS_DISCOVERY);
            ParticipantService.Instance.AddDiscovery(disc);
            ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            TransportRegistry.Instance.Release();
            ParticipantService.Instance.Shutdown();            
        }
    }
}
