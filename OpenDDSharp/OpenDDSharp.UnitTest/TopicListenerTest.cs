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
using System.Threading;
using System.Diagnostics;
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class TopicListenerTest
    {
        #region Constants        
        private const string TEST_CATEGORY = "TopicListener";
        #endregion

        #region Fields        
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
        [TestInitialize]
        public void TestInitialize()
        {
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();

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
        public void TestOnInconsistentTopic()
        {
            ManualResetEventSlim evt = new ManualResetEventSlim(false);

            // Attach to the event
            int count = 0;
            _listener.InconsistentTopic += (t, s) =>
            {
                Assert.AreEqual(_topic, t);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);
                count++;
                evt.Set();
            };

            // Enable entities
            ReturnCode result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            SupportProcessHelper supportProcess = new SupportProcessHelper(TestContext);
            Process process = supportProcess.SpawnSupportProcess(SupportTestKind.InconsistentTopicTest);

            // Wait the signal
            bool wait = evt.Wait(20000);
            Assert.IsTrue(wait);

            // Kill the process
            supportProcess.KillProcess(process);

            Assert.AreEqual(1, count);

            // Remove listener to avoid extra messages
            result = _topic.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion
    }
}
