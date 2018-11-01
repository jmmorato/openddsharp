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
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class TopicTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
        private DomainParticipant _participant;
        #endregion

        #region Initialization/Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dpf = ParticipantService.Instance.GetDomainParticipantFactory();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _participant = _dpf.CreateParticipant(DOMAIN_ID);
            Assert.IsNotNull(_participant);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_participant != null)
            {
                ReturnCode result = _participant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            if (_dpf != null)
            {
                ReturnCode result = _dpf.DeleteParticipant(_participant);
                Assert.AreEqual(ReturnCode.Ok, result);
            }
        }
        #endregion

        #region Test Methods
        [TestMethod]
        [TestCategory("Topic")]
        public void TestProperties()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestProperties), typeName);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestProperties), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);
        }

        [TestMethod]
        [TestCategory("Topic")]
        public void TestGetQos()
        {
            // Create a non-default QoS
            TopicQos qos = TestHelper.CreateNonDefaultTopicQos();

            // Create a new Topic using the non-default QoS
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestGetQos), typeName, qos);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestGetQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

            // Get the QoS and check it
            TopicQos getQos = new TopicQos();
            result = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultTopicQos(getQos);
            
            // Test GetQos with null parameter
            result = topic.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("Topic")]
        public void TestSetQos()
        {
            // Create a new Topic using the default QoS
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestSetQos), typeName);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestSetQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

            // Get the qos to ensure that is using the default properties
            TopicQos getQos = new TopicQos();
            result = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultTopicQos(getQos);

            // Try to change an immutable property
            TopicQos qos = new TopicQos();
            qos.DestinationOrder.Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos;            

            result = topic.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them
            qos = new TopicQos();
            qos.Deadline.Period = new Duration
            {
                Seconds = 5,
                NanoSeconds = 0
            };
            qos.LatencyBudget.Duration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Lifespan.Duration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.TopicData.Value = new List<byte> { 0x5 };

            result = topic.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos);
            Assert.IsNotNull(qos.Deadline);
            Assert.IsNotNull(qos.DestinationOrder);
            Assert.IsNotNull(qos.Durability);
            Assert.IsNotNull(qos.DurabilityService);
            Assert.IsNotNull(qos.History);
            Assert.IsNotNull(qos.LatencyBudget);
            Assert.IsNotNull(qos.Lifespan);
            Assert.IsNotNull(qos.Liveliness);
            Assert.IsNotNull(qos.Ownership);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TopicData);
            Assert.IsNotNull(qos.TransportPriority);
            Assert.AreEqual(5, qos.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.Deadline.Period.NanoSeconds);
            Assert.AreEqual(5, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual((uint)5, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(5, qos.Lifespan.Duration.Seconds);
            Assert.AreEqual((uint)5, qos.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(1, qos.TopicData.Value.Count());
            Assert.AreEqual(0x5, qos.TopicData.Value.First());

            // Create a disabled topic and try to set an inconsistent qos
            DomainParticipantQos pQos = new DomainParticipantQos();
            pQos.EntityFactory.AutoenableCreatedEntities = false;
            _participant.SetQos(pQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic otherTopic = _participant.CreateTopic("Other" + nameof(TestSetQos), typeName);
            Assert.IsNotNull(otherTopic);
            Assert.AreEqual("Other" + nameof(TestSetQos), otherTopic.Name);
            Assert.AreEqual(typeName, otherTopic.TypeName);
            Assert.AreEqual(_participant, otherTopic.Participant);

            qos = new TopicQos();
            qos.History.Kind = HistoryQosPolicyKind.KeepLastHistoryQos;
            qos.History.Depth = 200;            
            qos.ResourceLimits.MaxSamplesPerInstance = 1;

            result = otherTopic.SetQos(qos);
            Assert.AreEqual(ReturnCode.InconsistentPolicy, result);

            // Test SetQos with null parameter
            result = topic.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

        }

        [TestMethod]
        [TestCategory("Topic")]
        public void TestGetListener()
        {
            // Create a new Topic with a listener
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            MyTopicListener listener = new MyTopicListener();
            Topic topic = _participant.CreateTopic(nameof(TestGetQos), typeName, listener);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestGetQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

            // Call to GetListener and check the listener returned
            MyTopicListener received = (MyTopicListener)topic.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);
        }

        [TestMethod]
        [TestCategory("Topic")]
        public void TestSetListener()
        {
            // Create a new Topic without listener
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);
            
            Topic topic = _participant.CreateTopic(nameof(TestSetListener), typeName);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestSetListener), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

            MyTopicListener listener = (MyTopicListener)topic.GetListener();
            Assert.IsNull(listener);

            // Create a listener, set it and check that is correctly setted
            listener = new MyTopicListener();
            result = topic.SetListener(listener);
            Assert.AreEqual(ReturnCode.Ok, result);

            MyTopicListener received = (MyTopicListener)topic.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            // Remove the listener calling SetListener with null and check it
            result = topic.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            received = (MyTopicListener)topic.GetListener();
            Assert.IsNull(received);
        }

        [TestMethod]
        [TestCategory("Topic")]
        public void TestGetInconsistentTopicStatus()
        {
            // Create a new Topic and call GetInconsistentTopicStatus
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);
            
            Topic topic = _participant.CreateTopic(nameof(TestGetInconsistentTopicStatus), typeName);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestGetInconsistentTopicStatus), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

            InconsistentTopicStatus status = new InconsistentTopicStatus();
            result = topic.GetInconsistentTopicStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
        }
        #endregion
    }
}
