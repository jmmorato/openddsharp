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
using System.Linq;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="Subscriber"/> unit test class.
    /// </summary>
    [TestClass]
    public class SubscriberTest
    {
        #region Constants
        private const string TEST_CATEGORY = "Subscriber";
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
        /// Test the <see cref="Subscriber.Participant"/> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetParticipant()
        {
            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.AreEqual(_participant, subscriber.Participant);
        }

        /// <summary>
        /// Test the <see cref="SubscriberQos"/> default constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNewSubscriberQos()
        {
            SubscriberQos qos = new SubscriberQos();
            TestHelper.TestDefaultSubscriberQos(qos);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.GetQos(SubscriberQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a subscriber with it.
            SubscriberQos qos = TestHelper.CreateNonDefaultSubscriberQos();

            Subscriber subscriber = _participant.CreateSubscriber(qos);
            Assert.IsNotNull(subscriber);

            // Call GetQos and check the values received.
            qos = new SubscriberQos();
            ReturnCode result = subscriber.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultSubscriberQos(qos);

            // Test GetQos with null parameter.
            result = subscriber.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.SetQos(SubscriberQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Create a new Subscriber using the default QoS.
            Subscriber subscriber = _participant.CreateSubscriber();

            // Get the qos to ensure that is using the default properties.
            SubscriberQos qos = new SubscriberQos();
            ReturnCode result = subscriber.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultSubscriberQos(qos);

            // Try to change an immutable property.
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            result = subscriber.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them.
            qos = new SubscriberQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            result = subscriber.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new SubscriberQos();
            result = subscriber.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count);
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count);
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);

            // Try to set immutable QoS properties before enable the publisher
            DomainParticipantQos pQos = new DomainParticipantQos();
            pQos.EntityFactory.AutoenableCreatedEntities = false;
            result = _participant.SetQos(pQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            Subscriber otherSubscriber = _participant.CreateSubscriber();
            qos = new SubscriberQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            result = otherSubscriber.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new SubscriberQos();
            result = otherSubscriber.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count);
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count);
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);

            // Test SetQos with null parameter
            result = subscriber.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.GetListener"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetListener()
        {
            // Create a new Subscriber with a listener
            MySubscriberListener listener = new MySubscriberListener();
            Subscriber subscriber = _participant.CreateSubscriber(null, listener);
            Assert.IsNotNull(subscriber);

            // Call to GetListener and check the listener received
#pragma warning disable CS0618 // Type or member is obsolete
            MySubscriberListener received = (MySubscriberListener)subscriber.GetListener();
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.SetListener(SubscriberListener, StatusMask)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetListener()
        {
            // Create a new Subscriber without listener
            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            MySubscriberListener listener = (MySubscriberListener)subscriber.Listener;
            Assert.IsNull(listener);

            // Create a listener, set it and check that is correctly setted
            listener = new MySubscriberListener();
            ReturnCode result = subscriber.SetListener(listener, StatusMask.AllStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            MySubscriberListener received = (MySubscriberListener)subscriber.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            // Remove the listener calling SetListener with null and check it
            result = subscriber.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            received = (MySubscriberListener)subscriber.Listener;
            Assert.IsNull(received);
        }

        /// <summary>
        /// Test the <see cref="DataReaderQos"/> default constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNewDataReaderQos()
        {
            DataReaderQos qos = new DataReaderQos();
            TestHelper.TestDefaultDataReaderQos(qos);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.GetDefaultDataReaderQos(DataReaderQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDefaultDataReaderQos()
        {
            // Initialize entities.
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestGetDefaultDataReaderQos), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestGetDefaultDataReaderQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            // Create a non-default DataReader Qos, call GetDefaultDataReaderQos and check the default values.
            DataReaderQos qos = TestHelper.CreateNonDefaultDataReaderQos();
            result = subscriber.GetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataReaderQos(qos);

            // Test GetDefaultDataReaderQos with null parameter.
            result = subscriber.GetDefaultDataReaderQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.SetDefaultDataReaderQos(DataReaderQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultDataReaderQos()
        {
            // Initialize entities.
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestSetDefaultDataReaderQos), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestSetDefaultDataReaderQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            // Creates a non-default QoS, set it an check it.
            DataReaderQos qos = TestHelper.CreateNonDefaultDataReaderQos();
            result = subscriber.SetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataReaderQos();
            result = subscriber.GetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataReaderQos(qos);

            DataReader reader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(reader);

            qos = new DataReaderQos();
            result = reader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataReaderQos(qos);

            // Put back the default QoS and check it.
            qos = new DataReaderQos();
            result = subscriber.SetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = TestHelper.CreateNonDefaultDataReaderQos();
            result = subscriber.GetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataReaderQos(qos);

            DataReader otherReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(otherReader);

            qos = TestHelper.CreateNonDefaultDataReaderQos();
            result = otherReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataReaderQos(qos);

            // Create an inconsistent QoS and try to set it.
            qos = TestHelper.CreateNonDefaultDataReaderQos();
            qos.TimeBasedFilter.MinimumSeparation = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            result = subscriber.SetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.InconsistentPolicy, result);

            // Test SetDefaultDataReaderQos with null parameter.
            result = subscriber.SetDefaultDataReaderQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.LookupDataReader(string)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupDataReader()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestLookupDataReader), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestLookupDataReader), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            Subscriber otherSubscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(otherSubscriber);

            // Create a DataReader and lookup in the subscribers
            DataReader datareader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(datareader);
            Assert.AreEqual(subscriber, datareader.Subscriber);
            Assert.IsNull(datareader.Listener);

            DataReader received = subscriber.LookupDataReader(nameof(TestLookupDataReader));
            Assert.IsNotNull(received);
            Assert.AreEqual(datareader, received);

            received = otherSubscriber.LookupDataReader(nameof(TestLookupDataReader));
            Assert.IsNull(received);

            // Create other DataReader in the same topic and lookup again
            DataReader otherDatareader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(otherDatareader);
            Assert.AreEqual(subscriber, otherDatareader.Subscriber);
            Assert.IsNull(otherDatareader.Listener);

            received = subscriber.LookupDataReader(nameof(TestLookupDataReader));
            Assert.IsNotNull(received);
            Assert.IsTrue(datareader == received || otherDatareader == received);

            received = otherSubscriber.LookupDataReader(nameof(TestLookupDataReader));
            Assert.IsNull(received);

            // Lookup Built-in DataReader
            var bis = _participant.GetBuiltinSubscriber();
            var participantReader = bis.LookupDataReader("DCPSParticipant");
            Assert.IsNotNull(participantReader);

            result = participantReader.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.CreateDataReader(ITopicDescription, DataReaderQos, DataReaderListener, StatusMask)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateDataReader()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestCreateDataReader), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestCreateDataReader), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            // Test simplest overload
            DataReader datareader1 = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(datareader1);
            Assert.AreEqual(subscriber, datareader1.Subscriber);
            Assert.IsNull(datareader1.Listener);

            DataReaderQos qos = new DataReaderQos();
            result = datareader1.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataReaderQos(qos);

            // Test overload with QoS parameter
            qos = TestHelper.CreateNonDefaultDataReaderQos();
            DataReader datareader2 = subscriber.CreateDataReader(topic, qos);
            Assert.IsNotNull(datareader2);
            Assert.AreEqual(subscriber, datareader2.Subscriber);
            Assert.IsNull(datareader2.Listener);

            qos = new DataReaderQos();
            result = datareader2.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataReaderQos(qos);

            // Test overload with listener parameter
            MyDataReaderListener listener = new MyDataReaderListener();
            DataReader datareader3 = subscriber.CreateDataReader(topic, null, listener);
            Assert.IsNotNull(datareader3);
            Assert.AreEqual(subscriber, datareader3.Subscriber);
            MyDataReaderListener received = (MyDataReaderListener)datareader3.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataReaderQos();
            result = datareader3.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataReaderQos(qos);

            // Test overload with listener and StatusMask parameters
            listener = new MyDataReaderListener();
            DataReader datareader4 = subscriber.CreateDataReader(topic, null, listener, StatusMask.AllStatusMask);
            Assert.IsNotNull(datareader4);
            Assert.AreEqual(subscriber, datareader4.Subscriber);
            received = (MyDataReaderListener)datareader4.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataReaderQos();
            result = datareader4.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataReaderQos(qos);

            // Test overload with QoS and listener parameters
            qos = TestHelper.CreateNonDefaultDataReaderQos();
            listener = new MyDataReaderListener();
            DataReader datareader5 = subscriber.CreateDataReader(topic, qos, listener);
            Assert.IsNotNull(datareader5);
            Assert.AreEqual(subscriber, datareader5.Subscriber);
            received = (MyDataReaderListener)datareader5.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataReaderQos();
            result = datareader5.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataReaderQos(qos);

            // Test full call overload
            qos = TestHelper.CreateNonDefaultDataReaderQos();
            listener = new MyDataReaderListener();
            DataReader datareader6 = subscriber.CreateDataReader(topic, qos, listener, StatusMask.AllStatusMask);
            Assert.IsNotNull(datareader6);
            Assert.AreEqual(subscriber, datareader6.Subscriber);
            received = (MyDataReaderListener)datareader6.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataReaderQos();
            result = datareader6.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataReaderQos(qos);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.DeleteDataReader(DataReader)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteDataReader()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteDataReader), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestDeleteDataReader), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            Subscriber otherSubscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(otherSubscriber);

            // Create a DataReader and try to delete it with another subscriber
            DataReader datareader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(datareader);
            Assert.AreEqual(subscriber, datareader.Subscriber);
            Assert.IsNull(datareader.Listener);

            DataReaderQos qos = new DataReaderQos();
            result = datareader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataReaderQos(qos);

            result = otherSubscriber.DeleteDataReader(datareader);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Delete the datareader with the correct subscriber
            result = subscriber.DeleteDataReader(datareader);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Try to remove it again
            result = subscriber.DeleteDataReader(datareader);
            Assert.AreEqual(ReturnCode.Error, result);

            // Test with null parameter
            result = subscriber.DeleteDataReader(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.DeleteContainedEntities"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteContainedEntities()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteContainedEntities), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestDeleteContainedEntities), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            // Call DeleteContainedEntities in an empty subscriber
            result = subscriber.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a DataReader in the subscriber
            DataReader dataReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(dataReader);

            // Try to delete the publisher without delete the datareader
            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Call DeleteContainedEntities and remove the subscriber again
            result = subscriber.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a DataReader with null parameter
            Assert.ThrowsException<System.ArgumentNullException>(() => subscriber.CreateDataReader(null));

            // Create DataReader with incorrect qos
            DataReaderQos drQos = new DataReaderQos();
            drQos.ResourceLimits.MaxSamples = 1;
            drQos.ResourceLimits.MaxSamplesPerInstance = 2;
            var nullDataReader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNull(nullDataReader);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.NotifyDataReaders"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNotifyDataReaders()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestNotifyDataReaders), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestNotifyDataReaders), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            int subscriberReceived = 0;
            int readerReceived = 0;

            // Create the Subscriber and the DataReader with the corresponding listeners
            MySubscriberListener subListener = new MySubscriberListener();
            subListener.DataOnReaders += (sub) =>
            {
                subscriberReceived++;
                if (subscriberReceived % 2 == 0)
                {
                    sub.NotifyDataReaders();
                }
            };
            Subscriber subscriber = _participant.CreateSubscriber(null, subListener);
            Assert.IsNotNull(subscriber);

            MyDataReaderListener readListener = new MyDataReaderListener();
            readListener.DataAvailable += (read) =>
            {
                readerReceived++;
            };
            DataReader reader = subscriber.CreateDataReader(topic, null, readListener);
            Assert.IsNotNull(reader);

            Assert.IsTrue(writer.WaitForSubscriptions(1, 5000));
            Assert.IsTrue(reader.WaitForPublications(1, 5000));

            // Publish instances
            for (int i = 0; i < 10; i++)
            {
                dataWriter.Write(new TestStruct
                {
                    Id = i,
                    ShortField = (short)i,
                });

                var ret = dataWriter.WaitForAcknowledgments(new Duration
                {
                    Seconds = 5,
                });
                Assert.AreEqual(ReturnCode.Ok, ret);
            }

            System.Threading.Thread.Sleep(1000);

            // Check the received instances
            Assert.AreEqual(10, subscriberReceived);
            Assert.AreEqual(5, readerReceived);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.GetDataReaders(IList{DataReader}, SampleStateMask, ViewStateMask, InstanceStateMask)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDataReaders()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestNotifyDataReaders), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestNotifyDataReaders), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Topic otherTopic = _participant.CreateTopic("Other" + nameof(TestNotifyDataReaders), typeName);
            Assert.IsNotNull(otherTopic);
            Assert.IsNull(otherTopic.Listener);
            Assert.AreEqual("Other" + nameof(TestNotifyDataReaders), otherTopic.Name);
            Assert.AreEqual(typeName, otherTopic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            DataWriter otherWriter = publisher.CreateDataWriter(otherTopic);
            Assert.IsNotNull(otherWriter);
            TestStructDataWriter otherDataWriter = new TestStructDataWriter(otherWriter);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            DataReader otherReader = subscriber.CreateDataReader(otherTopic);
            Assert.IsNotNull(otherReader);
            TestStructDataReader otherDataReader = new TestStructDataReader(otherReader);

            Assert.IsTrue(reader.WaitForPublications(1, 5_000));
            Assert.IsTrue(writer.WaitForSubscriptions(1, 5_000));
            Assert.IsTrue(otherReader.WaitForPublications(1, 5_000));
            Assert.IsTrue(otherWriter.WaitForSubscriptions(1, 5_000));

            // Check that the GetDataReaders without sending any sample
            List<DataReader> list = new List<DataReader>();
            result = subscriber.GetDataReaders(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Publish in the topic and check again
            result = dataWriter.Write(new TestStruct
            {
                Id = 1,
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);

            result = subscriber.GetDataReaders(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, list.Count);

            // Publish in the otherTopic and check again
            result = otherDataWriter.Write(new TestStruct
            {
                Id = 1,
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);

            result = subscriber.GetDataReaders(list, SampleStateKind.NotReadSampleState | SampleStateKind.ReadSampleState, ViewStateMask.AnyViewState, InstanceStateMask.AnyInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(2, list.Count);

            // Take from both DataReaders and check again
            List<TestStruct> received = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();

            result = dataReader.Take(received, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = otherDataReader.Take(received, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = subscriber.GetDataReaders(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Test GetDataReaders with null parameter
            result = subscriber.GetDataReaders(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Subscriber.GetDataReaders(IList{DataReader}, SampleStateMask, ViewStateMask, InstanceStateMask)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestBeginEndAccess()
        {
            // OpenDDS Issue: Coherent sets for PRESENTATION QoS not Currently implemented on RTPS.
            // Just prepare the unit test for the moment.

            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestNotifyDataReaders), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestNotifyDataReaders), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Topic otherTopic = _participant.CreateTopic("Other" + nameof(TestNotifyDataReaders), typeName);
            Assert.IsNotNull(otherTopic);
            Assert.IsNull(otherTopic.Listener);
            Assert.AreEqual("Other" + nameof(TestNotifyDataReaders), otherTopic.Name);
            Assert.AreEqual(typeName, otherTopic.TypeName);

            PublisherQos pubQos = new PublisherQos();
            pubQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            pubQos.Presentation.CoherentAccess = true;
            pubQos.Presentation.OrderedAccess = true;
            Publisher publisher = _participant.CreatePublisher(pubQos);
            Assert.IsNotNull(publisher);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            DataWriter otherWriter = publisher.CreateDataWriter(otherTopic);
            Assert.IsNotNull(otherWriter);
            TestStructDataWriter otherDataWriter = new TestStructDataWriter(otherWriter);

            SubscriberQos subQos = new SubscriberQos();
            subQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            subQos.Presentation.CoherentAccess = true;
            subQos.Presentation.OrderedAccess = true;
            MySubscriberListener listener = new MySubscriberListener();
            listener.DataOnReaders += (sub) =>
            {
                result = sub.BeginAccess();
                Assert.AreEqual(ReturnCode.Ok, result);

                List<DataReader> list = new List<DataReader>();
                result = sub.GetDataReaders(list);

                // Here we should check that we received two DataReader
                // read the data of each one and confirm tha the group coherent access
                // is working as expected.

                result = sub.EndAccess();
                Assert.AreEqual(ReturnCode.Ok, result);
            };

            Subscriber subscriber = _participant.CreateSubscriber(subQos, listener);
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);
            Assert.IsNotNull(dataReader);

            DataReader otherReader = subscriber.CreateDataReader(otherTopic);
            Assert.IsNotNull(otherReader);
            TestStructDataReader otherDataReader = new TestStructDataReader(otherReader);
            Assert.IsNotNull(otherDataReader);

            // Call EndAccess without calling first BeginAccess
            result = subscriber.EndAccess();
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Publish a samples in both topics
            result = publisher.BeginCoherentChanges();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.Write(new TestStruct
            {
                Id = 1,
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = otherDataWriter.Write(new TestStruct
            {
                Id = 1,
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = publisher.EndCoherentChanges();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Give some time to the subscriber to process the messages
            System.Threading.Thread.Sleep(500);
        }
        #endregion
    }
}
