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
using System.Linq;
using System.Text;
using System.Threading;
using CdrWrapper;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace ConsoleDemoCore
{
    internal static class Program
    {
        private static readonly string TOPIC_NAME = Guid.NewGuid().ToString();

        private static void Main()
        {
            Console.WriteLine("ACE initialization...");
            Ace.Init();

            Console.WriteLine("Get domain participant factory...");
            var dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps.ini",
                "-DCPSDebugLevel", "10", "-ORBLogFile", "LogFile.log", "-ORBDebugLevel", "10");
            if (dpf == null)
            {
                Console.Error.WriteLine("Domain participant factory could NOT be created.");
                Environment.Exit(-1);
            }

            Console.WriteLine("Create participant...");
            var participant = dpf.CreateParticipant(42);
            if (participant == null)
            {
                Console.Error.WriteLine("Domain participant could NOT be created.");
                Environment.Exit(-1);
            }

            var topic = CreateTestTopic(participant);
            if (topic == null)
            {
                Console.Error.WriteLine("Topic could not be created.");
                Environment.Exit(-1);
            }

            Console.WriteLine("Create data writer...");
            var dw = CreateTestDataWriter(participant, topic);
            if (dw == null)
            {
                Console.Error.WriteLine("DataWriter could NOT be created.");
                Environment.Exit(-1);
            }

            Console.WriteLine("Create data reader...");
            var dr = CreateTestDataReader(participant, topic);
            if (dr == null)
            {
                Console.Error.WriteLine("DataReader could NOT be created.");
                Environment.Exit(-1);
            }

            Console.WriteLine("Waiting for the subscriber...");
            var wait = WaitForSubscriptions(dw, 1, 60000);

            if (!wait)
            {
                Console.Error.WriteLine("Subscription not found.");
                Environment.Exit(-1);
            }

            Console.WriteLine("Subscription found. Writing test data...");
            var data = CreateFullStruct();
            dw.Write(data);

            Console.WriteLine("Waiting for sample...");
            var received = new List<FullStruct>();
            var sampleInfo = new List<SampleInfo>();
            var ret = dr.Take(received, sampleInfo);
            while (ret != ReturnCode.Ok)
            {
                Console.WriteLine($"No sample received. Error code: {ret}");
                Thread.Sleep(500);
                ret = dr.Take(received, sampleInfo);
            }

            Console.WriteLine("================");
            Console.WriteLine("Sample received");
            Console.WriteLine("================");

            PrintReceivedSample(received[0]);

            Console.WriteLine("Shutting down... that's enough for today.");

            var test = new FullStruct();
            dr.GetKeyValue(test, sampleInfo[0].InstanceHandle);

            participant.DeleteContainedEntities();
            dpf.DeleteParticipant(participant);
            ParticipantService.Instance.Shutdown();

            Ace.Fini();
        }

        private static FullStructDataWriter CreateTestDataWriter(DomainParticipant participant, Topic topic)
        {
            var publisher = participant.CreatePublisher();
            if (publisher == null)
            {
                Console.Error.WriteLine("Publisher could not be created.");
                return null;
            }

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
                Durability =
                {
                    Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos,
                },
            };
            var dw = publisher.CreateDataWriter(topic, dwQos);
            if (dw != null)
            {
                return new FullStructDataWriter(dw);
            }

            Console.Error.WriteLine("DataWriter could not be created.");
            return null;
        }

        private static FullStructDataReader CreateTestDataReader(DomainParticipant participant, Topic topic)
        {
            var subscriber = participant.CreateSubscriber();
            if (subscriber == null)
            {
                Console.Error.WriteLine("Subscriber could NOT be created.");
                return null;
            }

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
                Durability =
                {
                    Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos,
                },
            };
            var dr = subscriber.CreateDataReader(topic, drQos);
            if (dr != null)
            {
                return new FullStructDataReader(dr);
            }

            Console.Error.WriteLine("DataReader could NOT be created.");
            return null;

        }

        private static FullStruct CreateFullStruct()
        {
            return new FullStruct
            {
                ShortField = -1,
                LongField = -2,
                LongLongField = -3,
                UnsignedShortField = 1,
                UnsignedLongField = 2,
                UnsignedLongLongField = 3,
                BooleanField = true,
                CharField = 'C',
                WCharField = 'W',
                FloatField = 42.42f,
                DoubleField = 0.42,
                OctetField = 0x42,
                UnboundedStringField = "Unbounded string field.",
                UnboundedWStringField = "Unbounded WString field.",
                BoundedStringField = "Bounded string field.",
                BoundedWStringField = "Bounded WString field.",
                BoundedBooleanSequenceField = { true, true, false },
                UnboundedBooleanSequenceField = { true, true, false, true, true, false },
                BoundedCharSequenceField = { '1', '2', '3', '4', '5' },
                UnboundedCharSequenceField = { '1', '2', '3', '4', '5', '6' },
                BoundedWCharSequenceField = { '1', '2', '3', '4', '5' },
                UnboundedWCharSequenceField = { '1', '2', '3', '4', '5', '6' },
                BoundedOctetSequenceField = { 0x42, 0x69 },
                UnboundedOctetSequenceField = { 0x42, 0x69, 0x42, 0x69, 0x42, 0x69 },
                BoundedShortSequenceField = { 1, 2, 3, 4, 5 },
                UnboundedShortSequenceField = { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 },
                BoundedUShortSequenceField = { 1, 2, 3, 4, 5 },
                UnboundedUShortSequenceField = { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 },
                BoundedLongSequenceField = { 1, 2, 3, 4, 5 },
                UnboundedLongSequenceField = { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 },
                BoundedULongSequenceField = { 1, 2, 3, 4, 5 },
                UnboundedULongSequenceField = { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 },
                BoundedLongLongSequenceField = { 1, 2, 3, 4, 5 },
                UnboundedLongLongSequenceField = { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 },
                BoundedULongLongSequenceField = { 1, 2, 3, 4, 5 },
                UnboundedULongLongSequenceField = { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 },
                BoundedFloatSequenceField = { 0.42f, 42.42f, 1f, 2f, 3f },
                UnboundedFloatSequenceField = { 0.42f, 42.42f, 1f, 2f, 3f, 0.42f, 42.42f, 1f, 2f, 3f },
                BoundedDoubleSequenceField = { 0.42, 42.42, 1, 2, 3 },
                UnboundedDoubleSequenceField = { 0.42, 42.42, 1, 2, 3, 0.42, 42.42, 1, 2, 3 },
                BoundedStringSequenceField = { "This", "is", "the", "end." },
                BoundedWStringSequenceField = { "This", "is", "the", "end." },
                UnboundedStringSequenceField = { "This", "is", "the", "end.", "This", "is", "the", "end." },
                UnboundedWStringSequenceField = { "This", "is", "the", "end.", "This", "is", "the", "end." },
                NestedStructField = { Id = 1, Message = "This is the end." },
                BoundedStructSequenceField =
                {
                    new NestedStruct { Id = 1, Message = "This is the end." },
                    new NestedStruct { Id = 2, Message = "my only friend, the end." },
                },
                UnboundedStructSequenceField =
                {
                    new NestedStruct { Id = 1, Message = "This is the end." },
                    new NestedStruct { Id = 2, Message = "my only friend, the end." },
                },
                TestEnumField = TestEnum.ENUM12,
                BoundedEnumSequenceField =
                {
                    TestEnum.ENUM1,
                    TestEnum.ENUM2,
                    TestEnum.ENUM3,
                    TestEnum.ENUM4,
                    TestEnum.ENUM5,
                },
                UnboundedEnumSequenceField =
                {
                    TestEnum.ENUM1, TestEnum.ENUM2, TestEnum.ENUM3, TestEnum.ENUM4, TestEnum.ENUM5, TestEnum.ENUM6,
                    TestEnum.ENUM7, TestEnum.ENUM8, TestEnum.ENUM9, TestEnum.ENUM10, TestEnum.ENUM11, TestEnum.ENUM12
                },
                ShortArrayField = new short[] { 1, -2, 3, -4, 5 },
                UnsignedShortArrayField = new ushort[] { 1, 2, 3, 4, 5 },
                LongArrayField = new[] { 1, -2, 3, -4, 5 },
                UnsignedLongArrayField = new uint[] { 1, 2, 3, 4, 5 },
                LongLongArrayField = new long[] { 1, -2, 3, -4, 5 },
                UnsignedLongLongArrayField = new ulong[] { 1, 2, 3, 4, 5 },
                CharArrayField = new[] { 'A', 'B', 'C', 'D', 'E' },
                WCharArrayField = new[] { 'A', 'B', 'C', 'D', 'E' },
                BooleanArrayField = new[] { true, true, false, true, true },
                OctetArrayField = new byte[] { 0x42, 0x42, 0x69, 0x42, 0x42 },
                FloatArrayField = new[] { 0.42f, 0.4242f, 1f, 2f, 3f },
                DoubleArrayField = new[] { 0.42, 0.4242, 1, 2, 3 },
                StringArrayField = new[] { "This", "is", "the", "end", "my only friend, the end." },
                WStringArrayField = new[] { "This", "is", "the", "end", "my only friend, the end." },
                EnumArrayField = new[]
                {
                    TestEnum.ENUM1,
                    TestEnum.ENUM2,
                    TestEnum.ENUM3,
                    TestEnum.ENUM4,
                    TestEnum.ENUM5,
                },
                StructArrayField = new[]
                {
                    new NestedStruct { Id = 1, Message = "This is the end." },
                    new NestedStruct { Id = 2, Message = "This is the end." },
                    new NestedStruct { Id = 3, Message = "This is the end." },
                    new NestedStruct { Id = 4, Message = "This is the end." },
                    new NestedStruct { Id = 5, Message = "This is the end." },
                },
                BooleanMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { true, false },
                        new[] { true, false },
                        new[] { true, false },
                        new[] { true, false },
                    },
                    new[]
                    {
                        new[] { true, false },
                        new[] { true, false },
                        new[] { true, false },
                        new[] { true, false },
                    },
                    new[]
                    {
                        new[] { true, false },
                        new[] { true, false },
                        new[] { true, false },
                        new[] { true, false },
                    },
                },
                CharMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { '1', '2' },
                        new[] { '3', '4' },
                        new[] { '5', '6' },
                        new[] { '7', '8' },
                    },
                    new[]
                    {
                        new[] { '9', '0' },
                        new[] { '1', '2' },
                        new[] { '3', '4' },
                        new[] { '5', '6' },
                    },
                    new[]
                    {
                        new[] { '7', '8' },
                        new[] { '9', '0' },
                        new[] { '1', '2' },
                        new[] { '3', '4' },
                    },
                },
                WCharMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { '1', '2' },
                        new[] { '3', '4' },
                        new[] { '5', '6' },
                        new[] { '7', '8' },
                    },
                    new[]
                    {
                        new[] { '9', '0' },
                        new[] { '1', '2' },
                        new[] { '3', '4' },
                        new[] { '5', '6' },
                    },
                    new[]
                    {
                        new[] { '7', '8' },
                        new[] { '9', '0' },
                        new[] { '1', '2' },
                        new[] { '3', '4' },
                    },
                },
                OctetMultiArrayField = new[]
                {
                    new[]
                    {
                        new byte[] { 01, 02 },
                        new byte[] { 03, 04 },
                        new byte[] { 05, 06 },
                        new byte[] { 07, 08 },
                    },
                    new[]
                    {
                        new byte[] { 09, 10 },
                        new byte[] { 11, 12 },
                        new byte[] { 13, 14 },
                        new byte[] { 15, 16 },
                    },
                    new[]
                    {
                        new byte[] { 17, 18 },
                        new byte[] { 19, 20 },
                        new byte[] { 21, 22 },
                        new byte[] { 23, 24 },
                    },
                },
                ShortMultiArrayField = new[]
                {
                    new[]
                    {
                        new short[] { 01, 02 },
                        new short[] { 03, 04 },
                        new short[] { 05, 06 },
                        new short[] { 07, 08 },
                    },
                    new[]
                    {
                        new short[] { 09, 10 },
                        new short[] { 11, 12 },
                        new short[] { 13, 14 },
                        new short[] { 15, 16 },
                    },
                    new[]
                    {
                        new short[] { 17, 18 },
                        new short[] { 19, 20 },
                        new short[] { 21, 22 },
                        new short[] { 23, 24 },
                    },
                },
                UnsignedShortMultiArrayField = new[]
                {
                    new[]
                    {
                        new ushort[] { 01, 02 },
                        new ushort[] { 03, 04 },
                        new ushort[] { 05, 06 },
                        new ushort[] { 07, 08 },
                    },
                    new[]
                    {
                        new ushort[] { 09, 10 },
                        new ushort[] { 11, 12 },
                        new ushort[] { 13, 14 },
                        new ushort[] { 15, 16 },
                    },
                    new[]
                    {
                        new ushort[] { 17, 18 },
                        new ushort[] { 19, 20 },
                        new ushort[] { 21, 22 },
                        new ushort[] { 23, 24 },
                    },
                },
                LongMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { -01, 02 },
                        new[] { -03, 04 },
                        new[] { -05, 06 },
                        new[] { -07, 08 },
                    },
                    new[]
                    {
                        new[] { -09, 10 },
                        new[] { -11, 12 },
                        new[] { -13, 14 },
                        new[] { -15, 16 },
                    },
                    new[]
                    {
                        new[] { -17, 18 },
                        new[] { -19, 20 },
                        new[] { -21, 22 },
                        new[] { -23, 24 },
                    },
                },
                UnsignedLongMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { 25U, 26U },
                        new[] { 27U, 28U },
                        new[] { 29U, 30U },
                        new[] { 31U, 32U },
                    },
                    new[]
                    {
                        new[] { 33U, 34U },
                        new[] { 35U, 36U },
                        new[] { 37U, 38U },
                        new[] { 39U, 40U },
                    },
                    new[]
                    {
                        new[] { 41U, 42U },
                        new[] { 43U, 44U },
                        new[] { 45U, 46U },
                        new[] { 47U, 48U },
                    },
                },
                LongLongMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { -25L, -26L },
                        new[] { -27L, -28L },
                        new[] { -29L, -30L },
                        new[] { -31L, -32L },
                    },
                    new[]
                    {
                        new[] { -33L, -34L },
                        new[] { -35L, -36L },
                        new[] { -37L, -38L },
                        new[] { -39L, -40L },
                    },
                    new[]
                    {
                        new[] { -41L, -42L },
                        new[] { -43L, -44L },
                        new[] { -45L, -46L },
                        new[] { -47L, -48L },
                    },
                },
                UnsignedLongLongMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { 49UL, 50UL },
                        new[] { 51UL, 52UL },
                        new[] { 53UL, 54UL },
                        new[] { 55UL, 56UL },
                    },
                    new[]
                    {
                        new[] { 57UL, 58UL },
                        new[] { 59UL, 60UL },
                        new[] { 61UL, 62UL },
                        new[] { 63UL, 64UL },
                    },
                    new[]
                    {
                        new[] { 65UL, 66UL },
                        new[] { 67UL, 68UL },
                        new[] { 69UL, 70UL },
                        new[] { 71UL, 72UL },
                    },
                },
                FloatMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { 01.01f, 02.02f },
                        new[] { 03.03f, 04.04f },
                        new[] { 05.05f, 06.06f },
                        new[] { 07.07f, 08.08f },
                    },
                    new[]
                    {
                        new[] { 09.09f, 10.10f },
                        new[] { 11.11f, 12.12f },
                        new[] { 13.13f, 14.14f },
                        new[] { 15.15f, 16.16f },
                    },
                    new[]
                    {
                        new[] { 17.17f, 18.18f },
                        new[] { 19.19f, 20.20f },
                        new[] { 21.21f, 22.22f },
                        new[] { 23.23f, 24.24f },
                    },
                },
                DoubleMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { 01.01, 02.02 },
                        new[] { 03.03, 04.04 },
                        new[] { 05.05, 06.06 },
                        new[] { 07.07, 08.08 },
                    },
                    new[]
                    {
                        new[] { 09.09, 10.10 },
                        new[] { 11.11, 12.12 },
                        new[] { 13.13, 14.14 },
                        new[] { 15.15, 16.16 },
                    },
                    new[]
                    {
                        new[] { 17.17, 18.18 },
                        new[] { 19.19, 20.20 },
                        new[] { 21.21, 22.22 },
                        new[] { 23.23, 24.24 },
                    },
                },
                StructMultiArrayField = new[]
                {
                    new[]
                    {
                        new[]
                        {
                            new NestedStruct{ Id = 1, Message = "01" },
                            new NestedStruct{ Id = 2, Message = "02" },
                        },
                        new[]
                        {
                            new NestedStruct { Id = 3, Message = "03" },
                            new NestedStruct { Id = 4, Message = "04" },
                        },
                        new[]
                        {
                            new NestedStruct { Id = 5, Message = "05" },
                            new NestedStruct { Id = 6, Message = "06" },
                        },
                        new[]
                        {
                            new NestedStruct { Id = 7, Message = "07" },
                            new NestedStruct { Id = 8, Message = "08" },
                        },
                    },
                    new[]
                    {
                        new[]
                        {
                            new NestedStruct { Id = 9, Message = "09" },
                            new NestedStruct { Id = 10, Message = "10" },
                        },
                        new[]
                        {
                            new NestedStruct { Id = 11, Message = "11" },
                            new NestedStruct { Id = 12, Message = "12" },
                        },
                        new[]
                        {
                            new NestedStruct { Id = 13, Message = "13" },
                            new NestedStruct { Id = 14, Message = "14" },
                        },
                        new[]
                        {
                            new NestedStruct { Id = 15, Message = "15" },
                            new NestedStruct{ Id = 16, Message = "16" },
                        },
                    },
                    new[]
                    {
                        new[]
                        {
                            new NestedStruct{ Id = 17, Message = "17" },
                            new NestedStruct{ Id = 18, Message = "18" },
                        },
                        new[]
                        {
                            new NestedStruct{ Id = 19, Message = "19" },
                            new NestedStruct{ Id = 20, Message = "20" },
                        },
                        new[]
                        {
                            new NestedStruct{ Id = 21, Message = "21" },
                            new NestedStruct{ Id = 22, Message = "22" },
                        },
                        new[]
                        {
                            new NestedStruct{ Id = 23, Message = "23" },
                            new NestedStruct{ Id = 24, Message = "24" },
                        },
                    },
                },
                StringMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { "01", "02" },
                        new[] { "03", "04" },
                        new[] { "05", "06" },
                        new[] { "07", "08" },
                    },
                    new[]
                    {
                        new[] { "09", "10" },
                        new[] { "11", "12" },
                        new[] { "13", "14" },
                        new[] { "15", "16" },
                    },
                    new[]
                    {
                        new[] { "17", "18" },
                        new[] { "19", "20" },
                        new[] { "21", "22" },
                        new[] { "23", "24" },
                    },
                },
                WStringMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { "01", "02" },
                        new[] { "03", "04" },
                        new[] { "05", "06" },
                        new[] { "07", "08" },
                    },
                    new[]
                    {
                        new[] { "09", "10" },
                        new[] { "11", "12" },
                        new[] { "13", "14" },
                        new[] { "15", "16" },
                    },
                    new[]
                    {
                        new[] { "17", "18" },
                        new[] { "19", "20" },
                        new[] { "21", "22" },
                        new[] { "23", "24" },
                    },
                },
                EnumMultiArrayField = new[]
                {
                    new[]
                    {
                        new[] { TestEnum.ENUM1, TestEnum.ENUM2 },
                        new[] { TestEnum.ENUM3, TestEnum.ENUM4 },
                        new[] { TestEnum.ENUM5, TestEnum.ENUM6 },
                        new[] { TestEnum.ENUM7, TestEnum.ENUM8 },
                    },
                    new[]
                    {
                        new[] { TestEnum.ENUM9, TestEnum.ENUM10 },
                        new[] { TestEnum.ENUM11, TestEnum.ENUM12 },
                        new[] { TestEnum.ENUM1, TestEnum.ENUM2 },
                        new[] { TestEnum.ENUM3, TestEnum.ENUM4 },
                    },
                    new[]
                    {
                        new[] { TestEnum.ENUM5, TestEnum.ENUM6 },
                        new[] { TestEnum.ENUM7, TestEnum.ENUM8 },
                        new[] { TestEnum.ENUM9, TestEnum.ENUM10 },
                        new[] { TestEnum.ENUM11, TestEnum.ENUM12 },
                    },
                },
            };
        }

        private static void PrintReceivedSample(FullStruct received)
        {
            PrintStructField(nameof(received.ShortField), received.ShortField);
            PrintStructField(nameof(received.LongField), received.LongField);
            PrintStructField(nameof(received.LongLongField), received.LongLongField);
            PrintStructField(nameof(received.UnsignedShortField), received.UnsignedShortField);
            PrintStructField(nameof(received.UnsignedLongField), received.UnsignedLongField);
            PrintStructField(nameof(received.UnsignedLongLongField), received.UnsignedLongLongField);
            PrintStructField(nameof(received.CharField), received.CharField);
            PrintStructField(nameof(received.WCharField), received.WCharField);
            PrintStructField(nameof(received.BooleanField), received.BooleanField);
            PrintStructField(nameof(received.OctetField), received.OctetField);
            PrintStructField(nameof(received.FloatField), received.FloatField);
            PrintStructField(nameof(received.DoubleField), received.DoubleField);
            PrintStructField(nameof(received.UnboundedStringField), received.UnboundedStringField);
            PrintStructField(nameof(received.UnboundedWStringField), received.UnboundedWStringField);
            PrintStructField(nameof(received.BoundedStringField), received.BoundedStringField);
            PrintStructField(nameof(received.BoundedWStringField), received.BoundedWStringField);
            PrintStructField(nameof(received.BoundedBooleanSequenceField), string.Join(", ", received.BoundedBooleanSequenceField));
            PrintStructField(nameof(received.UnboundedBooleanSequenceField), string.Join(", ", received.UnboundedBooleanSequenceField));
            PrintStructField(nameof(received.BoundedCharSequenceField), string.Join(", ", received.BoundedCharSequenceField));
            PrintStructField(nameof(received.UnboundedCharSequenceField), string.Join(", ", received.UnboundedCharSequenceField));
            PrintStructField(nameof(received.BoundedWCharSequenceField), string.Join(", ", received.BoundedWCharSequenceField));
            PrintStructField(nameof(received.UnboundedWCharSequenceField), string.Join(", ", received.UnboundedWCharSequenceField));
            PrintStructField(nameof(received.BoundedOctetSequenceField), string.Join(", ", received.BoundedOctetSequenceField));
            PrintStructField(nameof(received.UnboundedOctetSequenceField), string.Join(", ", received.UnboundedOctetSequenceField));
            PrintStructField(nameof(received.BoundedShortSequenceField), string.Join(", ", received.BoundedShortSequenceField));
            PrintStructField(nameof(received.UnboundedShortSequenceField), string.Join(", ", received.UnboundedShortSequenceField));
            PrintStructField(nameof(received.BoundedUShortSequenceField), string.Join(", ", received.BoundedUShortSequenceField));
            PrintStructField(nameof(received.UnboundedUShortSequenceField), string.Join(", ", received.UnboundedUShortSequenceField));
            PrintStructField(nameof(received.BoundedLongSequenceField), string.Join(", ", received.BoundedLongSequenceField));
            PrintStructField(nameof(received.UnboundedLongSequenceField), string.Join(", ", received.UnboundedLongSequenceField));
            PrintStructField(nameof(received.BoundedULongSequenceField), string.Join(", ", received.BoundedULongSequenceField));
            PrintStructField(nameof(received.UnboundedULongSequenceField), string.Join(", ", received.UnboundedULongSequenceField));
            PrintStructField(nameof(received.BoundedLongLongSequenceField), string.Join(", ", received.BoundedLongLongSequenceField));
            PrintStructField(nameof(received.UnboundedLongLongSequenceField), string.Join(", ", received.UnboundedLongLongSequenceField));
            PrintStructField(nameof(received.BoundedULongLongSequenceField), string.Join(", ", received.BoundedULongLongSequenceField));
            PrintStructField(nameof(received.UnboundedULongLongSequenceField), string.Join(", ", received.UnboundedULongLongSequenceField));
            PrintStructField(nameof(received.BoundedFloatSequenceField), string.Join(", ", received.BoundedFloatSequenceField));
            PrintStructField(nameof(received.UnboundedFloatSequenceField), string.Join(", ", received.UnboundedFloatSequenceField));
            PrintStructField(nameof(received.BoundedDoubleSequenceField), string.Join(", ", received.BoundedDoubleSequenceField));
            PrintStructField(nameof(received.UnboundedDoubleSequenceField), string.Join(", ", received.UnboundedDoubleSequenceField));
            PrintStructField(nameof(received.BoundedStringSequenceField), string.Join(" ", received.BoundedStringSequenceField));
            PrintStructField(nameof(received.BoundedWStringSequenceField), string.Join(" ", received.BoundedWStringSequenceField));
            PrintStructField(nameof(received.UnboundedStringSequenceField), string.Join(" ", received.UnboundedStringSequenceField));
            PrintStructField(nameof(received.UnboundedWStringSequenceField), string.Join(" ", received.UnboundedWStringSequenceField));
            PrintStructField(nameof(received.NestedStructField), $"Id: {received.NestedStructField.Id} Message: {received.NestedStructField.Message}");
            PrintStructField(nameof(received.BoundedStructSequenceField), string.Join(" ", received.BoundedStructSequenceField?.Select(s => $"ID: {s.Id} MESSAGE: {s.Message}") ?? Array.Empty<string>()));
            PrintStructField(nameof(received.UnboundedStructSequenceField), string.Join(" ", received.UnboundedStructSequenceField?.Select(s => $"ID: {s.Id} MESSAGE: {s.Message}") ?? Array.Empty<string>()));
            PrintStructField(nameof(received.TestEnumField), received.TestEnumField);
            PrintStructField(nameof(received.BoundedEnumSequenceField), string.Join(", ", received.BoundedEnumSequenceField));
            PrintStructField(nameof(received.UnboundedEnumSequenceField), string.Join(", ", received.UnboundedEnumSequenceField));
            PrintStructField(nameof(received.ShortArrayField), string.Join(", ", received.ShortArrayField));
            PrintStructField(nameof(received.UnsignedShortArrayField), string.Join(", ", received.UnsignedShortArrayField));
            PrintStructField(nameof(received.LongArrayField), string.Join(", ", received.LongArrayField));
            PrintStructField(nameof(received.UnsignedLongArrayField), string.Join(", ", received.UnsignedLongArrayField));
            PrintStructField(nameof(received.LongLongArrayField), string.Join(", ", received.LongLongArrayField));
            PrintStructField(nameof(received.UnsignedLongLongArrayField), string.Join(", ", received.UnsignedLongLongArrayField));
            PrintStructField(nameof(received.CharArrayField), string.Join(", ", received.CharArrayField));
            PrintStructField(nameof(received.WCharArrayField), string.Join(", ", received.WCharArrayField));
            PrintStructField(nameof(received.BooleanArrayField), string.Join(", ", received.BooleanArrayField));
            PrintStructField(nameof(received.OctetArrayField), string.Join(", ", received.OctetArrayField));
            PrintStructField(nameof(received.FloatArrayField), string.Join(", ", received.FloatArrayField));
            PrintStructField(nameof(received.DoubleArrayField), string.Join(", ", received.DoubleArrayField));
            PrintStructField(nameof(received.StringArrayField), string.Join(" ", received.StringArrayField));
            PrintStructField(nameof(received.WStringArrayField), string.Join(" ", received.WStringArrayField));
            PrintStructField(nameof(received.EnumArrayField), string.Join(" ", received.EnumArrayField));
            PrintStructField(nameof(received.StructArrayField), string.Join(" ", received.StructArrayField.Select(s => $"ID: {s.Id} MESSAGE: {s.Message}")));
            PrintStructField(nameof(received.ShortMultiArrayField), PrintMultiArray(received.ShortMultiArrayField));
            PrintStructField(nameof(received.UnsignedShortMultiArrayField), PrintMultiArray(received.UnsignedShortMultiArrayField));
            PrintStructField(nameof(received.LongMultiArrayField), PrintMultiArray(received.LongMultiArrayField));
            PrintStructField(nameof(received.UnsignedLongMultiArrayField), PrintMultiArray(received.UnsignedLongMultiArrayField));
            PrintStructField(nameof(received.LongLongMultiArrayField), PrintMultiArray(received.LongLongMultiArrayField));
            PrintStructField(nameof(received.UnsignedLongLongMultiArrayField), PrintMultiArray(received.UnsignedLongLongMultiArrayField));
            PrintStructField(nameof(received.FloatMultiArrayField), PrintMultiArray(received.FloatMultiArrayField));
            PrintStructField(nameof(received.DoubleMultiArrayField), PrintMultiArray(received.DoubleMultiArrayField));
            PrintStructField(nameof(received.BooleanMultiArrayField), PrintMultiArray(received.BooleanMultiArrayField));
            PrintStructField(nameof(received.OctetMultiArrayField), PrintMultiArray(received.OctetMultiArrayField));
            PrintStructField(nameof(received.EnumMultiArrayField), PrintMultiArray(received.EnumMultiArrayField));
            PrintStructField(nameof(received.StructMultiArrayField), PrintStructMultiArray(received.StructMultiArrayField));
            PrintStructField(nameof(received.StringMultiArrayField), PrintMultiArray(received.StringMultiArrayField));
            PrintStructField(nameof(received.WStringMultiArrayField), PrintMultiArray(received.WStringMultiArrayField));
            PrintStructField(nameof(received.CharMultiArrayField), PrintMultiArray(received.CharMultiArrayField));
            PrintStructField(nameof(received.WCharMultiArrayField), PrintMultiArray(received.WCharMultiArrayField));
            Console.ResetColor();
        }

        private static Topic CreateTestTopic(DomainParticipant participant)
        {
            var typeSupport = new FullStructTypeSupport();
            var typeName = typeSupport.GetTypeName();
            var ret = typeSupport.RegisterType(participant, typeName);
            if (ret != ReturnCode.Ok)
            {
                Console.Error.WriteLine("FullStructTypeSupport could not be registered: " + ret);
                return null;
            }

            var topic = participant.CreateTopic(TOPIC_NAME, typeName);
            if (topic != null)
            {
                return topic;
            }

            Console.Error.WriteLine("Topic could NOT be created.");
            return null;

        }

        private static bool WaitForSubscriptions(DataWriter writer, int subscriptionsCount, int milliseconds)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            PublicationMatchedStatus status = default;
            writer.GetPublicationMatchedStatus(ref status);
            var count = milliseconds / 100;
            while (status.CurrentCount != subscriptionsCount && count > 0)
            {
                Thread.Sleep(100);
                writer.GetPublicationMatchedStatus(ref status);
                count--;
            }

            return count != 0 || status.CurrentCount == subscriptionsCount;
        }

        private static void PrintStructField(string fieldName, object fieldValue)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(fieldName + ": ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(fieldValue);
            Console.WriteLine();
        }

        private static string PrintMultiArray<T>(T[][][] data)
        {
            var ret = new StringBuilder();
            foreach (var t in data)
            {
                foreach (var t1 in t)
                {
                    foreach (var t2 in t1)
                    {
                        if (ret.Length > 0)
                        {
                            ret.Append(", ");
                        }

                        ret.Append(t2);
                    }
                }
            }

            return ret.ToString();
        }

        private static string PrintStructMultiArray(NestedStruct[][][] data)
        {
            var ret = new StringBuilder();
            foreach (var t in data)
            {
                foreach (var t1 in t)
                {
                    foreach (var t2 in t1)
                    {
                        if (ret.Length > 0)
                        {
                            ret.Append(", ");
                        }

                        ret.Append($"ID: {t2.Id} MESSAGE: {t2.Message}");
                    }
                }
            }

            return ret.ToString();
        }
    }
}
