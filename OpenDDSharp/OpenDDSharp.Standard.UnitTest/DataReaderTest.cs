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
using System;
using System.Linq;
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.Standard.UnitTest.Helpers;
using Test;
using System.Diagnostics.CodeAnalysis;

namespace OpenDDSharp.Standard.UnitTest
{
    [TestClass]
    public class DataReaderTest
    {
        #region Constants
        private const string TEST_CATEGORY = "DataReader";
        #endregion

        #region Fields        
        private DomainParticipant _participant;
        private Publisher _publisher;
        private Subscriber _subscriber;
        private Topic _topic;
        private DataWriter _dw;
        private TestStructDataWriter _dataWriter;
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

            _dw = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_dw);
            _dataWriter = new TestStructDataWriter(_dw);
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

            _participant = null;
            _publisher = null;
            _subscriber = null;
            _topic = null;
            _dw = null;
        }
        #endregion

        #region Test Methods
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Included in the calling method.")]
        public void TestNewDataReaderQos()
        {
            DataReaderQos qos = new DataReaderQos();
            TestHelper.TestDefaultDataReaderQos(qos);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a datareader with it.
            DataReaderQos qos = TestHelper.CreateNonDefaultDataReaderQos();

            DataReader dataReader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(dataReader);

            // Call GetQos and check the values received.
            qos = new DataReaderQos();
            ReturnCode result = dataReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataReaderQos(qos);

            // Call GetQos with null parameter.
            result = dataReader.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Create a new DataReader using the default QoS.
            DataReader dataReader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(dataReader);

            // Get the qos to ensure that is using the default properties
            DataReaderQos qos = new DataReaderQos();
            ReturnCode result = dataReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataReaderQos(qos);

            // Try to change an immutable property
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            result = dataReader.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them.
            qos = new DataReaderQos();
            qos.UserData.Value = new List<byte> { 0x42 };

            result = dataReader.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataReaderQos();
            result = dataReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.UserData);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(1, qos.UserData.Value.Count);
            Assert.AreEqual(0x42, qos.UserData.Value.First());

            // Try to set immutable QoS properties before enable the datareader.
            SubscriberQos subQos = new SubscriberQos();
            subQos.EntityFactory.AutoenableCreatedEntities = false;
            result = _subscriber.SetQos(subQos); 
            Assert.AreEqual(ReturnCode.Ok, result);

            DataReader otherDataReader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(otherDataReader);

            qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            result = otherDataReader.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataReaderQos();
            result = otherDataReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, qos.Reliability.Kind);

            result = otherDataReader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Set back the default subscriber QoS
            subQos = new SubscriberQos();
            _subscriber.SetQos(subQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            // Call SetQos with null parameter
            result = dataReader.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }
        #endregion
    }
}
