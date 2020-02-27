/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
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
using System;
using System.Collections.Generic;
using System.Threading;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.Standard.UnitTest.Helpers
{
    public static class TestHelper
    {
        #region Extensions
        public static bool WaitForSubscriptions(this DataWriter writer, int subscriptionsCount, int milliseconds)
        {
            PublicationMatchedStatus status = new PublicationMatchedStatus();
            writer.GetPublicationMatchedStatus(ref status);
            int count = milliseconds / 100;
            while (status.CurrentCount != subscriptionsCount && count > 0)
            {
                Thread.Sleep(100);
                writer.GetPublicationMatchedStatus(ref status);
                count--;
            }

            if (count == 0 && status.CurrentCount != subscriptionsCount)
            {
                return false;
            }            

            return true;
        }

        public static bool WaitForPublications(this DataReader reader, int publicationsCount, int milliseconds)
        {
            List<InstanceHandle> handles = new List<InstanceHandle>();
            reader.GetMatchedPublications(handles);
            int count = milliseconds / 100;
            while (handles.Count != publicationsCount && count > 0)
            {
                Thread.Sleep(100);
                reader.GetMatchedPublications(handles);
                count--;
            }

            if (count == 0 && handles.Count != publicationsCount)
            {
                return false;
            }

            return true;
        }

        public static void BindRtpsUdpTransportConfig(this Entity entity)
        {
            string guid = Guid.NewGuid().ToString("N");
            string configName = "openddsharp_rtps_interop_" + guid;
            string instName = "internal_openddsharp_rtps_transport_" + guid;

            TransportConfig config = TransportRegistry.Instance.CreateConfig(configName);
            TransportInst inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
            RtpsUdpInst rui = new RtpsUdpInst(inst);
            config.Insert(inst);

            TransportRegistry.Instance.BindConfig(configName, entity);
        }
        #endregion
    }
}
