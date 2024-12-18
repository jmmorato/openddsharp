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
using CdrWrapper;
using CdrWrapperInclude;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="DataReader"/> unit test class.
    /// </summary>
    [TestClass]
    public class DataReaderCDRTest
    {
        #region Constants
        private const string TEST_CATEGORY = "DataReader";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Publisher _publisher;
        private Subscriber _subscriber;
        private Topic _topic;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="TestContext"/> property.
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
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);

            _subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(_subscriber);

            var typeSupport = new TestIncludeTypeSupport();
            var typeName = typeSupport.GetTypeName();
            var ret = typeSupport.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, ret);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _publisher?.DeleteContainedEntities();
            _subscriber?.DeleteContainedEntities();
            _participant?.DeletePublisher(_publisher);
            _participant?.DeleteSubscriber(_subscriber);
            _participant?.DeleteTopic(_topic);
            _participant?.DeleteContainedEntities();
            AssemblyInitializer.Factory?.DeleteParticipant(_participant);

            _participant = null;
            _publisher = null;
            _subscriber = null;
            _topic = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="DataReader" /> properties.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestProperties()
        {
            // Create a DataReader and check the TopicDescription and Subscriber properties
            var reader = _subscriber.CreateDataReader(_topic);

            Assert.IsNotNull(reader);
            Assert.IsNotNull(reader.TopicDescription);
            Assert.AreSame(_topic, reader.TopicDescription);
            Assert.AreEqual(_topic.Name, reader.TopicDescription.Name);
            Assert.AreSame(_topic.Participant, reader.TopicDescription.Participant);
            Assert.AreEqual(_topic.TypeName, reader.TopicDescription.TypeName);
            Assert.AreSame(_subscriber, reader.Subscriber);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
        }

        /// <summary>
        /// Test the <see cref="DataReaderQos" /> constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNewDataReaderQos()
        {
            var qos = new DataReaderQos();
            TestHelper.TestDefaultDataReaderQos(qos);
        }

        /// <summary>
        /// Test the <see cref="DataReader.GetQos(DataReaderQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a datareader with it.
            var qos = TestHelper.CreateNonDefaultDataReaderQos();

            var dataReader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(dataReader);

            // Call GetQos and check the values received.
            qos = new DataReaderQos();
            var result = dataReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataReaderQos(qos);

            // Call GetQos with null parameter.
            result = dataReader.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            dataReader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(dataReader);
        }

        /// <summary>
        /// Test the <see cref="DataReader.SetQos(DataReaderQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Create a new DataReader using the default QoS.
            var dataReader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(dataReader);

            // Get the qos to ensure that is using the default properties
            var qos = new DataReaderQos();
            var result = dataReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataReaderQos(qos);

            // Try to change an immutable property
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            result = dataReader.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them.
            qos = new DataReaderQos
            {
                UserData =
                {
                    Value = new List<byte> { 0x42 },
                },
            };

            result = dataReader.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataReaderQos();
            result = dataReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.UserData);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(1, qos.UserData.Value.Count);
            Assert.AreEqual(0x42, qos.UserData.Value[0]);

            // Try to set immutable QoS properties before enable the datareader.
            var subQos = new SubscriberQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
            };
            result = _subscriber.SetQos(subQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            var otherDataReader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(otherDataReader);

            qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
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

            dataReader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(dataReader);
        }

        /// <summary>
        /// Test the <see cref="DataReader.GetListener" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetListener()
        {
            // Create a new DataReader with a listener
            var listener = new MyDataReaderListener();
            var dataReader = _subscriber.CreateDataReader(_topic, null, listener);
            Assert.IsNotNull(dataReader);

            // Call to GetListener and check the listener received
#pragma warning disable CS0618 // Type or member is obsolete
            var received = (MyDataReaderListener)dataReader.GetListener();
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            Assert.AreEqual(ReturnCode.Ok, dataReader.SetListener(null, StatusMask.NoStatusMask));
            listener.Dispose();

            Assert.AreEqual(ReturnCode.Ok, dataReader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _subscriber.DeleteDataReader(dataReader));
        }

        /// <summary>
        /// Test the <see cref="DataReader.SetListener(DataReaderListener, StatusMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetListener()
        {
            // Create a new DataReader without listener
            var dataReader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(dataReader);

            Assert.IsNull((MyDataReaderListener)dataReader.Listener);

            // Create a listener, set it and check that is correctly set
            using (var listener = new MyDataReaderListener())
            {
                var result = dataReader.SetListener(listener, StatusMask.AllStatusMask);
                Assert.AreEqual(ReturnCode.Ok, result);

                var received = (MyDataReaderListener)dataReader.Listener;
                Assert.IsNotNull(received);
                Assert.AreEqual(listener, received);

                // Remove the listener calling SetListener with null and check it
                result = dataReader.SetListener(null, StatusMask.NoStatusMask);
                Assert.AreEqual(ReturnCode.Ok, result);

                received = (MyDataReaderListener)dataReader.Listener;
                Assert.IsNull(received);
            }

            dataReader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(dataReader);
        }

        /// <summary>
        /// Test the <see cref="DataReader.CreateReadCondition(SampleStateMask, ViewStateMask, InstanceStateMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateReadCondition()
        {
            // Initialize entities
            var reader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            // Create a read condition with the simplest overload
            var condition = reader.CreateReadCondition();
            Assert.IsNotNull(condition);
            Assert.AreSame(reader, condition.DataReader);
            Assert.AreEqual(InstanceStateMask.AnyInstanceState, condition.InstanceStateMask);
            Assert.AreEqual(SampleStateMask.AnySampleState, condition.SampleStateMask);
            Assert.AreEqual(ViewStateMask.AnyViewState, condition.ViewStateMask);
            Assert.IsFalse(condition.TriggerValue);

            // Create a read condition with the full parameters overload
            condition = reader.CreateReadCondition(
                SampleStateKind.ReadSampleState,
                ViewStateKind.NotNewViewState,
                InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState);

            Assert.IsNotNull(condition);
            Assert.AreSame(reader, condition.DataReader);
            Assert.AreEqual(InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState, condition.InstanceStateMask);
            Assert.AreEqual(SampleStateKind.ReadSampleState, condition.SampleStateMask);
            Assert.AreEqual(ViewStateKind.NotNewViewState, condition.ViewStateMask);
            Assert.IsFalse(condition.TriggerValue);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
        }

        /// <summary>
        /// Test the <see cref="DataReader.CreateQueryCondition(SampleStateMask, ViewStateMask, InstanceStateMask, string, string[])" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateQueryCondition()
        {
            // Initialize entities
            var reader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            const string expression = "Id > %0 AND Id < %1";
            const string parameter1 = "1";
            const string parameter2 = "5";

            // Create a QueryCondition with the simplest overload
            var condition = reader.CreateQueryCondition(expression, parameter1, parameter2);
            Assert.IsNotNull(condition);
            Assert.AreEqual(InstanceStateMask.AnyInstanceState, condition.InstanceStateMask);
            Assert.AreEqual(SampleStateMask.AnySampleState, condition.SampleStateMask);
            Assert.AreEqual(ViewStateMask.AnyViewState, condition.ViewStateMask);
            Assert.IsFalse(condition.TriggerValue);
            Assert.AreEqual(expression, condition.QueryExpression);

            var parameters = new List<string>();
            var result = condition.GetQueryParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);

            // Create a QueryCondition with the full parameters overload
            condition = reader.CreateQueryCondition(
                SampleStateKind.ReadSampleState,
                ViewStateKind.NotNewViewState,
                InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState,
                expression, parameter1, parameter2);

            Assert.IsNotNull(condition);
            Assert.AreEqual(InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState, condition.InstanceStateMask);
            Assert.AreEqual(SampleStateKind.ReadSampleState, condition.SampleStateMask);
            Assert.AreEqual(ViewStateKind.NotNewViewState, condition.ViewStateMask);
            Assert.IsFalse(condition.TriggerValue);
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

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
        }

        /// <summary>
        /// Test the <see cref="DataReader.DeleteReadCondition(ReadCondition)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteReadCondition()
        {
            // Initialize entities
            var reader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            const string expression = "Id > %0 AND Id < %1";
            const string parameter1 = "1";
            const string parameter2 = "5";

            // Create a read condition with the simplest overload
            var readCondition = reader.CreateReadCondition();
            Assert.IsNotNull(readCondition);
            Assert.AreEqual(reader, readCondition.DataReader);
            Assert.AreEqual(InstanceStateMask.AnyInstanceState, readCondition.InstanceStateMask);
            Assert.AreEqual(SampleStateMask.AnySampleState, readCondition.SampleStateMask);
            Assert.AreEqual(ViewStateMask.AnyViewState, readCondition.ViewStateMask);
            Assert.IsFalse(readCondition.TriggerValue);

            // Create a QueryCondition with the simplest overload
            var queryCondition = reader.CreateQueryCondition(expression, parameter1, parameter2);
            Assert.IsNotNull(queryCondition);
            Assert.AreEqual(InstanceStateMask.AnyInstanceState, queryCondition.InstanceStateMask);
            Assert.AreEqual(SampleStateMask.AnySampleState, queryCondition.SampleStateMask);
            Assert.AreEqual(ViewStateMask.AnyViewState, queryCondition.ViewStateMask);
            Assert.IsFalse(queryCondition.TriggerValue);
            Assert.AreEqual(expression, queryCondition.QueryExpression);

            var parameters = new List<string>();
            var result = queryCondition.GetQueryParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);

            // Create other reader
            var otherReader = _subscriber.CreateDataReader(_topic);
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

            reader.DeleteContainedEntities();
            otherReader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _subscriber.DeleteDataReader(otherReader);
        }

        /// <summary>
        /// Test the <see cref="DataReader.DeleteContainedEntities" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteContainedEntities()
        {
            // Initialize entities
            var reader = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            // Call DeleteContainedEntities in an empty DataReader
            var result = reader.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a ReadCondition and a QueryCondition in the datareader
            var readCondition = reader.CreateReadCondition();
            Assert.IsNotNull(readCondition);

            var queryCondition = reader.CreateQueryCondition("Id > 1");
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

        /// <summary>
        /// Test the <see cref="DataReader.GetSampleRejectedStatus(ref SampleRejectedStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetSampleRejectedStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                ResourceLimits =
                {
                    MaxInstances = 1,
                    MaxSamples = 1,
                    MaxSamplesPerInstance = 1,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.SampleRejectedStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = new DataWriterQos();
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            Assert.IsTrue(reader.WaitForPublications(1, 5_000));
            Assert.IsTrue(writer.WaitForSubscriptions(1, 5_000));

            // Call sample rejected status
            SampleRejectedStatus status = default;
            var result = reader.GetSampleRejectedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(status);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(SampleRejectedStatusKind.NotRejected, status.LastReason);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);

            // Write two samples of the same instances
            for (var i = 1; i <= 2; i++)
            {
                result = dataWriter.Write(new TestInclude
                {
                    Id = "1",
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Assert.IsTrue(evt.Wait(5_000));

            // Call sample rejected status
            result = reader.GetSampleRejectedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(status);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(SampleRejectedStatusKind.RejectedBySamplesPerInstanceLimit, status.LastReason);
            Assert.AreNotEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="DataReader.GetLivelinessChangedStatus(ref LivelinessChangedStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetLivelinessChangedStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Check the status when no writers are matched
            var drQos = new DataReaderQos
            {
                Liveliness =
                {
                    LeaseDuration = new Duration { Seconds = 1 },
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.LivelinessChangedStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            LivelinessChangedStatus status = default;
            var result = reader.GetLivelinessChangedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.AliveCount);
            Assert.AreEqual(0, status.AliveCountChange);
            Assert.AreEqual(0, status.NotAliveCount);
            Assert.AreEqual(0, status.NotAliveCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastPublicationHandle);

            // Creates a datawriter
            var dwQos = new DataWriterQos
            {
                Liveliness =
                {
                    Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos,
                    LeaseDuration = new Duration { Seconds = 1 },
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);
            Assert.IsNotNull(dataWriter);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5_000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5_000);
            Assert.IsTrue(found);

            // Assert liveliness in the writer
            result = writer.AssertLiveliness();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Receive the first alive event
            Assert.IsTrue(evt.Wait(1_500));

            status = default;
            result = reader.GetLivelinessChangedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.AliveCount);
            Assert.AreEqual(1, status.AliveCountChange);
            Assert.AreEqual(0, status.NotAliveCount);
            Assert.AreEqual(0, status.NotAliveCountChange);
            Assert.AreEqual(writer.InstanceHandle, status.LastPublicationHandle);

            evt.Reset();
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // After one second and a half one liveliness should be lost
            Assert.IsTrue(evt.Wait(1_500));

            result = reader.GetLivelinessChangedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.AliveCount);
            Assert.AreEqual(-1, status.AliveCountChange);
            Assert.AreEqual(1, status.NotAliveCount);
            Assert.AreEqual(1, status.NotAliveCountChange);
            Assert.AreEqual(writer.InstanceHandle, status.LastPublicationHandle);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="DataReader.GetRequestedDeadlineMissedStatus(ref RequestedDeadlineMissedStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetRequestedDeadlineMissedStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataWriterQos
            {
                Deadline =
                {
                    Period = new Duration { Seconds = 1 },
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            var drQos = new DataReaderQos
            {
                Deadline =
                {
                    Period = new Duration { Seconds = 1 },
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.RequestedDeadlineMissedStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // Wait for discovery and write an instance
            var found = writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            dataWriter.Write(new TestInclude { Id = "1" });

            // After half second deadline should not be lost yet
            Assert.IsFalse(evt.Wait(500));

            RequestedDeadlineMissedStatus status = default;
            var result = reader.GetRequestedDeadlineMissedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);

            // After one second and a half one deadline should be lost
            Assert.IsTrue(evt.Wait(1_500));

            result = reader.GetRequestedDeadlineMissedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreNotEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="DataReader.GetRequestedIncompatibleQosStatus(ref RequestedIncompatibleQosStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetRequestedIncompatibleQosStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.RequestedIncompatibleQosStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // If not matched writers should return the default status
            RequestedIncompatibleQosStatus status = default;
            var result = reader.GetRequestedIncompatibleQosStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.IsNotNull(status.Policies);
            Assert.AreEqual(0, status.Policies.Count);
            Assert.AreEqual(0, status.LastPolicyId);

            // Create a not compatible writer
            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);

            Assert.IsTrue(evt.Wait(1_500));

            status = default;
            result = reader.GetRequestedIncompatibleQosStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(11, status.LastPolicyId);
            Assert.IsNotNull(status.Policies);
            Assert.AreEqual(1, status.Policies.Count);
            Assert.AreEqual(1, status.Policies.First().Count);
            Assert.AreEqual(11, status.Policies.First().PolicyId);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="DataReader.GetSubscriptionMatchedStatus(ref SubscriptionMatchedStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetSubscriptionMatchedStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.SubscriptionMatchedStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // If not DataWriters are created should return the default status
            SubscriptionMatchedStatus status = default;
            var result = reader.GetSubscriptionMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.CurrentCount);
            Assert.AreEqual(0, status.CurrentCountChange);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastPublicationHandle);

            // Create a not compatible writer
            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);

            // Wait for discovery and check the status
            Assert.IsFalse(evt.Wait(1_500));
            result = reader.GetSubscriptionMatchedStatus(ref status);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.CurrentCount);
            Assert.AreEqual(0, status.CurrentCountChange);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastPublicationHandle);

            // Create a compatible writer
            var otherWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(otherWriter);

            // Wait for discovery and check the status
            Assert.IsTrue(evt.Wait(1_500));
            result = reader.GetSubscriptionMatchedStatus(ref status);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.CurrentCount);
            Assert.AreEqual(1, status.CurrentCountChange);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(otherWriter.InstanceHandle, status.LastPublicationHandle);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
            _publisher.DeleteDataWriter(otherWriter);
        }

        /// <summary>
        /// Test the <see cref="DataReader.GetSampleLostStatus(ref SampleLostStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetSampleLostStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
                DestinationOrder =
                {
                    Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepLastHistoryQos,
                    Depth = 1,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.SampleLostStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = new DataWriterQos();
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            Assert.IsTrue(reader.WaitForPublications(1, 5_000));
            Assert.IsTrue(writer.WaitForSubscriptions(1, 5_000));

            // Call sample lost status
            SampleLostStatus status = default;
            var result = reader.GetSampleLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(status);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);

            // Write two samples of the same instances
            var handle = dataWriter.RegisterInstance(new TestInclude { Id = "1" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            var time = DateTime.Now.ToTimestamp();
            result = dataWriter.Write(new TestInclude { Id = "1" }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsFalse(evt.Wait(500));

            time = DateTime.Now.Subtract(TimeSpan.FromSeconds(10)).ToTimestamp();
            result = dataWriter.Write(new TestInclude { Id = "1" }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));

            // Call sample lost status
            result = reader.GetSampleLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(status);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="DataReader.WaitForHistoricalData(Duration)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestWaitForHistoricalData()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
                Durability =
                {
                    Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Writes an instance
            var result = dataWriter.Write(new TestInclude { Id = "1" });
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a DataReader
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
                Durability =
                {
                    Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5_000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5_000);
            Assert.IsTrue(found);

            // OpenDDS Issue: Wait for historical data is not actually implemented. It returns always ReturnCode.Ok
            result = dataReader.WaitForHistoricalData(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));

            // Read the previous published instance
            var data = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.IsTrue(sampleInfos[0].ValidData);
            Assert.AreEqual(0, sampleInfos[0].AbsoluteGenerationRank);
            Assert.AreEqual(0, sampleInfos[0].DisposedGenerationCount);
            Assert.AreEqual(0, sampleInfos[0].GenerationRank);
            Assert.AreNotEqual(InstanceHandle.HandleNil, sampleInfos[0].InstanceHandle);
            Assert.AreEqual(InstanceStateKind.AliveInstanceState, sampleInfos[0].InstanceState);
            Assert.AreEqual(0, sampleInfos[0].NoWritersGenerationCount);
            Assert.AreNotEqual(InstanceHandle.HandleNil, sampleInfos[0].PublicationHandle);
            Assert.AreEqual(0, sampleInfos[0].SampleRank);
            Assert.AreEqual(SampleStateKind.NotReadSampleState, sampleInfos[0].SampleState);
            Assert.AreEqual(ViewStateKind.NewViewState, sampleInfos[0].ViewState);
            Assert.IsNotNull(sampleInfos[0].SourceTimestamp);
            Assert.IsTrue(sampleInfos[0].SourceTimestamp.Seconds > 0);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="DataReader.GetMatchedPublications(ICollection{InstanceHandle})" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetMatchedPublications()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.SubscriptionMatchedStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // Test matched publications without any match
            var list = new List<InstanceHandle> { InstanceHandle.HandleNil };
            var result = reader.GetMatchedPublications(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Test matched publications with null
            result = reader.GetMatchedPublications(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Create a not compatible writer
            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);

            // Wait for discovery and check the matched subscriptions
            Assert.IsFalse(evt.Wait(1_500));

            result = reader.GetMatchedPublications(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Create a compatible writer
            var otherWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(otherWriter);

            // Wait for discovery and check the matched subscriptions
            Assert.IsTrue(evt.Wait(1_500));
            result = reader.GetMatchedPublications(list);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(otherWriter.InstanceHandle, list[0]);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
            _publisher.DeleteDataWriter(otherWriter);
        }

        /// <summary>
        /// Test the <see cref="DataReader.GetMatchedPublicationData(InstanceHandle, ref PublicationBuiltinTopicData)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetMatchedPublicationData()
        {
            // Initialize entities
            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;

            // OPENDDS ISSUE: Cannot use ExclusiveOwnership for the test because when calling delete_datareader
            // the BitPubListenerImpl::on_data_available take_next_sample method enter an infinite loop if we already called
            // the GetMatchedPublicationData. It tries to take a not_read_sample, but it doesn't exist because it is already marked
            // as read in the GetMatchedPublicationData call.
            drQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            // DCPSInfoRepo-based discovery generates Built-In Topic data once (inside the
            // info repo process) and therefore all known entities in the domain are
            // reflected in the Built-In Topics. RTPS discovery, on the other hand, follows
            // the DDS specification and omits "local" entities from the Built-In Topics.
            // The definition of "local" means those entities belonging to the same Domain
            // Participant as the given Built-In Topic Subscriber.
            // https://github.com/OpenDDS/OpenDDS/blob/master/docs/design/RTPS

            // OPENDDS ISSUE: GetMatchedSubscriptions returns local entities but GetMatchedSubscriptionData doesn't
            // because it is looking in the Built-in topic. If not found in the built-in, shouldn't try to look locally?
            // WORKAROUND: Create another participant for the DataReader.
            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestIncludeTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var otherTopic = otherParticipant.CreateTopic(nameof(TestGetMatchedPublicationData), typeName);
            Assert.IsNotNull(otherTopic);

            var publisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Ownership.Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
            var writer = publisher.CreateDataWriter(otherTopic, dwQos);
            Assert.IsNotNull(writer);

            // Wait for publications
            var found = reader.WaitForPublications(1, 5000);
            Assert.IsTrue(found);

            // Get the matched subscriptions
            var list = new List<InstanceHandle>();
            result = reader.GetMatchedPublications(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, list.Count);

            // Get the matched publication data
            PublicationBuiltinTopicData data = default;
            result = reader.GetMatchedPublicationData(list[0], ref data);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultPublicationData(data);

            // Destroy the other participant
            result = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            publisher.DeleteDataWriter(writer);
            publisher.DeleteContainedEntities();
            otherParticipant.DeletePublisher(publisher);
            otherParticipant.DeleteTopic(otherTopic);
            AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.Read(List{TestInclude}, List{SampleInfo}, int, SampleStateMask, ViewStateMask, InstanceStateMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRead()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var duration = new Duration { Seconds = 5 };
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5_000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5_000);
            Assert.IsTrue(found);

            // Write an instance a wait for acknowledgment
            var result = dataWriter.Write(new TestInclude { Id = "1" });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));

            // Read the data with the simplest overload
            var data = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);

            // Write another sample of the same instance
            evt.Reset();
            TestHelper.CreateWaitSetThread(evt, statusCondition);
            result = dataWriter.Write(new TestInclude { Id = "1", ShortField = 2, IncludeField = new IncludeStruct { Message = "Test"} });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));

            // Read the data limiting the max samples
            data = new List<TestInclude>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);

            data = new List<TestInclude>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos, 2);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);
            Assert.AreEqual("1", data[1].Id);
            Assert.AreEqual(2, data[1].ShortField);

            // Read the data with a QueryCondition
            var condition = reader.CreateQueryCondition("ShortField = 2");
            Assert.IsNotNull(condition);

            data = new List<TestInclude>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.AreEqual(2, data[0].ShortField);

            // Read the data with mask parameters
            result = dataReader.Read(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateMask.AnyInstanceState);
            Assert.AreEqual(ReturnCode.NoData, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, sampleInfos.Count);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.Take(List{TestInclude}, List{SampleInfo}, int, SampleStateMask, ViewStateMask, InstanceStateMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTake()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var duration = new Duration { Seconds = 5 };
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5_000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5_000);
            Assert.IsTrue(found);

            // Write an instance a wait for acknowledgment
            var result = dataWriter.Write(new TestInclude { Id = "1" });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsTrue(evt.Wait(1_500));

            // Take the data with the simplest overload
            var data = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.Take(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);

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
                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = "2", ShortField = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));
            }

            // Take the data limiting the max samples
            result = dataReader.Take(data, sampleInfos, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("2", data[0].Id);
            Assert.AreEqual(1, data[0].ShortField);

            // Take all the remaining samples
            result = dataReader.Take(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual("2", data[0].Id);
            Assert.AreEqual(2, data[0].ShortField);
            Assert.AreEqual("2", data[1].Id);
            Assert.AreEqual(3, data[1].ShortField);

            // Write three samples more
            for (short i = 1; i <= 3; i++)
            {
                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = "3", ShortField = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));
            }

            // Take the data with a QueryCondition
            var condition = reader.CreateQueryCondition("ShortField = 2");
            Assert.IsNotNull(condition);
            data = new List<TestInclude>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Take(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("3", data[0].Id);
            Assert.AreEqual(2, data[0].ShortField);

            // Take the data with mask parameters
            result = dataReader.Take(data, sampleInfos, ResourceLimitsQosPolicy.LengthUnlimited,
                SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateMask.AnyInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual("3", data[0].Id);
            Assert.AreEqual(1, data[0].ShortField);
            Assert.AreEqual("3", data[1].Id);
            Assert.AreEqual(3, data[1].ShortField);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.ReadInstance(List{TestInclude}, List{SampleInfo}, InstanceHandle, int, SampleStateMask, ViewStateMask, InstanceStateMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            ReturnCode result;
            var duration = new Duration { Seconds = 5 };
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5_000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5_000);
            Assert.IsTrue(found);

            // Write two samples of three different instances
            for (short i = 1; i <= 3; i++)
            {
                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString() });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));

                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString(), ShortField = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));
            }

            // Read instance with the simplest overload
            var handle = dataReader.LookupInstance(new TestInclude { Id = "1" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            var data = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.ReadInstance(data, sampleInfos, handle);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);
            Assert.AreEqual("1", data[1].Id);
            Assert.AreEqual(1, data[1].ShortField);

            // Read instance limiting the max samples
            result = dataReader.ReadInstance(data, sampleInfos, handle, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);

            // Read instance with a QueryCondition
            var condition = reader.CreateQueryCondition("ShortField = 1");
            Assert.IsNotNull(condition);
            data = new List<TestInclude>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.ReadInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.AreEqual(1, data[0].ShortField);

            // Read instance with mask parameters
            result = dataReader.ReadInstance(data, sampleInfos, handle,
                ResourceLimitsQosPolicy.LengthUnlimited,
                SampleStateKind.NotReadSampleState,
                ViewStateKind.NewViewState | ViewStateKind.NotNewViewState,
                InstanceStateKind.AliveInstanceState | InstanceStateKind.NotAliveDisposedInstanceState | InstanceStateKind.NotAliveNoWritersInstanceState);
            Assert.AreEqual(ReturnCode.NoData, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(0, data.Count);
            Assert.AreEqual(0, sampleInfos.Count);

            handle = dataReader.LookupInstance(new TestInclude { Id = "2" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataReader.ReadInstance(data, sampleInfos, handle,
                ResourceLimitsQosPolicy.LengthUnlimited,
                SampleStateKind.NotReadSampleState,
                ViewStateMask.AnyViewState,
                InstanceStateMask.AnyInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual("2", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);
            Assert.AreEqual("2", data[1].Id);
            Assert.AreEqual(2, data[1].ShortField);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.TakeInstance(List{TestInclude}, List{SampleInfo}, InstanceHandle, int, SampleStateMask, ViewStateMask, InstanceStateMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            ReturnCode result;
            var duration = new Duration { Seconds = 5 };
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5_000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5_000);
            Assert.IsTrue(found);

            // Write two samples of three different instances
            for (short i = 1; i <= 3; i++)
            {
                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString() });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));

                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString(), ShortField = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));
            }

            // Take instance with the simplest overload
            var handle = dataReader.LookupInstance(new TestInclude { Id = "1" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            var data = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.TakeInstance(data, sampleInfos, handle);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);
            Assert.AreEqual("1", data[1].Id);
            Assert.AreEqual(1, data[1].ShortField);

            // Take again to ensure NoData is received
            result = dataReader.TakeInstance(data, sampleInfos, handle);
            Assert.AreEqual(ReturnCode.NoData, result);

            // Take instance limiting the max samples
            handle = dataReader.LookupInstance(new TestInclude { Id = "2" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataReader.TakeInstance(data, sampleInfos, handle, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("2", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);

            result = dataReader.TakeInstance(data, sampleInfos, handle,
                ResourceLimitsQosPolicy.LengthUnlimited);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("2", data[0].Id);
            Assert.AreEqual(2, data[0].ShortField);

            // Take instance with a QueryCondition
            handle = dataReader.LookupInstance(new TestInclude { Id = "3" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            var condition = reader.CreateQueryCondition("ShortField = 3");
            Assert.IsNotNull(condition);

            data = new List<TestInclude>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.ReadInstance(data, sampleInfos, handle,
                ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("3", data[0].Id);
            Assert.AreEqual(3, data[0].ShortField);

            // Take instance with mask parameters
            result = dataReader.ReadInstance(data, sampleInfos, handle,
                ResourceLimitsQosPolicy.LengthUnlimited,
                SampleStateKind.NotReadSampleState,
                ViewStateMask.AnyViewState,
                InstanceStateMask.AnyInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("3", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.ReadNextInstance(List{TestInclude}, List{SampleInfo}, InstanceHandle, int, SampleStateMask, ViewStateMask, InstanceStateMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            ReturnCode result;
            var duration = new Duration { Seconds = 5 };
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            var dataReader = new TestIncludeDataReader(reader);
            Assert.IsNotNull(dataReader);

            var statusCondition = dataReader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);

            var dataWriter = new TestIncludeDataWriter(writer);
            Assert.IsNotNull(dataWriter);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5_000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5_000);
            Assert.IsTrue(found);

            // // Write two samples of three different instances
            // for (short i = 1; i <= 3; i++)
            // {
            //     evt.Reset();
            //     TestHelper.CreateWaitSetThread(evt, statusCondition);
            //
            //     result = dataWriter.Write(new TestInclude { Id = i.ToString() });
            //     Assert.AreEqual(ReturnCode.Ok, result);
            //
            //     result = dataWriter.WaitForAcknowledgments(duration);
            //     Assert.AreEqual(ReturnCode.Ok, result);
            //
            //     Assert.IsTrue(evt.Wait(1_500));
            //
            //     evt.Reset();
            //     TestHelper.CreateWaitSetThread(evt, statusCondition);
            //
            //     result = dataWriter.Write(new TestInclude { Id = i.ToString(), ShortField = i });
            //     Assert.AreEqual(ReturnCode.Ok, result);
            //
            //     result = dataWriter.WaitForAcknowledgments(duration);
            //     Assert.AreEqual(ReturnCode.Ok, result);
            //
            //     Assert.IsTrue(evt.Wait(1_500));
            // }

            // Read next instance with the simplest overload
            var data = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.ReadNextInstance(data, sampleInfos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);
            Assert.AreEqual("1", data[1].Id);
            Assert.AreEqual(1, data[1].ShortField);

            // Read next instance limiting the max samples
            result = dataReader.ReadNextInstance(data, sampleInfos, InstanceHandle.HandleNil, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);

            // Read next instance with QueryCondition
            var condition = reader.CreateQueryCondition("ShortField = 3");
            Assert.IsNotNull(condition);

            result = dataReader.ReadNextInstance(data, sampleInfos, InstanceHandle.HandleNil, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("3", data[0].Id);
            Assert.AreEqual(3, data[0].ShortField);

            // Read next instance with mask parameters
            var handle = dataReader.LookupInstance(new TestInclude { Id = "2" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataReader.ReadNextInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateKind.AliveInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("3", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.TakeNextInstance(List{TestInclude}, List{SampleInfo}, InstanceHandle, int, SampleStateMask, ViewStateMask, InstanceStateMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            ReturnCode result;
            var duration = new Duration { Seconds = 5 };
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5000);
            Assert.IsTrue(found);

            // Write two samples of three different instances
            for (short i = 1; i <= 3; i++)
            {
                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString() });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));

                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString(), ShortField = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));
            }

            // Take next instance with the simplest overload
            var data = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.TakeNextInstance(data, sampleInfos, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2, sampleInfos.Count);
            Assert.AreEqual("1", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);
            Assert.AreEqual("1", data[1].Id);
            Assert.AreEqual(1, data[1].ShortField);

            // Read next instance limiting the max samples
            result = dataReader.TakeNextInstance(data, sampleInfos, InstanceHandle.HandleNil, 1);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("2", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);

            // Read next instance with QueryCondition
            var condition = reader.CreateQueryCondition("ShortField = 3");
            Assert.IsNotNull(condition);

            result = dataReader.TakeNextInstance(data, sampleInfos, InstanceHandle.HandleNil, ResourceLimitsQosPolicy.LengthUnlimited, condition);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("3", data[0].Id);
            Assert.AreEqual(3, data[0].ShortField);

            // Read next instance with mask parameters
            var handle = dataReader.LookupInstance(new TestInclude { Id = "2" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataReader.TakeNextInstance(data, sampleInfos, handle, ResourceLimitsQosPolicy.LengthUnlimited, SampleStateKind.NotReadSampleState, ViewStateMask.AnyViewState, InstanceStateKind.AliveInstanceState);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(data);
            Assert.IsNotNull(sampleInfos);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreEqual("3", data[0].Id);
            Assert.AreEqual(0, data[0].ShortField);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.ReadNextSample(TestInclude, SampleInfo)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestReadNextSample()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            ReturnCode result;
            var duration = new Duration { Seconds = 5 };
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5000);
            Assert.IsTrue(found);

            // Write two samples of two different instances
            for (short i = 1; i <= 2; i++)
            {
                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString() });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));

                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString(), ShortField = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));
            }

            // Read next samples
            var data = new TestInclude();
            var info = new SampleInfo();

            for (short i = 1; i <= 4; i++)
            {
                result = dataReader.ReadNextSample(data, info);
                Assert.AreEqual(ReturnCode.Ok, result);
                Assert.IsTrue(info.ValidData);
                Assert.IsNotNull(data);
                Assert.IsNotNull(info);
                if (i < 3)
                {
                    Assert.AreEqual("1", data.Id);
                    Assert.AreEqual(i % 2 == 0 ? 1 : 0, data.ShortField);
                }
                else
                {
                    Assert.AreEqual("2", data.Id);
                    Assert.AreEqual(i % 2 == 0 ? 2 : 0, data.ShortField);
                }
            }

            // Read next sample to check NoData
            result = dataReader.ReadNextSample(data, info);
            Assert.AreEqual(ReturnCode.NoData, result);

            // Read samples
            var samples = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(samples, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(4, samples.Count);
            Assert.AreEqual(4, sampleInfos.Count);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.TakeNextSample(TestInclude, SampleInfo)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestTakeNextSample()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            ReturnCode result;
            var duration = new Duration { Seconds = 5 };
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5000);
            Assert.IsTrue(found);

            // Write two samples of two different instances
            for (short i = 1; i <= 2; i++)
            {
                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString() });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));

                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                result = dataWriter.Write(new TestInclude { Id = i.ToString(), ShortField = i });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(duration);
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(1_500));
            }

            // Take next samples
            var data = new TestInclude();
            var info = new SampleInfo();

            for (short i = 1; i <= 4; i++)
            {
                result = dataReader.TakeNextSample(data, info);
                Assert.AreEqual(ReturnCode.Ok, result);
                Assert.IsTrue(info.ValidData);
                Assert.IsNotNull(data);
                Assert.IsNotNull(info);
                if (i < 3)
                {
                    Assert.AreEqual("1", data.Id);
                    Assert.AreEqual(i % 2 == 0 ? 1 : 0, data.ShortField);
                }
                else
                {
                    Assert.AreEqual("2", data.Id);
                    Assert.AreEqual(i % 2 == 0 ? 2 : 0, data.ShortField);
                }
            }

            // Take next sample to check NoData
            result = dataReader.TakeNextSample(data, info);
            Assert.AreEqual(ReturnCode.NoData, result);

            // Take samples
            var samples = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.Take(samples, sampleInfos);
            Assert.AreEqual(ReturnCode.NoData, result);
            Assert.AreEqual(0, samples.Count);
            Assert.AreEqual(0, sampleInfos.Count);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.GetKeyValue(TestInclude, InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetKeyValue()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Wait for discovery
            Assert.IsTrue(writer.WaitForSubscriptions(1, 5_000));
            Assert.IsTrue(reader.WaitForPublications(1, 5_000));

            // Register an instance and write it
            var handle1 = dataWriter.RegisterInstance(new TestInclude { Id = "1" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            var result = dataWriter.Write(new TestInclude { Id = "1" }, handle1);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));

            // Get the for an existing instance
            var structs = new List<TestInclude>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(structs, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, sampleInfos.Count);
            Assert.AreNotEqual(InstanceHandle.HandleNil, sampleInfos[0].InstanceHandle);

            var data = new TestInclude();
            result = dataReader.GetKeyValue(data, sampleInfos[0].InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual("1", data.Id);

            // Call GetKeyValue with HandleNil
            data = new TestInclude();
            result = dataReader.GetKeyValue(data, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestIncludeDataReader.LookupInstance(TestInclude)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupInstance()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(reader);
            var dataReader = new TestIncludeDataReader(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            var dataWriter = new TestIncludeDataWriter(writer);

            // Wait for discovery
            var found = reader.WaitForPublications(1, 5_000);
            Assert.IsTrue(found);
            found = writer.WaitForSubscriptions(1, 5_000);
            Assert.IsTrue(found);

            // Lookup for a non-existing instance
            var handle = dataReader.LookupInstance(new TestInclude { Id = "1" });
            Assert.AreEqual(InstanceHandle.HandleNil, handle);

            // Register an instance and write it
            var handle1 = dataWriter.RegisterInstance(new TestInclude { Id = "1" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            var result = dataWriter.Write(new TestInclude { Id = "1" }, handle1);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));

            // Lookup for an existing instance
            handle = dataReader.LookupInstance(new TestInclude { Id = "1" });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            reader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(reader);
            _publisher.DeleteDataWriter(writer);
        }
        #endregion
    }
}
