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
using System;
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;

namespace TestSupportProcess
{
    class Program
    {
        private const string RTPS_DISCOVERY = "RtpsDiscovery";

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ApplicationException("Incorrect number of arguments.");
            }

            RtpsDiscovery disc = new RtpsDiscovery(RTPS_DISCOVERY);
            ParticipantService.Instance.AddDiscovery(disc);
            ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;

            if (Enum.TryParse(args[0], out SupportTestKind testKind))
            {
                switch (testKind)
                {
                    case SupportTestKind.InconsistentTopicTest:
                        TestOnInconsistentTopic();
                        break;
                    case SupportTestKind.GetDiscoveredTopicsTest:
                        TestGetDiscoveredTopics();
                        break;
                    default:
                        throw new ApplicationException("Unkwnon test requested.");
                }
            }
            else
            {
                throw new ApplicationException("Argument is not a valid test kind.");
            }
        }

        private static void TestGetDiscoveredTopics()
        {
            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory();
            DomainParticipant participant = dpf.CreateParticipant(42);
            if (participant == null)
            {
                throw new ApplicationException("Failed to create the participant.");
            }

            BindRtpsUdpTransportConfig(participant);

            Subscriber subscriber = participant.CreateSubscriber();
            if (subscriber == null)
            {
                throw new ApplicationException("Failed to create the subscriber.");
            }

            AthleteTypeSupport support = new AthleteTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(participant, typeName);
            if (result != ReturnCode.Ok)
            {
                throw new ApplicationException("Failed to register the type." + result.ToString());
            }

            Topic topic = participant.CreateTopic(nameof(TestOnInconsistentTopic), typeName);
            if (topic == null)
            {
                throw new ApplicationException("Failed to create the topic.");
            }

            DataReader reader = subscriber.CreateDataReader(topic);
            if (reader == null)
            {
                throw new ApplicationException("Failed to create the reader.");
            }

            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }        

        private static void TestOnInconsistentTopic()
        {           
            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory();
            DomainParticipant participant = dpf.CreateParticipant(42);
            if (participant == null)
            {
                throw new ApplicationException("Failed to create the participant.");
            }

            BindRtpsUdpTransportConfig(participant);

            Subscriber subscriber = participant.CreateSubscriber();
            if (subscriber == null)
            {
                throw new ApplicationException("Failed to create the subscriber.");
            }

            AthleteTypeSupport support = new AthleteTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(participant, typeName);
            if (result != ReturnCode.Ok)
            {
                throw new ApplicationException("Failed to register the type." + result.ToString());
            }

            Topic topic = participant.CreateTopic(nameof(TestOnInconsistentTopic), typeName);
            if (topic == null)
            {
                throw new ApplicationException("Failed to create the topic.");
            }

            DataReader reader = subscriber.CreateDataReader(topic);
            if (reader == null)
            {
                throw new ApplicationException("Failed to create the reader.");
            }

            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void BindRtpsUdpTransportConfig(DomainParticipant participant)
        {
            long ticks = DateTime.Now.Ticks;
            string configName = "openddsharp_rtps_interop_" + ticks.ToString();
            string instName = "internal_openddsharp_rtps_transport_" + ticks.ToString();

            TransportConfig config = TransportRegistry.Instance.CreateConfig(configName);
            TransportInst inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
            RtpsUdpInst rui = new RtpsUdpInst(inst);
            config.Insert(inst);

            TransportRegistry.Instance.BindConfig(configName, participant);
        }
    }
}
