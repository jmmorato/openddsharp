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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="Topic"/> unit test class.
    /// </summary>
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
        /// <summary>
        /// The test initializer method.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _participant?.DeleteContainedEntities();
            AssemblyInitializer.Factory?.DeleteParticipant(_participant);

            _participant = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Tests the <see cref="Topic"/> class properties.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestProperties()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestProperties), typeName);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestProperties), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);
        }

        /// <summary>
        /// Test the <see cref="TopicQos"/> default constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Included in the calling method.")]
        public void TestNewTopicQos()
        {
            var qos = new TopicQos();
            TestHelper.TestDefaultTopicQos(qos);
        }

        /// <summary>
        /// Test the <see cref="Topic.GetQos(TopicQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            // Create a non-default QoS.
            var qos = TestHelper.CreateNonDefaultTopicQos();

            // Create a new Topic using the non-default QoS.
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestGetQos), typeName, qos);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestGetQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

            // Get the QoS and check it.
            var getQos = new TopicQos();
            result = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultTopicQos(getQos);

            // Test GetQos with null parameter.
            result = topic.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Topic.SetQos(TopicQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Create a new Topic using the default QoS
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestSetQos), typeName);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestSetQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

            // Get the qos to ensure that is using the default properties.
            var getQos = new TopicQos();
            result = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultTopicQos(getQos);

            // Try to change an immutable property
            var qos = new TopicQos
            {
                DestinationOrder =
                {
                    Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos,
                },
            };

            result = topic.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them
            qos = new TopicQos
            {
                Deadline =
                {
                    Period = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 0,
                    },
                },
                LatencyBudget =
                {
                    Duration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    }
                },
                Lifespan =
                {
                    Duration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                TopicData =
                {
                    Value = new List<byte> { 0x5 },
                },
            };

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
            Assert.AreEqual(0x5, qos.TopicData.Value[0]);

            // Create a disabled topic and try to set an inconsistent qos
            DomainParticipantQos pQos = new DomainParticipantQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
            };
            _participant.SetQos(pQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            var otherTopic = _participant.CreateTopic("Other" + nameof(TestSetQos), typeName);
            Assert.IsNotNull(otherTopic);
            Assert.AreEqual("Other" + nameof(TestSetQos), otherTopic.Name);
            Assert.AreEqual(typeName, otherTopic.TypeName);
            Assert.AreEqual(_participant, otherTopic.Participant);

            qos = new TopicQos
            {
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepLastHistoryQos,
                    Depth = 200,
                },
                ResourceLimits =
                {
                    MaxSamplesPerInstance = 1,
                },
            };

            result = otherTopic.SetQos(qos);
            Assert.AreEqual(ReturnCode.InconsistentPolicy, result);

            // If don't enable the entity before delete it, it fails on OpenDDS:
            // DomainParticipantImpl::delete_topic_i, remove_topic failed with return value 3
            // That didn't happen in OpenDDS 3.13.x.
            result = otherTopic.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test SetQos with null parameter
            result = topic.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Topic.GetListener"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetListener()
        {
            // Create a new Topic with a listener
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            using var listener = new MyTopicListener();
            var topic = _participant.CreateTopic(nameof(TestGetQos), typeName, null, listener);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestGetQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

#pragma warning disable CS0618 // Type or member is obsolete

            // Call to GetListener and check the listener returned
            var received = (MyTopicListener)topic.GetListener();

#pragma warning restore CS0618 // Type or member is obsolete

            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);
        }

        /// <summary>
        /// Test the <see cref="Topic.SetListener(TopicListener, StatusMask)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetListener()
        {
            // Create a new Topic without listener
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestSetListener), typeName);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestSetListener), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

            var listener = (MyTopicListener)topic.Listener;
            Assert.IsNull(listener);

            // Create a listener, set it and check that is correctly set
            listener = new MyTopicListener();
            result = topic.SetListener(listener);
            Assert.AreEqual(ReturnCode.Ok, result);

            var received = (MyTopicListener)topic.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            // Remove the listener calling SetListener with null and check it
            result = topic.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);
            listener.Dispose();

            received = (MyTopicListener)topic.Listener;
            Assert.IsNull(received);
        }

        /// <summary>
        /// Test the <see cref="Topic.GetInconsistentTopicStatus(ref InconsistentTopicStatus)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetInconsistentTopicStatus()
        {
            // Create a new Topic and call GetInconsistentTopicStatus
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestGetInconsistentTopicStatus), typeName);
            Assert.IsNotNull(topic);
            Assert.AreEqual(nameof(TestGetInconsistentTopicStatus), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            Assert.AreEqual(_participant, topic.Participant);

            InconsistentTopicStatus status = default;
            result = topic.GetInconsistentTopicStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
        }
        #endregion
    }
}
