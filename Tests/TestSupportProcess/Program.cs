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
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;

namespace TestSupportProcess
{
    class Program
    {
        private const string RTPS_DISCOVERY = "RtpsDiscovery";
        private const string INFOREPO_DISCOVERY = "InfoRepo";
        private const string INFOREPO_IOR = "repo.ior";
        private const int INFOREPO_DOMAIN = 23;
        private const int RTPS_DOMAIN = 42;

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ApplicationException("Incorrect number of arguments.");
            }

            OpenDDSharp.Ace.Init();

            RtpsDiscovery disc = new RtpsDiscovery(RTPS_DISCOVERY);
            ParticipantService.Instance.AddDiscovery(disc);
            ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;

            InfoRepoDiscovery infoRepo = new InfoRepoDiscovery(INFOREPO_DISCOVERY, "file://" + INFOREPO_IOR);
            ParticipantService.Instance.AddDiscovery(infoRepo);
            ParticipantService.Instance.SetRepoDomain(INFOREPO_DOMAIN, INFOREPO_DISCOVERY);            

            if (Enum.TryParse(args[0], out SupportTestKind testKind))
            {
                switch (testKind)
                {
                    case SupportTestKind.InconsistentTopicTest:
                        TestOnInconsistentTopic();
                        break;
                    case SupportTestKind.PublicationDisconnectedTest:
                    case SupportTestKind.PublicationLostTest:
                        TestOnPublicationLostDisconnected();
                        break;
                    case SupportTestKind.SubscriptionDisconnectedTest:
                    case SupportTestKind.SubscriptionLostTest:
                        TestOnSubscriptionLostDisconnected();
                        break;
                    default:
                        throw new ApplicationException("Unkwnon test requested." + testKind.ToString());
                }
            }
            else
            {
                throw new ApplicationException("Argument is not a valid test kind.");
            }

            OpenDDSharp.Ace.Fini();
        }

        private static void TestOnSubscriptionLostDisconnected()
        {
            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory();
            DomainParticipant participant = dpf.CreateParticipant(INFOREPO_DOMAIN);
            if (participant == null)
            {
                throw new ApplicationException("Failed to create the participant.");
            }
            BindTcpTransportConfig(participant);

            Publisher publisher = participant.CreatePublisher();
            if (publisher == null)
            {
                throw new ApplicationException("Failed to create the publisher.");
            }

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(participant, typeName);
            if (result != ReturnCode.Ok)
            {
                throw new ApplicationException("Failed to register the type." + result.ToString());
            }

            Topic topic = participant.CreateTopic("TestOnSubscriptionLostDisconnected", typeName);
            if (topic == null)
            {
                throw new ApplicationException("Failed to create the topic.");
            }

            DataWriter writer = publisher.CreateDataWriter(topic);
            if (writer == null)
            {
                throw new ApplicationException("Failed to create the writer.");
            }

            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void TestOnPublicationLostDisconnected()
        {
            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory();
            DomainParticipant participant = dpf.CreateParticipant(INFOREPO_DOMAIN);
            if (participant == null)
            {
                throw new ApplicationException("Failed to create the participant.");
            }
            BindTcpTransportConfig(participant);            

            Subscriber subscriber = participant.CreateSubscriber();
            if (subscriber == null)
            {
                throw new ApplicationException("Failed to create the subscriber.");
            }

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(participant, typeName);            
            if (result != ReturnCode.Ok)
            {
                throw new ApplicationException("Failed to register the type." + result.ToString());
            }

            Topic topic = participant.CreateTopic("TestOnPublicationLostDisconnected", typeName);
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
            DomainParticipant participant = dpf.CreateParticipant(RTPS_DOMAIN);
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
            string guid = Guid.NewGuid().ToString("N");
            string configName = "openddsharp_rtps_interop_" + guid;
            string instName = "internal_openddsharp_rtps_transport_" + guid;

            TransportConfig config = TransportRegistry.Instance.CreateConfig(configName);
            TransportInst inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
            RtpsUdpInst rui = new RtpsUdpInst(inst);
            config.Insert(inst);

            TransportRegistry.Instance.BindConfig(configName, participant);
        }

        private static TcpInst BindTcpTransportConfig(Entity entity)
        {
            string guid = Guid.NewGuid().ToString("N");
            string configName = "openddsharp_tcp_" + guid;
            string instName = "internal_openddsharp_tcp_transport_" + guid;

            TransportConfig config = TransportRegistry.Instance.CreateConfig(configName);
            TransportInst inst = TransportRegistry.Instance.CreateInst(instName, "tcp");
            TcpInst tcpi = new TcpInst(inst)
            {
                PublicAddress = $"127.0.0.1:0",
                LocalAddress = $"127.0.0.1:0",
            };

            config.Insert(inst);

            TransportRegistry.Instance.BindConfig(config, entity);

            return tcpi;
        }
    }
}
