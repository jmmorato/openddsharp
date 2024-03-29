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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using JsonWrapper;
using JsonWrapperInclude;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// Code generator JSON unit test class.
    /// </summary>
    [TestClass]
    public class CodeGeneratorJsonWrapperTest
    {
        #region Constants
        private const string TEST_CATEGORY = "CodeGenerator";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Publisher _publisher;
        private Subscriber _subscriber;
        private Topic _topic;
        private DataReader _dr;
        private TestStructDataReader _dataReader;
        private DataWriter _dw;
        private TestStructDataWriter _dataWriter;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets test context object.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by MSTest")]
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
        /// <summary>
        /// The test initializer method.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);

            _subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(_subscriber);

            var typeSupport = new TestStructTypeSupport();
            var typeName = typeSupport.GetTypeName();
            var ret = typeSupport.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, ret);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            _dr = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(_dr);
            _dataReader = new TestStructDataReader(_dr);

            _dw = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_dw);
            _dataWriter = new TestStructDataWriter(_dw);

            Assert.IsTrue(_dataWriter.WaitForSubscriptions(1, 5000));
            Assert.IsTrue(_dataReader.WaitForPublications(1, 5000));
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _publisher?.DeleteDataWriter(_dw);
            _publisher?.DeleteContainedEntities();
            _dr?.DeleteContainedEntities();
            _subscriber?.DeleteDataReader(_dr);
            _subscriber?.DeleteContainedEntities();
            _participant?.DeletePublisher(_publisher);
            _participant?.DeleteSubscriber(_subscriber);
            _participant?.DeleteTopic(_topic);
            _participant?.DeleteContainedEntities();
            AssemblyInitializer.Factory?.DeleteParticipant(_participant);

            _participant = null;
            _publisher = null;
            _subscriber = null;
            _topic = null;
            _dw = null;
            _dr = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test include idl files.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestInclude()
        {
            using var evt = new ManualResetEventSlim(false);

            var test = new TestInclude();
            Assert.AreEqual(typeof(IncludeStruct), test.IncludeField.GetType());
            Assert.IsNotNull(test.IncludeField);
            Assert.AreEqual(test.IncludeField.Message.GetType(), typeof(string));

            var typeSupport = new TestIncludeTypeSupport();
            var typeName = typeSupport.GetTypeName();
            var ret = typeSupport.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, ret);

            var topic = _participant.CreateTopic("TestTopic", typeName);
            Assert.IsNotNull(topic);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var dr = _subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dr);
            var dataReader = new TestIncludeDataReader(dr);

            var statusCondition = dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dw = _publisher.CreateDataWriter(topic);
            Assert.IsNotNull(dw);
            var dataWriter = new TestIncludeDataWriter(dw);

            Assert.IsTrue(dataWriter.WaitForSubscriptions(1, 5000));
            Assert.IsTrue(dataReader.WaitForPublications(1, 5000));

            test = new TestInclude
            {
                Id = "1",
                IncludeField = new IncludeStruct
                {
                    Message = "Test",
                },
            };
            ret = dataWriter.Write(test);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestInclude();
            var sampleInfo = new SampleInfo();
            ret = dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.AreEqual(test.Id, received.Id);
            Assert.AreEqual(test.IncludeField.Message, received.IncludeField.Message);

            dr.DeleteContainedEntities();
            _subscriber.DeleteDataReader(dr);
            _subscriber.DeleteContainedEntities();
            _publisher.DeleteDataWriter(dw);
            _publisher.DeleteContainedEntities();
            _participant.DeleteTopic(topic);
            _participant.DeleteContainedEntities();
        }

        /// <summary>
        /// Test the code generated for the basic types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedBasicTypes()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                Int8Field = -1,
                UInt8Field = 1,
                ShortField = -1,
                LongField = -2,
                LongLongField = -3,
                UnsignedShortField = 1,
                UnsignedLongField = 2,
                UnsignedLongLongField = 3,
                CharField = 'a',
                WCharField = 'b',
                BooleanField = true,
                OctetField = 0x42,
                FloatField = 42.0f,
                DoubleField = 23.23d,
                // LongDoubleField = 69.69m,
            };
            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.AreEqual(data.Int8Field, received.Int8Field);
            Assert.AreEqual(data.UInt8Field, received.UInt8Field);
            Assert.AreEqual(data.ShortField, received.ShortField);
            Assert.AreEqual(data.LongField, received.LongField);
            Assert.AreEqual(data.LongLongField, received.LongLongField);
            Assert.AreEqual(data.UnsignedShortField, received.UnsignedShortField);
            Assert.AreEqual(data.UnsignedLongField, received.UnsignedLongField);
            Assert.AreEqual(data.UnsignedLongLongField, received.UnsignedLongLongField);
            Assert.AreEqual(data.CharField, received.CharField);
            Assert.AreEqual(data.WCharField, received.WCharField);
            Assert.AreEqual(data.BooleanField, received.BooleanField);
            Assert.AreEqual(data.OctetField, received.OctetField);
            Assert.AreEqual(data.FloatField, received.FloatField);
            Assert.AreEqual(data.DoubleField, received.DoubleField);
            // Assert.AreEqual(data.LongDoubleField, received.LongDoubleField);

            Assert.AreEqual(typeof(short), data.ShortField.GetType());
            Assert.AreEqual(typeof(int), data.LongField.GetType());
            Assert.AreEqual(typeof(long), data.LongLongField.GetType());
            Assert.AreEqual(typeof(ushort), data.UnsignedShortField.GetType());
            Assert.AreEqual(typeof(uint), data.UnsignedLongField.GetType());
            Assert.AreEqual(typeof(ulong), data.UnsignedLongLongField.GetType());
            Assert.AreEqual(typeof(char), data.CharField.GetType());
            Assert.AreEqual(typeof(char), data.WCharField.GetType());
            Assert.AreEqual(typeof(bool), data.BooleanField.GetType());
            Assert.AreEqual(typeof(byte), data.OctetField.GetType());
            Assert.AreEqual(typeof(float), data.FloatField.GetType());
            Assert.AreEqual(typeof(double), data.DoubleField.GetType());
            // Assert.AreEqual(typeof(decimal), data.LongDoubleField.GetType());

            Assert.AreEqual(0, defaultStruct.ShortField);
            Assert.AreEqual(0, defaultStruct.LongField);
            Assert.AreEqual(0, defaultStruct.LongLongField);
            Assert.AreEqual((ushort)0, defaultStruct.UnsignedShortField);
            Assert.AreEqual(0U, defaultStruct.UnsignedLongField);
            Assert.AreEqual(0UL, defaultStruct.UnsignedLongLongField);
            Assert.AreEqual('\0', defaultStruct.CharField);
            Assert.AreEqual('\0', defaultStruct.WCharField);
            Assert.AreEqual(false, defaultStruct.BooleanField);
            Assert.AreEqual(0, defaultStruct.OctetField);
            Assert.AreEqual(0.0f, defaultStruct.FloatField);
            Assert.AreEqual(0.0, defaultStruct.DoubleField);
            // Assert.AreEqual(0.0m, defaultStruct.LongDoubleField);
        }

        /// <summary>
        /// Test the code generated for the sequences of basic types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedBasicTypeSequences()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // TODO: Bounded lists that exceed the bound throws an exception.
            // As per documentation: Bounds checking on bounded sequences may raise an exception if necessary.
            // Check bound before toNative and throw a user friendly exception.
            // Another option is to implement an internal BoundedList that check the bound before Add/Insert.
            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                BoundedBooleanSequenceField = { true, true, false, false, true },
                UnboundedBooleanSequenceField = { true, true, false, false, true, true, false },
                BoundedCharSequenceField = { 'z' },
                UnboundedCharSequenceField = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' },
                BoundedWCharSequenceField = { 'i', 'j', 'k', 'l', 'm' },
                UnboundedWCharSequenceField = { 'n', 'o', 'p' },
                BoundedOctetSequenceField = { 0x01, 0x02, 0x03 },
                UnboundedOctetSequenceField = { 0x04, 0x05, 0x06, 0x07, 0x08 },
                BoundedShortSequenceField = { -1, 2, -3 },
                UnboundedShortSequenceField = { 4, -5, 6, -7, 8 },
                BoundedUShortSequenceField = { 1, 2, 3 },
                UnboundedUShortSequenceField = { 4, 5, 6, 7, 8 },
                BoundedLongSequenceField = { -1, 2, -3, 100, -200 },
                UnboundedLongSequenceField = { 1, -2, 3, -100, 200, -300, 1000 },
                BoundedULongSequenceField = { 1, 2, 3, 100, 200 },
                UnboundedULongSequenceField = { 1, 2, 3, 100, 200, 300, 1000 },
                BoundedLongLongSequenceField = { -1, 2, -3, 100, -200 },
                UnboundedLongLongSequenceField = { 1, -2, 3, -100, 200, -300, 1000 },
                BoundedULongLongSequenceField = { 1, 2, 3, 100, 200 },
                UnboundedULongLongSequenceField = { 1, 2, 3, 100, 200, 300, 1000 },
                BoundedFloatSequenceField = { -1.0f, 2.1f, -3.2f, 100.3f, -200.4f },
                UnboundedFloatSequenceField = { 1f, -2.6f, 3.7f, -100.8f, 200.9f, -300.1f, 1000.1f },
                BoundedDoubleSequenceField = { -1.0d, 2.1d, -3.2d, 100.3d, -200.4d },
                UnboundedDoubleSequenceField = { 1.0d, -2.6d, 3.7d, -100.8d, 200.9d, -300.02d, 1000.1d },
                // BoundedLongDoubleSequenceField = { -1.0m, 2.1m, -3.2m, 100.3m, -200.4m },
                // UnboundedLongDoubleSequenceField = { 1.5m, -2.6m, 3.7m, -100.8m, 200.9m, -300.0m, 1000.1m },
                BoundedInt8SequenceField = { -1, 2, -3 },
                UnboundedInt8SequenceField = { 4, -5, 6, -7, 8 },
                BoundedUInt8SequenceField = { 1, 2, 3 },
                UnboundedUInt8SequenceField = { 4, 5, 6, 7, 8 },
            };
            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(data.BoundedBooleanSequenceField.SequenceEqual(received.BoundedBooleanSequenceField));
            Assert.IsTrue(data.UnboundedBooleanSequenceField.SequenceEqual(received.UnboundedBooleanSequenceField));
            Assert.IsTrue(data.BoundedCharSequenceField.SequenceEqual(received.BoundedCharSequenceField));
            Assert.IsTrue(data.UnboundedCharSequenceField.SequenceEqual(received.UnboundedCharSequenceField));
            Assert.IsTrue(data.BoundedWCharSequenceField.SequenceEqual(received.BoundedWCharSequenceField));
            Assert.IsTrue(data.UnboundedWCharSequenceField.SequenceEqual(received.UnboundedWCharSequenceField));
            Assert.IsTrue(data.BoundedOctetSequenceField.SequenceEqual(received.BoundedOctetSequenceField));
            Assert.IsTrue(data.UnboundedOctetSequenceField.SequenceEqual(received.UnboundedOctetSequenceField));
            Assert.IsTrue(data.BoundedShortSequenceField.SequenceEqual(received.BoundedShortSequenceField));
            Assert.IsTrue(data.UnboundedShortSequenceField.SequenceEqual(received.UnboundedShortSequenceField));
            Assert.IsTrue(data.BoundedUShortSequenceField.SequenceEqual(received.BoundedUShortSequenceField));
            Assert.IsTrue(data.UnboundedUShortSequenceField.SequenceEqual(received.UnboundedUShortSequenceField));
            Assert.IsTrue(data.BoundedLongSequenceField.SequenceEqual(received.BoundedLongSequenceField));
            Assert.IsTrue(data.UnboundedLongSequenceField.SequenceEqual(received.UnboundedLongSequenceField));
            Assert.IsTrue(data.BoundedULongSequenceField.SequenceEqual(received.BoundedULongSequenceField));
            Assert.IsTrue(data.UnboundedULongSequenceField.SequenceEqual(received.UnboundedULongSequenceField));
            Assert.IsTrue(data.BoundedLongLongSequenceField.SequenceEqual(received.BoundedLongLongSequenceField));
            Assert.IsTrue(data.UnboundedLongLongSequenceField.SequenceEqual(received.UnboundedLongLongSequenceField));
            Assert.IsTrue(data.BoundedULongLongSequenceField.SequenceEqual(received.BoundedULongLongSequenceField));
            Assert.IsTrue(data.UnboundedULongLongSequenceField.SequenceEqual(received.UnboundedULongLongSequenceField));
            Assert.IsTrue(data.BoundedFloatSequenceField.SequenceEqual(received.BoundedFloatSequenceField));
            Assert.IsTrue(data.UnboundedFloatSequenceField.SequenceEqual(received.UnboundedFloatSequenceField));
            Assert.IsTrue(data.BoundedDoubleSequenceField.SequenceEqual(received.BoundedDoubleSequenceField));
            Assert.IsTrue(data.UnboundedDoubleSequenceField.SequenceEqual(received.UnboundedDoubleSequenceField));
            // Assert.IsTrue(data.BoundedLongDoubleSequenceField.SequenceEqual(received.BoundedLongDoubleSequenceField));
            // Assert.IsTrue(data.UnboundedLongDoubleSequenceField.SequenceEqual(received.UnboundedLongDoubleSequenceField));
            Assert.IsTrue(data.BoundedInt8SequenceField.SequenceEqual(received.BoundedInt8SequenceField));
            Assert.IsTrue(data.UnboundedInt8SequenceField.SequenceEqual(received.UnboundedInt8SequenceField));
            Assert.IsTrue(data.BoundedUInt8SequenceField.SequenceEqual(received.BoundedUInt8SequenceField));
            Assert.IsTrue(data.UnboundedUInt8SequenceField.SequenceEqual(received.UnboundedUInt8SequenceField));

            Assert.AreEqual(data.BoundedBooleanSequenceField.GetType(), typeof(List<bool>));
            Assert.AreEqual(data.UnboundedBooleanSequenceField.GetType(), typeof(List<bool>));
            Assert.AreEqual(data.BoundedCharSequenceField.GetType(), typeof(List<char>));
            Assert.AreEqual(data.UnboundedCharSequenceField.GetType(), typeof(List<char>));
            Assert.AreEqual(data.BoundedWCharSequenceField.GetType(), typeof(List<char>));
            Assert.AreEqual(data.UnboundedWCharSequenceField.GetType(), typeof(List<char>));
            Assert.AreEqual(data.BoundedOctetSequenceField.GetType(), typeof(List<byte>));
            Assert.AreEqual(data.UnboundedOctetSequenceField.GetType(), typeof(List<byte>));
            Assert.AreEqual(data.BoundedShortSequenceField.GetType(), typeof(List<short>));
            Assert.AreEqual(data.UnboundedShortSequenceField.GetType(), typeof(List<short>));
            Assert.AreEqual(data.BoundedUShortSequenceField.GetType(), typeof(List<ushort>));
            Assert.AreEqual(data.UnboundedUShortSequenceField.GetType(), typeof(List<ushort>));
            Assert.AreEqual(data.BoundedLongSequenceField.GetType(), typeof(List<int>));
            Assert.AreEqual(data.UnboundedLongSequenceField.GetType(), typeof(List<int>));
            Assert.AreEqual(data.BoundedULongSequenceField.GetType(), typeof(List<uint>));
            Assert.AreEqual(data.UnboundedULongSequenceField.GetType(), typeof(List<uint>));
            Assert.AreEqual(data.BoundedLongLongSequenceField.GetType(), typeof(List<long>));
            Assert.AreEqual(data.UnboundedLongLongSequenceField.GetType(), typeof(List<long>));
            Assert.AreEqual(data.BoundedULongLongSequenceField.GetType(), typeof(List<ulong>));
            Assert.AreEqual(data.UnboundedULongLongSequenceField.GetType(), typeof(List<ulong>));
            Assert.AreEqual(data.BoundedFloatSequenceField.GetType(), typeof(List<float>));
            Assert.AreEqual(data.UnboundedFloatSequenceField.GetType(), typeof(List<float>));
            Assert.AreEqual(data.BoundedDoubleSequenceField.GetType(), typeof(List<double>));
            Assert.AreEqual(data.UnboundedDoubleSequenceField.GetType(), typeof(List<double>));
            // Assert.AreEqual(data.BoundedLongDoubleSequenceField.GetType(), typeof(List<decimal>));
            // Assert.AreEqual(data.UnboundedLongDoubleSequenceField.GetType(), typeof(List<decimal>));
            Assert.AreEqual(data.BoundedInt8SequenceField.GetType(), typeof(List<sbyte>));
            Assert.AreEqual(data.UnboundedInt8SequenceField.GetType(), typeof(List<sbyte>));
            Assert.AreEqual(data.BoundedUInt8SequenceField.GetType(), typeof(List<byte>));
            Assert.AreEqual(data.UnboundedUInt8SequenceField.GetType(), typeof(List<byte>));

            Assert.IsNotNull(defaultStruct.BoundedBooleanSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedBooleanSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedBooleanSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedBooleanSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedCharSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedCharSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedCharSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedCharSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedWCharSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedWCharSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedWCharSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedWCharSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedOctetSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedOctetSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedOctetSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedOctetSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedShortSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedShortSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedShortSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedShortSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedUShortSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedUShortSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedUShortSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedUShortSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedLongSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedLongSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedLongSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedLongSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedULongSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedULongSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedULongSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedULongSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedLongLongSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedLongLongSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedLongLongSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedLongLongSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedULongLongSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedULongLongSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedULongLongSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedULongLongSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedFloatSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedFloatSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedFloatSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedFloatSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedDoubleSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedDoubleSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedDoubleSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedDoubleSequenceField.Count);
            // Assert.IsNotNull(defaultStruct.BoundedLongDoubleSequenceField);
            // Assert.AreEqual(0, defaultStruct.BoundedLongDoubleSequenceField.Count);
            // Assert.IsNotNull(defaultStruct.UnboundedLongDoubleSequenceField);
            // Assert.AreEqual(0, defaultStruct.UnboundedLongDoubleSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedInt8SequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedInt8SequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedUInt8SequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedUInt8SequenceField.Count);
        }

        /// <summary>
        /// Test the code generated for the arrays of basic types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedBasicTypeArrays()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                BooleanArrayField = new[] { true, true, false, false, true },
                CharArrayField = new[] { 'a', 'b', 'c', 'd', 'e' },
                WCharArrayField = new[] { 'i', 'j', 'k', 'l', 'm' },
                OctetArrayField = new byte[] { 0x04, 0x05, 0x06, 0x07, 0x08 },
                ShortArrayField = new short[] { 4, -5, 6, -7, 8 },
                UnsignedShortArrayField = new ushort[] { 4, 5, 6, 7, 8 },
                LongArrayField = new[] { -1, 2, -3, 100, -200 },
                UnsignedLongArrayField = new[] { 1u, 2u, 3u, 100u, 200u },
                LongLongArrayField = new[] { -1L, 2L, -3L, 100L, -200L },
                UnsignedLongLongArrayField = new[] { 1UL, 2UL, 3UL, 100UL, 200UL },
                FloatArrayField = new[] { -1.0f, 2.1f, -3.2f, 100.3f, -200.4f },
                DoubleArrayField = new[] { -1.0d, 2.1d, -3.2d, 100.3d, -200.4d },
                // LongDoubleArrayField = new[] { -1.0m, 2.1m, -3.2m, 100.3m, -200.4m },
                Int8ArrayField = new sbyte[] { 4, -5, 6, -7, 8 },
                UInt8ArrayField = new byte[] { 4, 5, 6, 7, 8 },
            };
            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var sampleInfo = new SampleInfo();
            var received = new TestStruct();
            ret = _dataReader.ReadNextSample(received, sampleInfo);

            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.IsTrue(data.BooleanArrayField.SequenceEqual(received.BooleanArrayField));
            Assert.IsTrue(data.CharArrayField.SequenceEqual(received.CharArrayField));
            Assert.IsTrue(data.WCharArrayField.SequenceEqual(received.WCharArrayField));
            Assert.IsTrue(data.OctetArrayField.SequenceEqual(received.OctetArrayField));
            Assert.IsTrue(data.ShortArrayField.SequenceEqual(received.ShortArrayField));
            Assert.IsTrue(data.UnsignedShortArrayField.SequenceEqual(received.UnsignedShortArrayField));
            Assert.IsTrue(data.LongArrayField.SequenceEqual(received.LongArrayField));
            Assert.IsTrue(data.UnsignedLongArrayField.SequenceEqual(received.UnsignedLongArrayField));
            Assert.IsTrue(data.LongLongArrayField.SequenceEqual(received.LongLongArrayField));
            Assert.IsTrue(data.UnsignedLongLongArrayField.SequenceEqual(received.UnsignedLongLongArrayField));
            Assert.IsTrue(data.FloatArrayField.SequenceEqual(received.FloatArrayField));
            Assert.IsTrue(data.DoubleArrayField.SequenceEqual(received.DoubleArrayField));
            // Assert.IsTrue(data.LongDoubleArrayField.SequenceEqual(received.LongDoubleArrayField));
            Assert.IsTrue(data.Int8ArrayField.SequenceEqual(received.Int8ArrayField));
            Assert.IsTrue(data.UInt8ArrayField.SequenceEqual(received.UInt8ArrayField));

            Assert.AreEqual(typeof(bool[]), data.BooleanArrayField.GetType());
            Assert.AreEqual(typeof(char[]), data.CharArrayField.GetType());
            Assert.AreEqual(typeof(char[]), data.WCharArrayField.GetType());
            Assert.AreEqual(typeof(byte[]), data.OctetArrayField.GetType());
            Assert.AreEqual(typeof(short[]), data.ShortArrayField.GetType());
            Assert.AreEqual(typeof(ushort[]), data.UnsignedShortArrayField.GetType());
            Assert.AreEqual(typeof(int[]), data.LongArrayField.GetType());
            Assert.AreEqual(typeof(uint[]), data.UnsignedLongArrayField.GetType());
            Assert.AreEqual(typeof(long[]), data.LongLongArrayField.GetType());
            Assert.AreEqual(typeof(ulong[]), data.UnsignedLongLongArrayField.GetType());
            Assert.AreEqual(typeof(float[]), data.FloatArrayField.GetType());
            Assert.AreEqual(typeof(double[]), data.DoubleArrayField.GetType());
            // Assert.AreEqual(typeof(decimal[]), data.LongDoubleArrayField.GetType());
            Assert.AreEqual(typeof(sbyte[]), data.Int8ArrayField.GetType());
            Assert.AreEqual(typeof(byte[]), data.UInt8ArrayField.GetType());

            Assert.IsNotNull(defaultStruct.BooleanArrayField);
            Assert.AreEqual(5, defaultStruct.BooleanArrayField.Length);
            foreach (var i in defaultStruct.BooleanArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.CharArrayField);
            Assert.AreEqual(5, defaultStruct.CharArrayField.Length);
            foreach (var i in defaultStruct.CharArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.WCharArrayField);
            Assert.AreEqual(5, defaultStruct.WCharArrayField.Length);
            foreach (var i in defaultStruct.WCharArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.OctetArrayField);
            Assert.AreEqual(5, defaultStruct.OctetArrayField.Length);
            foreach (var i in defaultStruct.OctetArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.ShortArrayField);
            Assert.AreEqual(5, defaultStruct.ShortArrayField.Length);
            foreach (var i in defaultStruct.ShortArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.UnsignedShortArrayField);
            Assert.AreEqual(5, defaultStruct.UnsignedShortArrayField.Length);
            foreach (var i in defaultStruct.UnsignedShortArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.LongArrayField);
            Assert.AreEqual(5, defaultStruct.LongArrayField.Length);
            foreach (var i in defaultStruct.LongArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.UnsignedLongArrayField);
            Assert.AreEqual(5, defaultStruct.UnsignedLongArrayField.Length);
            foreach (var i in defaultStruct.UnsignedLongArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.LongLongArrayField);
            Assert.AreEqual(5, defaultStruct.LongLongArrayField.Length);
            foreach (var i in defaultStruct.LongLongArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.UnsignedLongLongArrayField);
            Assert.AreEqual(5, defaultStruct.UnsignedLongLongArrayField.Length);
            foreach (var i in defaultStruct.UnsignedLongLongArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.FloatArrayField);
            Assert.AreEqual(5, defaultStruct.FloatArrayField.Length);
            foreach (var i in defaultStruct.FloatArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.DoubleArrayField);
            Assert.AreEqual(5, defaultStruct.DoubleArrayField.Length);
            foreach (var i in defaultStruct.DoubleArrayField)
            {
                Assert.AreEqual(default, i);
            }

            // Assert.IsNotNull(defaultStruct.LongDoubleArrayField);
            // Assert.AreEqual(5, defaultStruct.LongDoubleArrayField.Length);
            // foreach (var i in defaultStruct.LongDoubleArrayField)
            // {
            //     Assert.AreEqual(default, i);
            // }

            Assert.IsNotNull(defaultStruct.Int8ArrayField);
            Assert.AreEqual(5, defaultStruct.Int8ArrayField.Length);
            foreach (var i in defaultStruct.Int8ArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.UInt8ArrayField);
            Assert.AreEqual(5, defaultStruct.UInt8ArrayField.Length);
            foreach (var i in defaultStruct.UInt8ArrayField)
            {
                Assert.AreEqual(default, i);
            }
        }

        /// <summary>
        /// Test the code generated for the multi-array of basic types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedBasicTypeMultiArrays()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
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
                // LongDoubleMultiArrayField = new[]
                // {
                //     new[]
                //     {
                //         new[] { 01.01m, 02.02m },
                //         new[] { 03.03m, 04.04m },
                //         new[] { 05.05m, 06.06m },
                //         new[] { 07.07m, 08.08m },
                //     },
                //     new[]
                //     {
                //         new[] { 09.09m, 10.10m },
                //         new[] { 11.11m, 12.12m },
                //         new[] { 13.13m, 14.14m },
                //         new[] { 15.15m, 16.16m },
                //     },
                //     new[]
                //     {
                //         new[] { 17.17m, 18.18m },
                //         new[] { 19.19m, 20.20m },
                //         new[] { 21.21m, 22.22m },
                //         new[] { 23.23m, 24.24m },
                //     },
                // },
                Int8MultiArrayField = new[]
                {
                    new[]
                    {
                        new sbyte[] { 01, 02 },
                        new sbyte[] { 03, 04 },
                        new sbyte[] { 05, 06 },
                        new sbyte[] { 07, 08 },
                    },
                    new[]
                    {
                        new sbyte[] { 09, 10 },
                        new sbyte[] { 11, 12 },
                        new sbyte[] { 13, 14 },
                        new sbyte[] { 15, 16 },
                    },
                    new[]
                    {
                        new sbyte[] { 17, 18 },
                        new sbyte[] { 19, 20 },
                        new sbyte[] { 21, 22 },
                        new sbyte[] { 23, 24 },
                    },
                },
                UInt8MultiArrayField = new[]
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
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(TestHelper.CompareMultiArray(data.BooleanMultiArrayField, received.BooleanMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.CharMultiArrayField, received.CharMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.WCharMultiArrayField, received.WCharMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.OctetMultiArrayField, received.OctetMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.ShortMultiArrayField, received.ShortMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.UnsignedShortMultiArrayField, received.UnsignedShortMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.LongMultiArrayField, received.LongMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.UnsignedLongMultiArrayField, received.UnsignedLongMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.LongLongMultiArrayField, received.LongLongMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.UnsignedLongLongMultiArrayField, received.UnsignedLongLongMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.FloatMultiArrayField, received.FloatMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.DoubleMultiArrayField, received.DoubleMultiArrayField));
            // Assert.IsTrue(CompareMultiArray(data.LongDoubleMultiArrayField, received.LongDoubleMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.Int8MultiArrayField, received.Int8MultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.UInt8MultiArrayField, received.UInt8MultiArrayField));

            Assert.AreEqual(typeof(bool[][][]), data.BooleanMultiArrayField.GetType());
            Assert.AreEqual(typeof(char[][][]), data.CharMultiArrayField.GetType());
            Assert.AreEqual(typeof(char[][][]), data.WCharMultiArrayField.GetType());
            Assert.AreEqual(typeof(byte[][][]), data.OctetMultiArrayField.GetType());
            Assert.AreEqual(typeof(short[][][]), data.ShortMultiArrayField.GetType());
            Assert.AreEqual(typeof(ushort[][][]), data.UnsignedShortMultiArrayField.GetType());
            Assert.AreEqual(typeof(int[][][]), data.LongMultiArrayField.GetType());
            Assert.AreEqual(typeof(uint[][][]), data.UnsignedLongMultiArrayField.GetType());
            Assert.AreEqual(typeof(long[][][]), data.LongLongMultiArrayField.GetType());
            Assert.AreEqual(typeof(ulong[][][]), data.UnsignedLongLongMultiArrayField.GetType());
            Assert.AreEqual(typeof(float[][][]), data.FloatMultiArrayField.GetType());
            Assert.AreEqual(typeof(double[][][]), data.DoubleMultiArrayField.GetType());
            // Assert.AreEqual(typeof(decimal[][][]), data.LongDoubleMultiArrayField.GetType());
            Assert.AreEqual(typeof(sbyte[][][]), data.Int8MultiArrayField.GetType());
            Assert.AreEqual(typeof(byte[][][]), data.UInt8MultiArrayField.GetType());

            for (var i0 = 0; i0 < 3; i0++)
            {
                for (var i1 = 0; i1 < 4; i1++)
                {
                    for (var i2 = 0; i2 < 2; i2++)
                    {
                        Assert.AreEqual(default, defaultStruct.BooleanMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.CharMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.WCharMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.OctetMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.ShortMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.UnsignedShortMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.LongMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.UnsignedLongMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.LongLongMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.UnsignedLongLongMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.FloatMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.DoubleMultiArrayField[i0][i1][i2]);
                        // Assert.AreEqual(default, defaultStruct.LongDoubleMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.Int8MultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.UInt8MultiArrayField[i0][i1][i2]);
                    }
                }
            }
        }

        /// <summary>
        /// Test the code generated for the string types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStringTypes()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                Id = 1,
                UnboundedStringField = "Hello, I love you, won't you tell me your name?",
                UnboundedWStringField = "She's walking down the street\nBlind to every eye she meets\nDo you think you'll be the guy\nTo make the queen of the angels sigh?",
                BoundedStringField = "Hello, I love you, won't you te",
                BoundedWStringField = "Hello, I love you, won't you te",
            };
            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.AreEqual(data.UnboundedStringField, received.UnboundedStringField);
            Assert.AreEqual(data.UnboundedWStringField, received.UnboundedWStringField);

            // TODO: I would expect ".Substring(0, 32)" is received but it seems the whole string is transported.
            // Check with OpenDDS test and report for clarification if the behaviour is confirmed.
            Assert.AreEqual(data.BoundedStringField, received.BoundedStringField);
            Assert.AreEqual(data.BoundedWStringField, received.BoundedWStringField);

            Assert.AreEqual(typeof(string), data.UnboundedStringField.GetType());
            Assert.AreEqual(typeof(string), data.UnboundedWStringField.GetType());
            Assert.AreEqual(typeof(string), data.BoundedStringField.GetType());
            Assert.AreEqual(typeof(string), data.BoundedWStringField.GetType());

            Assert.AreEqual(defaultStruct.UnboundedStringField, string.Empty);
            Assert.AreEqual(defaultStruct.UnboundedWStringField, string.Empty);
            Assert.AreEqual(defaultStruct.BoundedStringField, string.Empty);
            Assert.AreEqual(defaultStruct.BoundedWStringField, string.Empty);
        }

        /// <summary>
        /// Test the code generated for the sequences of strings.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStringSequences()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                BoundedStringSequenceField =
                {
                    "Pressure pushing down on me",
                    "Pressing down on you, no man ask for",
                    "Under pressure that burns a building down",
                    "Splits a family in two",
                    "Puts people on streets",
                },
                UnboundedStringSequenceField =
                {
                    "You've got your mother in a whirl",
                    "She's not sure if you're a boy or a girl",
                    "Hey babe! your hair's alright",
                    "Hey babe! let's go out tonight",
                    "You like me, and I like it all",
                    "We like dancing and we look divine",
                    "You love bands when they're playing hard",
                    "You want more and you want it fast",
                    "They put you down, they say I'm wrong",
                    "You tacky thing, you put them on",
                },
                BoundedWStringSequenceField =
                {
                    "Rebel Rebel, you've turn your dress",
                    "Rebel Rebel, your face is a mess",
                    "Rebel Rebel, how could they know?",
                    "Hot tramp, I love you so!",
                },
                UnboundedWStringSequenceField =
                {
                    "Well, you've got your diamonds",
                    "And you've got your pretty clothes",
                    "And the chauffer drives your car",
                    "You let everybody know",
                    "But don't play with me,",
                    "'cause you're playing with fire",
                },
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(data.BoundedStringSequenceField.SequenceEqual(received.BoundedStringSequenceField));
            Assert.IsTrue(data.UnboundedStringSequenceField.SequenceEqual(received.UnboundedStringSequenceField));
            Assert.IsTrue(data.BoundedWStringSequenceField.SequenceEqual(received.BoundedWStringSequenceField));
            Assert.IsTrue(data.UnboundedWStringSequenceField.SequenceEqual(received.UnboundedWStringSequenceField));

            Assert.AreEqual(data.BoundedStringSequenceField.GetType(), typeof(List<string>));
            Assert.AreEqual(data.UnboundedStringSequenceField.GetType(), typeof(List<string>));
            Assert.AreEqual(data.BoundedWStringSequenceField.GetType(), typeof(List<string>));
            Assert.AreEqual(data.UnboundedWStringSequenceField.GetType(), typeof(List<string>));

            Assert.IsNotNull(defaultStruct.BoundedStringSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedStringSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedStringSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedStringSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedWStringSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedWStringSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedWStringSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedWStringSequenceField.Count);
        }

        /// <summary>
        /// Test the code generated for the array of strings.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStringArrays()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                StringArrayField = new[]
                {
                    "Pressure pushing down on me",
                    "Pressing down on you, no man ask for",
                    "Under pressure that burns a building down",
                    "Splits a family in two",
                    "Puts people on streets",
                },
                WStringArrayField = new[]
                {
                    "Rebel Rebel, you've turn your dress",
                    "Rebel Rebel, your face is a mess",
                    "Rebel Rebel, how could they know?",
                    "Hot tramp,",
                    "I love you so!",
                },
            };

            _dataWriter.Write(data);

            var ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(data.StringArrayField.SequenceEqual(received.StringArrayField));
            Assert.IsTrue(data.WStringArrayField.SequenceEqual(received.WStringArrayField));

            Assert.AreEqual(typeof(string[]), data.StringArrayField.GetType());
            Assert.AreEqual(typeof(string[]), data.WStringArrayField.GetType());

            Assert.IsNotNull(defaultStruct.StringArrayField);
            Assert.AreEqual(5, defaultStruct.StringArrayField.Length);
            foreach (var s in defaultStruct.StringArrayField)
            {
                Assert.AreEqual(string.Empty, s);
            }

            Assert.IsNotNull(defaultStruct.WStringArrayField);
            Assert.AreEqual(5, defaultStruct.WStringArrayField.Length);
            foreach (var s in defaultStruct.WStringArrayField)
            {
                Assert.AreEqual(string.Empty, s);
            }
        }

        /// <summary>
        /// Test the code generated for the multi-array of strings.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStringMultiArrays()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
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
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(TestHelper.CompareMultiArray(data.StringMultiArrayField, received.StringMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.WStringMultiArrayField, received.WStringMultiArrayField));

            Assert.AreEqual(typeof(string[][][]), data.StringMultiArrayField.GetType());
            Assert.AreEqual(typeof(string[][][]), data.WStringMultiArrayField.GetType());

            var defaultArray = new[]
            {
                new[]
                {
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                },
                new[]
                {
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                },
                new[]
                {
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                },
            };

            Assert.IsTrue(TestHelper.CompareMultiArray(defaultStruct.StringMultiArrayField, defaultArray));
            Assert.IsTrue(TestHelper.CompareMultiArray(defaultStruct.WStringMultiArrayField, defaultArray));
        }

        /// <summary>
        /// Test the code generated for the structures types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStructuresTypes()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                NestedStructField = new NestedStruct { Id = 1, Message = "Do androids dream of electric sheep?" },
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);

            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.IsNotNull(received.NestedStructField);
            Assert.AreEqual(data.NestedStructField.Id, received.NestedStructField.Id);
            Assert.AreEqual(data.NestedStructField.Message, received.NestedStructField.Message);

            Assert.AreEqual(typeof(NestedStruct), data.NestedStructField.GetType());

            Assert.IsNotNull(defaultStruct.NestedStructField);
            Assert.AreEqual(0, defaultStruct.NestedStructField.Id);
            Assert.AreEqual(string.Empty, defaultStruct.NestedStructField.Message);
        }

        /// <summary>
        /// Test the code generated for the sequence of structures.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStructureSequences()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                UnboundedStructSequenceField =
                {
                    new NestedStruct { Id = 1, Message = "With your feet in the air and your head on the ground" },
                    new NestedStruct { Id = 2, Message = "Try this trick and spin it, yeah" },
                    new NestedStruct { Id = 3, Message = "Your head will collapse" },
                    new NestedStruct { Id = 4, Message = "But there's nothing in it" },
                    new NestedStruct { Id = 5, Message = "And you'll ask yourself" },
                    new NestedStruct { Id = 6, Message = "Where is my mind?" },
                },
                BoundedStructSequenceField =
                {
                    new NestedStruct { Id = 1, Message = "With your feet in the air and your head on the ground" },
                    new NestedStruct { Id = 2, Message = "Try this trick and spin it, yeah" },
                    new NestedStruct { Id = 3, Message = "Your head will collapse" },
                    new NestedStruct { Id = 4, Message = "But there's nothing in it" },
                    new NestedStruct { Id = 5, Message = "And you'll ask yourself" },
                },
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            for (var i = 1; i < data.BoundedStructSequenceField.Count; i++)
            {
                Assert.AreEqual(data.BoundedStructSequenceField[i].Id, received.BoundedStructSequenceField[i].Id);
                Assert.AreEqual(data.BoundedStructSequenceField[i].Message, received.BoundedStructSequenceField[i].Message);
            }
            for (var i = 1; i < data.BoundedStructSequenceField.Count; i++)
            {
                Assert.AreEqual(data.UnboundedStructSequenceField[i].Id, received.UnboundedStructSequenceField[i].Id);
                Assert.AreEqual(data.UnboundedStructSequenceField[i].Message, received.UnboundedStructSequenceField[i].Message);
            }

            Assert.AreEqual(data.BoundedStructSequenceField.GetType(), typeof(List<NestedStruct>));
            Assert.AreEqual(data.UnboundedStructSequenceField.GetType(), typeof(List<NestedStruct>));

            Assert.IsNotNull(defaultStruct.BoundedStructSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedStructSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedStructSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedStructSequenceField.Count);
        }

        /// <summary>
        /// Test the code generated for the array of structures.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStructureArrays()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                StructArrayField = new[]
                {
                    new NestedStruct { Message = "Pressure pushing down on me", Id = 1 },
                    new NestedStruct { Message = "Pressing down on you, no man ask for", Id = 2 },
                    new NestedStruct { Message = "Under pressure that burns a building down", Id = 3 },
                    new NestedStruct { Message = "Splits a family in two", Id = 4 },
                    new NestedStruct { Message = "Puts people on streets", Id = 5 },
                },
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            for (var i = 0; i < 5; i++)
            {
                Assert.AreEqual(data.StructArrayField[i].Id, received.StructArrayField[i].Id);
                Assert.AreEqual(data.StructArrayField[i].Message, received.StructArrayField[i].Message);
            }

            Assert.AreEqual(typeof(NestedStruct[]), data.StructArrayField.GetType());

            Assert.IsNotNull(defaultStruct.StructArrayField);
            Assert.AreEqual(5, defaultStruct.StructArrayField.Length);
            foreach (var s in defaultStruct.StructArrayField)
            {
                Assert.IsNotNull(s);
            }
        }

        /// <summary>
        /// Test the code generated for the multi-array of structures.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStructureMultiArrays()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
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
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            for (var i0 = 0; i0 < 3; i0++)
            {
                for (var i1 = 0; i1 < 4; i1++)
                {
                    for (var i2 = 0; i2 < 2; i2++)
                    {
                        Assert.AreEqual(data.StructMultiArrayField[i0][i1][i2].Id,
                            received.StructMultiArrayField[i0][i1][i2].Id);
                        Assert.AreEqual(data.StructMultiArrayField[i0][i1][i2].Message,
                            received.StructMultiArrayField[i0][i1][i2].Message);
                    }
                }
            }

            Assert.AreEqual(typeof(NestedStruct[][][]), data.StructMultiArrayField.GetType());

            for (var i0 = 0; i0 < 3; i0++)
            {
                for (var i1 = 0; i1 < 4; i1++)
                {
                    for (var i2 = 0; i2 < 2; i2++)
                    {
                        Assert.IsNotNull(defaultStruct.StructMultiArrayField[i0][i1][i2]);
                    }
                }
            }
        }

        /// <summary>
        /// Test the code generated for the enumerations.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedEnumType()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                TestEnumField = TestEnum.ENUM5,
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.AreEqual(data.TestEnumField, received.TestEnumField);

            Assert.AreEqual(typeof(TestEnum), data.TestEnumField.GetType());

            Assert.AreEqual(TestEnum.ENUM1, defaultStruct.TestEnumField);
        }

        /// <summary>
        /// Test the code generated for the sequence of enumerations.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedEnumSequences()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                UnboundedEnumSequenceField =
                {
                    TestEnum.ENUM10,
                    TestEnum.ENUM9,
                    TestEnum.ENUM8,
                    TestEnum.ENUM7,
                    TestEnum.ENUM6,
                    TestEnum.ENUM5,
                },
                BoundedEnumSequenceField =
                {
                    TestEnum.ENUM1,
                    TestEnum.ENUM2,
                    TestEnum.ENUM3,
                    TestEnum.ENUM4,
                    TestEnum.ENUM5,
                },
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(data.BoundedEnumSequenceField.SequenceEqual(received.BoundedEnumSequenceField));
            Assert.IsTrue(data.UnboundedEnumSequenceField.SequenceEqual(received.UnboundedEnumSequenceField));

            Assert.AreEqual(data.BoundedEnumSequenceField.GetType(), typeof(List<TestEnum>));
            Assert.AreEqual(data.UnboundedEnumSequenceField.GetType(), typeof(List<TestEnum>));

            Assert.IsNotNull(defaultStruct.BoundedEnumSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedEnumSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedEnumSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedEnumSequenceField.Count);
        }

        /// <summary>
        /// Test the code generated for the array of enumerations.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedEnumArrays()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
                EnumArrayField = new[]
                {
                    TestEnum.ENUM1,
                    TestEnum.ENUM3,
                    TestEnum.ENUM5,
                    TestEnum.ENUM7,
                    TestEnum.ENUM11,
                },
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(data.EnumArrayField.SequenceEqual(received.EnumArrayField));

            Assert.AreEqual(typeof(TestEnum[]), data.EnumArrayField.GetType());

            Assert.IsNotNull(defaultStruct.EnumArrayField);
            Assert.AreEqual(5, defaultStruct.EnumArrayField.Length);
            foreach (var s in defaultStruct.EnumArrayField)
            {
                Assert.AreEqual(default, s);
            }
        }

        /// <summary>
        /// Test the code generated for the multi-array of enumerations.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedEnumMultiArrays()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestStruct();

            var data = new TestStruct
            {
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

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestStruct();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(TestHelper.CompareMultiArray(data.EnumMultiArrayField, received.EnumMultiArrayField));

            Assert.AreEqual(typeof(TestEnum[][][]), data.EnumMultiArrayField.GetType());

            var defaultArray = new[]
            {
                new[]
                {
                    new TestEnum[] { default, default },
                    new TestEnum[] { default, default },
                    new TestEnum[] { default, default },
                    new TestEnum[] { default, default },
                },
                new[]
                {
                    new TestEnum[] { default, default },
                    new TestEnum[] { default, default },
                    new TestEnum[] { default, default },
                    new TestEnum[] { default, default },
                },
                new[]
                {
                    new TestEnum[] { default, default },
                    new TestEnum[] { default, default },
                    new TestEnum[] { default, default },
                    new TestEnum[] { default, default },
                },
            };

            Assert.IsNotNull(defaultStruct.EnumMultiArrayField);
            Assert.IsTrue(TestHelper.CompareMultiArray(defaultStruct.EnumMultiArrayField, defaultArray));
        }

        /// <summary>
        /// Test the code generated for the constants.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedConstants()
        {
            Assert.AreEqual(typeof(short), TEST_SHORT_CONST.Value.GetType());
            Assert.AreEqual(typeof(int), TEST_LONG_CONST.Value.GetType());
            Assert.AreEqual(typeof(long), TEST_LONGLONG_CONST.Value.GetType());
            Assert.AreEqual(typeof(ushort), TEST_USHORT_CONST.Value.GetType());
            Assert.AreEqual(typeof(uint), TEST_ULONG_CONST.Value.GetType());
            Assert.AreEqual(typeof(ulong), TEST_ULONGLONG_CONST.Value.GetType());
            Assert.AreEqual(typeof(char), TEST_CHAR_CONST.Value.GetType());
            Assert.AreEqual(typeof(char), TEST_WCHAR_CONST.Value.GetType());
            Assert.AreEqual(typeof(bool), TEST_BOOLEAN_CONST.Value.GetType());
            Assert.AreEqual(typeof(byte), TEST_OCTET_CONST.Value.GetType());
            Assert.AreEqual(typeof(float), TEST_FLOAT_CONST.Value.GetType());
            Assert.AreEqual(typeof(double), TEST_DOUBLE_CONST.Value.GetType());
            Assert.AreEqual(typeof(TestEnum), TEST_ENUM_CONST.Value.GetType());

            Assert.AreEqual(-1, TEST_SHORT_CONST.Value);
            Assert.AreEqual((ushort)1, TEST_USHORT_CONST.Value);
            Assert.AreEqual(-2, TEST_LONG_CONST.Value);
            Assert.AreEqual(2U, TEST_ULONG_CONST.Value);
            Assert.AreEqual(-3L, TEST_LONGLONG_CONST.Value);
            Assert.AreEqual(3UL, TEST_ULONGLONG_CONST.Value);
            Assert.AreEqual(4.1f, TEST_FLOAT_CONST.Value);
            Assert.AreEqual(5.1, TEST_DOUBLE_CONST.Value);
            Assert.AreEqual('X', TEST_CHAR_CONST.Value);
            Assert.AreEqual('S', TEST_WCHAR_CONST.Value);
            Assert.AreEqual(0x42, TEST_OCTET_CONST.Value);
            Assert.IsTrue(TEST_BOOLEAN_CONST.Value);
            Assert.AreEqual("Hello, I love you, won't you tell me your name?", TEST_STRING_CONST.Value);
            Assert.AreEqual("Hello, I love you, won't you tell me your name?", TEST_WSTRING_CONST.Value);
            Assert.AreEqual(TestEnum.ENUM6, TEST_ENUM_CONST.Value);
        }
        #endregion
    }
}
