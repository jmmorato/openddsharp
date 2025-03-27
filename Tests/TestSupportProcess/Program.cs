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
using System.Globalization;
using JsonWrapper;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;

namespace OpenDDSharp.TestSupportProcess;

internal static class Program
{
    private const string RTPS_DISCOVERY = "RtpsDiscovery";
    private const string INFOREPO_DISCOVERY = "InfoRepo";
    private const string INFOREPO_IOR = "repo.ior";
    private const int INFOREPO_DOMAIN = 23;
    private const int RTPS_DOMAIN = 42;

    private static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            throw new InvalidOperationException("Incorrect number of arguments.");
        }

        Ace.Init();

        var disc = new RtpsDiscovery(RTPS_DISCOVERY);
        ParticipantService.Instance.AddDiscovery(disc);
        ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;

        var infoRepo = new InfoRepoDiscovery(INFOREPO_DISCOVERY, "file://" + INFOREPO_IOR);
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
                    throw new InvalidOperationException("Unknown test requested." + testKind.ToString());
            }
        }
        else
        {
            throw new InvalidOperationException("Argument is not a valid test kind.");
        }

        Ace.Fini();
    }

    private static void TestOnSubscriptionLostDisconnected()
    {
        var dpf = ParticipantService.Instance.GetDomainParticipantFactory();
        var participant = dpf.CreateParticipant(INFOREPO_DOMAIN);
        if (participant == null)
        {
            throw new InvalidOperationException("Failed to create the participant.");
        }

        BindTcpTransportConfig(participant);

        var publisher = participant.CreatePublisher();
        if (publisher == null)
        {
            throw new InvalidOperationException("Failed to create the publisher.");
        }

        var support = new TestStructTypeSupport();
        var typeName = support.GetTypeName();
        var result = support.RegisterType(participant, typeName);
        if (result != ReturnCode.Ok)
        {
            throw new InvalidOperationException("Failed to register the type." + result.ToString());
        }

        var topic = participant.CreateTopic("TestOnSubscriptionLostDisconnected", typeName);
        if (topic == null)
        {
            throw new InvalidOperationException("Failed to create the topic.");
        }

        var writer = publisher.CreateDataWriter(topic);
        if (writer == null)
        {
            throw new InvalidOperationException("Failed to create the writer.");
        }

        while (true)
        {
            System.Threading.Thread.Sleep(100);
        }
    }

    private static void TestOnPublicationLostDisconnected()
    {
        var dpf = ParticipantService.Instance.GetDomainParticipantFactory();
        var participant = dpf.CreateParticipant(INFOREPO_DOMAIN);
        if (participant == null)
        {
            throw new InvalidOperationException("Failed to create the participant.");
        }

        BindTcpTransportConfig(participant);

        var subscriber = participant.CreateSubscriber();
        if (subscriber == null)
        {
            throw new InvalidOperationException("Failed to create the subscriber.");
        }

        var support = new TestStructTypeSupport();
        var typeName = support.GetTypeName();
        var result = support.RegisterType(participant, typeName);
        if (result != ReturnCode.Ok)
        {
            throw new InvalidOperationException("Failed to register the type." + result.ToString());
        }

        var topic = participant.CreateTopic("TestOnPublicationLostDisconnected", typeName);
        if (topic == null)
        {
            throw new InvalidOperationException("Failed to create the topic.");
        }

        var reader = subscriber.CreateDataReader(topic);
        if (reader == null)
        {
            throw new InvalidOperationException("Failed to create the reader.");
        }

        while (true)
        {
            System.Threading.Thread.Sleep(100);
        }
    }

    private static void TestOnInconsistentTopic()
    {
        var dpf = ParticipantService.Instance.GetDomainParticipantFactory();
        var participant = dpf.CreateParticipant(RTPS_DOMAIN);
        if (participant == null)
        {
            throw new InvalidOperationException("Failed to create the participant.");
        }

        BindRtpsUdpTransportConfig(participant);

        var subscriber = participant.CreateSubscriber();
        if (subscriber == null)
        {
            throw new InvalidOperationException("Failed to create the subscriber.");
        }

        var support = new AthleteTypeSupport();
        var typeName = support.GetTypeName();
        var result = support.RegisterType(participant, typeName);
        if (result != ReturnCode.Ok)
        {
            throw new InvalidOperationException("Failed to register the type." + result.ToString());
        }

        var topic = participant.CreateTopic(nameof(TestOnInconsistentTopic), typeName);
        if (topic == null)
        {
            throw new InvalidOperationException("Failed to create the topic.");
        }

        var reader = subscriber.CreateDataReader(topic);
        if (reader == null)
        {
            throw new InvalidOperationException("Failed to create the reader.");
        }

        while (true)
        {
            System.Threading.Thread.Sleep(100);
        }
    }

    private static void BindRtpsUdpTransportConfig(DomainParticipant participant)
    {
        var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        var configName = "openddsharp_rtps_interop_" + guid;
        var instName = "internal_openddsharp_rtps_transport_" + guid;

        var config = TransportRegistry.Instance.CreateConfig(configName);
        var inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
        var rui = new RtpsUdpInst(inst);
        config.Insert(rui);

        TransportRegistry.Instance.BindConfig(configName, participant);
    }

    private static void BindTcpTransportConfig(Entity entity)
    {
        var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        var configName = "openddsharp_tcp_" + guid;
        var instName = "internal_openddsharp_tcp_transport_" + guid;

        var config = TransportRegistry.Instance.CreateConfig(configName);
        var inst = TransportRegistry.Instance.CreateInst(instName, "tcp");
        var tcpi = new TcpInst(inst);
        config.Insert(tcpi);

        TransportRegistry.Instance.BindConfig(config, entity);
    }
}