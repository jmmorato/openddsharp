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
using System.Threading;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using Test;

namespace ConsoleDemoCore
{
    internal static class Program
    {
        private static void Main()
        {
            Ace.Init();

            var dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps.ini", "-DCPSDebugLevel", "10", "-ORBLogFile", "LogFile.log", "-ORBDebugLevel", "10");
            if (dpf == null)
            {
                Console.Error.WriteLine("Domain participant factory could NOT be created.");
                Environment.Exit(-1);
            }

            var participant = dpf.CreateParticipant(42);
            if (participant == null)
            {
                Console.Error.WriteLine("Domain participant could NOT be created.");
                Environment.Exit(-1);
            }

            var dw = CreateTestDataWriter(participant);
            if (dw == null)
            {
                Console.Error.WriteLine("DataWriter could NOT be created.");
                Environment.Exit(-1);
            }
            var dr = CreateTestDataReader(participant);
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
            var data = CreateTestStruct();
            dw.Write(data);

            Console.WriteLine("Waiting for sample...");
            var received = new List<TestStruct>();
            var sampleInfo = new List<SampleInfo>();
            var ret = dr.Take(received, sampleInfo);
            while (ret != ReturnCode.Ok)
            {
                Thread.Sleep(100);
                ret = dr.Take(received, sampleInfo);
            }

            Console.WriteLine("================");
            Console.WriteLine("Sample received");
            Console.WriteLine("================");

            PrintReceivedSample(received[0]);

            Console.WriteLine("Shutting down... that's enough for today.");

            participant.DeleteContainedEntities();
            dpf.DeleteParticipant(participant);
            ParticipantService.Instance.Shutdown();

            Ace.Fini();
        }

        private static TestStructDataWriter CreateTestDataWriter(DomainParticipant participant)
        {
            var publisher = participant.CreatePublisher();
            if (publisher == null)
            {
                Console.Error.WriteLine("Publisher could not be created.");
                return null;
            }

            var topic = CreateTestTopic(participant);
            if (topic == null)
            {
                Console.Error.WriteLine("Topic could not be created.");
                return null;
            }

            var qos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var dw = publisher.CreateDataWriter(topic, qos);
            if (dw != null)
            {
                return new TestStructDataWriter(dw);
            }

            Console.Error.WriteLine("DataWriter could not be created.");
            return null;
        }

        private static TestStructDataReader CreateTestDataReader(DomainParticipant participant)
        {
            var subscriber = participant.CreateSubscriber();
            if (subscriber == null)
            {
                Console.Error.WriteLine("Subscriber could NOT be created.");
                return null;
            }

            var topic = CreateTestTopic(participant);
            if (topic == null)
            {
                Console.Error.WriteLine("Topic could NOT be created.");
                return null;
            }

            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var dr = subscriber.CreateDataReader(topic, qos);
            if (dr != null)
            {
                return new TestStructDataReader(dr);
            }

            Console.Error.WriteLine("DataReader could NOT be created.");
            return null;

        }

