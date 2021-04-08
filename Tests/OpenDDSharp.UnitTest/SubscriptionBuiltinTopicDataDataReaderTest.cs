/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
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
using System.Linq;
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.UnitTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace OpenDDSharp.UnitTest
{
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
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
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
            List<SubscriptionBuiltinTopicData> data = new List<SubscriptionBuiltinTopicData>();
            List<SampleInfo> infos = new List<SampleInfo>();            
            ReturnCode ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);

            int count = 100;
            ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                ret = _dr.Read(data, infos);
            }
            
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data.First());

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTake()
        {
            List<SubscriptionBuiltinTopicData> data = new List<SubscriptionBuiltinTopicData>();
            List<SampleInfo> infos = new List<SampleInfo>();
            ReturnCode ret = _dr.Take(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            int count = 100;
            ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                ret = _dr.Take(data, infos);
            }
            
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data.First());

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadInstance()
        {
            List<SubscriptionBuiltinTopicData> data = new List<SubscriptionBuiltinTopicData>();
            List<SampleInfo> infos = new List<SampleInfo>();
            ReturnCode ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            int count = 100;
            ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            }
            
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data.First());

            var handle = infos.First().InstanceHandle;
            data = new List<SubscriptionBuiltinTopicData>();
            infos = new List<SampleInfo>();

            ret = _dr.ReadInstance(data, infos, handle);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data.First());

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeInstance()
        {
            List<SubscriptionBuiltinTopicData> data = new List<SubscriptionBuiltinTopicData>();
            List<SampleInfo> infos = new List<SampleInfo>();
            ReturnCode ret = _dr.Read(data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            int count = 100;
            ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            }

            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);

            var handle = infos.First().InstanceHandle;
            data = new List<SubscriptionBuiltinTopicData>();
            infos = new List<SampleInfo>();

            ret = _dr.TakeInstance(data, infos, handle);
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data.First());

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextInstance()
        {
            List<SubscriptionBuiltinTopicData> data = new List<SubscriptionBuiltinTopicData>();
            List<SampleInfo> infos = new List<SampleInfo>();
            ReturnCode ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            int count = 100;
            ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                ret = _dr.ReadNextInstance(data, infos, InstanceHandle.HandleNil);
            }
            
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data.First());

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextInstance()
        {
            List<SubscriptionBuiltinTopicData> data = new List<SubscriptionBuiltinTopicData>();
            List<SampleInfo> infos = new List<SampleInfo>();
            ReturnCode ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.NoData, ret);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, infos.Count);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            int count = 100;
            ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                ret = _dr.TakeNextInstance(data, infos, InstanceHandle.HandleNil);
            }

            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, infos.Count);
            TestHelper.TestNonDefaultSubscriptionData(data.First());

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextSample()
        {
            SubscriptionBuiltinTopicData data = default;
            SampleInfo infos = new SampleInfo();
            ReturnCode ret = _dr.ReadNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            int count = 100;
            ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                ret = _dr.ReadNextSample(ref data, infos);
            }
            
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultSubscriptionData(data);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextSample()
        {
            SubscriptionBuiltinTopicData data = default;
            SampleInfo infos = new SampleInfo();
            ReturnCode ret = _dr.TakeNextSample(ref data, infos);
            Assert.AreEqual(ReturnCode.NoData, ret);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            int count = 100;
            ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                ret = _dr.TakeNextSample(ref data, infos);
            }

            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultSubscriptionData(data);

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetKeyValue()
        {
            // Call GetKeyValue with HandleNil
            SubscriptionBuiltinTopicData data = default;
            SampleInfo info = new SampleInfo();
            ReturnCode ret = _dr.GetKeyValue(ref data, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.BadParameter, ret);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            int count = 100;
            ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                // Get the for an existing instance
                ret = _dr.ReadNextSample(ref data, info);
            }
            
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultSubscriptionData(data);

            SubscriptionBuiltinTopicData aux = default;
            ret = _dr.GetKeyValue(ref aux, info.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, ret);
            for (int i = 0; i < 16; i++)
            {
                Assert.AreEqual(data.Key.Value[i], aux.Key.Value[i]);
            }

            ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupInstance()
        {
            SubscriptionBuiltinTopicData data = default;
            SampleInfo info = new SampleInfo();

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = otherParticipant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(topic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader dataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(dataReader);

            int count = 100;
            ReturnCode ret = ReturnCode.NoData;
            while (ret == ReturnCode.NoData && count > 0)
            {
                Thread.Sleep(100);
                // Get the for an existing instance
                ret = _dr.ReadNextSample(ref data, info);
            }
             
            Assert.AreEqual(ReturnCode.Ok, ret);
            TestHelper.TestNonDefaultSubscriptionData(data);

            // Lookup for an existing instance
            var handle = _dr.LookupInstance(data);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

             ret = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }
        #endregion
    }
}
