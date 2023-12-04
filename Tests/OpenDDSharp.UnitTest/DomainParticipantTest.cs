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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="DomainParticipant"/> unit test class.
    /// </summary>
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
        /// <summary>
        /// Gets or sets test context object.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by MSTest")]
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
        /// <summary>
        /// The test initializer method.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            if (TestContext.TestName == nameof(TestGetDiscoveredTopicData) ||
                TestContext.TestName == nameof(TestGetDiscoveredTopics))
            {
                return;
            }

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
        /// Test the <see cref="DomainParticipant.DomainId" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDomainId()
        {
            Assert.IsNotNull(_participant);
            Assert.AreEqual(AssemblyInitializer.RTPS_DOMAIN, _participant.DomainId);
        }

        /// <summary>
        /// Test the default values for a new <see cref="DomainParticipantQos"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNewParticipantQos()
        {
            var qos = new DomainParticipantQos();
            Assert.IsNotNull(qos.UserData);
            TestHelper.TestDefaultDomainParticipantQos(qos);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetQos(DomainParticipantQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            var qos = TestHelper.CreateNonDefaultDomainParticipantQos();

            var result = _participant.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test with null parameter.
            result = _participant.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.SetQos(DomainParticipantQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Creates a non-default QoS, set it an check it.
            var qos = TestHelper.CreateNonDefaultDomainParticipantQos();

            var result = _participant.SetQos(qos);
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

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetListener()" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetListener()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var listener = _participant.GetListener();
            Assert.IsNull(listener);

            using var otherListener = new MyParticipantListener();
            var other = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_OTHER_DOMAIN, null, otherListener);
            Assert.IsNotNull(other);
            other.BindRtpsUdpTransportConfig();

            listener = other.GetListener();
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.IsNotNull(listener);

            var result = AssemblyInitializer.Factory.DeleteParticipant(other);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.SetListener(DomainParticipantListener)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetListener()
        {
            var listener = _participant.Listener;
            Assert.IsNull(listener);

            using var myListener = new MyParticipantListener();
            var result = _participant.SetListener(myListener);
            Assert.AreEqual(ReturnCode.Ok, result);

            listener = _participant.Listener;
            Assert.AreEqual(myListener, listener);

            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            listener = _participant.Listener;
            Assert.IsNull(listener);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.DeleteContainedEntities()" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteContainedEntities()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestDeleteContainedEntities), typeName);
            Assert.IsNotNull(topic);
            Assert.IsTrue(_participant.ContainsEntity(topic.InstanceHandle));

            var pub = _participant.CreatePublisher();
            Assert.IsNotNull(pub);
            Assert.IsTrue(_participant.ContainsEntity(pub.InstanceHandle));

            var sub = _participant.CreateSubscriber();
            Assert.IsNotNull(sub);
            Assert.IsTrue(_participant.ContainsEntity(sub.InstanceHandle));

            var dataWriter = pub.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);
            Assert.IsTrue(_participant.ContainsEntity(dataWriter.InstanceHandle));

            var dataReader = sub.CreateDataReader(topic);
            Assert.IsNotNull(dataReader);
            Assert.IsTrue(_participant.ContainsEntity(dataReader.InstanceHandle));

            result = _participant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the default values for a new <see cref="TopicQos"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNewTopicQos()
        {
            var qos = new TopicQos();
            Assert.IsNotNull(qos.TopicData);
            TestHelper.TestDefaultTopicQos(qos);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetDefaultTopicQos(TopicQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDefaultTopicQos()
        {
            var qos = TestHelper.CreateNonDefaultTopicQos();

            var result = _participant.GetDefaultTopicQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultTopicQos(qos);

            // Test with null parameter
            result = _participant.GetDefaultTopicQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.SetDefaultTopicQos(TopicQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultTopicQos()
        {
            var qos = TestHelper.CreateNonDefaultTopicQos();

            var result = _participant.SetDefaultTopicQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new TopicQos();
            result = _participant.GetDefaultTopicQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultTopicQos(qos);

            // Test with null parameter
            result = _participant.SetDefaultTopicQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.CreateTopic(string, string, TopicQos, TopicListener, StatusMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateTopic()
        {
            var qos = new TopicQos
            {
                Durability =
                {
                    Kind = DurabilityQosPolicyKind.PersistentDurabilityQos,
                },
            };

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with full parameters
            using var listener = new MyTopicListener();
            var topic = _participant.CreateTopic(nameof(TestCreateTopic), typeName, qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(topic);
            Assert.IsNotNull(topic.Listener);
            Assert.AreEqual(nameof(TestCreateTopic), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var getQos = new TopicQos();
            result = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(DurabilityQosPolicyKind.PersistentDurabilityQos, getQos.Durability.Kind);

            // Test with only listener and status mask
            var topic1 = _participant.CreateTopic(nameof(TestCreateTopic) + "1", typeName, qos, listener);
            Assert.IsNotNull(topic1);
            Assert.IsNotNull(topic1.Listener);
            Assert.AreEqual(nameof(TestCreateTopic) + "1", topic1.Name);
            Assert.AreEqual(typeName, topic1.TypeName);

            // Test with only listener and status mask
            var topic2 = _participant.CreateTopic(nameof(TestCreateTopic) + "2", typeName, null, listener, StatusKind.DataAvailableStatus | StatusKind.InconsistentTopicStatus);
            Assert.IsNotNull(topic2);
            Assert.IsNotNull(topic2.Listener);
            Assert.AreEqual(nameof(TestCreateTopic) + "2", topic2.Name);
            Assert.AreEqual(typeName, topic2.TypeName);

            // Test with null topic name
            var topic3 = _participant.CreateTopic(null, typeName, null, listener, StatusKind.DataAvailableStatus | StatusKind.InconsistentTopicStatus);
            Assert.IsNull(topic3);

            // Test with null type name
            var topic4 = _participant.CreateTopic(nameof(TestCreateTopic) + "3", null, null, listener, StatusKind.DataAvailableStatus | StatusKind.InconsistentTopicStatus);
            Assert.IsNull(topic4);

            // Test with wrong configuration
            var topic5 = _participant.CreateTopic(nameof(TestCreateTopic), "OtherName", null, listener, StatusKind.DataAvailableStatus | StatusKind.InconsistentTopicStatus);
            Assert.IsNull(topic5);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.FindTopic(string, Duration)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestFindTopic()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestFindTopic), typeName);
            Assert.IsNotNull(topic);

            var foundTopic = _participant.FindTopic(nameof(TestFindTopic), new Duration
            {
                Seconds = 60,
                NanoSeconds = 0,
            });

            Assert.IsNotNull(foundTopic);
            Assert.AreEqual(nameof(TestFindTopic), foundTopic.Name);
            Assert.AreEqual(typeName, foundTopic.TypeName);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.LookupTopicDescription(string)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupTopicDescription()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestLookupTopicDescription), typeName);
            Assert.IsNotNull(topic);

            var foundTopic = _participant.LookupTopicDescription(nameof(TestLookupTopicDescription));
            Assert.IsNotNull(foundTopic);

            var notFoundTopic = _participant.LookupTopicDescription("NoFoundTopic");
            Assert.IsNull(notFoundTopic);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.DeleteTopic(Topic)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteTopic()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestDeleteTopic), typeName);
            Assert.IsNotNull(topic);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var reader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(reader);

            result = _participant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            result = publisher.DeleteDataWriter(writer);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with null parameter
            result = _participant.DeleteTopic(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the default values for a new <see cref="PublisherQos"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNewPublisherQos()
        {
            var qos = new PublisherQos();
            Assert.IsNotNull(qos.GroupData);
            TestHelper.TestDefaultPublisherQos(qos);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetDefaultPublisherQos(PublisherQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDefaultPublisherQos()
        {
            var qos = TestHelper.CreateNonDefaultPublisherQos();

            var result = _participant.GetDefaultPublisherQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultPublisherQos(qos);

            // Test with null parameter
            result = _participant.GetDefaultPublisherQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.SetDefaultPublisherQos(PublisherQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultPublisherQos()
        {
            var qos = TestHelper.CreateNonDefaultPublisherQos();

            var result = _participant.SetDefaultPublisherQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new PublisherQos();
            result = _participant.GetDefaultPublisherQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultPublisherQos(qos);

            // Test with null parameter
            result = _participant.SetDefaultPublisherQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.CreatePublisher(PublisherQos, PublisherListener, StatusMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreatePublisher()
        {
            var qos = TestHelper.CreateNonDefaultPublisherQos();

            using var listener = new MyPublisherListener();
            var publisher = _participant.CreatePublisher(qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(publisher);
            Assert.IsNotNull(publisher.Listener);

            var getPublisherQos = new PublisherQos();
            var result = publisher.GetQos(getPublisherQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultPublisherQos(getPublisherQos);

            // Test with listener and status mask
            var publisher1 = _participant.CreatePublisher(null, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(publisher1);
            Assert.IsNotNull(publisher.Listener);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.DeletePublisher(Publisher)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeletePublisher()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestDeletePublisher), typeName);
            Assert.IsNotNull(topic);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);

            // Cannot be deleted if have active DataWriters
            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            result = publisher.DeleteDataWriter(writer);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with null parameter
            result = _participant.DeletePublisher(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the default values for a new <see cref="SubscriberQos"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNewSubscriberQos()
        {
            var qos = new SubscriberQos();
            Assert.IsNotNull(qos.GroupData);
            TestHelper.TestDefaultSubscriberQos(qos);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetDefaultSubscriberQos(SubscriberQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDefaultSubscriberQos()
        {
            var qos = TestHelper.CreateNonDefaultSubscriberQos();

            var result = _participant.GetDefaultSubscriberQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultSubscriberQos(qos);

            result = _participant.GetDefaultSubscriberQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.SetDefaultSubscriberQos(SubscriberQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultSubscriberQos()
        {
            var qos = TestHelper.CreateNonDefaultSubscriberQos();

            var result = _participant.SetDefaultSubscriberQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new SubscriberQos();
            result = _participant.GetDefaultSubscriberQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultSubscriberQos(qos);

            // Test with null parameter
            result = _participant.SetDefaultSubscriberQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.CreateSubscriber(SubscriberQos, SubscriberListener, StatusMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateSubscriber()
        {
            var qos = TestHelper.CreateNonDefaultSubscriberQos();

            using var listener = new MySubscriberListener();
            var subscriber = _participant.CreateSubscriber(qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(subscriber);
            Assert.IsNotNull(subscriber.Listener);

            var getSubscriberQos = new SubscriberQos();
            var result = subscriber.GetQos(getSubscriberQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultSubscriberQos(qos);

            // Test with listener and status mask parameters
            subscriber = _participant.CreateSubscriber(null, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(subscriber);
            Assert.IsNotNull(subscriber.Listener);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.DeleteSubscriber(Subscriber)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteSubscriber()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestDeleteSubscriber), typeName);
            Assert.IsNotNull(topic);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var reader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(reader);

            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with null parameter
            result = _participant.DeleteSubscriber(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetBuiltinSubscriber" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetBuiltinSubscriber()
        {
            var subscriber = _participant.GetBuiltinSubscriber();
            Assert.IsNotNull(subscriber);

            var other = _participant.GetBuiltinSubscriber();
            Assert.AreSame(subscriber, other);

            Assert.IsNull(subscriber.Listener);
            Assert.AreNotEqual(subscriber.InstanceHandle, InstanceHandle.HandleNil);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.ContainsEntity(InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestContainsEntity()
        {
            var builtin = _participant.GetBuiltinSubscriber();
            Assert.IsNotNull(builtin);

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestContainsEntity), typeName);
            Assert.IsNotNull(topic);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var dataWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);

            var dataReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(dataReader);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_OTHER_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();
            Assert.AreNotSame(otherParticipant, _participant);
            Assert.AreNotSame(otherParticipant.InstanceHandle, _participant.InstanceHandle);
            Assert.AreNotEqual(otherParticipant.InstanceHandle, _participant.InstanceHandle);

            result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var otherTopic = otherParticipant.CreateTopic("Other" + nameof(TestContainsEntity), typeName);
            Assert.IsNotNull(otherTopic);

            var otherBuiltin = otherParticipant.GetBuiltinSubscriber();
            Assert.IsNotNull(otherBuiltin);

            var otherSubscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(otherSubscriber);

            var otherPublisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(otherPublisher);

            var otherDataReader = otherSubscriber.CreateDataReader(otherTopic);
            Assert.IsNotNull(otherDataReader);

            var otherDataWriter = otherPublisher.CreateDataWriter(otherTopic);
            Assert.IsNotNull(otherDataWriter);

            Assert.IsTrue(_participant.ContainsEntity(builtin.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(topic.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(publisher.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(subscriber.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(dataWriter.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(dataReader.InstanceHandle));

            Assert.IsTrue(otherParticipant.ContainsEntity(otherBuiltin.InstanceHandle));
            Assert.IsTrue(otherParticipant.ContainsEntity(otherTopic.InstanceHandle));
            Assert.IsTrue(otherParticipant.ContainsEntity(otherSubscriber.InstanceHandle));
            Assert.IsTrue(otherParticipant.ContainsEntity(otherPublisher.InstanceHandle));
            Assert.IsTrue(otherParticipant.ContainsEntity(otherDataWriter.InstanceHandle));
            Assert.IsTrue(otherParticipant.ContainsEntity(otherDataReader.InstanceHandle));

            // TODO: OpenDDS ISSUE
            // ISSUE: Instance handle overlap between participants.
            // Possible solution: Move the InstanceHandleGenerator to the Service_Participant.
#pragma warning disable S125  // Sections of code should not be commented out

            // Assert.IsFalse(_participant.ContainsEntity(otherBuiltin.InstanceHandle));
            // Assert.IsFalse(_participant.ContainsEntity(otherTopic.InstanceHandle));
            // Assert.IsFalse(_participant.ContainsEntity(otherSubscriber.InstanceHandle));
            // Assert.IsFalse(_participant.ContainsEntity(otherPublisher.InstanceHandle));
            // Assert.IsFalse(_participant.ContainsEntity(otherDataWriter.InstanceHandle));
            // Assert.IsFalse(_participant.ContainsEntity(otherDataReader.InstanceHandle));

            // Assert.IsFalse(otherParticipant.ContainsEntity(builtin.InstanceHandle));
            // Assert.IsFalse(otherParticipant.ContainsEntity(topic.InstanceHandle));
            // Assert.IsFalse(otherParticipant.ContainsEntity(publisher.InstanceHandle));
            // Assert.IsFalse(otherParticipant.ContainsEntity(subscriber.InstanceHandle));
            // Assert.IsFalse(otherParticipant.ContainsEntity(dataWriter.InstanceHandle));
            // Assert.IsFalse(otherParticipant.ContainsEntity(dataReader.InstanceHandle));
            result = otherParticipant.DeleteContainedEntities();
#pragma warning restore S125 // Sections of code should not be commented out

            Assert.AreEqual(ReturnCode.Ok, result);
            result = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.IgnoreParticipant(InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestIgnoreParticipant()
        {
            var other = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(other);
            other.BindRtpsUdpTransportConfig();

            var result = _participant.IgnoreParticipant(other.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = other.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(other);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.IgnoreTopic(InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestIgnoreTopic()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestIgnoreTopic), support.GetTypeName());
            Assert.IsNotNull(topic);

            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteTopic(topic));

            result = _participant.IgnoreTopic(topic.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.IgnoreTopic(InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestIgnorePublication()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestIgnorePublication), support.GetTypeName());
            Assert.IsNotNull(topic);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var dataWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);

            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteDataWriter(dataWriter));
            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _participant.DeletePublisher(publisher));
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteTopic(topic));

            result = _participant.IgnorePublication(dataWriter.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.IgnoreSubscription(InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestIgnoreSubscription()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestIgnoreSubscription), support.GetTypeName());
            Assert.IsNotNull(topic);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var dataReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(dataReader);

            Assert.AreEqual(ReturnCode.Ok, dataReader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(dataReader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteSubscriber(subscriber));
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteTopic(topic));

            result = _participant.IgnoreSubscription(dataReader.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.AssertLiveliness" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestAssertLiveliness()
        {
            var result = _participant.AssertLiveliness();
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetCurrentTimestamp(ref Timestamp)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetCurrentTimestamp()
        {
            var ts = new Timestamp()
            {
                Seconds = 0,
                NanoSeconds = 0,
            };

            var now = DateTime.Now;
            var result = _participant.GetCurrentTimestamp(ref ts);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsTrue(ts.Seconds > 0);

            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(ts.Seconds);
            dtDateTime = dtDateTime.AddMilliseconds(ts.NanoSeconds / 100000d);

            Assert.AreEqual(dtDateTime.Year, now.Year);
            Assert.AreEqual(dtDateTime.Month, now.Month);
            Assert.AreEqual(dtDateTime.Day, now.Day);
            Assert.AreEqual(dtDateTime.DayOfWeek, now.DayOfWeek);
            Assert.AreEqual(dtDateTime.DayOfYear, now.DayOfYear);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetDiscoveredParticipants(ICollection{InstanceHandle})" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDiscoveredParticipants()
        {
            var handles = new List<InstanceHandle>();
            var result = _participant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, handles.Count);

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 20_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 20_000));

            result = _participant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, handles.Count);

            handles = new List<InstanceHandle>();
            result = otherParticipant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, handles.Count);

            // Test with null parameter
            result = _participant.GetDiscoveredParticipants(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            result = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetDiscoveredParticipantData(ref ParticipantBuiltinTopicData, InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDiscoveredParticipantData()
        {
            var qos = new DomainParticipantQos
            {
                UserData =
                {
                    Value = new List<byte> { 0x42 },
                },
            };
            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN, qos);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 5_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 5_000));

            var handles = new List<InstanceHandle>();
            var result = _participant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, handles.Count);

            ParticipantBuiltinTopicData data = default;
            result = _participant.GetDiscoveredParticipantData(ref data, handles[0]);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, data.UserData.Value.Count);
            Assert.AreEqual(0x42, data.UserData.Value[0]);
            Assert.IsNotNull(data.Key);
            Assert.IsNotNull(data.Key.Value);

            result = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetDiscoveredTopics(ICollection{InstanceHandle})" /> method.
        /// </summary>
        /// <remarks>
        /// OPENDDS ISSUE: Not working correctly with RTPS.
        /// OPENDDS ISSUE: Only discover local topics.
        /// </remarks>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDiscoveredTopics()
        {
            var participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(participant);
            participant.BindTcpTransportConfig();

            var handles = new List<InstanceHandle>();
            var result = participant.GetDiscoveredTopics(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, handles.Count);

            // Create a new topic and check that is discovered
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            result = support.RegisterType(participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = participant.CreateTopic(nameof(TestGetDiscoveredTopics), typeName);
            Assert.IsNotNull(topic);
            var handle = topic.InstanceHandle;

            result = participant.GetDiscoveredTopics(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, handles.Count);
            Assert.AreEqual(handle, handles[0]);

            // Test with null parameter
            result = participant.GetDiscoveredTopics(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Remove the participant
            participant.DeleteContainedEntities();
            AssemblyInitializer.Factory.DeleteParticipant(participant);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetDiscoveredTopicData(ref TopicBuiltinTopicData, InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDiscoveredTopicData()
        {
            var participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(participant);
            participant.BindTcpTransportConfig();

            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindTcpTransportConfig();

            var handles = new List<InstanceHandle>();
            var result = participant.GetDiscoveredTopics(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, handles.Count);

            // Create a new topic and check that is discovered
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            result = support.RegisterType(participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var qos = new TopicQos
            {
                TopicData =
                {
                    Value = new byte[] { 0x42 },
                },
            };
            var topic = participant.CreateTopic(nameof(TestGetDiscoveredTopicData), typeName, qos);
            Assert.IsNotNull(topic);

            var otherTopic = otherParticipant.CreateTopic(nameof(TestGetDiscoveredTopicData), typeName, qos);
            Assert.IsNotNull(otherTopic);

            var publisher = participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var reader = subscriber.CreateDataReader(otherTopic);
            Assert.IsNotNull(reader);

            Assert.IsTrue(reader.WaitForPublications(1, 50_000));
            Assert.IsTrue(writer.WaitForSubscriptions(1, 50_000));

            var handle = topic.InstanceHandle;
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = participant.GetDiscoveredTopics(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsTrue(handles.Count > 0);
            Assert.IsTrue(handles.Contains(handle));

            TopicBuiltinTopicData data = default;
            result = participant.GetDiscoveredTopicData(ref data, handle);
            while (result == ReturnCode.BadParameter)
            {
                result = participant.GetDiscoveredTopicData(ref data, handle);
            }

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(nameof(TestGetDiscoveredTopicData), data.Name);
            Assert.AreEqual(typeName, data.TypeName);
            Assert.IsNotNull(data.Key);
            Assert.IsNotNull(data.Key);
            Assert.IsNotNull(data.TopicData);
            Assert.IsNotNull(data.TopicData.Value);
            Assert.AreEqual(1, data.TopicData.Value.Count);
            Assert.AreEqual(0x42, data.TopicData.Value[0]);

            // Remove the participants
            participant.DeleteContainedEntities();
            AssemblyInitializer.Factory.DeleteParticipant(participant);

            otherParticipant.DeleteContainedEntities();
            AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.CreateContentFilteredTopic(string, Topic, string, string[])" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateContentFilteredTopic()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestCreateContentFilteredTopic), typeName);
            Assert.IsNotNull(topic);

            const int totalInstances = 10;
            const int filterCount = 5;
            var filteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", topic, "(Id <= %0)", filterCount.ToString());
            Assert.IsNotNull(filteredTopic);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);

            var filteredReader = subscriber.CreateDataReader(filteredTopic, drQos);
            Assert.IsNotNull(filteredReader);

            var dataReader = new TestStructDataReader(reader);
            var filteredDataReader = new TestStructDataReader(filteredReader);

            var waitSet = new WaitSet();
            var cancelConditionReader = new GuardCondition();
            var statusConditionReader = reader.StatusCondition;
            waitSet.AttachCondition(cancelConditionReader);
            waitSet.AttachCondition(statusConditionReader);
            statusConditionReader.EnabledStatuses = StatusKind.DataAvailableStatus;

            var countReader = 0;
            var threadReader = new Thread(() =>
            {
                while (true)
                {
                    ICollection<Condition> conditions = new List<Condition>();
                    var duration = new Duration
                    {
                        Seconds = 60,
                    };
                    waitSet.Wait(conditions, duration);

                    foreach (var cond in conditions)
                    {
                        if (cond == statusConditionReader && cond.TriggerValue)
                        {
                            var sCond = (StatusCondition)cond;
                            var mask = sCond.EnabledStatuses;
                            if ((mask & StatusKind.DataAvailableStatus) != 0)
                            {
                                var receivedData = new List<TestStruct>();
                                var sampleInfos = new List<SampleInfo>();
                                dataReader.Take(receivedData, sampleInfos);

                                countReader += sampleInfos.Count(sampleInfo =>
                                        sampleInfo.ValidData &&
                                        sampleInfo.InstanceState == InstanceStateKind.AliveInstanceState);
                            }
                        }

                        if (cond != cancelConditionReader || !cond.TriggerValue)
                        {
                            continue;
                        }

                        // We reset the cancellation condition because it is a good practice, but in this implementation it probably doesn't change anything.
                        cancelConditionReader.TriggerValue = false;

                        // The thread activity has been canceled.
                        return;
                    }
                }
            });

            var waitSetFiltered = new WaitSet();
            var cancelConditionFilteredReader = new GuardCondition();
            var statusConditionFilteredReader = filteredReader.StatusCondition;
            waitSetFiltered.AttachCondition(cancelConditionFilteredReader);
            waitSetFiltered.AttachCondition(statusConditionFilteredReader);
            statusConditionFilteredReader.EnabledStatuses = StatusKind.DataAvailableStatus;

            var countFilteredReader = 0;
            var threadFilteredReader = new Thread(() =>
            {
                while (true)
                {
                    ICollection<Condition> conditions = new List<Condition>();
                    var duration = new Duration
                    {
                        Seconds = 60,
                    };
                    waitSetFiltered.Wait(conditions, duration);

                    foreach (var cond in conditions)
                    {
                        if (cond == statusConditionFilteredReader && cond.TriggerValue)
                        {
                            var sCond = (StatusCondition)cond;
                            var mask = sCond.EnabledStatuses;
                            if ((mask & StatusKind.DataAvailableStatus) != 0)
                            {
                                var receivedData = new List<TestStruct>();
                                var sampleInfos = new List<SampleInfo>();
                                filteredDataReader.Take(receivedData, sampleInfos);

                                countFilteredReader += sampleInfos.Count(sampleInfo =>
                                    sampleInfo.ValidData &&
                                    sampleInfo.InstanceState == InstanceStateKind.AliveInstanceState);
                            }
                        }

                        if (cond != cancelConditionFilteredReader || !cond.TriggerValue)
                        {
                            continue;
                        }

                        // We reset the cancellation condition because it is a good practice, but in this implementation it probably doesn't change anything.
                        cancelConditionFilteredReader.TriggerValue = false;

                        // The thread activity has been canceled.
                        return;
                    }
                }
            });

            threadReader.IsBackground = true;
            threadReader.Priority = ThreadPriority.Highest;
            threadReader.Start();

            threadFilteredReader.IsBackground = true;
            threadFilteredReader.Priority = ThreadPriority.Highest;
            threadFilteredReader.Start();

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(writer);

            var dataWriter = new TestStructDataWriter(writer);

            // Wait for subscriptions
            dataWriter.WaitForSubscriptions(2, 5_000);

            for (var i = 1; i <= totalInstances; i++)
            {
                result = dataWriter.Write(new TestStruct
                {
                    Id = i,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(new Duration
                {
                    Seconds = 5,
                });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            cancelConditionReader.TriggerValue = true;
            cancelConditionFilteredReader.TriggerValue = true;
            threadFilteredReader.Join();
            threadReader.Join();

            result = waitSet.DetachCondition(cancelConditionReader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = waitSet.DetachCondition(statusConditionReader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = waitSetFiltered.DetachCondition(cancelConditionFilteredReader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = waitSetFiltered.DetachCondition(statusConditionFilteredReader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = subscriber.DeleteDataReader(filteredReader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = publisher.DeleteDataWriter(writer);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteContentFilteredTopic(filteredTopic);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.AreEqual(totalInstances, countReader);
            Assert.AreEqual(filterCount, countFilteredReader);

            // Test with null name
            var nullFilteredTopic = _participant.CreateContentFilteredTopic(null, topic,
                "(Id <= %0)", filterCount.ToString());
            Assert.IsNull(nullFilteredTopic);

            // Test with null related topic
            nullFilteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", null,
                "(Id <= %0)", filterCount.ToString());
            Assert.IsNull(nullFilteredTopic);

            // Test with null expression
            nullFilteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", topic,
                null, filterCount.ToString());
            Assert.IsNull(nullFilteredTopic);

            // Test wrong topic creation (same name than other topic is not allowed)
            nullFilteredTopic = _participant.CreateContentFilteredTopic(nameof(TestCreateContentFilteredTopic), topic,
                "(Id <= %0)", filterCount.ToString());
            Assert.IsNull(nullFilteredTopic);

            // Test without expression parameters
            var filteredTopic1 = _participant.CreateContentFilteredTopic("FilteredTopic1", topic, "(Id <= 1)");
            Assert.IsNotNull(filteredTopic1);

            // Test with null expression parameters
            var filteredTopic2 = _participant.CreateContentFilteredTopic("FilteredTopic2", topic, "(Id <= 1)", null);
            Assert.IsNotNull(filteredTopic2);

            var filteredTopic3 = _participant.CreateContentFilteredTopic("FilteredTopic", null, "(Id <= %1 AND Id <= %2)", filterCount.ToString(), "2");
            Assert.IsNull(filteredTopic3);

            result = _participant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.DeleteContentFilteredTopic(ContentFilteredTopic)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteContentFilteredTopic()
        {
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestDeleteContentFilteredTopic), support.GetTypeName());
            Assert.IsNotNull(topic);

            const int filterCount = 5;
            var filteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", topic, "(Id < %0)", filterCount.ToString());
            Assert.IsNotNull(filteredTopic);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var reader = subscriber.CreateDataReader(filteredTopic);
            Assert.IsNotNull(reader);

            result = _participant.DeleteContentFilteredTopic(filteredTopic);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteContentFilteredTopic(filteredTopic);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with null parameter
            result = _participant.DeleteContentFilteredTopic(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.CreateMultiTopic(string, string, string, string[])" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateMultiTopic()
        {
            using var evt = new ManualResetEventSlim(false);

            var athleteSupport = new AthleteTypeSupport();
            var athleteTypeName = athleteSupport.GetTypeName();
            var result = athleteSupport.RegisterType(_participant, athleteTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var athleteTopic = _participant.CreateTopic("AthleteTopic", athleteTypeName);
            Assert.IsNotNull(athleteTopic);

            var resultSupport = new ResultTypeSupport();
            var resultTypeName = resultSupport.GetTypeName();
            result = resultSupport.RegisterType(_participant, resultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var resultTopic = _participant.CreateTopic("ResultTopic", resultTypeName);
            Assert.IsNotNull(resultTopic);

            var athleteResultSupport = new AthleteResultTypeSupport();
            var athleteResultTypeName = athleteResultSupport.GetTypeName();
            result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var multiTopic = _participant.CreateMultiTopic("AthleteResultTopic", athleteResultTypeName,
                "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic");
            Assert.IsNotNull(multiTopic);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var dr = subscriber.CreateDataReader(multiTopic, drQos);
            Assert.IsNotNull(dr);
            var dataReader = new AthleteResultDataReader(dr);

            var statusCondition = dataReader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var dwAthlete = publisher.CreateDataWriter(athleteTopic, dwQos);
            Assert.IsNotNull(dwAthlete);
            var athleteDataWriter = new AthleteDataWriter(dwAthlete);

            var dwResult = publisher.CreateDataWriter(resultTopic, dwQos);
            Assert.IsNotNull(dwResult);
            var resultDataWriter = new ResultDataWriter(dwResult);

            // Wait for subscriptions
            Assert.IsTrue(athleteDataWriter.WaitForSubscriptions(1, 5_000));
            Assert.IsTrue(resultDataWriter.WaitForSubscriptions(1, 5_000));

            for (var i = 1; i <= 5; i++)
            {
                athleteDataWriter.Write(new Athlete
                {
                    AthleteId = i,
                    FirstName = "FirstName" + i,
                    SecondName = "SecondName" + i,
                    Country = "Country" + i,
                });

                result = athleteDataWriter.WaitForAcknowledgments(new Duration
                {
                    Seconds = 5,
                });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            for (var i = 1; i <= 3; i++)
            {
                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                resultDataWriter.Write(new Result
                {
                    AthleteId = i,
                    Rank = i,
                    Score = 10f - i,
                });

                result = resultDataWriter.WaitForAcknowledgments(new Duration
                {
                    Seconds = 5,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(5_000));
            }

            var receivedData = new List<AthleteResult>();

            while (receivedData.Count < 3)
            {
                var samples = new List<AthleteResult>();
                var sampleInfos = new List<SampleInfo>();
                result = dataReader.Take(samples, sampleInfos);
                if (result == ReturnCode.Ok)
                {
                    receivedData.AddRange(samples);
                }
            }

            var sample = new AthleteResult();
            var sampleInfo = new SampleInfo();
            Assert.AreEqual(ReturnCode.NoData, dataReader.ReadNextSample(sample, sampleInfo));

            Assert.AreEqual(3, receivedData.Count);
            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual(i + 1, receivedData[i].AthleteId);
                Assert.AreEqual(i + 1, receivedData[i].Rank);
                Assert.AreEqual(10f - (i + 1), receivedData[i].Score);
                Assert.AreEqual("FirstName" + (i + 1), receivedData[i].FirstName);
                Assert.AreEqual("SecondName" + (i + 1), receivedData[i].SecondName);
                Assert.AreEqual("Country" + (i + 1), receivedData[i].Country);
            }

            // Test with null name
            var nullMultiTopic = _participant.CreateMultiTopic(null, athleteResultTypeName, "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic");
            Assert.IsNull(nullMultiTopic);

            // Test with null type name
            nullMultiTopic = _participant.CreateMultiTopic("AthleteResultTopic", null, "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic");
            Assert.IsNull(nullMultiTopic);

            // Test with null expression
            nullMultiTopic = _participant.CreateMultiTopic("AthleteResultTopic", athleteResultTypeName, null);
            Assert.IsNull(nullMultiTopic);

            // Test wrong creation (same name than other topic is not allowed)
            nullMultiTopic = _participant.CreateMultiTopic("AthleteTopic", athleteResultTypeName, "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic");
            Assert.IsNull(nullMultiTopic);

            // Test with null expression parameters
            var multiTopic1 = _participant.CreateMultiTopic("AthleteResultTopic1",
                athleteResultTypeName,
                "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic", null);
            Assert.IsNotNull(multiTopic1);

            // Test with expression parameters
            var multiTopic2 = _participant.CreateMultiTopic("AthleteResultTopic2",
                athleteResultTypeName,
                "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic WHERE Id >= %0 AND Id <= %1", "0", "10");
            Assert.IsNotNull(multiTopic2);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.DeleteMultiTopic(MultiTopic)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteMultiTopic()
        {
            var athleteSupport = new AthleteTypeSupport();
            var athleteTypeName = athleteSupport.GetTypeName();
            var result = athleteSupport.RegisterType(_participant, athleteTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var athleteTopic = _participant.CreateTopic("AthleteTopic", athleteTypeName);
            Assert.IsNotNull(athleteTopic);

            var resultSupport = new ResultTypeSupport();
            var resultTypeName = resultSupport.GetTypeName();
            result = resultSupport.RegisterType(_participant, resultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var resultTopic = _participant.CreateTopic("ResultTopic", resultTypeName);
            Assert.IsNotNull(resultTopic);

            var athleteResultSupport = new AthleteResultTypeSupport();
            var athleteResultTypeName = athleteResultSupport.GetTypeName();
            result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var multiTopic = _participant.CreateMultiTopic(nameof(TestDeleteMultiTopic),
                athleteResultTypeName,
                "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic");
            Assert.IsNotNull(multiTopic);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var reader = subscriber.CreateDataReader(multiTopic);
            Assert.IsNotNull(reader);

            result = _participant.DeleteMultiTopic(multiTopic);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteMultiTopic(multiTopic);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with null parameter
            result = _participant.DeleteMultiTopic(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion
    }
}
