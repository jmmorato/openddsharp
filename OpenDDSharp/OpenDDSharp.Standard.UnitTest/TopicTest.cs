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
using Test;
using OpenDDSharp.Standard.UnitTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace OpenDDSharp.Standard.UnitTest
{
    [TestClass]
    public class TopicTest
    {
        #region Constants
        private const string TEST_CATEGORY = "Topic";
        #endregion

        #region Fields
        private DomainParticipant _participant;
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
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Included in the calling method.")]
        public void TestNewTopicQos()
        {
            TopicQos qos = new TopicQos();
            TestHelper.TestDefaultTopicQos(qos);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            // Create a non-default QoS.
            TopicQos qos = TestHelper.CreateNonDefaultTopicQos();

            // Create a new Topic using the non-default QoS.
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestGetQos), typeName, qos);
            Assert.IsNotNull(topic);
            // TODO: Uncomment when properties implemented.
            //Assert.AreEqual(nameof(TestGetQos), topic.Name);
            //Assert.AreEqual(typeName, topic.TypeName);
            //Assert.AreEqual(_participant, topic.Participant);

            // Get the QoS and check it.
            TopicQos getQos = new TopicQos();
            result = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultTopicQos(getQos);

            // Test GetQos with null parameter.
            result = topic.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Create a new Topic using the default QoS
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestSetQos), typeName);
            Assert.IsNotNull(topic);
            // TODO: Uncomment when properties implemented.
            //Assert.AreEqual(nameof(TestSetQos), topic.Name);
            //Assert.AreEqual(typeName, topic.TypeName);
            //Assert.AreEqual(_participant, topic.Participant);

            // Get the qos to ensure that is using the default properties.
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
                NanoSeconds = 0,
            };
            qos.LatencyBudget.Duration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5,
            };
            qos.Lifespan.Duration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5,
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
            Assert.AreEqual(5U, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(5, qos.Lifespan.Duration.Seconds);
            Assert.AreEqual(5U, qos.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(1, qos.TopicData.Value.Count);
            Assert.AreEqual(0x5, qos.TopicData.Value.First());

            // Create a disabled topic and try to set an inconsistent qos
            DomainParticipantQos pQos = new DomainParticipantQos();
            pQos.EntityFactory.AutoenableCreatedEntities = false;
            _participant.SetQos(pQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic otherTopic = _participant.CreateTopic("Other" + nameof(TestSetQos), typeName);
            Assert.IsNotNull(otherTopic);
            // TODO: Uncomment when properties implemented.
            //Assert.AreEqual("Other" + nameof(TestSetQos), otherTopic.Name);
            //Assert.AreEqual(typeName, otherTopic.TypeName);
            //Assert.AreEqual(_participant, otherTopic.Participant);

            qos = new TopicQos();
            qos.History.Kind = HistoryQosPolicyKind.KeepLastHistoryQos;
            qos.History.Depth = 200;
            qos.ResourceLimits.MaxSamplesPerInstance = 1;

            result = otherTopic.SetQos(qos);
            Assert.AreEqual(ReturnCode.InconsistentPolicy, result);

            // TODO: Investigate
            // If don't enable the entity before delete it, it fails on OpenDDS:
            // DomainParticipantImpl::delete_topic_i, remove_topic failed with return value 3
            // That didn't happen in OpenDDS 3.13.x.
            result = otherTopic.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test SetQos with null parameter
            result = topic.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }
        #endregion
    }
}
