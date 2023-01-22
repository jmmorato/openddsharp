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
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using OpenDDSharp.Standard.UnitTest.Listeners;
using Test;

namespace OpenDDSharp.Standard.UnitTest
{
    /// <summary>
    /// <see cref="TopicListener"/> unit test class.
    /// </summary>
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
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _listener = new MyTopicListener();
            _topic = _participant.CreateTopic(TestContext.TestName, typeName, null, _listener);
            Assert.IsNotNull(_topic);
            Assert.IsNotNull(_topic.Listener);
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

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _publisher?.DeleteDataWriter(_writer);
            _publisher?.DeleteContainedEntities();
            _participant?.DeletePublisher(_publisher);
            _participant?.DeleteTopic(_topic);
            _participant?.DeleteContainedEntities();
            AssemblyInitializer.Factory?.DeleteParticipant(_participant);

            _participant = null;
            _publisher = null;
            _topic = null;
            _writer = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="TopicListener.OnInconsistentTopic(Topic, InconsistentTopicStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnInconsistentTopic()
        {
            using (ManualResetEventSlim evt = new ManualResetEventSlim(false))
            {
                Topic topic = null;
                int totalCount = 0;
                int totalCountChange = 0;

                // Attach to the event
                int count = 0;
                _listener.InconsistentTopic += (t, s) =>
                {
                    topic = t;
                    totalCount = s.TotalCount;
                    totalCountChange = s.TotalCountChange;

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
                Assert.AreSame(_topic, topic);
                Assert.AreEqual(1, totalCount);
                Assert.AreEqual(1, totalCountChange);

                // Kill the process
                supportProcess.KillProcess(process);

                Assert.AreEqual(1, count);

                // Remove listener to avoid extra messages
                result = _topic.SetListener(null);
                Assert.AreEqual(ReturnCode.Ok, result);
            }
        }
        #endregion
    }
}
