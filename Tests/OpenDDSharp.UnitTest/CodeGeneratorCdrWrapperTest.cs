using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using CdrWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// Code generator unit test class.
    /// </summary>
    [TestClass]
    public class CodeGeneratorCdrWrapperTest
    {
        #region Constants
        private const string TEST_CATEGORY = "CodeGenerator";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Publisher _publisher;
        private Subscriber _subscriber;
        private Topic _topic;
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
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
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
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the code generated for the primitives types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedPrimitivesTypes()
        {
            using var evt = new ManualResetEventSlim(false);

            var typeSupport = new TestPrimitiveTypeSupport();
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
            var dr = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(dr);
            var dataReader = new TestPrimitiveDataReader(dr);

            var dw = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(dw);
            var dataWriter = new TestPrimitiveDataWriter(dw);

            Assert.IsTrue(dataWriter.WaitForSubscriptions(1, 5000));
            Assert.IsTrue(dataReader.WaitForPublications(1, 5000));

            var statusCondition = dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestPrimitive();

            var data = new TestPrimitive
            {
                Int16Field = -11,
                Int32Field = 22,
                Int64Field = -33,
                BoolField = true,
                ByteField = 0x42,
                UInt16Field = 11,
                UInt32Field = 22,
                UInt64Field = 33,
                FloatField = 42.42f,
                DoubleField = -23.23,
                CharField = 'A',
                WCharField = 'あ',
            };
            ret = dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestPrimitive();
            var sampleInfo = new SampleInfo();
            ret = dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.AreEqual(data.Int16Field, received.Int16Field);
            Assert.AreEqual(data.Int32Field, received.Int32Field);
            Assert.AreEqual(data.Int64Field, received.Int64Field);
            Assert.AreEqual(data.BoolField, received.BoolField);
            Assert.AreEqual(data.ByteField, received.ByteField);
            Assert.AreEqual(data.UInt16Field, received.UInt16Field);
            Assert.AreEqual(data.UInt32Field, received.UInt32Field);
            Assert.AreEqual(data.UInt64Field, received.UInt64Field);
            Assert.AreEqual(data.FloatField, received.FloatField);
            Assert.AreEqual(data.DoubleField, received.DoubleField);
            Assert.AreEqual(data.CharField, received.CharField);
            Assert.AreEqual(data.WCharField, received.WCharField);

            Assert.AreEqual(typeof(short), data.Int16Field.GetType());
            Assert.AreEqual(typeof(int), data.Int32Field.GetType());
            Assert.AreEqual(typeof(long), data.Int64Field.GetType());
            Assert.AreEqual(typeof(bool), data.BoolField.GetType());
            Assert.AreEqual(typeof(byte), data.ByteField.GetType());
            Assert.AreEqual(typeof(ushort), data.UInt16Field.GetType());
            Assert.AreEqual(typeof(uint), data.UInt32Field.GetType());
            Assert.AreEqual(typeof(ulong), data.UInt64Field.GetType());
            Assert.AreEqual(typeof(float), data.FloatField.GetType());
            Assert.AreEqual(typeof(double), data.DoubleField.GetType());
            Assert.AreEqual(typeof(char), data.CharField.GetType());
            Assert.AreEqual(typeof(char), data.WCharField.GetType());

            Assert.AreEqual(0, defaultStruct.Int16Field);
            Assert.AreEqual(0, defaultStruct.Int32Field);
            Assert.AreEqual(0, defaultStruct.Int64Field);
            Assert.AreEqual(false, defaultStruct.BoolField);
            Assert.AreEqual(0, defaultStruct.ByteField);
            Assert.AreEqual(0, defaultStruct.UInt16Field);
            Assert.AreEqual<uint>(0, defaultStruct.UInt32Field);
            Assert.AreEqual<ulong>(0, defaultStruct.UInt64Field);
            Assert.AreEqual(0, defaultStruct.FloatField);
            Assert.AreEqual(0, defaultStruct.DoubleField);
            Assert.AreEqual('\0', defaultStruct.CharField);
            Assert.AreEqual('\0', defaultStruct.WCharField);

            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(dw));
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, dr.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _subscriber.DeleteDataReader(dr));
        }

        /// <summary>
        /// Test the code generated for the primitives sequences types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedPrimitivesSequenceTypes()
        {
            using var evt = new ManualResetEventSlim(false);

            var typeSupport = new TestPrimitiveSequenceTypeSupport();
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
            var dr = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(dr);
            var dataReader = new TestPrimitiveSequenceDataReader(dr);

            var dw = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(dw);
            var dataWriter = new TestPrimitiveSequenceDataWriter(dw);

            Assert.IsTrue(dataWriter.WaitForSubscriptions(1, 5_000));
            Assert.IsTrue(dataReader.WaitForPublications(1, 5_000));

            var statusCondition = dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestPrimitiveSequence();

            var data = new TestPrimitiveSequence
            {
                BoundedBooleanSequenceField = { true, true, false, false, true },
                UnboundedBooleanSequenceField = { true, true, false, false, true, true, false },
                BoundedCharSequenceField = { 'z' },
                UnboundedCharSequenceField = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' },
                BoundedWCharSequenceField = { 'あ' },
                UnboundedWCharSequenceField = { 'あ', 'な', 'た', 'の', '基', '地', 'は', 'す' },
                BoundedByteSequenceField = { 0x01, 0x02, 0x03 },
                UnboundedByteSequenceField = { 0x04, 0x05, 0x06, 0x07, 0x08 },
                BoundedInt16SequenceField = { -1, 2, -3 },
                UnboundedInt16SequenceField = { 4, -5, 6, -7, 8 },
                BoundedUInt16SequenceField = { 1, 2, 3 },
                UnboundedUInt16SequenceField = { 4, 5, 6, 7, 8 },
                BoundedInt32SequenceField = { -1, 2, -3, 100, -200 },
                UnboundedInt32SequenceField = { 1, -2, 3, -100, 200, -300, 1000 },
                BoundedUInt32SequenceField = { 1, 2, 3, 100, 200 },
                UnboundedUInt32SequenceField = { 1, 2, 3, 100, 200, 300, 1000 },
                BoundedInt64SequenceField = { -1, 2, -3, 100, -200 },
                UnboundedInt64SequenceField = { 1, -2, 3, -100, 200, -300, 1000 },
                BoundedUInt64SequenceField = { 1, 2, 3, 100, 200 },
                UnboundedUInt64SequenceField = { 1, 2, 3, 100, 200, 300, 1000 },
                BoundedFloatSequenceField = { -1.0f, 2.1f, -3.2f, 100.3f, -200.4f },
                UnboundedFloatSequenceField = { 1f, -2.6f, 3.7f, -100.8f, 200.9f, -300.1f, 1000.1f },
                BoundedDoubleSequenceField = { -1.0d, 2.1d, -3.2d, 100.3d, -200.4d },
                UnboundedDoubleSequenceField = { 1.0d, -2.6d, 3.7d, -100.8d, 200.9d, -300.02d, 1000.1d },
            };

            ret = dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestPrimitiveSequence();
            var sampleInfo = new SampleInfo();
            ret = dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(data.BoundedBooleanSequenceField.SequenceEqual(received.BoundedBooleanSequenceField));
            Assert.IsTrue(data.UnboundedBooleanSequenceField.SequenceEqual(received.UnboundedBooleanSequenceField));
            Assert.IsTrue(data.BoundedCharSequenceField.SequenceEqual(received.BoundedCharSequenceField));
            Assert.IsTrue(data.UnboundedCharSequenceField.SequenceEqual(received.UnboundedCharSequenceField));
            Assert.IsTrue(data.BoundedWCharSequenceField.SequenceEqual(received.BoundedWCharSequenceField));
            Assert.IsTrue(data.UnboundedWCharSequenceField.SequenceEqual(received.UnboundedWCharSequenceField));
            Assert.IsTrue(data.BoundedByteSequenceField.SequenceEqual(received.BoundedByteSequenceField));
            Assert.IsTrue(data.UnboundedByteSequenceField.SequenceEqual(received.UnboundedByteSequenceField));
            Assert.IsTrue(data.BoundedInt16SequenceField.SequenceEqual(received.BoundedInt16SequenceField));
            Assert.IsTrue(data.UnboundedInt16SequenceField.SequenceEqual(received.UnboundedInt16SequenceField));
            Assert.IsTrue(data.BoundedUInt16SequenceField.SequenceEqual(received.BoundedUInt16SequenceField));
            Assert.IsTrue(data.UnboundedUInt16SequenceField.SequenceEqual(received.UnboundedUInt16SequenceField));
            Assert.IsTrue(data.BoundedInt32SequenceField.SequenceEqual(received.BoundedInt32SequenceField));
            Assert.IsTrue(data.UnboundedInt32SequenceField.SequenceEqual(received.UnboundedInt32SequenceField));
            Assert.IsTrue(data.BoundedUInt32SequenceField.SequenceEqual(received.BoundedUInt32SequenceField));
            Assert.IsTrue(data.UnboundedUInt32SequenceField.SequenceEqual(received.UnboundedUInt32SequenceField));
            Assert.IsTrue(data.BoundedInt64SequenceField.SequenceEqual(received.BoundedInt64SequenceField));
            Assert.IsTrue(data.UnboundedInt64SequenceField.SequenceEqual(received.UnboundedInt64SequenceField));
            Assert.IsTrue(data.BoundedUInt64SequenceField.SequenceEqual(received.BoundedUInt64SequenceField));
            Assert.IsTrue(data.UnboundedUInt64SequenceField.SequenceEqual(received.UnboundedUInt64SequenceField));
            Assert.IsTrue(data.BoundedFloatSequenceField.SequenceEqual(received.BoundedFloatSequenceField));
            Assert.IsTrue(data.UnboundedFloatSequenceField.SequenceEqual(received.UnboundedFloatSequenceField));
            Assert.IsTrue(data.BoundedDoubleSequenceField.SequenceEqual(received.BoundedDoubleSequenceField));
            Assert.IsTrue(data.UnboundedDoubleSequenceField.SequenceEqual(received.UnboundedDoubleSequenceField));


            Assert.AreEqual(data.BoundedBooleanSequenceField.GetType(), typeof(List<bool>));
            Assert.AreEqual(data.UnboundedBooleanSequenceField.GetType(), typeof(List<bool>));
            Assert.AreEqual(data.BoundedCharSequenceField.GetType(), typeof(List<char>));
            Assert.AreEqual(data.UnboundedCharSequenceField.GetType(), typeof(List<char>));
            Assert.AreEqual(data.BoundedWCharSequenceField.GetType(), typeof(List<char>));
            Assert.AreEqual(data.UnboundedWCharSequenceField.GetType(), typeof(List<char>));
            Assert.AreEqual(data.BoundedByteSequenceField.GetType(), typeof(List<byte>));
            Assert.AreEqual(data.UnboundedByteSequenceField.GetType(), typeof(List<byte>));
            Assert.AreEqual(data.BoundedInt16SequenceField.GetType(), typeof(List<short>));
            Assert.AreEqual(data.UnboundedInt16SequenceField.GetType(), typeof(List<short>));
            Assert.AreEqual(data.BoundedUInt16SequenceField.GetType(), typeof(List<ushort>));
            Assert.AreEqual(data.UnboundedUInt16SequenceField.GetType(), typeof(List<ushort>));
            Assert.AreEqual(data.BoundedInt32SequenceField.GetType(), typeof(List<int>));
            Assert.AreEqual(data.UnboundedInt32SequenceField.GetType(), typeof(List<int>));
            Assert.AreEqual(data.BoundedUInt32SequenceField.GetType(), typeof(List<uint>));
            Assert.AreEqual(data.UnboundedUInt32SequenceField.GetType(), typeof(List<uint>));
            Assert.AreEqual(data.BoundedInt64SequenceField.GetType(), typeof(List<long>));
            Assert.AreEqual(data.UnboundedInt64SequenceField.GetType(), typeof(List<long>));
            Assert.AreEqual(data.BoundedUInt64SequenceField.GetType(), typeof(List<ulong>));
            Assert.AreEqual(data.UnboundedUInt64SequenceField.GetType(), typeof(List<ulong>));
            Assert.AreEqual(data.BoundedFloatSequenceField.GetType(), typeof(List<float>));
            Assert.AreEqual(data.UnboundedFloatSequenceField.GetType(), typeof(List<float>));
            Assert.AreEqual(data.BoundedDoubleSequenceField.GetType(), typeof(List<double>));
            Assert.AreEqual(data.UnboundedDoubleSequenceField.GetType(), typeof(List<double>));

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
            Assert.IsNotNull(defaultStruct.UnboundedByteSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedByteSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedByteSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedByteSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedInt16SequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedInt16SequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedInt16SequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedInt16SequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedUInt16SequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedUInt16SequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedUInt16SequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedUInt16SequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedInt32SequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedInt32SequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedInt32SequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedInt32SequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedUInt32SequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedUInt32SequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedUInt32SequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedUInt32SequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedInt64SequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedInt64SequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedInt64SequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedInt64SequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedUInt64SequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedUInt64SequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedUInt64SequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedUInt64SequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedFloatSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedFloatSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedFloatSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedFloatSequenceField.Count);
            Assert.IsNotNull(defaultStruct.BoundedDoubleSequenceField);
            Assert.AreEqual(0, defaultStruct.BoundedDoubleSequenceField.Count);
            Assert.IsNotNull(defaultStruct.UnboundedDoubleSequenceField);
            Assert.AreEqual(0, defaultStruct.UnboundedDoubleSequenceField.Count);
        }

        /// <summary>
        /// Test the code generated for the primitives array types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedPrimitivesArrayTypes()
        {
            using var evt = new ManualResetEventSlim(false);

            var typeSupport = new TestPrimitiveArrayTypeSupport();
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
            var dr = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(dr);
            var dataReader = new TestPrimitiveArrayDataReader(dr);

            var dw = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(dw);
            var dataWriter = new TestPrimitiveArrayDataWriter(dw);

            Assert.IsTrue(dataWriter.WaitForSubscriptions(1, 5_000));
            Assert.IsTrue(dataReader.WaitForPublications(1, 5_000));

            var statusCondition = dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestPrimitiveArray();

            var data = new TestPrimitiveArray
            {
                BooleanArrayField = new[] { true, true, false, false, true },
                CharArrayField = new[] { 'a', 'b', 'c', 'd', 'e' },
                WCharArrayField = new[] { 'あ', 'な', 'た', 'の', '基' },
                ByteArrayField = new byte[] { 0x04, 0x05, 0x06, 0x07, 0x08 },
                Int16ArrayField = new short[] { 4, -5, 6, -7, 8 },
                UInt16ArrayField = new ushort[] { 4, 5, 6, 7, 8 },
                Int32ArrayField = new[] { -1, 2, -3, 100, -200 },
                UInt32ArrayField = new[] { 1u, 2u, 3u, 100u, 200u },
                Int64ArrayField = new[] { -1L, 2L, -3L, 100L, -200L },
                UInt64ArrayField = new[] { 1UL, 2UL, 3UL, 100UL, 200UL },
                FloatArrayField = new[] { -1.0f, 2.1f, -3.2f, 100.3f, -200.4f },
                DoubleArrayField = new[] { -1.0d, 2.1d, -3.2d, 100.3d, -200.4d },
            };
            ret = dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var sampleInfo = new SampleInfo();
            var received = new TestPrimitiveArray();
            ret = dataReader.ReadNextSample(received, sampleInfo);

            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.IsTrue(data.BooleanArrayField.SequenceEqual(received.BooleanArrayField));
            Assert.IsTrue(data.CharArrayField.SequenceEqual(received.CharArrayField));
            Assert.IsTrue(data.WCharArrayField.SequenceEqual(received.WCharArrayField));
            Assert.IsTrue(data.ByteArrayField.SequenceEqual(received.ByteArrayField));
            Assert.IsTrue(data.Int16ArrayField.SequenceEqual(received.Int16ArrayField));
            Assert.IsTrue(data.UInt16ArrayField.SequenceEqual(received.UInt16ArrayField));
            Assert.IsTrue(data.Int32ArrayField.SequenceEqual(received.Int32ArrayField));
            Assert.IsTrue(data.UInt32ArrayField.SequenceEqual(received.UInt32ArrayField));
            Assert.IsTrue(data.Int64ArrayField.SequenceEqual(received.Int64ArrayField));
            Assert.IsTrue(data.UInt64ArrayField.SequenceEqual(received.UInt64ArrayField));
            Assert.IsTrue(data.FloatArrayField.SequenceEqual(received.FloatArrayField));
            Assert.IsTrue(data.DoubleArrayField.SequenceEqual(received.DoubleArrayField));

            Assert.AreEqual(typeof(bool[]), data.BooleanArrayField.GetType());
            Assert.AreEqual(typeof(char[]), data.CharArrayField.GetType());
            Assert.AreEqual(typeof(char[]), data.WCharArrayField.GetType());
            Assert.AreEqual(typeof(byte[]), data.ByteArrayField.GetType());
            Assert.AreEqual(typeof(short[]), data.Int16ArrayField.GetType());
            Assert.AreEqual(typeof(ushort[]), data.UInt16ArrayField.GetType());
            Assert.AreEqual(typeof(int[]), data.Int32ArrayField.GetType());
            Assert.AreEqual(typeof(uint[]), data.UInt32ArrayField.GetType());
            Assert.AreEqual(typeof(long[]), data.Int64ArrayField.GetType());
            Assert.AreEqual(typeof(ulong[]), data.UInt64ArrayField.GetType());
            Assert.AreEqual(typeof(float[]), data.FloatArrayField.GetType());
            Assert.AreEqual(typeof(double[]), data.DoubleArrayField.GetType());

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

            Assert.IsNotNull(defaultStruct.ByteArrayField);
            Assert.AreEqual(5, defaultStruct.ByteArrayField.Length);
            foreach (var i in defaultStruct.ByteArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.Int16ArrayField);
            Assert.AreEqual(5, defaultStruct.Int16ArrayField.Length);
            foreach (var i in defaultStruct.Int16ArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.UInt16ArrayField);
            Assert.AreEqual(5, defaultStruct.UInt16ArrayField.Length);
            foreach (var i in defaultStruct.UInt16ArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.Int32ArrayField);
            Assert.AreEqual(5, defaultStruct.Int32ArrayField.Length);
            foreach (var i in defaultStruct.Int32ArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.UInt32ArrayField);
            Assert.AreEqual(5, defaultStruct.UInt32ArrayField.Length);
            foreach (var i in defaultStruct.UInt32ArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.Int64ArrayField);
            Assert.AreEqual(5, defaultStruct.Int64ArrayField.Length);
            foreach (var i in defaultStruct.Int64ArrayField)
            {
                Assert.AreEqual(default, i);
            }

            Assert.IsNotNull(defaultStruct.UInt64ArrayField);
            Assert.AreEqual(5, defaultStruct.UInt64ArrayField.Length);
            foreach (var i in defaultStruct.UInt64ArrayField)
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
        }

        /// <summary>
        /// Test the code generated for the primitives multi-array types.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGeneratedPrimitivesMultiArrayTypes()
        {
            using var evt = new ManualResetEventSlim(false);

            var typeSupport = new TestPrimitiveMultiArrayTypeSupport();
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
            var dr = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(dr);
            var dataReader = new TestPrimitiveMultiArrayDataReader(dr);

            var dw = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(dw);
            var dataWriter = new TestPrimitiveMultiArrayDataWriter(dw);

            Assert.IsTrue(dataWriter.WaitForSubscriptions(1, 5_000));
            Assert.IsTrue(dataReader.WaitForPublications(1, 5_000));

            var statusCondition = dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var defaultStruct = new TestPrimitiveMultiArray();

            var data = new TestPrimitiveMultiArray()
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
                        new[] { 'あ', 'な' },
                        new[] { 'た', 'の' },
                        new[] { '基', '地' },
                        new[] { 'は', 'す' },
                    },
                    new[]
                    {
                        new[] { 'べ', 'て' },
                        new[] { '私', 'た' },
                        new[] { 'ち', 'の' },
                        new[] { 'も', 'の' },
                    },
                    new[]
                    {
                        new[] { 'で', 'す' },
                        new[] { 'た', 'の' },
                        new[] { '基', '地' },
                        new[] { 'は', 'す' },
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
                Int16MultiArrayField = new[]
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
                UInt16MultiArrayField = new[]
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
                Int32MultiArrayField = new[]
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
                UInt32MultiArrayField = new[]
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
                Int64MultiArrayField = new[]
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
                UInt64MultiArrayField = new[]
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
            };

            ret = dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestPrimitiveMultiArray();
            var sampleInfo = new SampleInfo();
            ret = dataReader.ReadNextSample(received, sampleInfo);
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(TestHelper.CompareMultiArray(data.BooleanMultiArrayField, received.BooleanMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.CharMultiArrayField, received.CharMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.WCharMultiArrayField, received.WCharMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.OctetMultiArrayField, received.OctetMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.Int16MultiArrayField, received.Int16MultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.UInt16MultiArrayField, received.UInt16MultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.Int32MultiArrayField, received.Int32MultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.UInt32MultiArrayField, received.UInt32MultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.Int64MultiArrayField, received.Int64MultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.UInt64MultiArrayField, received.UInt64MultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.FloatMultiArrayField, received.FloatMultiArrayField));
            Assert.IsTrue(TestHelper.CompareMultiArray(data.DoubleMultiArrayField, received.DoubleMultiArrayField));

            Assert.AreEqual(typeof(bool[][][]), data.BooleanMultiArrayField.GetType());
            Assert.AreEqual(typeof(char[][][]), data.CharMultiArrayField.GetType());
            Assert.AreEqual(typeof(char[][][]), data.WCharMultiArrayField.GetType());
            Assert.AreEqual(typeof(byte[][][]), data.OctetMultiArrayField.GetType());
            Assert.AreEqual(typeof(short[][][]), data.Int16MultiArrayField.GetType());
            Assert.AreEqual(typeof(ushort[][][]), data.UInt16MultiArrayField.GetType());
            Assert.AreEqual(typeof(int[][][]), data.Int32MultiArrayField.GetType());
            Assert.AreEqual(typeof(uint[][][]), data.UInt32MultiArrayField.GetType());
            Assert.AreEqual(typeof(long[][][]), data.Int64MultiArrayField.GetType());
            Assert.AreEqual(typeof(ulong[][][]), data.UInt64MultiArrayField.GetType());
            Assert.AreEqual(typeof(float[][][]), data.FloatMultiArrayField.GetType());
            Assert.AreEqual(typeof(double[][][]), data.DoubleMultiArrayField.GetType());

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
                        Assert.AreEqual(default, defaultStruct.Int16MultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.UInt16MultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.Int32MultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.UInt32MultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.Int64MultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.UInt64MultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.FloatMultiArrayField[i0][i1][i2]);
                        Assert.AreEqual(default, defaultStruct.DoubleMultiArrayField[i0][i1][i2]);
                    }
                }
            }
        }
        #endregion
    }
}