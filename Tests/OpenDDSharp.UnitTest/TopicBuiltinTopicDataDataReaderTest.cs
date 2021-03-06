﻿/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

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
using System.Linq;
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.UnitTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace OpenDDSharp.UnitTest
{
    [Ignore]
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
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
        [TestInitialize]
        public void TestInitialize()
        {
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();

            _subscriber = _participant.GetBuiltinSubscriber();
            Assert.IsNotNull(_subscriber);

            _dataReader = _subscriber.LookupDataReader(TopicBuiltinTopicDataDataReader.BUILT_IN_TOPIC_TOPIC);
            Assert.IsNotNull(_dataReader);

            _dr = new TopicBuiltinTopicDataDataReader(_dataReader);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_participant != null)
            {
                ReturnCode result = _participant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            if (AssemblyInitializer.Factory != null)
            {
                ReturnCode result = AssemblyInitializer.Factory.DeleteParticipant(_participant);
                Assert.AreEqual(ReturnCode.Ok, result);
            }
        }
        #endregion

        #region Test Methods
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRead()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                List<TopicBuiltinTopicData> data = new List<TopicBuiltinTopicData>();
                List<SampleInfo> infos = new List<SampleInfo>();
                ret = _dr.Read(data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    ret = _dr.Read(data, infos);
                    count--;
                }

                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data.First().TypeName);
                Assert.IsNotNull(data.First().Key);
                TestHelper.TestNonDefaultTopicData(data.First());

            }
            finally
            {
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTake()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                List<TopicBuiltinTopicData> data = new List<TopicBuiltinTopicData>();
                List<SampleInfo> infos = new List<SampleInfo>();
                ret = _dr.Take(data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    ret = _dr.Take(data, infos);
                    count--;
                }

                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data.First().TypeName);
                Assert.IsNotNull(data.First().Key);
                TestHelper.TestNonDefaultTopicData(data.First());
            }
            finally
            {
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadInstance()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                List<TopicBuiltinTopicData> data = new List<TopicBuiltinTopicData>();
                List<SampleInfo> infos = new List<SampleInfo>();
                ret = _dr.Read(data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
                    count--;
                }

                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);

                var handle = infos.First().InstanceHandle;
                data = new List<TopicBuiltinTopicData>();
                infos = new List<SampleInfo>();

                ret = _dr.ReadInstance(data, infos, handle);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data.First().TypeName);
                Assert.IsNotNull(data.First().Key);
                TestHelper.TestNonDefaultTopicData(data.First());
            }
            finally
            {
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeInstance()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                List<TopicBuiltinTopicData> data = new List<TopicBuiltinTopicData>();
                List<SampleInfo> infos = new List<SampleInfo>();
                ret = _dr.Read(data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
                    count--;
                }

                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count); Assert.AreEqual(typeName, data.First().TypeName);
                Assert.IsNotNull(data.First().Key);
                TestHelper.TestNonDefaultTopicData(data.First());

                var handle = infos.First().InstanceHandle;
                data = new List<TopicBuiltinTopicData>();
                infos = new List<SampleInfo>();

                ret = _dr.TakeInstance(data, infos, handle);
                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data.First().TypeName);
                Assert.IsNotNull(data.First().Key);
                TestHelper.TestNonDefaultTopicData(data.First());
            }
            finally
            {
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextInstance()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                List<TopicBuiltinTopicData> data = new List<TopicBuiltinTopicData>();
                List<SampleInfo> infos = new List<SampleInfo>();
                ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
                    count--;
                }

                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data.First().TypeName);
                Assert.IsNotNull(data.First().Key);
                TestHelper.TestNonDefaultTopicData(data.First());
            }
            finally
            {
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextInstance()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                List<TopicBuiltinTopicData> data = new List<TopicBuiltinTopicData>();
                List<SampleInfo> infos = new List<SampleInfo>();
                ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.NoData, ret);
                Assert.AreEqual(0, data.Count);
                Assert.AreEqual(0, infos.Count);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
                    count--;
                }

                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, infos.Count);
                Assert.AreEqual(typeName, data.First().TypeName);
                Assert.IsNotNull(data.First().Key);
                TestHelper.TestNonDefaultTopicData(data.First());
            }
            finally
            {
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextSample()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                TopicBuiltinTopicData data = default;
                SampleInfo infos = new SampleInfo();
                ret = _dr.ReadNextSample(ref data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    ret = _dr.ReadNextSample(ref data, infos);
                    count--;
                }

                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(typeName, data.TypeName);
                Assert.IsNotNull(data.Key);
                TestHelper.TestNonDefaultTopicData(data);
            }
            finally
            {
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextSample()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                TopicBuiltinTopicData data = default;
                SampleInfo infos = new SampleInfo();
                ret = _dr.TakeNextSample(ref data, infos);
                Assert.AreEqual(ReturnCode.NoData, ret);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    ret = _dr.TakeNextSample(ref data, infos);
                    count--;
                }

                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(typeName, data.TypeName);
                Assert.IsNotNull(data.Key);
                TestHelper.TestNonDefaultTopicData(data);
            }
            finally
            {
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetKeyValue()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                // Call GetKeyValue with HandleNil
                TopicBuiltinTopicData data = default;
                SampleInfo info = new SampleInfo();
                ret = _dr.GetKeyValue(ref data, InstanceHandle.HandleNil);
                Assert.AreEqual(ReturnCode.BadParameter, ret);

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    // Get the for an existing instance
                    ret = _dr.ReadNextSample(ref data, info);
                    count--;
                }

                Assert.AreEqual(ReturnCode.Ok, ret);
                Assert.AreEqual(typeName, data.TypeName);
                Assert.IsNotNull(data.Key);
                TestHelper.TestNonDefaultTopicData(data);

                TopicBuiltinTopicData aux = default;
                ret = _dr.GetKeyValue(ref aux, info.InstanceHandle);
                Assert.AreEqual(ReturnCode.Ok, ret);
                for (int i = 0; i < 16; i++)
                {
                    Assert.AreEqual(data.Key.Value[i], aux.Key.Value[i]);
                }
            }
            finally
            {
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupInstance()
        {
            ReturnCode ret;
            DomainParticipant otherParticipant = null;
            Topic topic = null;
            try
            {
                TopicBuiltinTopicData data = default;
                SampleInfo info = new SampleInfo();

                otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
                Assert.IsNotNull(otherParticipant);
                otherParticipant.BindRtpsUdpTransportConfig();

                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(otherParticipant, typeName);
                Assert.AreEqual(ReturnCode.Ok, result);

                TopicQos qos = TestHelper.CreateNonDefaultTopicQos();
                topic = otherParticipant.CreateTopic(TestContext.TestName, typeName, qos);
                Assert.IsNotNull(topic);

                int count = 200;
                ret = ReturnCode.NoData;
                while (ret != ReturnCode.Ok && count > 0)
                {
                    Thread.Sleep(100);
                    ret = _dr.ReadNextSample(ref data, info);
                    count--;
                }

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
                ret = otherParticipant.DeleteTopic(topic);
                Assert.AreEqual(ReturnCode.Ok, ret);
                ret = otherParticipant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, ret);

                ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
                Assert.AreEqual(ReturnCode.Ok, ret);
            }
        }
        #endregion
    }
}
