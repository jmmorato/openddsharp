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
    /// <see cref="TopicBuiltinTopicDataDataReader"/> unit test.
    /// </summary>
    [TestClass]
    public class TopicBuiltinTopicDataDataReaderTest
    {
        #region Constants
        private const string TEST_CATEGORY = "TopicBuiltinTopicDataDataReader";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Subscriber _subscriber;
        private DataReader _dataReader;
        private TopicBuiltinTopicDataDataReader _dr;
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
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindTcpTransportConfig();

            _subscriber = _participant.GetBuiltinSubscriber();
            Assert.IsNotNull(_subscriber);

            _dataReader = _subscriber.LookupDataReader(TopicBuiltinTopicDataDataReader.BUILT_IN_TOPIC_TOPIC);
            Assert.IsNotNull(_dataReader);

            _dr = new TopicBuiltinTopicDataDataReader(_dataReader);
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
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.Read(List{TopicBuiltinTopicData}, List{SampleInfo})" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRead()
        {
            using var evt = new ManualResetEventSlim(false);
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                var data = new List<TopicBuiltinTopicData>();
                var infos = new List<SampleInfo>();
                ret = _dr.Read(data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.Read(data, infos);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data[0].TypeName);
                Assert.IsNotNull(data[0].Key);
                TestHelper.TestNonDefaultTopicData(data[0]);
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }

        /// <summary>
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.Take(List{TopicBuiltinTopicData}, List{SampleInfo})" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTake()
        {
            using var evt = new ManualResetEventSlim(false);

            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                var data = new List<TopicBuiltinTopicData>();
                var infos = new List<SampleInfo>();
                ret = _dr.Take(data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.Take(data, infos);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data[0].TypeName);
                Assert.IsNotNull(data[0].Key);
                TestHelper.TestNonDefaultTopicData(data[0]);
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }

        /// <summary>
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.ReadInstance(List{TopicBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                var data = new List<TopicBuiltinTopicData>();
                var infos = new List<SampleInfo>();
                ret = _dr.Read(data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);

                var handle = infos[0].InstanceHandle;
                data = new List<TopicBuiltinTopicData>();
                infos = new List<SampleInfo>();

                ret = _dr.ReadInstance(data, infos, handle);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data[0].TypeName);
                Assert.IsNotNull(data[0].Key);
                TestHelper.TestNonDefaultTopicData(data[0]);
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }

        /// <summary>
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.TakeInstance(List{TopicBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                var data = new List<TopicBuiltinTopicData>();
                var infos = new List<SampleInfo>();
                ret = _dr.Read(data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data[0].TypeName);
                Assert.IsNotNull(data[0].Key);
                TestHelper.TestNonDefaultTopicData(data[0]);

                var handle = infos[0].InstanceHandle;
                data = new List<TopicBuiltinTopicData>();
                infos = new List<SampleInfo>();

                ret = _dr.TakeInstance(data, infos, handle);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data[0].TypeName);
                Assert.IsNotNull(data[0].Key);
                TestHelper.TestNonDefaultTopicData(data[0]);
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }

        /// <summary>
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.ReadNextInstance(List{TopicBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                var data = new List<TopicBuiltinTopicData>();
                var infos = new List<SampleInfo>();
                ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data[0].TypeName);
                Assert.IsNotNull(data[0].Key);
                TestHelper.TestNonDefaultTopicData(data[0]);
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }

        /// <summary>
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.TakeNextInstance(List{TopicBuiltinTopicData}, List{SampleInfo}, InstanceHandle)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                var data = new List<TopicBuiltinTopicData>();
                var infos = new List<SampleInfo>();
                ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data[0].TypeName);
                Assert.IsNotNull(data[0].Key);
                TestHelper.TestNonDefaultTopicData(data[0]);
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }

        /// <summary>
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.ReadNextSample(ref TopicBuiltinTopicData, SampleInfo)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextSample()
        {
            using var evt = new ManualResetEventSlim(false);

            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                TopicBuiltinTopicData data = default;
                var infos = new SampleInfo();
                ret = _dr.ReadNextSample(ref data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.ReadNextSample(ref data, infos);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(typeName, data.TypeName);
                Assert.IsNotNull(data.Key);
                TestHelper.TestNonDefaultTopicData(data);
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }

        /// <summary>
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.TakeNextSample(ref TopicBuiltinTopicData, SampleInfo)" />
        /// method and its overloads.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextSample()
        {
            using var evt = new ManualResetEventSlim(false);

            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                TopicBuiltinTopicData data = default;
                var infos = new SampleInfo();
                ret = _dr.TakeNextSample(ref data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.TakeNextSample(ref data, infos);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(typeName, data.TypeName);
                Assert.IsNotNull(data.Key);
                TestHelper.TestNonDefaultTopicData(data);
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }

        /// <summary>
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.GetKeyValue(ref TopicBuiltinTopicData, InstanceHandle)" />
        /// method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetKeyValue()
        {
            using var evt = new ManualResetEventSlim(false);

            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                // Call GetKeyValue with HandleNil
                TopicBuiltinTopicData data = default;
                var info = new SampleInfo();
                ret = _dr.GetKeyValue(ref data, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.BadParameter, ret);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.ReadNextSample(ref data, info);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(typeName, data.TypeName);
                Assert.IsNotNull(data.Key);
                TestHelper.TestNonDefaultTopicData(data);

                TopicBuiltinTopicData aux = default;
                ret = _dr.GetKeyValue(ref aux, info.InstanceHandle);
                Assert.AreEqual(ReturnCode.Ok, ret);
                for (var i = 0; i < 16; i++)
                {
                    Assert.AreEqual(data.Key.Value[i], aux.Key.Value[i]);
                }
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }

        /// <summary>
        /// Test the <see cref="TopicBuiltinTopicDataDataReader.LookupInstance(TopicBuiltinTopicData)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                TopicBuiltinTopicData data = default;
                var info = new SampleInfo();

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                var support = new TestStructTypeSupport();
                var typeName = support.GetTypeName();
                var result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                var statusCondition = _dr.StatusCondition;
                Assert.IsNotNull(statusCondition);
                statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                Assert.IsTrue(evt.Wait(1_500));

                ret = _dr.ReadNextSample(ref data, info);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(typeName, data.TypeName);
                Assert.IsNotNull(data.Key);
                TestHelper.TestNonDefaultTopicData(data);

                // Lookup for an existing instance
                var handle = _dr.LookupInstance(data);
                Assert.AreNotEqual(InstanceHandle.HandleNil, handle);
            }
            finally
            {
                if (otherParticipant != null && topic != null)
                {
                    ret = otherParticipant.DeleteTopic(topic);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }

                if (otherParticipant != null)
                {
                    ret = otherParticipant.DeleteContainedEntities();
                    Assert.AreEqual(ReturnCode.Ok, ret);

                    ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                }
            }
        }
        #endregion
    }
}