        private static TestStruct CreateTestStruct()
        {
            return new TestStruct
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
                ShortMultiArrayField = new short[,,]
                {
                    {
                        { -01, -02 },
                        { -03, -04 },
                        { -05, -06 },
                        { -07, -08 },
                    },
                    {
                        { -09, -10 },
                        { -11, -12 },
                        { -13, -14 },
                        { -15, -16 },
                    },
                    {
                        { -17, -18 },
                        { -19, -20 },
                        { -21, -22 },
                        { -23, -24 },
                    }
                },
                UnsignedShortMultiArrayField = new ushort[,,]
                {
                    {
                        { 01, 02 },
                        { 03, 04 },
                        { 05, 06 },
                        { 07, 08 },
                    },
                    {
                        { 09, 10 },
                        { 11, 12 },
                        { 13, 14 },
                        { 15, 16 },
                    },
                    {
                        { 17, 18 },
                        { 19, 20 },
                        { 21, 22 },
                        { 23, 24 },
                    }
                },
                LongMultiArrayField = new[,,]
                {
                    {
                        { -01, 02 },
                        { -03, 04 },
                        { -05, 06 },
                        { -07, 08 },
                    },
                    {
                        { -09, 10 },
                        { -11, 12 },
                        { -13, 14 },
                        { -15, 16 },
                    },
                    {
                        { -17, 18 },
                        { -19, 20 },
                        { -21, 22 },
                        { -23, 24 },
                    },
                },
                UnsignedLongMultiArrayField = new[,,]
                {
                    {
                        { 25U, 26U },
                        { 27U, 28U },
                        { 29U, 30U },
                        { 31U, 32U },
                    },
                    {
                        { 33U, 34U },
                        { 35U, 36U },
                        { 37U, 38U },
                        { 39U, 40U },
                    },
                    {
                        { 41U, 42U },
                        { 43U, 44U },
                        { 45U, 46U },
                        { 47U, 48U },
                    },
                },
                LongLongMultiArrayField = new[,,]
                {
                    {
                        { -25L, -26L },
                        { -27L, -28L },
                        { -29L, -30L },
                        { -31L, -32L },
                    },
                    {
                        { -33L, -34L },
                        { -35L, -36L },
                        { -37L, -38L },
                        { -39L, -40L },
                    },
                    {
                        { -41L, -42L },
                        { -43L, -44L },
                        { -45L, -46L },
                        { -47L, -48L },
                    },
                },
                UnsignedLongLongMultiArrayField = new[,,]
                {
                    {
                        { 49UL, 50UL },
                        { 51UL, 52UL },
                        { 53UL, 54UL },
                        { 55UL, 56UL },
                    },
                    {
                        { 57UL, 58UL },
                        { 59UL, 60UL },
                        { 61UL, 62UL },
                        { 63UL, 64UL },
                    },
                    {
                        { 65UL, 66UL },
                        { 67UL, 68UL },
                        { 69UL, 70UL },
                        { 71UL, 72UL },
                    },
                },
                FloatMultiArrayField = new[,,]
                {
                    {
                        { 01.01f, 02.02f },
                        { 03.03f, 04.04f },
                        { 05.05f, 06.06f },
                        { 07.07f, 08.08f },
                    },
                    {
                        { 09.09f, 10.10f },
                        { 11.11f, 12.12f },
                        { 13.13f, 14.14f },
                        { 15.15f, 16.16f },
                    },
                    {
                        { 17.17f, 18.18f },
                        { 19.19f, 20.20f },
                        { 21.21f, 22.22f },
                        { 23.23f, 24.24f },
                    },
                },
                DoubleMultiArrayField = new[,,]
                {
                    {
                        { 01.01, 02.02 },
                        { 03.03, 04.04 },
                        { 05.05, 06.06 },
                        { 07.07, 08.08 },
                    },
                    {
                        { 09.09, 10.10 },
                        { 11.11, 12.12 },
                        { 13.13, 14.14 },
                        { 15.15, 16.16 },
                    },
                    {
                        { 17.17, 18.18 },
                        { 19.19, 20.20 },
                        { 21.21, 22.22 },
                        { 23.23, 24.24 },
                    },
                },
                BooleanMultiArrayField = new[,,]
                {
                    {
                        { true, false },
                        { true, false },
                        { true, false },
                        { true, false },
                    },
                    {
                        { true, false },
                        { true, false },
                        { true, false },
                        { true, false },
                    },
                    {
                        { true, false },
                        { true, false },
                        { true, false },
                        { true, false },
                    },
                },
                OctetMultiArrayField = new byte[,,]
                {
                    {
                        { 01, 02 },
                        { 03, 04 },
                        { 05, 06 },
                        { 07, 08 },
                    },
                    {
                        { 09, 10 },
                        { 11, 12 },
                        { 13, 14 },
                        { 15, 16 },
                    },
                    {
                        { 17, 18 },
                        { 19, 20 },
                        { 21, 22 },
                        { 23, 24 },
                    },
                },
                EnumMultiArrayField = new[,,]
                {
                    {
                        { TestEnum.ENUM1, TestEnum.ENUM2 },
                        { TestEnum.ENUM3, TestEnum.ENUM4 },
                        { TestEnum.ENUM5, TestEnum.ENUM6 },
                        { TestEnum.ENUM7, TestEnum.ENUM8 },
                    },
                    {
                        { TestEnum.ENUM9, TestEnum.ENUM10 },
                        { TestEnum.ENUM11, TestEnum.ENUM12 },
                        { TestEnum.ENUM1, TestEnum.ENUM2 },
                        { TestEnum.ENUM3, TestEnum.ENUM4 },
                    },
                    {
                        { TestEnum.ENUM5, TestEnum.ENUM6 },
                        { TestEnum.ENUM7, TestEnum.ENUM8 },
                        { TestEnum.ENUM9, TestEnum.ENUM10 },
                        { TestEnum.ENUM11, TestEnum.ENUM12 },
                    },
                },
                StructMultiArrayField = new[,,]
                {
                    {
                        { new NestedStruct { Id = 1, Message = "01" }, new NestedStruct { Id = 2, Message = "02" } },
                        { new NestedStruct { Id = 3, Message = "03" }, new NestedStruct { Id = 4, Message = "04" } },
                        { new NestedStruct { Id = 5, Message = "05" }, new NestedStruct { Id = 6, Message = "06" } },
                        { new NestedStruct { Id = 7, Message = "07" }, new NestedStruct { Id = 8, Message = "08" } },
                    },
                    {
                        { new NestedStruct { Id = 9, Message = "09" }, new NestedStruct { Id = 10, Message = "10" } },
                        { new NestedStruct { Id = 11, Message = "11" }, new NestedStruct { Id = 12, Message = "12" } },
                        { new NestedStruct { Id = 13, Message = "13" }, new NestedStruct { Id = 14, Message = "14" } },
                        { new NestedStruct { Id = 15, Message = "15" }, new NestedStruct { Id = 16, Message = "16" } },
                    },
                    {
                        { new NestedStruct { Id = 17, Message = "17" }, new NestedStruct { Id = 18, Message = "18" } },
                        { new NestedStruct { Id = 19, Message = "19" }, new NestedStruct { Id = 20, Message = "20" } },
                        { new NestedStruct { Id = 21, Message = "21" }, new NestedStruct { Id = 22, Message = "22" } },
                        { new NestedStruct { Id = 23, Message = "23" }, new NestedStruct { Id = 24, Message = "24" } },
                    },
                },
                StringMultiArrayField = new[,,]
                {
                    {
                        { "01", "02" },
                        { "03", "04" },
                        { "05", "06" },
                        { "07", "08" },
                    },
                    {
                        { "09", "10" },
                        { "11", "12" },
                        { "13", "14" },
                        { "15", "16" },
                    },
                    {
                        { "17", "18" },
                        { "19", "20" },
                        { "21", "22" },
                        { "23", "24" },
                    },
                },
                WStringMultiArrayField = new[,,]
                {
                    {
                        { "01", "02" },
                        { "03", "04" },
                        { "05", "06" },
                        { "07", "08" },
                    },
                    {
                        { "09", "10" },
                        { "11", "12" },
                        { "13", "14" },
                        { "15", "16" },
                    },
                    {
                        { "17", "18" },
                        { "19", "20" },
                        { "21", "22" },
                        { "23", "24" },
                    },
                },
                CharMultiArrayField = new[,,]
                {
                    {
                        { '1', '2' },
                        { '3', '4' },
                        { '5', '6' },
                        { '7', '8' },
                    },
                    {
                        { '9', '0' },
                        { '1', '2' },
                        { '3', '4' },
                        { '5', '6' },
                    },
                    {
                        { '7', '8' },
                        { '9', '0' },
                        { '1', '2' },
                        { '3', '4' },
                    },
                },
                WCharMultiArrayField = new[,,]
                {
                    {
                        { '1', '2' },
                        { '3', '4' },
                        { '5', '6' },
                        { '7', '8' },
                    },
                    {
                        { '9', '0' },
                        { '1', '2' },
                        { '3', '4' },
                        { '5', '6' },
                    },
                    {
                        { '7', '8' },
                        { '9', '0' },
                        { '1', '2' },
                        { '3', '4' },
                    },
                },
            };
        }

