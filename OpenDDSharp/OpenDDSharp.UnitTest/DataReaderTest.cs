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
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class DataReaderTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
        private DomainParticipant _participant;
        private Topic _topic;
        private Subscriber _subscriber;
        #endregion

        #region Properties
        public TestContext TestContext { get; set; }
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

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);
            Assert.IsNull(_topic.GetListener());
            Assert.AreEqual(TestContext.TestName, _topic.Name);
            Assert.AreEqual(typeName, _topic.TypeName);

            _subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(_subscriber);
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
        [TestCategory("DataReader")]
        public void TestProperties()
        {
            // Create a DataReader and check the TopicDescription and Subscriber properties
            DataReader reader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);
            Assert.AreEqual(_topic.Name, reader.TopicDescription.Name);
            Assert.AreEqual(_topic.Participant, reader.TopicDescription.Participant);
            Assert.AreEqual(_topic.TypeName, reader.TopicDescription.TypeName);
            Assert.AreEqual(_subscriber, reader.Subscriber);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a datareader with it
            DataReaderQos qos = TestHelper.CreateNonDefaultDataReaderQos();

            DataReader dataReader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(dataReader);

            // Call GetQos and check the values received
            qos = new DataReaderQos();
            ReturnCode result = dataReader.GetQos(qos);
            TestHelper.TestNonDefaultDataReaderQos(qos);

            // Call GetQos with null parameter
            result = dataReader.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestSetQos()
        {
            // Create a new DataReader using the default QoS
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

            // Change some mutable properties and check them
            qos = new DataReaderQos();
            qos.UserData.Value = new List<byte> { 0x42 };

            result = dataReader.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataReaderQos();
            result = dataReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.UserData);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(1, qos.UserData.Value.Count());
            Assert.AreEqual(0x42, qos.UserData.Value.First());

            // Try to set immutable QoS properties before enable the datareader
            SubscriberQos pubQos = new SubscriberQos();
            pubQos.EntityFactory.AutoenableCreatedEntities = false;
            result = _subscriber.SetQos(pubQos);
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
            pubQos = new SubscriberQos();
            _subscriber.SetQos(pubQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Call SetQos with null parameter
            result = dataReader.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetListener()
        {
            // Create a new DataReader with a listener
            MyDataReaderListener listener = new MyDataReaderListener();
            DataReader dataReader = _subscriber.CreateDataReader(_topic, listener);
            Assert.IsNotNull(dataReader);

            // Call to GetListener and check the listener received
            MyDataReaderListener received = (MyDataReaderListener)dataReader.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestSetListener()
        {
            // Create a new DataReader without listener
            DataReader dataReader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(dataReader);

            MyDataReaderListener listener = (MyDataReaderListener)dataReader.GetListener();
            Assert.IsNull(listener);

            // Create a listener, set it and check that is correctly setted
            listener = new MyDataReaderListener();
            ReturnCode result = dataReader.SetListener(listener, StatusMask.AllStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            MyDataReaderListener received = (MyDataReaderListener)dataReader.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            // Remove the listener calling SetListener with null and check it
            result = dataReader.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            received = (MyDataReaderListener)dataReader.GetListener();
            Assert.IsNull(received);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestCreateReadCondition()
        {
            // Initialize entities
            DataReader reader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            // Create a read condition with the simplest overload
            ReadCondition condition = reader.CreateReadCondition();
            Assert.IsNotNull(condition);
            Assert.AreEqual(reader, condition.DataReader);
            Assert.AreEqual(InstanceStateMask.AnyInstanceState, condition.InstanceStateMask);
            Assert.AreEqual(SampleStateMask.AnySampleState, condition.SampleStateMask);
            Assert.AreEqual(ViewStateMask.AnyViewState, condition.ViewStateMask);
            Assert.AreEqual(false, condition.TriggerValue);

            // Create a read condition with the full parameters overload
            condition = reader.CreateReadCondition(SampleStateKind.ReadSampleState, ViewStateKind.NotNewViewState, 
                                                   InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState);
            Assert.IsNotNull(condition);
            Assert.AreEqual(reader, condition.DataReader);
            Assert.AreEqual(InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState, condition.InstanceStateMask);
            Assert.AreEqual(SampleStateKind.ReadSampleState, condition.SampleStateMask);
            Assert.AreEqual(ViewStateKind.NotNewViewState, condition.ViewStateMask);
            Assert.AreEqual(false, condition.TriggerValue);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestCreateQueryCondition()
        {
            // Initialize entities
            DataReader reader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            string expression = "Id > %0 AND Id < %1";
            string parameter1 = "1";
            string parameter2 = "5";

            // Create a QueryCondition with the simplest overload
            QueryCondition condition = reader.CreateQueryCondition(expression, parameter1, parameter2);
            Assert.IsNotNull(condition);
            Assert.AreEqual(InstanceStateMask.AnyInstanceState, condition.InstanceStateMask);
            Assert.AreEqual(SampleStateMask.AnySampleState, condition.SampleStateMask);
            Assert.AreEqual(ViewStateMask.AnyViewState, condition.ViewStateMask);
            Assert.AreEqual(false, condition.TriggerValue);
            Assert.AreEqual(expression, condition.QueryExpression);

            List<string> parameters = new List<string>();
            ReturnCode result = condition.GetQueryParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);

            // Create a QueryCondition with the full parameters overload
            condition = reader.CreateQueryCondition(SampleStateKind.ReadSampleState, ViewStateKind.NotNewViewState,
                                                    InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState,
                                                    expression, parameter1, parameter2);
            Assert.IsNotNull(condition);
            Assert.AreEqual(InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState, condition.InstanceStateMask);
            Assert.AreEqual(SampleStateKind.ReadSampleState, condition.SampleStateMask);
            Assert.AreEqual(ViewStateKind.NotNewViewState, condition.ViewStateMask);
            Assert.AreEqual(false, condition.TriggerValue);
            Assert.AreEqual(expression, condition.QueryExpression);

            parameters = new List<string>();
            result = condition.GetQueryParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);

            // Create a QueryCondition with an invalid number of parameters
            condition = reader.CreateQueryCondition(expression, parameter1);
            Assert.IsNull(condition);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestDeleteReadCondition()
        {
            // Initialize entities
            DataReader reader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            string expression = "Id > %0 AND Id < %1";
            string parameter1 = "1";
            string parameter2 = "5";

            // Create a read condition with the simplest overload
            ReadCondition readCondition = reader.CreateReadCondition();
            Assert.IsNotNull(readCondition);
            Assert.AreEqual(reader, readCondition.DataReader);
            Assert.AreEqual(InstanceStateMask.AnyInstanceState, readCondition.InstanceStateMask);
            Assert.AreEqual(SampleStateMask.AnySampleState, readCondition.SampleStateMask);
            Assert.AreEqual(ViewStateMask.AnyViewState, readCondition.ViewStateMask);
            Assert.AreEqual(false, readCondition.TriggerValue);
            
            // Create a QueryCondition with the simplest overload
            QueryCondition queryCondition = reader.CreateQueryCondition(expression, parameter1, parameter2);
            Assert.IsNotNull(queryCondition);
            Assert.AreEqual(InstanceStateMask.AnyInstanceState, queryCondition.InstanceStateMask);
            Assert.AreEqual(SampleStateMask.AnySampleState, queryCondition.SampleStateMask);
            Assert.AreEqual(ViewStateMask.AnyViewState, queryCondition.ViewStateMask);
            Assert.AreEqual(false, queryCondition.TriggerValue);
            Assert.AreEqual(expression, queryCondition.QueryExpression);

            List<string> parameters = new List<string>();
            ReturnCode result = queryCondition.GetQueryParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);

            // Create other reader
            DataReader otherReader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(otherReader);

            // Delete read condition with null
            result = otherReader.DeleteReadCondition(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Delete the previous conditions with the other reader
            result = otherReader.DeleteReadCondition(queryCondition);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            result = otherReader.DeleteReadCondition(readCondition);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Delete the query condition with the correct reader
            result = reader.DeleteReadCondition(queryCondition);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Delete the read condition with the correct reader
            result = reader.DeleteReadCondition(readCondition);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestDeleteContainedEntities()
        {
            // Initialize entities
            DataReader reader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            // Call DeleteContainedEntities in an empty DataReader
            ReturnCode result = reader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a ReadCondition and a QueryCondition in the datareader
            ReadCondition readCondition = reader.CreateReadCondition();
            Assert.IsNotNull(readCondition);

            QueryCondition queryCondition = reader.CreateQueryCondition("Id > 1");
            Assert.IsNotNull(queryCondition);

            // Try to delete the DataReader without delete the ReadCondition
            result = _subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Call DeleteContainedEntities and remove the DataReader again
            result = reader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetSampleRejectedStatus()
        {
            // Initialize entities
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.ResourceLimits.MaxInstances = 1;
            drQos.ResourceLimits.MaxSamples = 1;
            drQos.ResourceLimits.MaxSamplesPerInstance = 1;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            Publisher publisher = _participant.CreatePublisher();
            DataWriterQos dwQos = new DataWriterQos();
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);
        
            // Call sample rejected status
            SampleRejectedStatus status = new SampleRejectedStatus();
            ReturnCode result = reader.GetSampleRejectedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(status);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(SampleRejectedStatusKind.NotRejected, status.LastReason);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);

            // Write two samples of the same instances
            for (int i = 1; i <= 2; i++)
            {
                result = dataWriter.Write(new TestStruct
                {
                    Id = 1
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(100);

            // Call sample rejected status
            result = reader.GetSampleRejectedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(status);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(SampleRejectedStatusKind.RejectedBySamplesPerInstanceLimit, status.LastReason);
            Assert.AreNotEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetLivelinessChangedStatus()
        {
            // Check the status when no writers are matched
            DataReaderQos drQos = new DataReaderQos();
            drQos.Liveliness.LeaseDuration = new Duration { Seconds = 1 };
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            LivelinessChangedStatus status = new LivelinessChangedStatus();
            ReturnCode result = reader.GetLivelinessChangedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.AliveCount);
            Assert.AreEqual(0, status.AliveCountChange);
            Assert.AreEqual(0, status.NotAliveCount);
            Assert.AreEqual(0, status.NotAliveCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastPublicationHandle);

            // Creates a datawriter
            Publisher publisher = _participant.CreatePublisher();
            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Liveliness.Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos;
            dwQos.Liveliness.LeaseDuration = new Duration { Seconds = 1 };
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Assert liveliness in the writer
            result = writer.AssertLiveliness();
            Assert.AreEqual(ReturnCode.Ok, result);

            // After half second liveliness should not be lost yet
            System.Threading.Thread.Sleep(500);

            status = new LivelinessChangedStatus();
            result = reader.GetLivelinessChangedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.AliveCount);
            Assert.AreEqual(1, status.AliveCountChange);
            Assert.AreEqual(0, status.NotAliveCount);
            Assert.AreEqual(0, status.NotAliveCountChange);
            Assert.AreEqual(writer.InstanceHandle, status.LastPublicationHandle);

            // After one second and a half one liveliness should be lost
            System.Threading.Thread.Sleep(1000);

            result = reader.GetLivelinessChangedStatus(ref status);            
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.AliveCount);
            Assert.AreEqual(-1, status.AliveCountChange);
            Assert.AreEqual(1, status.NotAliveCount);
            Assert.AreEqual(1, status.NotAliveCountChange);
            Assert.AreEqual(writer.InstanceHandle, status.LastPublicationHandle);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetRequestedDeadlineMissedStatus()
        {
            // Initialize entities
            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos qos = new DataWriterQos();
            qos.Deadline.Period = new Duration { Seconds = 1 };
            DataWriter writer = publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Deadline.Period = new Duration { Seconds = 1 };
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            // Wait for discovery and write an instance
            bool found = writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            dataWriter.Write(new TestStruct { Id = 1 });

            // After half second deadline should not be lost yet
            System.Threading.Thread.Sleep(500);

            RequestedDeadlineMissedStatus status = new RequestedDeadlineMissedStatus();
            ReturnCode result = reader.GetRequestedDeadlineMissedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);

            // After one second and a half one deadline should be lost
            System.Threading.Thread.Sleep(1000);

            result = reader.GetRequestedDeadlineMissedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreNotEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetRequestedIncompatibleQosStatus()
        {
            // Initialize entities
            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);

            // If not matched writers should return the default status
            RequestedIncompatibleQosStatus status = new RequestedIncompatibleQosStatus();
            ReturnCode result = reader.GetRequestedIncompatibleQosStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.IsNotNull(status.Policies);
            Assert.AreEqual(0, status.Policies.Count());
            Assert.AreEqual(0, status.LastPolicyId);

            // Create a not compatible writer
            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);

            // Wait for discovery and check the status
            System.Threading.Thread.Sleep(100);

            status = new RequestedIncompatibleQosStatus();
            result = reader.GetRequestedIncompatibleQosStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(11, status.LastPolicyId);
            Assert.IsNotNull(status.Policies);
            Assert.AreEqual(1, status.Policies.Count());
            Assert.AreEqual(1, status.Policies.First().Count);
            Assert.AreEqual(11, status.Policies.First().PolicyId);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetSubscriptionMatchedStatus()
        {
            // Initialize entities
            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);

            // If not datawriters are created should return the default status
            SubscriptionMatchedStatus status = new SubscriptionMatchedStatus();
            ReturnCode result = reader.GetSubscriptionMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.CurrentCount);
            Assert.AreEqual(0, status.CurrentCountChange);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastPublicationHandle);

            // Create a not compatible writer
            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);

            // Wait for discovery and check the status
            System.Threading.Thread.Sleep(100);

            result = reader.GetSubscriptionMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.CurrentCount);
            Assert.AreEqual(0, status.CurrentCountChange);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastPublicationHandle);

            // Create a compatible writer
            DataWriter otherWriter = publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(otherWriter);

            // Wait for discovery and check the status
            System.Threading.Thread.Sleep(100);

            result = reader.GetSubscriptionMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.CurrentCount);
            Assert.AreEqual(1, status.CurrentCountChange);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(otherWriter.InstanceHandle, status.LastPublicationHandle);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetSampleLostStatus()
        {
            // Initialize entities
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            drQos.DestinationOrder.Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepLastHistoryQos;
            drQos.History.Depth = 1;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            Publisher publisher = _participant.CreatePublisher();
            DataWriterQos dwQos = new DataWriterQos();
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Call sample lost status
            SampleLostStatus status = new SampleLostStatus();
            ReturnCode result = reader.GetSampleLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(status);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);

            // Write two samples of the same instances
            InstanceHandle handle = dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            Timestamp time = DateTime.Now.ToTimestamp();
            result = dataWriter.Write(new TestStruct { Id = 1 }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);

            time = DateTime.Now.Subtract(TimeSpan.FromSeconds(10)).ToTimestamp();
            result = dataWriter.Write(new TestStruct { Id = 1 }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);

            // Call sample lost status
            result = reader.GetSampleLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(status);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestWaitForHistoricalData()
        {
            // Initialize entities
            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            dwQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            dwQos.Durability.Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos;            
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Writes an instance
            ReturnCode result = dataWriter.Write(new TestStruct { Id = 1 });
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a DataReader
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            drQos.Durability.Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos;            
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            // Wait for discovery
            bool found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            #region OpenDDS issue
            // Wait for historical data is not actually implemented.
            // It returns 
            result = dataReader.WaitForHistoricalData(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(2000);
            #endregion

            // Read the previous published instance
            List<TestStruct> data = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, data.First().Id);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.IsTrue(sampleInfos.First().ValidData);
            Assert.AreEqual(0, sampleInfos.First().AbsoluteGenerationRank);
            Assert.AreEqual(0, sampleInfos.First().DisposedGenerationCount);
            Assert.AreEqual(0, sampleInfos.First().GenerationRank);
            Assert.AreNotEqual(InstanceHandle.HandleNil, sampleInfos.First().InstanceHandle);
            Assert.AreEqual(InstanceStateKind.AliveInstanceState, sampleInfos.First().InstanceState);
            Assert.AreEqual(0, sampleInfos.First().NoWritersGenerationCount);
            Assert.AreNotEqual(InstanceHandle.HandleNil, sampleInfos.First().PublicationHandle);
            Assert.AreEqual(0, sampleInfos.First().SampleRank);
            Assert.AreEqual(SampleStateKind.NotReadSampleState, sampleInfos.First().SampleState);
            Assert.AreEqual(ViewStateKind.NewViewState, sampleInfos.First().ViewState);
            Assert.IsNotNull(sampleInfos.First().SourceTimestamp);
            Assert.IsTrue(sampleInfos.First().SourceTimestamp.Seconds > 0);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetMatchedPublications()
        {
            // Initialize entities  
            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);

            // Test matched publications without any match
            List<InstanceHandle> list = new List<InstanceHandle> { InstanceHandle.HandleNil };
            ReturnCode result = reader.GetMatchedPublications(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Test matched publications with null
            result = reader.GetMatchedPublications(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Create a not compatible writer
            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(reader);

            // Wait for discovery and check the matched subscriptions
            System.Threading.Thread.Sleep(100);

            result = reader.GetMatchedPublications(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Create a compatible writer
            DataWriter otherWriter = publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(otherWriter);

            // Wait for discovery and check the matched subscriptions
            System.Threading.Thread.Sleep(100);

            result = reader.GetMatchedPublications(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(otherWriter.InstanceHandle, list.First());
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetMatchedPublicationData()
        {
            // Initialize entities
            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            // DCPSInfoRepo-based discovery generates Built-In Topic data once (inside the
            // info repo process) and therefore all known entities in the domain are
            // reflected in the Built-In Topics.  RTPS discovery, on the other hand, follows
            // the DDS specification and omits "local" entities from the Built-In Topics.
            // The definition of "local" means those entities belonging to the same Domain
            // Participant as the given Built-In Topic Subscriber.
            // https://github.com/objectcomputing/OpenDDS/blob/master/docs/design/RTPS

            // OPENDDS ISSUE: GetMatchedSubscriptions returns local entities but GetMatchedSubscriptionData doesn't
            // because is looking in the Built-in topic. If not found in the built-in, shouldn't try to look locally?
            // WORKAROUND: Create another particpant for the DataReader.
            DomainParticipant otherParticipant = _dpf.CreateParticipant(DOMAIN_ID);
            Assert.IsNotNull(otherParticipant);

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic otherTopic = otherParticipant.CreateTopic(nameof(TestGetMatchedPublicationData), typeName);
            Assert.IsNotNull(otherTopic);

            Publisher publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = TestHelper.CreateNonDefaultDataWriterQos();            
            DataWriter writer = publisher.CreateDataWriter(otherTopic, dwQos);
            Assert.IsNotNull(writer);

            // Wait for publications
            bool found = reader.WaitForPublications(1, 5000);
            Assert.IsTrue(found);

            // Get the matched subscriptions
            List<InstanceHandle> list = new List<InstanceHandle>();
            result = reader.GetMatchedPublications(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, list.Count);

            // Get the matched publication data
            PublicationBuiltinTopicData data = new PublicationBuiltinTopicData();
            result = reader.GetMatchedPublicationData(list.First(), ref data);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultPublicationData(data);

            // Destroy the other participant
            result = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestRead()
        {
            // Initialize entities
            Duration duration = new Duration { Seconds = 5 };
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write an instance a wait for acknowledgment
            ReturnCode result = dataWriter.Write(new TestStruct { Id = 1 });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);
            System.Threading.Thread.Sleep(50);

            // Read the data with the simplest overload
            List<TestStruct> data = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(1, data.First().Id);

            // Write another sample of the same instance
            result = dataWriter.Write(new TestStruct { Id = 1, ShortType = 2 });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);
            System.Threading.Thread.Sleep(50);

            // Read the data limiting the max samples
            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(1, data.First().Id);

            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos, 2);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual(1, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);
            Assert.AreEqual(1, data[1].Id);
            Assert.AreEqual(2, data[1].ShortType);

            // Read the data with a QueryCondition
            QueryCondition condition = reader.CreateQueryCondition("ShortType = 2");
            Assert.IsNotNull(condition);

            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(1, data.First().Id);
            Assert.AreEqual(2, data.First().ShortType);

            // Read the data with mask parameters
            result = dataReader.Read(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateMask.AnyInstanceState);
            Assert.AreEqual(ReturnCode.NoData, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, sampleInfos.Count);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestTake()
        {
            // Initialize entities
            Duration duration = new Duration { Seconds = 5 };
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write an instance a wait for acknowledgment
            ReturnCode result = dataWriter.Write(new TestStruct { Id = 1 });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);
            System.Threading.Thread.Sleep(50);

            // Take the data with the simplest overload
            List<TestStruct> data = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.Take(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(1, data.First().Id);

            // Take again to test NoData
            result = dataReader.Take(data, sampleInfos);
            Assert.AreEqual(ReturnCode.NoData, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, sampleInfos.Count);

            // Write three samples
            for (short i = 1; i <= 3; i++)
            {
                result = dataWriter.Write(new TestStruct { Id = 2, ShortType = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);                
            }

            System.Threading.Thread.Sleep(50);

            // Take the data limiting the max samples
            result = dataReader.Take(data, sampleInfos, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(2, data.First().Id);
            Assert.AreEqual(1, data.First().ShortType);

            // Take all the remaining samples
            result = dataReader.Take(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual(2, data[0].Id);
            Assert.AreEqual(2, data[0].ShortType);
            Assert.AreEqual(2, data[1].Id);
            Assert.AreEqual(3, data[1].ShortType);

            // Write three samples more
            for (short i = 1; i <= 3; i++)
            {
                result = dataWriter.Write(new TestStruct { Id = 3, ShortType = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(50);

            // Take the data with a QueryCondition
            QueryCondition condition = reader.CreateQueryCondition("ShortType = 2");
            Assert.IsNotNull(condition);
            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Take(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(3, data.First().Id);
            Assert.AreEqual(2, data.First().ShortType);

            // Take the data with mask parameters
            result = dataReader.Take(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateMask.AnyInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual(3, data[0].Id);
            Assert.AreEqual(1, data[0].ShortType);
            Assert.AreEqual(3, data[1].Id);
            Assert.AreEqual(3, data[1].ShortType);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestReadInstance()
        {
            // Initialize entities
            ReturnCode result = ReturnCode.Error;
            Duration duration = new Duration { Seconds = 5 };
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write two samples of three different instances
            for (short i = 1; i <= 3; i++)
            {
                result = dataWriter.Write(new TestStruct { Id = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.Write(new TestStruct { Id = i, ShortType = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(50);

            // Read instance with the simplest overload
            InstanceHandle handle = dataReader.LookupInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            List<TestStruct> data = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.ReadInstance(data, sampleInfos, handle);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual(1, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);
            Assert.AreEqual(1, data[1].Id);
            Assert.AreEqual(1, data[1].ShortType);

            // Read instance limiting the max samples
            result = dataReader.ReadInstance(data, sampleInfos, handle, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(1, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);

            // Read instance with a QueryCondition
            QueryCondition condition = reader.CreateQueryCondition("ShortType = 1");
            Assert.IsNotNull(condition);
            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.ReadInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(1, data.First().Id);
            Assert.AreEqual(1, data.First().ShortType);

            // Read instance with mask parameters
            result = dataReader.ReadInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateKind.NewViewState | ViewStateKind.NotNewViewState, InstanceStateKind.AliveInstanceState | InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState);
            Assert.AreEqual(ReturnCode.NoData, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, sampleInfos.Count);

            handle = dataReader.LookupInstance(new TestStruct { Id = 2 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataReader.ReadInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateMask.AnyInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual(2, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);
            Assert.AreEqual(2, data[1].Id);
            Assert.AreEqual(2, data[1].ShortType);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestTakeInstance()
        {
            // Initialize entities
            ReturnCode result = ReturnCode.Error;
            Duration duration = new Duration { Seconds = 5 };
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write two samples of three different instances
            for (short i = 1; i <= 3; i++)
            {
                result = dataWriter.Write(new TestStruct { Id = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.Write(new TestStruct { Id = i, ShortType = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(50);

            // Take instance with the simplest overload
            InstanceHandle handle = dataReader.LookupInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            List<TestStruct> data = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.TakeInstance(data, sampleInfos, handle);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual(1, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);
            Assert.AreEqual(1, data[1].Id);
            Assert.AreEqual(1, data[1].ShortType);

            // Take again to ensure NoData is received
            result = dataReader.TakeInstance(data, sampleInfos, handle);
            Assert.AreEqual(ReturnCode.NoData, result);

            // Take instance limiting the max samples
            handle = dataReader.LookupInstance(new TestStruct { Id = 2 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataReader.TakeInstance(data, sampleInfos, handle, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(2, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);

            result = dataReader.TakeInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(2, data[0].Id);
            Assert.AreEqual(2, data[0].ShortType);

            // Take instance with a QueryCondition
            handle = dataReader.LookupInstance(new TestStruct { Id = 3 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            QueryCondition condition = reader.CreateQueryCondition("ShortType = 3");
            Assert.IsNotNull(condition);

            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.ReadInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(3, data.First().Id);
            Assert.AreEqual(3, data.First().ShortType);

            // Take instance with mask parameters
            result = dataReader.ReadInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateMask.AnyInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(3, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestReadNextInstance()
        {
            // Initialize entities
            ReturnCode result = ReturnCode.Error;
            Duration duration = new Duration { Seconds = 5 };
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write two samples of three different instances
            for (short i = 1; i <= 3; i++)
            {
                result = dataWriter.Write(new TestStruct { Id = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.Write(new TestStruct { Id = i, ShortType = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(50);

            // Read next instance with the simplest overload
            List<TestStruct> data = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.ReadNextInstance(data, sampleInfos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual(1, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);
            Assert.AreEqual(1, data[1].Id);
            Assert.AreEqual(1, data[1].ShortType);

            // Read next instance limiting the max samples
            result = dataReader.ReadNextInstance(data, sampleInfos, InstanceHandle.HandleNil, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(1, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);

            // Read next instance with QueryCondition
            QueryCondition condition = reader.CreateQueryCondition("ShortType = 3");
            Assert.IsNotNull(condition);

            result = dataReader.ReadNextInstance(data, sampleInfos, InstanceHandle.HandleNil, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(3, data[0].Id);
            Assert.AreEqual(3, data[0].ShortType);

            // Read next instance with mask parameters
            InstanceHandle handle = dataReader.LookupInstance(new TestStruct { Id = 2 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataReader.ReadNextInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateKind.AliveInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(3, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestTakeNextInstance()
        {
            // Initialize entities
            ReturnCode result = ReturnCode.Error;
            Duration duration = new Duration { Seconds = 5 };
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write two samples of three different instances
            for (short i = 1; i <= 3; i++)
            {
                result = dataWriter.Write(new TestStruct { Id = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.Write(new TestStruct { Id = i, ShortType = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(50);

            // Take next instance with the simplest overload
            List<TestStruct> data = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.TakeNextInstance(data, sampleInfos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual(1, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);
            Assert.AreEqual(1, data[1].Id);
            Assert.AreEqual(1, data[1].ShortType);

            // Read next instance limiting the max samples
            result = dataReader.TakeNextInstance(data, sampleInfos, InstanceHandle.HandleNil, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(2, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);

            // Read next instance with QueryCondition
            QueryCondition condition = reader.CreateQueryCondition("ShortType = 3");
            Assert.IsNotNull(condition);

            result = dataReader.TakeNextInstance(data, sampleInfos, InstanceHandle.HandleNil, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(3, data[0].Id);
            Assert.AreEqual(3, data[0].ShortType);

            // Read next instance with mask parameters
            InstanceHandle handle = dataReader.LookupInstance(new TestStruct { Id = 2 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataReader.TakeNextInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateKind.AliveInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual(3, data[0].Id);
            Assert.AreEqual(0, data[0].ShortType);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestReadNextSample()
        {
            // Initialize entities
            ReturnCode result = ReturnCode.Error;
            Duration duration = new Duration { Seconds = 5 };
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write two samples of two different instances
            for (short i = 1; i <= 2; i++)
            {
                result = dataWriter.Write(new TestStruct { Id = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.Write(new TestStruct { Id = i, ShortType = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(50);

            // Read next samples
            TestStruct data = new TestStruct();
            SampleInfo info = new SampleInfo();
            
            for (short i = 1; i <= 4; i++)
            {               
                result = dataReader.ReadNextSample(data, info);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsNotNull(data);
                Assert.IsNotNull(info);
                if (i < 3)
                {                    
                    Assert.AreEqual(1, data.Id);
                    Assert.AreEqual(i % 2 == 0 ? 1 : 0, data.ShortType);
                }
                else
                {
                    Assert.AreEqual(2, data.Id);
                    Assert.AreEqual(i % 2 == 0 ? 2 : 0, data.ShortType);
                }
            }

            // Read next sample to check NoData 
            result = dataReader.ReadNextSample(data, info);
            Assert.AreEqual(ReturnCode.NoData, result);

            // Read samples
            List<TestStruct> samples = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(samples, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(4, samples.Count);
            Assert.AreEqual(4, sampleInfos.Count);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestTakeNextSample()
        {
            // Initialize entities
            ReturnCode result = ReturnCode.Error;
            Duration duration = new Duration { Seconds = 5 };
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write two samples of two different instances
            for (short i = 1; i <= 2; i++)
            {
                result = dataWriter.Write(new TestStruct { Id = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.Write(new TestStruct { Id = i, ShortType = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(50);

            // Take next samples
            TestStruct data = new TestStruct();
            SampleInfo info = new SampleInfo();

            for (short i = 1; i <= 4; i++)
            {
                result = dataReader.TakeNextSample(data, info);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsNotNull(data);
                Assert.IsNotNull(info);
                if (i < 3)
                {
                    Assert.AreEqual(1, data.Id);
                    Assert.AreEqual(i % 2 == 0 ? 1 : 0, data.ShortType);
                }
                else
                {
                    Assert.AreEqual(2, data.Id);
                    Assert.AreEqual(i % 2 == 0 ? 2 : 0, data.ShortType);
                }
            }

            // Take next sample to check NoData 
            result = dataReader.TakeNextSample(data, info);
            Assert.AreEqual(ReturnCode.NoData, result);

            // Take samples
            List<TestStruct> samples = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.Take(samples, sampleInfos);
            Assert.AreEqual(ReturnCode.NoData, result);
            Assert.AreEqual(0, samples.Count);
            Assert.AreEqual(0, sampleInfos.Count);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetKeyValue()
        {
            // Initialize entities
            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            DataWriter writer = publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Call GetKeyValue with HandleNil
            TestStruct data = new TestStruct();
            ReturnCode result = dataReader.GetKeyValue(data, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Register an instance and write it
            InstanceHandle handle1 = dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            result = dataWriter.Write(new TestStruct { Id = 1 }, handle1);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);

            // Get the for an existing instance
            List<TestStruct> structs = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(structs, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreNotEqual(InstanceHandle.HandleNil, sampleInfos.First().InstanceHandle);

            result = dataReader.GetKeyValue(data, sampleInfos.First().InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, data.Id);
        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestLookupInstance()
        {
            // Initialize entities
            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);
            TestStructDataReader dataReader = new TestStructDataReader(reader);

            Publisher publisher = _participant.CreatePublisher();
            DataWriter writer = publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for discovery
            bool found = writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Lookup for a non-existing instance
            InstanceHandle handle = dataReader.LookupInstance(new TestStruct { Id = 1 });
            Assert.AreEqual(InstanceHandle.HandleNil, handle);

            // Register an instance and write it
            InstanceHandle handle1 = dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            ReturnCode result = dataWriter.Write(new TestStruct { Id = 1 }, handle1);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);

            // Lookup for an existing instance
            handle = dataReader.LookupInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);
        }
        #endregion
    }
}
