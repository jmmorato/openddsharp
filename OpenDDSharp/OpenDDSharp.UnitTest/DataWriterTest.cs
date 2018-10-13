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
    public class DataWriterTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
        private DomainParticipant _participant;
        private Topic _topic;
        private Publisher _publisher;
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

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);
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
        [TestCategory("DataWriter")]
        public void TestProperties()
        {            
            // Create a DataWriter and check the Topic and Participant properties
            DataWriter writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            Assert.AreEqual(_topic, writer.Topic);
            Assert.AreEqual(_publisher, writer.Publisher);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetQos()
        {
            
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestSetQos()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetListener()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestSetListener()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestWaitForAcknowledgments()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetLivelinessLostStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetOfferedDeadlineMissedStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetOfferedIncompatibleQosStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetPublicationMatchedStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestAssertLiveliness()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetMatchedSubscriptions()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetMatchedSubscriptionData()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestRead()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestTake()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestReadInstance()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestTakeInstance()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestReadNextInstance()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestTakeNextInstance()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestReadNextSample()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestTakeNextSample()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetKeyValue()
        {

        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestLookupInstance()
        {

        }
        #endregion
    }
}
