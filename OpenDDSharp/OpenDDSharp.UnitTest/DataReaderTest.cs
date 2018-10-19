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

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetQos()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestSetQos()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetListener()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestSetListener()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestCreateReadCondition()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestCreateQueryCondition()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestDeleteReadCondition()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestDeleteContainedEntities()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetSampleRejectedStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetLivelinessChangedStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetRequestedDeadlineMissedStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetRequestedIncompatibleQosStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetSubscriptionMatchedStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetSampleLostStatus()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestWaitForHistoricalData()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetMatchedPublications()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetMatchedPublicationData()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestRead()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestTake()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestReadInstance()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestTakeInstance()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestReadNextInstance()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestTakeNextInstance()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestReadNextSample()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestTakeNextSample()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestGetKeyValue()
        {

        }

        [TestMethod]
        [TestCategory("DataReader")]
        public void TestLookupInstance()
        {

        }
        #endregion
    }
}
