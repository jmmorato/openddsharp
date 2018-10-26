﻿/*********************************************************************
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
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class TopicListenerTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        private const string TEST_CATEGORY = "TopicListener";
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
        private DomainParticipant _participant;
        private Topic _topic;        
        private Publisher _publisher;
        private DataWriter _writer;        
        private MyTopicListener _listener;        
        #endregion

        #region Properties
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dpf = ParticipantService.Instance.GetDomainParticipantFactory(new string[] { "-DCPSDebugLevel", "10" });
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

            _listener = new MyTopicListener();
            _topic = _participant.CreateTopic(TestContext.TestName, typeName, _listener);
            Assert.IsNotNull(_topic);
            Assert.IsNotNull(_topic.GetListener());
            Assert.AreEqual(TestContext.TestName, _topic.Name);
            Assert.AreEqual(typeName, _topic.TypeName);

            PublisherQos pQos = new PublisherQos();
            pQos.EntityFactory.AutoenableCreatedEntities = false;
            pQos.Presentation.OrderedAccess = true;
            pQos.Presentation.CoherentAccess = true;
            pQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos;
            _publisher = _participant.CreatePublisher(pQos);
            Assert.IsNotNull(_publisher);

            _writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_writer);
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
        [TestCategory(TEST_CATEGORY)]
        public void TestOnInconsistentTopic()
        {
            // TODO: Cannot be tested with only one process because it shares the discovery between participants
            // and the creation of the otherTopic fails before the inconsistent_topic is checked.
            // Try to spawn a support process for the test

            #region Commented code
            //// Attach to the event
            //int count = 0;
            //_listener.InconsistentTopic += (t, s) =>
            //{
            //    Assert.AreEqual(_topic, t);
            //    Assert.AreEqual(1, s.TotalCount);
            //    Assert.AreEqual(1, s.TotalCountChange);
            //    count++;
            //};

            //// Enable entities
            //ReturnCode result = _writer.Enable();
            //Assert.AreEqual(ReturnCode.Ok, result);

            //// Create a inconsistent topic
            //DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory();
            //DomainParticipant otherParticipant = dpf.CreateParticipant(DOMAIN_ID);
            //Assert.IsNotNull(otherParticipant);

            //Subscriber subscriber = otherParticipant.CreateSubscriber();
            //Assert.IsNotNull(subscriber);

            ////AthleteTypeSupport support = new AthleteTypeSupport();            
            //TestStructTypeSupport tSupport = new TestStructTypeSupport();
            //result = tSupport.RegisterType(otherParticipant, tSupport.GetTypeName());
            //Assert.AreEqual(ReturnCode.Ok, result);

            //Topic otherTopic = otherParticipant.CreateTopic(nameof(TestOnInconsistentTopic), "OtherName");
            //Assert.IsNotNull(otherTopic);

            //DataReader reader = subscriber.CreateDataReader(otherTopic);
            //Assert.IsNotNull(reader);

            //// Wait for discovery
            //System.Threading.Thread.Sleep(500);
            //Assert.AreEqual(1, count);

            //// Remove listener to avoid extra messages
            //result = _topic.SetListener(null);
            //Assert.AreEqual(ReturnCode.Ok, result);

            //result = otherParticipant.DeleteContainedEntities();
            //Assert.AreEqual(ReturnCode.Ok, result);

            //result = _dpf.DeleteParticipant(otherParticipant);
            //Assert.AreEqual(ReturnCode.Ok, result);
            #endregion
        }
        #endregion
    }
}
