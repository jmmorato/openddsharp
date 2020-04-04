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
using System.Diagnostics;
using OpenDDSharp.DDS;
using Test;
using OpenDDSharp.Standard.UnitTest.Helpers;
using OpenDDSharp.Standard.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.Standard.UnitTest
{
    /// <summary>
    /// <see cref="DomainParticipantListener"/> unit test class.
    /// </summary>
    [TestClass]
    public class DomainParticipantListenerTest
    {
        #region Constants
        private const string TEST_CATEGORY = "DomainParticipantListener";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Topic _topic;
        private Subscriber _subscriber;
        private Publisher _publisher;
        private DataWriter _writer;
        private TestStructDataWriter _dataWriter;
        private MyParticipantListener _listener;
        private DataReader _reader;
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
            _listener = new MyParticipantListener();
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN, null, _listener);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);
            // TODO: Uncomment when implemented
            //Assert.IsNull(_topic.GetListener());
            //Assert.AreEqual(TestContext.TestName, _topic.Name);
            //Assert.AreEqual(typeName, _topic.TypeName);

            SubscriberQos sQos = new SubscriberQos();
            sQos.EntityFactory.AutoenableCreatedEntities = false;
            sQos.Presentation.OrderedAccess = true;
            sQos.Presentation.CoherentAccess = true;
            sQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos;
            _subscriber = _participant.CreateSubscriber(sQos);
            Assert.IsNotNull(_subscriber);

            PublisherQos pQos = new PublisherQos();
            pQos.EntityFactory.AutoenableCreatedEntities = false;
            pQos.Presentation.OrderedAccess = true;
            pQos.Presentation.CoherentAccess = true;
            pQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos;
            _publisher = _participant.CreatePublisher(pQos);
            Assert.IsNotNull(_publisher);

            _writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_writer);
            _dataWriter = new TestStructDataWriter(_writer);

            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            _reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(_reader);
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
            _writer = null;
            _dataWriter = null;
            _reader = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnDataOnReaders(Subscriber)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnDataOnReaders()
        {
            // Attach to the event
            int count = 0;
            Subscriber subscriber = null;
            _listener.DataOnReaders += (s) =>
            {
                subscriber = s;
                count++;
            };

            //// Enable entities
            ReturnCode result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Write some instances
            int total = 5;
            for (int i = 1; i <= total; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = i,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Thread.Sleep(100);

            Assert.AreEqual(total, count);
            Assert.IsNotNull(subscriber);
            Assert.AreEqual(_subscriber, subscriber);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnDataAvailable(DataReader)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnDataAvailable()
        {
            // Prepare status mask:
            // If a ParticipantListener has both on_data_on_readers() and on_data_available() callbacks enabled
            // (by turning on both status bits), only on_data_on_readers() is called.
            ReturnCode result = _participant.SetListener(_listener, StatusKind.DataAvailableStatus);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Attach to the event
            int count = 0;
            DataReader reader = null;
            _listener.DataAvailable += (r) =>
            {
                reader = r;
                count++;
            };

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Write some instances
            int total = 5;
            for (int i = 1; i <= total; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = i,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Thread.Sleep(100);

            Assert.AreEqual(total, count);
            Assert.IsNotNull(reader);
            Assert.AreEqual(_reader, reader);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnRequestedDeadlineMissed(DataReader, RequestedDeadlineMissedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnRequestedDeadlineMissed()
        {
            // Prepare qos for the test
            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Deadline.Period = new Duration { Seconds = 1 };
            ReturnCode result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Deadline.Period = new Duration { Seconds = 1 };
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Attach to the event
            int count = 0;
            DataReader reader = null;
            int totalCount = 0;
            int totalCountChange = 0;
            InstanceHandle lastInstanceHandle = InstanceHandle.HandleNil;
            _listener.RequestedDeadlineMissed += (r, s) =>
            {
                reader = r;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastInstanceHandle = s.LastInstanceHandle;
                count++;
            };

            // Write an instance
            _dataWriter.Write(new TestStruct { Id = 1 });

            // After half second deadline should not be lost yet
            Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one deadline should be lost
            Thread.Sleep(1000);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, reader);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreNotEqual(InstanceHandle.HandleNil, lastInstanceHandle);
        }
        #endregion
    }
}
