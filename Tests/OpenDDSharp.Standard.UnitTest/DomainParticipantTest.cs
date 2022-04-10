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
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using OpenDDSharp.Standard.UnitTest.Listeners;
using Test;

namespace OpenDDSharp.Standard.UnitTest
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
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
        /// <summary>
        /// The test initializer method.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            if (TestContext.TestName != nameof(TestGetDiscoveredTopicData) && TestContext.TestName != nameof(TestGetDiscoveredTopics))
            {
                _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
                Assert.IsNotNull(_participant);
                _participant.BindRtpsUdpTransportConfig();
            }
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            if (_participant != null)
            {
                ReturnCode result = _participant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, result);

                if (AssemblyInitializer.Factory != null)
                {
                    result = AssemblyInitializer.Factory.DeleteParticipant(_participant);
                    Assert.AreEqual(ReturnCode.Ok, result);
                    _participant = null;
                }
            }
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
            DomainParticipantQos qos = new DomainParticipantQos();
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
            DomainParticipantQos qos = TestHelper.CreateNonDefaultDomainParticipantQos();

            ReturnCode result = _participant.GetQos(qos);
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

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetListener()" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetListener()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            DomainParticipantListener listener = _participant.GetListener();
            Assert.IsNull(listener);

            MyParticipantListener otherListener = new MyParticipantListener();
            DomainParticipant other = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_OTHER_DOMAIN, null, otherListener);
            Assert.IsNotNull(other);
            other.BindRtpsUdpTransportConfig();

            listener = other.GetListener();
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.IsNotNull(listener);

            ReturnCode result = AssemblyInitializer.Factory.DeleteParticipant(other);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.SetListener(DomainParticipantListener)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetListener()
        {
            DomainParticipantListener listener = _participant.Listener;
            Assert.IsNull(listener);

            MyParticipantListener myListener = new MyParticipantListener();
            ReturnCode result = _participant.SetListener(myListener);
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
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteContainedEntities), typeName);
            Assert.IsNotNull(topic);
            Assert.IsTrue(_participant.ContainsEntity(topic.InstanceHandle));

            Publisher pub = _participant.CreatePublisher();
            Assert.IsNotNull(pub);
            Assert.IsTrue(_participant.ContainsEntity(pub.InstanceHandle));

            Subscriber sub = _participant.CreateSubscriber();
            Assert.IsNotNull(sub);
            Assert.IsTrue(_participant.ContainsEntity(sub.InstanceHandle));

            DataWriter dataWriter = pub.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);
            Assert.IsTrue(_participant.ContainsEntity(dataWriter.InstanceHandle));

            DataReader dataReader = sub.CreateDataReader(topic);
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
            TopicQos qos = new TopicQos();
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
            TopicQos qos = TestHelper.CreateNonDefaultTopicQos();

            ReturnCode result = _participant.GetDefaultTopicQos(qos);
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

        /// <summary>
        /// Test the <see cref="DomainParticipant.CreateTopic(string, string, TopicQos, TopicListener, StatusMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateTopic()
        {
            TopicQos qos = new TopicQos();
            qos.Durability.Kind = DurabilityQosPolicyKind.PersistentDurabilityQos;

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with full parameters
            MyTopicListener listener = new MyTopicListener();
            Topic topic = _participant.CreateTopic(nameof(TestCreateTopic), typeName, qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(topic);
            Assert.IsNotNull(topic.Listener);
            Assert.AreEqual(nameof(TestCreateTopic), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            TopicQos getQos = new TopicQos();
            result = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(DurabilityQosPolicyKind.PersistentDurabilityQos, getQos.Durability.Kind);

            // Test with only listener and status mask
            Topic topic1 = _participant.CreateTopic(nameof(TestCreateTopic) + "1", typeName, qos, listener);
            Assert.IsNotNull(topic1);
            Assert.IsNotNull(topic1.Listener);
            Assert.AreEqual(nameof(TestCreateTopic) + "1", topic1.Name);
            Assert.AreEqual(typeName, topic1.TypeName);

            // Test with only listener and status mask
            Topic topic2 = _participant.CreateTopic(nameof(TestCreateTopic) + "2", typeName, null, listener, StatusKind.DataAvailableStatus | StatusKind.InconsistentTopicStatus);
            Assert.IsNotNull(topic2);
            Assert.IsNotNull(topic2.Listener);
            Assert.AreEqual(nameof(TestCreateTopic) + "2", topic2.Name);
            Assert.AreEqual(typeName, topic2.TypeName);

            // Test with null topic name
            Topic topic3 = _participant.CreateTopic(null, typeName, null, listener, StatusKind.DataAvailableStatus | StatusKind.InconsistentTopicStatus);
            Assert.IsNull(topic3);

            // Test with null type name
            Topic topic4 = _participant.CreateTopic(nameof(TestCreateTopic) + "3", null, null, listener, StatusKind.DataAvailableStatus | StatusKind.InconsistentTopicStatus);
            Assert.IsNull(topic4);

            // Test with wrong configuration
            Topic topic5 = _participant.CreateTopic(nameof(TestCreateTopic), "OtherName", null, listener, StatusKind.DataAvailableStatus | StatusKind.InconsistentTopicStatus);
            Assert.IsNull(topic5);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.FindTopic(string, Duration)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestFindTopic()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestFindTopic), typeName);
            Assert.IsNotNull(topic);

            Topic foundTopic = _participant.FindTopic(nameof(TestFindTopic), new Duration
            {
                Seconds = 60,
                NanoSeconds = 0,
            });

            Assert.IsNotNull(foundTopic);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.LookupTopicDescription(string)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupTopicDescription()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestLookupTopicDescription), typeName);
            Assert.IsNotNull(topic);

            ITopicDescription foundTopic = _participant.LookupTopicDescription(nameof(TestLookupTopicDescription));
            Assert.IsNotNull(foundTopic);

            ITopicDescription notFoundTopic = _participant.LookupTopicDescription("NoFoundTopic");
            Assert.IsNull(notFoundTopic);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.DeleteTopic(Topic)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteTopic()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteTopic), typeName);
            Assert.IsNotNull(topic);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(topic);
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
            PublisherQos qos = new PublisherQos();
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
            PublisherQos qos = TestHelper.CreateNonDefaultPublisherQos();

            ReturnCode result = _participant.GetDefaultPublisherQos(qos);
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

        /// <summary>
        /// Test the <see cref="DomainParticipant.CreatePublisher(PublisherQos, PublisherListener, StatusMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreatePublisher()
        {
            PublisherQos qos = TestHelper.CreateNonDefaultPublisherQos();

            MyPublisherListener listener = new MyPublisherListener();
            Publisher publisher = _participant.CreatePublisher(qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(publisher);
            Assert.IsNotNull(publisher.Listener);

            PublisherQos getPublisherQos = new PublisherQos();
            ReturnCode result = publisher.GetQos(getPublisherQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultPublisherQos(getPublisherQos);

            // Test with listener and status mask
            Publisher publisher1 = _participant.CreatePublisher(null, listener, StatusMask.DefaultStatusMask);
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
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeletePublisher), typeName);
            Assert.IsNotNull(topic);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);

            // Cannot be deleted if have active datawriters
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
            SubscriberQos qos = new SubscriberQos();
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
            SubscriberQos qos = TestHelper.CreateNonDefaultSubscriberQos();

            ReturnCode result = _participant.GetDefaultSubscriberQos(qos);
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

        /// <summary>
        /// Test the <see cref="DomainParticipant.CreateSubscriber(SubscriberQos, SubscriberListener, StatusMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateSubscriber()
        {
            SubscriberQos qos = TestHelper.CreateNonDefaultSubscriberQos();

            MySubscriberListener listener = new MySubscriberListener();
            Subscriber subscriber = _participant.CreateSubscriber(qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(subscriber);
            Assert.IsNotNull(subscriber.Listener);

            SubscriberQos getSubscriberQos = new SubscriberQos();
            ReturnCode result = subscriber.GetQos(getSubscriberQos);
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
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteSubscriber), typeName);
            Assert.IsNotNull(topic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(topic);
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
            Subscriber subscriber = _participant.GetBuiltinSubscriber();
            Assert.IsNotNull(subscriber);

            Subscriber other = _participant.GetBuiltinSubscriber();
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
            Subscriber builtin = _participant.GetBuiltinSubscriber();
            Assert.IsNotNull(builtin);

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestContainsEntity), typeName);
            Assert.IsNotNull(topic);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataWriter dataWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);

            DataReader dataReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(dataReader);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_OTHER_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();
            Assert.AreNotSame(otherParticipant, _participant);
            Assert.AreNotSame(otherParticipant.InstanceHandle, _participant.InstanceHandle);
            Assert.AreNotEqual(otherParticipant.InstanceHandle, _participant.InstanceHandle);

            result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic otherTopic = otherParticipant.CreateTopic("Other" + nameof(TestContainsEntity), typeName);
            Assert.IsNotNull(otherTopic);

            Subscriber otherBuiltin = otherParticipant.GetBuiltinSubscriber();
            Assert.IsNotNull(otherBuiltin);

            Subscriber otherSubscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(otherSubscriber);

            Publisher otherPublisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(otherPublisher);

            DataReader otherDataReader = otherSubscriber.CreateDataReader(otherTopic);
            Assert.IsNotNull(otherDataReader);

            DataWriter otherDataWriter = otherPublisher.CreateDataWriter(otherTopic);
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
            DomainParticipant other = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(other);
            other.BindRtpsUdpTransportConfig();

            ReturnCode result = _participant.IgnoreParticipant(other.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);

            // TODO: Test that actually is ignored

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
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestIgnoreTopic), support.GetTypeName());
            Assert.IsNotNull(topic);

            result = _participant.IgnoreTopic(topic.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);

            // TODO: Test that actually is ignored
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.IgnoreTopic(InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestIgnorePublication()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestIgnorePublication), support.GetTypeName());
            Assert.IsNotNull(topic);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriter dataWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);

            result = _participant.IgnorePublication(dataWriter.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);

            // TODO: Test that actually is ignored
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.IgnoreSubscription(InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestIgnoreSubscription()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestIgnoreSubscription), support.GetTypeName());
            Assert.IsNotNull(topic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader dataReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(dataReader);

            result = _participant.IgnoreSubscription(dataReader.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);

            // TODO: Test that actually is ignored
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.AssertLiveliness" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestAssertLiveliness()
        {
            ReturnCode result = _participant.AssertLiveliness();
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipant.GetCurrentTimestamp(ref Timestamp)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetCurrentTimestamp()
        {
            Timestamp ts = new Timestamp()
            {
                Seconds = 0,
                NanoSeconds = 0,
            };

            DateTime now = DateTime.Now;
            ReturnCode result = _participant.GetCurrentTimestamp(ref ts);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsTrue(ts.Seconds > 0);

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(ts.Seconds);
            dtDateTime.AddMilliseconds(ts.NanoSeconds / 100000d);

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
            List<InstanceHandle> handles = new List<InstanceHandle>();
            ReturnCode result = _participant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, handles.Count);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
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
            DomainParticipantQos qos = new DomainParticipantQos();
            qos.UserData.Value = new List<byte> { 0x42 };
            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN, qos);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            Assert.IsTrue(_participant.WaitForParticipants(1, 5_000));
            Assert.IsTrue(otherParticipant.WaitForParticipants(1, 5_000));

            List<InstanceHandle> handles = new List<InstanceHandle>();
            ReturnCode result = _participant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, handles.Count);

            ParticipantBuiltinTopicData data = default;
            result = _participant.GetDiscoveredParticipantData(ref data, handles.First());
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, data.UserData.Value.Count);
            Assert.AreEqual(0x42, data.UserData.Value.First());
            Assert.IsNotNull(data.Key);
            Assert.IsNotNull(data.Key.Value);

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
            DomainParticipant participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(participant);
            participant.BindTcpTransportConfig();

            List<InstanceHandle> handles = new List<InstanceHandle>();
            ReturnCode result = participant.GetDiscoveredTopics(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, handles.Count);

            // Create a new topic and check that is discovered
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            result = support.RegisterType(participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = participant.CreateTopic(nameof(TestGetDiscoveredTopics), typeName);
            Assert.IsNotNull(topic);
            InstanceHandle handle = topic.InstanceHandle;

            result = participant.GetDiscoveredTopics(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, handles.Count);
            Assert.AreEqual(handle, handles.First());

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
            DomainParticipant participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(participant);
            participant.BindTcpTransportConfig();

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindTcpTransportConfig();

            List<InstanceHandle> handles = new List<InstanceHandle>();
            ReturnCode result = participant.GetDiscoveredTopics(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, handles.Count);

            // Create a new topic and check that is discovered
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            result = support.RegisterType(participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            TopicQos qos = new TopicQos();
            qos.TopicData.Value = new byte[] { 0x42 };
            Topic topic = participant.CreateTopic(nameof(TestGetDiscoveredTopicData), typeName, qos);
            Assert.IsNotNull(topic);

            Topic otherTopic = otherParticipant.CreateTopic(nameof(TestGetDiscoveredTopicData), typeName, qos);
            Assert.IsNotNull(otherTopic);

            Publisher publisher = participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);

            Subscriber subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(otherTopic);
            Assert.IsNotNull(reader);

            Assert.IsTrue(reader.WaitForPublications(1, 50_000));
            Assert.IsTrue(writer.WaitForSubscriptions(1, 50_000));

            InstanceHandle handle = topic.InstanceHandle;
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = participant.GetDiscoveredTopics(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsTrue(handles.Count > 0);
            Assert.IsTrue(handles.Contains(handle));

            // OpenDDS ISSUE: Need to wait for the topic data if not it returns bad parameter
            TopicBuiltinTopicData data = default;
            var count = 200;
            result = ReturnCode.NoData;
            while (result != ReturnCode.Ok && count > 0)
            {
                Thread.Sleep(500);
                result = participant.GetDiscoveredTopicData(ref data, handle);
                count--;
            }

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(nameof(TestGetDiscoveredTopicData), data.Name);
            Assert.AreEqual(typeName, data.TypeName);
            Assert.IsNotNull(data.Key);
            Assert.IsNotNull(data.Key);
            Assert.IsNotNull(data.TopicData);
            Assert.IsNotNull(data.TopicData.Value);
            Assert.AreEqual(1, data.TopicData.Value.Count());
            Assert.AreEqual(0x42, data.TopicData.Value.First());

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
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestCreateContentFilteredTopic), typeName);
            Assert.IsNotNull(topic);

            int totalInstances = 10;
            int filterCount = 5;
            ContentFilteredTopic filteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", topic, "(Id <= %0)", filterCount.ToString());
            Assert.IsNotNull(filteredTopic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);

            DataReader filteredReader = subscriber.CreateDataReader(filteredTopic, drQos);
            Assert.IsNotNull(filteredReader);

            TestStructDataReader dataReader = new TestStructDataReader(reader);
            TestStructDataReader filteredDataReader = new TestStructDataReader(filteredReader);

            WaitSet waitSet = new WaitSet();
            GuardCondition cancelConditionReader = new GuardCondition();
            StatusCondition statusConditionReader = reader.StatusCondition;
            waitSet.AttachCondition(cancelConditionReader);
            waitSet.AttachCondition(statusConditionReader);
            statusConditionReader.EnabledStatuses = StatusKind.DataAvailableStatus;

            int countReader = 0;
            Thread threadReader = new Thread(() =>
            {
                while (true)
                {
                    ICollection<Condition> conditions = new List<Condition>();
                    Duration duration = new Duration
                    {
                        Seconds = 60,
                    };
                    waitSet.Wait(conditions, duration);

                    foreach (Condition cond in conditions)
                    {
                        if (cond == statusConditionReader && cond.TriggerValue)
                        {
                            StatusCondition sCond = (StatusCondition)cond;
                            StatusMask mask = sCond.EnabledStatuses;
                            if ((mask & StatusKind.DataAvailableStatus) != 0)
                            {
                                List<TestStruct> receivedData = new List<TestStruct>();
                                List<SampleInfo> sampleInfos = new List<SampleInfo>();
                                dataReader.Take(receivedData, sampleInfos);

                                foreach (var sampleInfo in sampleInfos)
                                {
                                    if (sampleInfo.ValidData && sampleInfo.InstanceState == InstanceStateKind.AliveInstanceState)
                                    {
                                        countReader++;
                                    }
                                }
                            }
                        }

                        if (cond == cancelConditionReader && cond.TriggerValue)
                        {
                            // We reset the cancellation condition because it is a good practice, but in this implementation it probably doesn't change anything.
                            cancelConditionReader.TriggerValue = false;

                            // The thread activity has been canceled.
                            return;
                        }
                    }
                }
            });

            WaitSet waitSetFiltered = new WaitSet();
            GuardCondition cancelConditionFilteredReader = new GuardCondition();
            StatusCondition statusConditionFilteredReader = filteredReader.StatusCondition;
            waitSetFiltered.AttachCondition(cancelConditionFilteredReader);
            waitSetFiltered.AttachCondition(statusConditionFilteredReader);
            statusConditionFilteredReader.EnabledStatuses = StatusKind.DataAvailableStatus;

            int countFilteredReader = 0;
            Thread threadFilteredReader = new Thread(() =>
            {
                while (true)
                {
                    ICollection<Condition> conditions = new List<Condition>();
                    Duration duration = new Duration
                    {
                        Seconds = 60,
                    };
                    waitSetFiltered.Wait(conditions, duration);

                    foreach (Condition cond in conditions)
                    {
                        if (cond == statusConditionFilteredReader && cond.TriggerValue)
                        {
                            StatusCondition sCond = (StatusCondition)cond;
                            StatusMask mask = sCond.EnabledStatuses;
                            if ((mask & StatusKind.DataAvailableStatus) != 0)
                            {
                                List<TestStruct> receivedData = new List<TestStruct>();
                                List<SampleInfo> sampleInfos = new List<SampleInfo>();
                                filteredDataReader.Take(receivedData, sampleInfos);

                                foreach (var sampleInfo in sampleInfos)
                                {
                                    if (sampleInfo.ValidData && sampleInfo.InstanceState == InstanceStateKind.AliveInstanceState)
                                    {
                                        countFilteredReader++;
                                    }
                                }
                            }
                        }

                        if (cond == cancelConditionFilteredReader && cond.TriggerValue)
                        {
                            // We reset the cancellation condition because it is a good practice, but in this implementation it probably doesn't change anything.
                            cancelConditionFilteredReader.TriggerValue = false;

                            // The thread activity has been canceled.
                            return;
                        }
                    }
                }
            });

            threadReader.IsBackground = true;
            threadReader.Priority = ThreadPriority.Highest;
            threadReader.Start();

            threadFilteredReader.IsBackground = true;
            threadFilteredReader.Priority = ThreadPriority.Highest;
            threadFilteredReader.Start();

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(writer);

            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for subscriptions
            PublicationMatchedStatus status = default;
            do
            {
                dataWriter.GetPublicationMatchedStatus(ref status);
                Thread.Sleep(100);
            }
            while (status.CurrentCount < 2);

            for (int i = 1; i <= totalInstances; i++)
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
            ContentFilteredTopic nullFilteredTopic = _participant.CreateContentFilteredTopic(null, topic, "(Id <= %0)", filterCount.ToString());
            Assert.IsNull(nullFilteredTopic);

            // Test with null related topic
            nullFilteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", null, "(Id <= %0)", filterCount.ToString());
            Assert.IsNull(nullFilteredTopic);

            // Test with null expression
            nullFilteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", topic, null, filterCount.ToString());
            Assert.IsNull(nullFilteredTopic);

            // Test wrong topic creation (same name than other topic is not allowed)
            nullFilteredTopic = _participant.CreateContentFilteredTopic(nameof(TestCreateContentFilteredTopic), topic, "(Id <= %0)", filterCount.ToString());
            Assert.IsNull(nullFilteredTopic);

            // Test without expression parameters
            ContentFilteredTopic filteredTopic1 = _participant.CreateContentFilteredTopic("FilteredTopic1", topic, "(Id <= 1)");
            Assert.IsNotNull(filteredTopic1);

            // Test with null expression parameters
            ContentFilteredTopic filteredTopic2 = _participant.CreateContentFilteredTopic("FilteredTopic2", topic, "(Id <= 1)", null);
            Assert.IsNotNull(filteredTopic2);

            ContentFilteredTopic filteredTopic3 = _participant.CreateContentFilteredTopic("FilteredTopic", null, "(Id <= %1 AND Id <= %2)", filterCount.ToString(), "2");
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
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteContentFilteredTopic), support.GetTypeName());
            Assert.IsNotNull(topic);

            int filterCount = 5;
            ContentFilteredTopic filteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", topic, "(Id < %0)", filterCount.ToString());
            Assert.IsNotNull(filteredTopic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(filteredTopic);
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
        }
        #endregion
    }
}
