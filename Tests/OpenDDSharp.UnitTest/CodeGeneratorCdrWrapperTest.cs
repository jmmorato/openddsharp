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
        // private DataReader _dr;
        // private TestPrimitiveDataReader _dataReader;
        // private DataWriter _dw;
        // private TestPrimitiveDataWriter _dataWriter;
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
        #endregion
    }
}