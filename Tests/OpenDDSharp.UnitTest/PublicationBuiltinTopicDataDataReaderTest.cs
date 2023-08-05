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
    /// <see cref="PublicationBuiltinTopicDataDataReader"/> unit test.
    /// </summary>
    [TestClass]
    public class PublicationBuiltinTopicDataDataReaderTest
    {
        #region Constants
        private const string TEST_CATEGORY = "PublicationBuiltinTopicDataDataReader";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Subscriber _subscriber;
        private DataReader _dataReader;
        private PublicationBuiltinTopicDataDataReader _dr;
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

            _dataReader = _subscriber.LookupDataReader(PublicationBuiltinTopicDataDataReader.BUILT_IN_PUBLICATION_TOPIC);
            Assert.IsNotNull(_dataReader);

            _dr = new PublicationBuiltinTopicDataDataReader(_dataReader);
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
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.Read(List{PublicationBuiltinTopicData}, List{SampleInfo})" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRead()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<PublicationBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultPublicationData(data[0]);

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.Take(List{PublicationBuiltinTopicData}, List{SampleInfo})" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTake()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<PublicationBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.Take(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            ret = _dr.Take(data, infos);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultPublicationData(data[0]);

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.ReadInstance(List{PublicationBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<PublicationBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultPublicationData(data[0]);

            var handle = infos[0].InstanceHandle;
            data = new List<PublicationBuiltinTopicData>();
            infos = new List<SampleInfo>();

            ret = _dr.ReadInstance(data, infos, handle);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultPublicationData(data[0]);

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.TakeInstance(List{PublicationBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<PublicationBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultPublicationData(data[0]);

            var handle = infos[0].InstanceHandle;
            data = new List<PublicationBuiltinTopicData>();
            infos = new List<SampleInfo>();

            ret = _dr.TakeInstance(data, infos, handle);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultPublicationData(data[0]);

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.ReadNextInstance(List{PublicationBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<PublicationBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultPublicationData(data[0]);

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.TakeNextInstance(List{PublicationBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            var data = new List<PublicationBuiltinTopicData>();
            var infos = new List<SampleInfo>();
            var ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultPublicationData(data[0]);

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.ReadNextSample(ref PublicationBuiltinTopicData, SampleInfo)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextSample()
        {
            using var evt = new ManualResetEventSlim(false);

            PublicationBuiltinTopicData data = default;
            var infos = new SampleInfo();
            var ret = _dr.ReadNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            ret = _dr.ReadNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultPublicationData(data);

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.TakeNextSample(ref PublicationBuiltinTopicData, SampleInfo)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextSample()
        {
            using var evt = new ManualResetEventSlim(false);

            PublicationBuiltinTopicData data = default;
            var infos = new SampleInfo();
            var ret = _dr.TakeNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            ret = _dr.TakeNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultPublicationData(data);

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.GetKeyValue(ref PublicationBuiltinTopicData, InstanceHandle)" />
        /// method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetKeyValue()
        {
            using var evt = new ManualResetEventSlim(false);

            // Call GetKeyValue with HandleNil
            PublicationBuiltinTopicData data = default;
            var info = new SampleInfo();
            var ret = _dr.GetKeyValue(ref data, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.BadParameter, ret);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            ret = _dr.ReadNextSample(ref data, info);
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultPublicationData(data);

            PublicationBuiltinTopicData aux = default;
            ret = _dr.GetKeyValue(ref aux, info.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, ret);
            for (var i = 0; i < 16; i++)
            {
                Assert.AreEqual(data.Key.Value[i], aux.Key.Value[i]);
            }

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        /// <summary>
        /// Test the <see cref="PublicationBuiltinTopicDataDataReader.LookupInstance(PublicationBuiltinTopicData)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            PublicationBuiltinTopicData data = default;
            var info = new SampleInfo();

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var statusCondition = _dr.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var dataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(dataWriter);

            Assert.IsTrue(evt.Wait(5_000));

            var ret = _dr.ReadNextSample(ref data, info);
            Assert.AreEqual(ReturnCode.Ok, ret);

            // Lookup for an existing instance
            var handle = _dr.LookupInstance(data);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            ret = publisher.DeleteDataWriter(dataWriter);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = otherParticipant.DeletePublisher(publisher);
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