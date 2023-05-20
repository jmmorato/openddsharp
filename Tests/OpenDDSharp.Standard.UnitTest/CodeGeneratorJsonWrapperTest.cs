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
using System.Linq;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.Standard.UnitTest.Helpers;

namespace OpenDDSharp.Standard.UnitTest
{
    /// <summary>
    /// Code generator unit test class.
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

            TestStructTypeSupport typeSupport = new TestStructTypeSupport();
            string typeName = typeSupport.GetTypeName();
            ReturnCode ret = typeSupport.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, ret);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
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
        /// Test the code generated for the basic types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedBasicTypes()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
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

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

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
            // TODO: Bounded lists that exceed the bound throws an exception.
            // As per documentation: Bounds checking on bounded sequences may raise an exception if necessary.
            // Check bound before toNative and throw a user friendly exception.
            // Another option is to implement an internal BoundedList that check the bound before Add/Insert.
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
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
                UnboundedDoubleSequenceField = { 1d, -2.6d, 3.7d, -100.8d, 200.9d, -300.02d, 1000.1d },
                // //BoundedLongDoubleSequenceField = { -1.0m, 2.1m, -3.2m, 100.3m, -200.4m },
                // UnboundedLongDoubleSequenceField = { 1.5m, -2.6m, 3.7m, -100.8m, 200.9m, -300.0m, 1000.1m },
            };
            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
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
            //Assert.IsTrue(data.BoundedLongDoubleSequenceField.SequenceEqual(received.BoundedLongDoubleSequenceField));
            // Assert.IsTrue(data.UnboundedLongDoubleSequenceField.SequenceEqual(received.UnboundedLongDoubleSequenceField));

            Assert.IsTrue(data.BoundedBooleanSequenceField is IList<bool>);
            Assert.IsTrue(data.UnboundedBooleanSequenceField is IList<bool>);
            Assert.IsTrue(data.BoundedCharSequenceField is IList<char>);
            Assert.IsTrue(data.UnboundedCharSequenceField is IList<char>);
            Assert.IsTrue(data.BoundedWCharSequenceField is IList<char>);
            Assert.IsTrue(data.UnboundedWCharSequenceField is IList<char>);
            Assert.IsTrue(data.BoundedOctetSequenceField is IList<byte>);
            Assert.IsTrue(data.UnboundedOctetSequenceField is IList<byte>);
            Assert.IsTrue(data.BoundedShortSequenceField is IList<short>);
            Assert.IsTrue(data.UnboundedShortSequenceField is IList<short>);
            Assert.IsTrue(data.BoundedUShortSequenceField is IList<ushort>);
            Assert.IsTrue(data.UnboundedUShortSequenceField is IList<ushort>);
            Assert.IsTrue(data.BoundedLongSequenceField is IList<int>);
            Assert.IsTrue(data.UnboundedLongSequenceField is IList<int>);
            Assert.IsTrue(data.BoundedULongSequenceField is IList<uint>);
            Assert.IsTrue(data.UnboundedULongSequenceField is IList<uint>);
            Assert.IsTrue(data.BoundedLongLongSequenceField is IList<long>);
            Assert.IsTrue(data.UnboundedLongLongSequenceField is IList<long>);
            Assert.IsTrue(data.BoundedULongLongSequenceField is IList<ulong>);
            Assert.IsTrue(data.UnboundedULongLongSequenceField is IList<ulong>);
            Assert.IsTrue(data.BoundedFloatSequenceField is IList<float>);
            Assert.IsTrue(data.UnboundedFloatSequenceField is IList<float>);
            Assert.IsTrue(data.BoundedDoubleSequenceField is IList<double>);
            Assert.IsTrue(data.UnboundedDoubleSequenceField is IList<double>);
            //Assert.IsTrue(data.BoundedLongDoubleSequenceField is IList<decimal>);
            // Assert.IsTrue(data.UnboundedLongDoubleSequenceField is IList<decimal>);

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
            //Assert.IsNotNull(defaultStruct.BoundedLongDoubleSequenceField);
            //Assert.AreEqual(0, defaultStruct.BoundedLongDoubleSequenceField.Count);
            // Assert.IsNotNull(defaultStruct.UnboundedLongDoubleSequenceField);
            // Assert.AreEqual(0, defaultStruct.UnboundedLongDoubleSequenceField.Count);
        }

        /// <summary>
        /// Test the code generated for the arrays of basic types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedBasicTypeArrays()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
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
            };
            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);
            SampleInfo sampleInfo = new SampleInfo();
            TestStruct received = new TestStruct();
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
        }

        // /// <summary>
        // /// Test the code generated for the multiarray of basic types.
        // /// </summary>
        // [TestMethod]
        // [TestCategory(TEST_CATEGORY)]
        // public void TestGeneratedBasicTypeMultiArrays()
        // {
        //     TestStruct defaultStruct = new TestStruct();
        //
        //     TestStruct data = new TestStruct
        //     {
        //         BooleanMultiArrayField = new[,,]
        //         {
        //             {
        //                 { true, false },
        //                 { true, false },
        //                 { true, false },
        //                 { true, false },
        //             },
        //             {
        //                 { true, false },
        //                 { true, false },
        //                 { true, false },
        //                 { true, false },
        //             },
        //             {
        //                 { true, false },
        //                 { true, false },
        //                 { true, false },
        //                 { true, false },
        //             }
        //         },
        //         CharMultiArrayField = new[,,]
        //         {
        //             {
        //                 { '1', '2' },
        //                 { '3', '4' },
        //                 { '5', '6' },
        //                 { '7', '8' },
        //             },
        //             {
        //                 { '9', '0' },
        //                 { '1', '2' },
        //                 { '3', '4' },
        //                 { '5', '6' },
        //             },
        //             {
        //                 { '7', '8' },
        //                 { '9', '0' },
        //                 { '1', '2' },
        //                 { '3', '4' },
        //             }
        //         },
        //         WCharMultiArrayField = new[,,]
        //         {
        //             {
        //                 { '1', '2' },
        //                 { '3', '4' },
        //                 { '5', '6' },
        //                 { '7', '8' }
        //             },
        //             {
        //                 { '9', '0' },
        //                 { '1', '2' },
        //                 { '3', '4' },
        //                 { '5', '6' },
        //             },
        //             {
        //                 { '7', '8' },
        //                 { '9', '0' },
        //                 { '1', '2' },
        //                 { '3', '4' },
        //             }
        //         },
        //         OctetMultiArrayField = new byte[,,]
        //         {
        //             {
        //                 { 01, 02 },
        //                 { 03, 04 },
        //                 { 05, 06 },
        //                 { 07, 08 },
        //             },
        //             {
        //                 { 09, 10 },
        //                 { 11, 12 },
        //                 { 13, 14 },
        //                 { 15, 16 },
        //             },
        //             {
        //                 { 17, 18 },
        //                 { 19, 20 },
        //                 { 21, 22 },
        //                 { 23, 24 },
        //             }
        //         },
        //         ShortMultiArrayField = new short[,,]
        //         {
        //             {
        //                 { -01, -02 },
        //                 { -03, -04 },
        //                 { -05, -06 },
        //                 { -07, -08 },
        //             },
        //             {
        //                 { -09, -10 },
        //                 { -11, -12 },
        //                 { -13, -14 },
        //                 { -15, -16 },
        //             },
        //             {
        //                 { -17, -18 },
        //                 { -19, -20 },
        //                 { -21, -22 },
        //                 { -23, -24 },
        //             }
        //         },
        //         UnsignedShortMultiArrayField = new ushort[,,]
        //         {
        //             {
        //                 { 01, 02 },
        //                 { 03, 04 },
        //                 { 05, 06 },
        //                 { 07, 08 },
        //             },
        //             {
        //                 { 09, 10 },
        //                 { 11, 12 },
        //                 { 13, 14 },
        //                 { 15, 16 },
        //             },
        //             {
        //                 { 17, 18 },
        //                 { 19, 20 },
        //                 { 21, 22 },
        //                 { 23, 24 },
        //             }
        //         },
        //         LongMultiArrayField = new[,,]
        //         {
        //             {
        //                 { -01, 02 },
        //                 { -03, 04 },
        //                 { -05, 06 },
        //                 { -07, 08 },
        //             },
        //             {
        //                 { -09, 10 },
        //                 { -11, 12 },
        //                 { -13, 14 },
        //                 { -15, 16 },
        //             },
        //             {
        //                 { -17, 18 },
        //                 { -19, 20 },
        //                 { -21, 22 },
        //                 { -23, 24 },
        //             }
        //         },
        //         UnsignedLongMultiArrayField = new[,,]
        //         {
        //             {
        //                 { 25U, 26U },
        //                 { 27U, 28U },
        //                 { 29U, 30U },
        //                 { 31U, 32U },
        //             },
        //             {
        //                 { 33U, 34U },
        //                 { 35U, 36U },
        //                 { 37U, 38U },
        //                 { 39U, 40U },
        //             },
        //             {
        //                 { 41U, 42U },
        //                 { 43U, 44U },
        //                 { 45U, 46U },
        //                 { 47U, 48U },
        //             }
        //         },
        //         LongLongMultiArrayField = new[,,]
        //         {
        //             {
        //                 { -25L, -26L },
        //                 { -27L, -28L },
        //                 { -29L, -30L },
        //                 { -31L, -32L },
        //             },
        //             {
        //                 { -33L, -34L },
        //                 { -35L, -36L },
        //                 { -37L, -38L },
        //                 { -39L, -40L },
        //             },
        //             {
        //                 { -41L, -42L },
        //                 { -43L, -44L },
        //                 { -45L, -46L },
        //                 { -47L, -48L },
        //             }
        //         },
        //         UnsignedLongLongMultiArrayField = new[,,]
        //         {
        //             {
        //                 { 49UL, 50UL },
        //                 { 51UL, 52UL },
        //                 { 53UL, 54UL },
        //                 { 55UL, 56UL },
        //             },
        //             {
        //                 { 57UL, 58UL },
        //                 { 59UL, 60UL },
        //                 { 61UL, 62UL },
        //                 { 63UL, 64UL },
        //             },
        //             {
        //                 { 65UL, 66UL },
        //                 { 67UL, 68UL },
        //                 { 69UL, 70UL },
        //                 { 71UL, 72UL },
        //             }
        //         },
        //         FloatMultiArrayField = new[,,]
        //         {
        //             {
        //                 { 01.01f, 02.02f },
        //                 { 03.03f, 04.04f },
        //                 { 05.05f, 06.06f },
        //                 { 07.07f, 08.08f }
        //             },
        //             {
        //                 { 09.09f, 10.10f },
        //                 { 11.11f, 12.12f },
        //                 { 13.13f, 14.14f },
        //                 { 15.15f, 16.16f },
        //             },
        //             {
        //                 { 17.17f, 18.18f },
        //                 { 19.19f, 20.20f },
        //                 { 21.21f, 22.22f },
        //                 { 23.23f, 24.24f },
        //             }
        //         },
        //         DoubleMultiArrayField = new[,,]
        //         {
        //             {
        //                 { 01.01, 02.02 },
        //                 { 03.03, 04.04 },
        //                 { 05.05, 06.06 },
        //                 { 07.07, 08.08 },
        //             },
        //             {
        //                 { 09.09, 10.10 },
        //                 { 11.11, 12.12 },
        //                 { 13.13, 14.14 },
        //                 { 15.15, 16.16 },
        //             },
        //             {
        //                 { 17.17, 18.18 },
        //                 { 19.19, 20.20 },
        //                 { 21.21, 22.22 },
        //                 { 23.23, 24.24 },
        //             }
        //         },
        //         //LongDoubleMultiArrayField = new[,,]
        //         //{
        //         //    {
        //         //        { 01.01m, 02.02m },
        //         //        { 03.03m, 04.04m },
        //         //        { 05.05m, 06.06m },
        //         //        { 07.07m, 08.08m },
        //         //    },
        //         //    {
        //         //        { 09.09m, 10.10m },
        //         //        { 11.11m, 12.12m },
        //         //        { 13.13m, 14.14m },
        //         //        { 15.15m, 16.16m },
        //         //    },
        //         //    {
        //         //        { 17.17m, 18.18m },
        //         //        { 19.19m, 20.20m },
        //         //        { 21.21m, 22.22m },
        //         //        { 23.23m, 24.24m },
        //         //    }
        //         //},
        //     };
        //     _dataWriter.Write(data);
        //
        //     var ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });            
        //     Assert.AreEqual(ReturnCode.Ok, ret);
        //
        //     // To allow to change the thread context.
        //     System.Threading.Thread.Sleep(10);
        //
        //     TestStruct received = new TestStruct();
        //     SampleInfo sampleInfo = new SampleInfo();
        //     ret = _dataReader.ReadNextSample(received, sampleInfo);
        //     Assert.AreEqual(ReturnCode.Ok, ret);
        //
        //     Assert.IsTrue(CompareMultiArray(data.BooleanMultiArrayField, received.BooleanMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.CharMultiArrayField, received.CharMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.WCharMultiArrayField, received.WCharMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.OctetMultiArrayField, received.OctetMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.ShortMultiArrayField, received.ShortMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.UnsignedShortMultiArrayField, received.UnsignedShortMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.LongMultiArrayField, received.LongMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.UnsignedLongMultiArrayField, received.UnsignedLongMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.LongLongMultiArrayField, received.LongLongMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.UnsignedLongLongMultiArrayField, received.UnsignedLongLongMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.FloatMultiArrayField, received.FloatMultiArrayField));
        //     Assert.IsTrue(CompareMultiArray(data.DoubleMultiArrayField, received.DoubleMultiArrayField));
        //     //Assert.IsTrue(CompareMultiArray(data.LongDoubleMultiArrayField, received.LongDoubleMultiArrayField));
        //
        //     Assert.AreEqual(typeof(bool[,,]), data.BooleanMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(char[,,]), data.CharMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(char[,,]), data.WCharMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(byte[,,]), data.OctetMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(short[,,]), data.ShortMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(ushort[,,]), data.UnsignedShortMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(int[,,]), data.LongMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(uint[,,]), data.UnsignedLongMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(long[,,]), data.LongLongMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(ulong[,,]), data.UnsignedLongLongMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(float[,,]), data.FloatMultiArrayField.GetType());
        //     Assert.AreEqual(typeof(double[,,]), data.DoubleMultiArrayField.GetType());
        //     //Assert.AreEqual(typeof(decimal[,,]), data.LongDoubleMultiArrayField.GetType());
        //
        //     Assert.IsNotNull(defaultStruct.BooleanMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.BooleanMultiArrayField.Length);
        //     foreach (var i in defaultStruct.BooleanMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.CharMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.CharMultiArrayField.Length);
        //     foreach (var i in defaultStruct.CharMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.WCharMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.WCharMultiArrayField.Length);
        //     foreach (var i in defaultStruct.WCharMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.OctetMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.OctetMultiArrayField.Length);
        //     foreach (var i in defaultStruct.OctetMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.ShortMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.ShortMultiArrayField.Length);
        //     foreach (var i in defaultStruct.ShortMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.UnsignedShortMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.UnsignedShortMultiArrayField.Length);
        //     foreach (var i in defaultStruct.UnsignedShortMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.LongMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.LongMultiArrayField.Length);
        //     foreach (var i in defaultStruct.LongMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.UnsignedLongMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.UnsignedLongMultiArrayField.Length);
        //     foreach (var i in defaultStruct.UnsignedLongMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.LongLongMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.LongLongMultiArrayField.Length);
        //     foreach (var i in defaultStruct.LongLongMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.UnsignedLongLongMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.UnsignedLongLongMultiArrayField.Length);
        //     foreach (var i in defaultStruct.UnsignedLongLongMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.FloatMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.FloatMultiArrayField.Length);
        //     foreach (var i in defaultStruct.FloatMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     Assert.IsNotNull(defaultStruct.DoubleMultiArrayField);
        //     Assert.AreEqual(24, defaultStruct.DoubleMultiArrayField.Length);
        //     foreach (var i in defaultStruct.DoubleMultiArrayField)
        //     {
        //         Assert.AreEqual(default, i);
        //     }
        //
        //     //Assert.IsNotNull(defaultStruct.LongDoubleMultiArrayField);
        //     //Assert.AreEqual(24, defaultStruct.LongDoubleMultiArrayField.Length);
        //     //foreach (var i in defaultStruct.LongDoubleMultiArrayField)
        //     //{
        //     //    Assert.AreEqual(default, i);
        //     //}
        // }

        /// <summary>
        /// Test the code generated for the string types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStringTypes()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
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

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
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
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
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

            _dataWriter.Write(data);

            var ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(data.BoundedStringSequenceField.SequenceEqual(received.BoundedStringSequenceField));
            Assert.IsTrue(data.UnboundedStringSequenceField.SequenceEqual(received.UnboundedStringSequenceField));
            Assert.IsTrue(data.BoundedWStringSequenceField.SequenceEqual(received.BoundedWStringSequenceField));
            Assert.IsTrue(data.UnboundedWStringSequenceField.SequenceEqual(received.UnboundedWStringSequenceField));

            Assert.IsTrue(data.BoundedStringSequenceField is IList<string>);
            Assert.IsTrue(data.UnboundedStringSequenceField is IList<string>);
            Assert.IsTrue(data.BoundedWStringSequenceField is IList<string>);
            Assert.IsTrue(data.UnboundedWStringSequenceField is IList<string>);

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
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
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

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
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

        // /// <summary>
        // /// Test the code generated for the multiarray of strings.
        // /// </summary>
        // [TestMethod]
        // [TestCategory(TEST_CATEGORY)]
        // public void TestGeneratedStringMultiArrays()
        // {
        //     TestStruct defaultStruct = new TestStruct();
        //
        //     TestStruct data = new TestStruct
        //     {
        //         // StringMultiArrayField = new[,,]
        //         // {
        //         //     {
        //         //         { "01", "02" },
        //         //         { "03", "04" },
        //         //         { "05", "06" },
        //         //         { "07", "08" },
        //         //     },
        //         //     {
        //         //         { "09", "10" },
        //         //         { "11", "12" },
        //         //         { "13", "14" },
        //         //         { "15", "16" },
        //         //     },
        //         //     {
        //         //         { "17", "18" },
        //         //         { "19", "20" },
        //         //         { "21", "22" },
        //         //         { "23", "24" },
        //         //     },
        //         // },
        //         // WStringMultiArrayField = new[,,]
        //         // {
        //         //     {
        //         //         { "01", "02" },
        //         //         { "03", "04" },
        //         //         { "05", "06" },
        //         //         { "07", "08" },
        //         //     },
        //         //     {
        //         //         { "09", "10" },
        //         //         { "11", "12" },
        //         //         { "13", "14" },
        //         //         { "15", "16" },
        //         //     },
        //         //     {
        //         //         { "17", "18" },
        //         //         { "19", "20" },
        //         //         { "21", "22" },
        //         //         { "23", "24" },
        //         //     },
        //         // },
        //     };
        //
        //     _dataWriter.Write(data);
        //
        //     var ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });            
        //     Assert.AreEqual(ReturnCode.Ok, ret);
        //
        //     // To allow to change the thread context.
        //     System.Threading.Thread.Sleep(10);
        //
        //     TestStruct received = new TestStruct();
        //     SampleInfo sampleInfo = new SampleInfo();
        //     ret = _dataReader.ReadNextSample(received, sampleInfo);
        //     Assert.AreEqual(ReturnCode.Ok, ret);
        //
        //     // Assert.IsTrue(CompareMultiArray(data.StringMultiArrayField, received.StringMultiArrayField));
        //     // Assert.IsTrue(CompareMultiArray(data.WStringMultiArrayField, received.WStringMultiArrayField));
        //     //
        //     // Assert.AreEqual(typeof(string[,,]), data.StringMultiArrayField.GetType());
        //     // Assert.AreEqual(typeof(string[,,]), data.WStringMultiArrayField.GetType());
        //     //
        //     // Assert.IsNotNull(defaultStruct.StringMultiArrayField);
        //     // Assert.AreEqual(24, defaultStruct.StringMultiArrayField.Length);
        //     // foreach (var s in defaultStruct.StringMultiArrayField)
        //     // {
        //     //     Assert.AreEqual(string.Empty, s);
        //     // }
        //     //
        //     // Assert.IsNotNull(defaultStruct.WStringMultiArrayField);
        //     // Assert.AreEqual(24, defaultStruct.WStringMultiArrayField.Length);
        //     // foreach (var s in defaultStruct.WStringMultiArrayField)
        //     // {
        //     //     Assert.AreEqual(string.Empty, s);
        //     // }
        // }

        /// <summary>
        /// Test the code generated for the structures types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStructuresTypes()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                NestedStructField = new NestedStruct { Id = 1, Message = "Do androids dream of electric sheep?" },
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
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
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
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
                }
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            for (int i = 1; i < data.BoundedStructSequenceField.Count; i++)
            {
                Assert.AreEqual(data.BoundedStructSequenceField[i].Id, received.BoundedStructSequenceField[i].Id);
                Assert.AreEqual(data.BoundedStructSequenceField[i].Message, received.BoundedStructSequenceField[i].Message);
            }
            for (int i = 1; i < data.BoundedStructSequenceField.Count; i++)
            {
                Assert.AreEqual(data.UnboundedStructSequenceField[i].Id, received.UnboundedStructSequenceField[i].Id);
                Assert.AreEqual(data.UnboundedStructSequenceField[i].Message, received.UnboundedStructSequenceField[i].Message);
            }

            Assert.IsTrue(data.BoundedStructSequenceField is IList<NestedStruct>);
            Assert.IsTrue(data.UnboundedStructSequenceField is IList<NestedStruct>);

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
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                StructArrayField = new NestedStruct[]
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

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            for (int i = 0; i < 5; i++)
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

        // /// <summary>
        // /// Test the code generated for the multiarray of structures.
        // /// </summary>
        // [TestMethod]
        // [TestCategory(TEST_CATEGORY)]
        // public void TestGeneratedStructureMultiArrays()
        // {
        //     TestStruct defaultStruct = new TestStruct();
        //
        //     TestStruct data = new TestStruct
        //     {
        //         // StructMultiArrayField = new NestedStruct[,,]
        //         // {
        //         //     {
        //         //         { new NestedStruct{ Id = 1, Message = "01" }, new NestedStruct{ Id = 2, Message = "02" } },
        //         //         { new NestedStruct{ Id = 3, Message = "03" }, new NestedStruct{ Id = 4, Message = "04" } },
        //         //         { new NestedStruct{ Id = 5, Message = "05" }, new NestedStruct{ Id = 6, Message = "06" } },
        //         //         { new NestedStruct{ Id = 7, Message = "07" }, new NestedStruct{ Id = 8, Message = "08" } },
        //         //     },
        //         //     {
        //         //         { new NestedStruct{ Id = 9, Message = "09" }, new NestedStruct{ Id = 10, Message = "10" } },
        //         //         { new NestedStruct{ Id = 11, Message = "11" }, new NestedStruct{ Id = 12, Message = "12" } },
        //         //         { new NestedStruct{ Id = 13, Message = "13" }, new NestedStruct{ Id = 14, Message = "14" } },
        //         //         { new NestedStruct{ Id = 15, Message = "15" }, new NestedStruct{ Id = 16, Message = "16" } },
        //         //     },
        //         //     {
        //         //         { new NestedStruct{ Id = 17, Message = "17" }, new NestedStruct{ Id = 18, Message = "18" } },
        //         //         { new NestedStruct{ Id = 19, Message = "19" }, new NestedStruct{ Id = 20, Message = "20" } },
        //         //         { new NestedStruct{ Id = 21, Message = "21" }, new NestedStruct{ Id = 22, Message = "22" } },
        //         //         { new NestedStruct{ Id = 23, Message = "23" }, new NestedStruct{ Id = 24, Message = "24" } },
        //         //     },
        //         // },
        //     };
        //
        //     _dataWriter.Write(data);
        //
        //     var ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
        //     Assert.AreEqual(ReturnCode.Ok, ret);
        //
        //     // To allow to change the thread context.
        //     System.Threading.Thread.Sleep(10);
        //
        //     TestStruct received = new TestStruct();
        //     SampleInfo sampleInfo = new SampleInfo();
        //     ret = _dataReader.ReadNextSample(received, sampleInfo);
        //
        //     // Assert.AreEqual(ReturnCode.Ok, ret);
        //     // for (int i0 = 0; i0 < 3; i0++)
        //     // {
        //     //     for (int i1 = 0; i1 < 4; i1++)
        //     //     {
        //     //         for (int i2 = 0; i2 < 2; i2++)
        //     //         {
        //     //             Assert.AreEqual(data.StructMultiArrayField[i0, i1, i2].Id, received.StructMultiArrayField[i0, i1, i2].Id);
        //     //             Assert.AreEqual(data.StructMultiArrayField[i0, i1, i2].Message, received.StructMultiArrayField[i0, i1, i2].Message);
        //     //         }
        //     //     }
        //     // }
        //     //
        //     // Assert.AreEqual(typeof(NestedStruct[,,]), data.StructMultiArrayField.GetType());
        //     //
        //     // Assert.IsNotNull(defaultStruct.StructMultiArrayField);
        //     // Assert.AreEqual(24, defaultStruct.StructMultiArrayField.Length);
        //     // foreach (var s in defaultStruct.StructMultiArrayField)
        //     // {
        //     //     Assert.IsNotNull(s);
        //     // }
        // }

        /// <summary>
        /// Test the code generated for the enumerations.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedEnumType()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                TestEnumField = TestEnum.ENUM5,
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
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
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                UnboundedEnumSequenceField = { TestEnum.ENUM10, TestEnum.ENUM9, TestEnum.ENUM8, TestEnum.ENUM7, TestEnum.ENUM6, TestEnum.ENUM5 },
                BoundedEnumSequenceField = { TestEnum.ENUM1, TestEnum.ENUM2, TestEnum.ENUM3, TestEnum.ENUM4, TestEnum.ENUM5 },
            };

            _dataWriter.Write(data);

            var ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(data.BoundedEnumSequenceField.SequenceEqual(received.BoundedEnumSequenceField));
            Assert.IsTrue(data.UnboundedEnumSequenceField.SequenceEqual(received.UnboundedEnumSequenceField));

            Assert.IsTrue(data.BoundedEnumSequenceField is IList<TestEnum>);
            Assert.IsTrue(data.UnboundedEnumSequenceField is IList<TestEnum>);

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
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                EnumArrayField = new TestEnum[] { TestEnum.ENUM1, TestEnum.ENUM3, TestEnum.ENUM5, TestEnum.ENUM7, TestEnum.ENUM11 },
            };

            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            // To allow to change the thread context.
            System.Threading.Thread.Sleep(10);

            TestStruct received = new TestStruct();
            SampleInfo sampleInfo = new SampleInfo();
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

        // /// <summary>
        // /// Test the code generated for the multiarray of enumerations.
        // /// </summary>
        // [TestMethod]
        // [TestCategory(TEST_CATEGORY)]
        // public void TestGeneratedEnumMultiArrays()
        // {
        //     TestStruct defaultStruct = new TestStruct();
        //
        //     TestStruct data = new TestStruct
        //     {
        //         // EnumMultiArrayField = new TestEnum[,,]
        //         // {
        //         //     {
        //         //         { TestEnum.ENUM1, TestEnum.ENUM2 },
        //         //         { TestEnum.ENUM3, TestEnum.ENUM4 },
        //         //         { TestEnum.ENUM5, TestEnum.ENUM6 },
        //         //         { TestEnum.ENUM7, TestEnum.ENUM8 },
        //         //     },
        //         //     {
        //         //         { TestEnum.ENUM9, TestEnum.ENUM10 },
        //         //         { TestEnum.ENUM11, TestEnum.ENUM12 },
        //         //         { TestEnum.ENUM1, TestEnum.ENUM2 },
        //         //         { TestEnum.ENUM3, TestEnum.ENUM4 },
        //         //     },
        //         //     {
        //         //         { TestEnum.ENUM5, TestEnum.ENUM6 },
        //         //         { TestEnum.ENUM7, TestEnum.ENUM8 },
        //         //         { TestEnum.ENUM9, TestEnum.ENUM10 },
        //         //         { TestEnum.ENUM11, TestEnum.ENUM12 },
        //         //     },
        //         // },
        //     };
        //
        //     _dataWriter.Write(data);
        //
        //     var ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
        //     Assert.AreEqual(ReturnCode.Ok, ret);
        //
        //     // To allow to change the thread context.
        //     System.Threading.Thread.Sleep(10);
        //
        //     TestStruct received = new TestStruct();
        //     SampleInfo sampleInfo = new SampleInfo();
        //     ret = _dataReader.ReadNextSample(received, sampleInfo);
        //     Assert.AreEqual(ReturnCode.Ok, ret);
        //
        //     // Assert.IsTrue(CompareMultiArray(data.EnumMultiArrayField, received.EnumMultiArrayField));
        //     //
        //     // Assert.AreEqual(typeof(TestEnum[,,]), data.EnumMultiArrayField.GetType());
        //     //
        //     // Assert.IsNotNull(defaultStruct.EnumMultiArrayField);
        //     // Assert.AreEqual(24, defaultStruct.EnumMultiArrayField.Length);
        //     // foreach (var s in defaultStruct.EnumMultiArrayField)
        //     // {
        //     //     Assert.AreEqual(default, s);
        //     // }
        // }

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

        #region Methods
        private bool CompareMultiArray<T>(T[,,] data1, T[,,] data2)
        {
            return data1.Rank == data2.Rank &&
                   Enumerable.Range(0, data1.Rank).All(dimension => data1.GetLength(dimension) == data2.GetLength(dimension)) &&
                   data1.Cast<T>().SequenceEqual(data2.Cast<T>());
        }
        #endregion
    }
}
