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
using System.Threading;
using System.Collections.Generic;
using OpenDDSharp.DDS;
using Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.Standard.UnitTest.Listeners;

namespace OpenDDSharp.Standard.UnitTest
{
    [TestClass]
    public class DomainParticipantTest
    {
        #region Constants
        private const string TEST_CATEGORY = "DomainParticipant";
        #endregion

        #region Fields
        private DomainParticipant _participant;
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
        public void TestGetDefaultPublisherQos()
        {
            PublisherQos qos = TestHelper.CreateNonDefaultPublisherQos();

            ReturnCode result = _participant.GetDefaultPublisherQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultPublisherQos(qos);

            // Test with null parameter
            result = _participant.GetDefaultPublisherQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultPulisherQos()
        {
            PublisherQos qos = TestHelper.CreateNonDefaultPublisherQos();

            ReturnCode result = _participant.SetDefaultPublisherQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new PublisherQos();
            result = _participant.GetDefaultPublisherQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultPublisherQos(qos);

            // Test with null parameter
            result = _participant.SetDefaultPublisherQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDefaultSubscriberQos()
        {
            SubscriberQos qos = TestHelper.CreateNonDefaultSubscriberQos();

            ReturnCode result = _participant.GetDefaultSubscriberQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultSubscriberQos(qos);

            result = _participant.GetDefaultSubscriberQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultSubscriberQos()
        {
            SubscriberQos qos = TestHelper.CreateNonDefaultSubscriberQos();

            ReturnCode result = _participant.SetDefaultSubscriberQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new SubscriberQos();
            result = _participant.GetDefaultSubscriberQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultSubscriberQos(qos);

            // Test with null parameter
            result = _participant.SetDefaultSubscriberQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDefaultTopicQos()
        {
            TopicQos qos = TestHelper.CreateNonDefaultTopicQos();

            ReturnCode result = _participant.GetDefaultTopicQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultTopicQos(qos);

            // Test with null parameter
            result = _participant.GetDefaultTopicQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultTopicQos()
        {
            TopicQos qos = TestHelper.CreateNonDefaultTopicQos();

            ReturnCode result = _participant.SetDefaultTopicQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new TopicQos();
            result = _participant.GetDefaultTopicQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultTopicQos(qos);

            // Test with null parameter
            result = _participant.SetDefaultTopicQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            DomainParticipantQos qos = TestHelper.CreateNonDefaultDomainParticipantQos();

            ReturnCode result = _participant.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test with null parameter.
            result = _participant.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Creates a non-default QoS, set it an check it.
            DomainParticipantQos qos = TestHelper.CreateNonDefaultDomainParticipantQos();

            ReturnCode result = _participant.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DomainParticipantQos();
            result = _participant.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDomainParticipantQos(qos);

            // Put back the default QoS and check it.
            qos = new DomainParticipantQos();
            result = _participant.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test with null parameter.
            result = _participant.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetListener()
        {
            DomainParticipantListener listener = _participant.GetListener();
            Assert.IsNull(listener);

            MyParticipantListener otherListener = new MyParticipantListener();
            DomainParticipant other = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_OTHER_DOMAIN, null, otherListener);
            Assert.IsNotNull(other);
            other.BindRtpsUdpTransportConfig();

            listener = other.GetListener();
            Assert.IsNotNull(listener);

            ReturnCode result = AssemblyInitializer.Factory.DeleteParticipant(other);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetListener()
        {
            DomainParticipantListener listener = _participant.GetListener();
            Assert.IsNull(listener);

            MyParticipantListener myListener = new MyParticipantListener();
            ReturnCode result = _participant.SetListener(myListener);
            Assert.AreEqual(ReturnCode.Ok, result);

            listener = _participant.GetListener();
            Assert.AreEqual(myListener, listener);

            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            listener = _participant.GetListener();
            Assert.IsNull(listener);
        }
        #endregion
    }
}
