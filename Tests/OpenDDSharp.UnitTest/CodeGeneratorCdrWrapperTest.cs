using System.Diagnostics.CodeAnalysis;
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
        private DataReader _dr;
        private TestPrimitiveDataReader _dataReader;
        private DataWriter _dw;
        private TestPrimitiveDataWriter _dataWriter;
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
            _dr = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(_dr);
            _dataReader = new TestPrimitiveDataReader(_dr);

            _dw = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_dw);
            _dataWriter = new TestPrimitiveDataWriter(_dw);

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
        public void TestGeneratedPrimitivesTypes()
        {
            using var evt = new ManualResetEventSlim(false);

            var statusCondition = _dr.StatusCondition;
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
                // WCharField = 'あ',
            };
            var ret = _dataWriter.Write(data);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, ret);

            Assert.IsTrue(evt.Wait(1_500));

            var received = new TestPrimitive();
            var sampleInfo = new SampleInfo();
            ret = _dataReader.ReadNextSample(received, sampleInfo);
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
            // Assert.AreEqual(data.WCharField, received.WCharField);

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
            // Assert.AreEqual(typeof(char), data.WCharField.GetType());

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
            // Assert.AreEqual('\0', defaultStruct.WCharField);
        }
        #endregion
    }
}