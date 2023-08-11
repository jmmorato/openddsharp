/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2022 Jose Morato

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
using System.Threading;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="SubscriptionBuiltinTopicDataDataReader"/> unit test.
    /// </summary>
    [TestClass]
    public class SubscriptionBuiltinTopicDataDataReaderTest
    {
        #region Constants
        private const string TEST_CATEGORY = "SubscriptionBuiltinTopicDataDataReader";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Subscriber _subscriber;
        private DataReader _dataReader;
        private SubscriptionBuiltinTopicDataDataReader _dr;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets access to the <see cref="TestContext"/>.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by MSTest")]
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
        /// <summary>
        /// Test the properties default values after calling the constructor.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();
            _subscriber = _participant.GetBuiltinSubscriber();
            Assert.IsNotNull(_subscriber);

            _dataReader = _subscriber.LookupDataReader(SubscriptionBuiltinTopicDataDataReader.BUILT_IN_SUBSCRIPTION_TOPIC);
            Assert.IsNotNull(_dataReader);

            _dr = new SubscriptionBuiltinTopicDataDataReader(_dataReader);
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _dr?.DeleteContainedEntities();
            _subscriber?.DeleteDataReader(_dr);
            _subscriber?.DeleteContainedEntities();
            _participant?.DeleteSubscriber(_subscriber);
            _participant?.DeleteContainedEntities();
            AssemblyInitializer.Factory?.DeleteParticipant(_participant);

            _participant = null;
            _subscriber = null;
            _dr = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.Read(List{SubscriptionBuiltinTopicData}, List{SampleInfo})" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRead()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<SubscriptionBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);

            Assert.IsTrue(evt.Wait(1_500));

            ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data[0]);

            ret = reader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.Take(List{SubscriptionBuiltinTopicData}, List{SampleInfo})" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTake()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<SubscriptionBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.Take(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            Assert.IsTrue(evt.Wait(1_500));

            ret = _dr.Take(data, infos);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data[0]);

            ret = dataReader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(dataReader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.ReadInstance(List{SubscriptionBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<SubscriptionBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            Assert.IsTrue(evt.Wait(1_500));

            ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data[0]);

            var handle = infos[0].InstanceHandle;
            data = new List<SubscriptionBuiltinTopicData>();
            infos = new List<SampleInfo>();

            ret = _dr.ReadInstance(data, infos, handle);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data[0]);

            ret = dataReader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(dataReader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.TakeInstance(List{SubscriptionBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<SubscriptionBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            Assert.IsTrue(evt.Wait(1_500));

            ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);

            var handle = infos[0].InstanceHandle;
            data = new List<SubscriptionBuiltinTopicData>();
            infos = new List<SampleInfo>();

            ret = _dr.TakeInstance(data, infos, handle);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data[0]);

            ret = dataReader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(dataReader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.ReadNextInstance(List{SubscriptionBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<SubscriptionBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            Assert.IsTrue(evt.Wait(1_500));

            ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data[0]);

            ret = dataReader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(dataReader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.TakeNextInstance(List{SubscriptionBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<SubscriptionBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            Assert.IsTrue(evt.Wait(1_500));

            ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data[0]);

            ret = dataReader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(dataReader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.ReadNextSample(ref SubscriptionBuiltinTopicData, SampleInfo)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextSample()
        {
            using var evt = new ManualResetEventSlim(false);

            SubscriptionBuiltinTopicData data = default;
            var infos = new SampleInfo();
            var ret = _dr.ReadNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            Assert.IsTrue(evt.Wait(1_500));

            ret = _dr.ReadNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultSubscriptionData(data);

            ret = dataReader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(dataReader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.TakeNextSample(ref SubscriptionBuiltinTopicData, SampleInfo)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextSample()
        {
            using var evt = new ManualResetEventSlim(false);

            SubscriptionBuiltinTopicData data = default;
            var infos = new SampleInfo();
            var ret = _dr.TakeNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            Assert.IsTrue(evt.Wait(1_500));

            ret = _dr.TakeNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultSubscriptionData(data);

            ret = dataReader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(dataReader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.GetKeyValue(ref SubscriptionBuiltinTopicData, InstanceHandle)" />
        /// method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetKeyValue()
        {
            using var evt = new ManualResetEventSlim(false);

            // Call GetKeyValue with HandleNil
            SubscriptionBuiltinTopicData data = default;
            var info = new SampleInfo();
            var ret = _dr.GetKeyValue(ref data, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.BadParameter, ret);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            Assert.IsTrue(evt.Wait(1_500));

            ret = _dr.ReadNextSample(ref data, info);
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultSubscriptionData(data);

            SubscriptionBuiltinTopicData aux = default;
            ret = _dr.GetKeyValue(ref aux, info.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, ret);
            for (var i = 0; i < 16; i++)
            {
                Assert.AreEqual(data.Key.Value[i], aux.Key.Value[i]);
            }

            ret = dataReader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(dataReader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="SubscriptionBuiltinTopicDataDataReader.LookupInstance(SubscriptionBuiltinTopicData)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            SubscriptionBuiltinTopicData data = default;
            var info = new SampleInfo();

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var statusCondition = _dr.StatusCondition;
            Assert.IsNotNull(statusCondition);
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            Assert.IsTrue(evt.Wait(1_500));

            var ret = _dr.ReadNextSample(ref data, info);
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultSubscriptionData(data);

            // Lookup for an existing instance
            var handle = _dr.LookupInstance(data);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            ret = dataReader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = subscriber.DeleteDataReader(dataReader);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }
        #endregion
    }
}