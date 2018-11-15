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
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class ContentFilteredTopicTest
    {
        #region Constants        
        private const string TEST_CATEGORY = "ContentFilteredTopic";
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
        private DomainParticipant _participant;
        private Topic _topic;
        private Subscriber _subscriber;
        private Publisher _publisher;
        private DataWriter _writer;
        private TestStructDataWriter _dataWriter;        
        private DataReader _reader;
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
            _participant = _dpf.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();

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

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);

            _writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_writer);
            _dataWriter = new TestStructDataWriter(_writer);

            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            _reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(_reader);
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
        public void TestProperties()
        {
            // Create a content filtered topic and check the properties
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            string query = "Id <= 1";
            string topicName = "FilteredTopic";
            ContentFilteredTopic filteredTopic = _participant.CreateContentFilteredTopic(topicName, _topic, query);
            Assert.IsNotNull(filteredTopic);
            Assert.AreEqual(topicName, filteredTopic.Name);
            Assert.AreEqual(typeName, filteredTopic.TypeName);
            Assert.AreEqual(query, filteredTopic.FilterExpression);
            Assert.AreEqual(_participant, filteredTopic.Participant);
            Assert.AreEqual(_topic, filteredTopic.RelatedTopic);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetExpressionParameters()
        {
            // Create a content filtered topic without expression parameters
            string query = "Id <= 1";
            string topicName = "FilteredTopic";
            ContentFilteredTopic filteredTopic = _participant.CreateContentFilteredTopic(topicName, _topic, query);
            Assert.IsNotNull(filteredTopic);

            // Test with null parameter
            ReturnCode result = filteredTopic.GetExpressionParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Check empty expression parameters
            List<string> parameters = new List<string> { "1" };
            result = filteredTopic.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(0, parameters.Count);

            // Create a content filtered topic with one expression parameter
            query = "Id <= %0";
            topicName = "FilteredTopic1";
            string parameter1 = "10";
            ContentFilteredTopic filteredTopic1 = _participant.CreateContentFilteredTopic(topicName, _topic, query, parameter1);
            Assert.IsNotNull(filteredTopic1);

            parameters = new List<string> { "1" };
            result = filteredTopic1.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);

            // Create a content filtered topic with two expression parameter
            query = "Id <= %0 AND Id <= %1";
            topicName = "FilteredTopic2";
            parameter1 = "0";
            string parameter2 = "10";
            ContentFilteredTopic filteredTopic2 = _participant.CreateContentFilteredTopic(topicName, _topic, query, parameter1, parameter2);
            Assert.IsNotNull(filteredTopic2);

            parameters = new List<string> { "1" };
            result = filteredTopic2.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetExpressionParameters()
        {
            // Create a content filtered topic with two expression parameter
            string query = "Id <= %0 AND Id <= %1";
            string topicName = "FilteredTopic";
            string parameter1 = "0";
            string parameter2 = "10";

            // Test with incorrect number of parameters
            ContentFilteredTopic filteredTopic = _participant.CreateContentFilteredTopic(topicName, _topic, query, parameter1);
            Assert.IsNull(filteredTopic);

            // Test with correct number of parameters
            filteredTopic = _participant.CreateContentFilteredTopic(topicName, _topic, query, parameter1, parameter2);
            Assert.IsNotNull(filteredTopic);

            // Test with null parameter
            ReturnCode result = filteredTopic.SetExpressionParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test with incorrect number of expression parameters
            result = filteredTopic.SetExpressionParameters(parameter1);
            Assert.AreEqual(ReturnCode.Error, result);

            result = filteredTopic.SetExpressionParameters();
            Assert.AreEqual(ReturnCode.Error, result);

            // Test with correct number of expression parameters
            result = filteredTopic.SetExpressionParameters(parameter2, parameter1);
            Assert.AreEqual(ReturnCode.Ok, result);

            List<string> parameters = new List<string> { "1" };
            result = filteredTopic.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter2, parameters[0]);
            Assert.AreEqual(parameter1, parameters[1]);
        }
        #endregion
    }
}