        private static void PrintReceivedSample(TestStruct received)
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
            PrintStructField(nameof(received.ShortMultiArrayField), string.Join(" ", received.ShortMultiArrayField.Cast<short>()));
            PrintStructField(nameof(received.UnsignedShortMultiArrayField), string.Join(" ", received.UnsignedShortMultiArrayField.Cast<ushort>()));
            PrintStructField(nameof(received.LongMultiArrayField), string.Join(" ", received.LongMultiArrayField.Cast<int>()));
            PrintStructField(nameof(received.UnsignedLongMultiArrayField), string.Join(" ", received.UnsignedLongMultiArrayField.Cast<uint>()));
            PrintStructField(nameof(received.LongLongMultiArrayField), string.Join(" ", received.LongLongMultiArrayField.Cast<long>()));
            PrintStructField(nameof(received.UnsignedLongLongMultiArrayField), string.Join(" ", received.UnsignedLongLongMultiArrayField.Cast<ulong>()));
            PrintStructField(nameof(received.FloatMultiArrayField), string.Join(" ", received.FloatMultiArrayField.Cast<float>()));
            PrintStructField(nameof(received.DoubleMultiArrayField), string.Join(" ", received.DoubleMultiArrayField.Cast<double>()));
            PrintStructField(nameof(received.BooleanMultiArrayField), string.Join(" ", received.BooleanMultiArrayField.Cast<bool>()));
            PrintStructField(nameof(received.OctetMultiArrayField), string.Join(" ", received.OctetMultiArrayField.Cast<byte>()));
            PrintStructField(nameof(received.EnumMultiArrayField), string.Join(" ", received.EnumMultiArrayField.Cast<TestEnum>()));
            PrintStructField(nameof(received.StructMultiArrayField), string.Join(" ", received.StructMultiArrayField.Cast<NestedStruct>().Select(s => $"ID: {s.Id} MESSAGE: {s.Message}")));
            PrintStructField(nameof(received.StringMultiArrayField), string.Join(" ", received.StringMultiArrayField.Cast<string>()));
            PrintStructField(nameof(received.WStringMultiArrayField), string.Join(" ", received.WStringMultiArrayField.Cast<string>()));
            PrintStructField(nameof(received.CharMultiArrayField), string.Join(" ", received.CharMultiArrayField.Cast<char>()));
            PrintStructField(nameof(received.WCharMultiArrayField), string.Join(" ", received.WCharMultiArrayField.Cast<char>()));
            Console.ResetColor();
        }

        private static Topic CreateTestTopic(DomainParticipant participant)
        {
            var typeSupport = new TestStructTypeSupport();
            var typeName = typeSupport.GetTypeName();
            var ret = typeSupport.RegisterType(participant, typeName);
            if (ret != ReturnCode.Ok)
            {
                Console.Error.WriteLine("TestStructTypeSupport could not be registered: " + ret);
                return null;
            }

            var topic = participant.CreateTopic("TestTopic", typeName);
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
    }
}
