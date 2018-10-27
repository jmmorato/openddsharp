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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class MultiTopicTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        private const string TEST_CATEGORY = "MultiTopic";
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
        private DomainParticipant _participant;
        private Topic _athleteTopic;
        private Topic _resultTopic;
        private Subscriber _subscriber;
        private Publisher _publisher;
        private DataWriter _athleteWriter;
        private AthleteDataWriter _athleteDataWriter;
        private DataWriter _resultWriter;
        private ResultDataWriter _resultDataWriter;
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
            _participant = _dpf.CreateParticipant(DOMAIN_ID);
            Assert.IsNotNull(_participant);

            AthleteTypeSupport athleteSupport = new AthleteTypeSupport();
            string athleteTypeName = athleteSupport.GetTypeName();
            ReturnCode result = athleteSupport.RegisterType(_participant, athleteTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _athleteTopic = _participant.CreateTopic("AthleteTopic", athleteTypeName);
            Assert.IsNotNull(_athleteTopic);
            Assert.IsNull(_athleteTopic.GetListener());
            Assert.AreEqual("AthleteTopic", _athleteTopic.Name);
            Assert.AreEqual(athleteTypeName, _athleteTopic.TypeName);

            ResultTypeSupport resultSupport = new ResultTypeSupport();
            string resultTypeName = resultSupport.GetTypeName();
            result = resultSupport.RegisterType(_participant, resultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _resultTopic = _participant.CreateTopic("ResultTopic", resultTypeName);
            Assert.IsNotNull(_resultTopic);
            Assert.IsNull(_resultTopic.GetListener());
            Assert.AreEqual("ResultTopic", _resultTopic.Name);
            Assert.AreEqual(resultTypeName, _resultTopic.TypeName);

            _subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(_subscriber);

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);

            _athleteWriter = _publisher.CreateDataWriter(_athleteTopic);
            Assert.IsNotNull(_athleteWriter);
            _athleteDataWriter = new AthleteDataWriter(_athleteWriter);

            _resultWriter = _publisher.CreateDataWriter(_resultTopic);
            Assert.IsNotNull(_resultWriter);
            _resultDataWriter = new ResultDataWriter(_resultWriter);

            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            _reader = _subscriber.CreateDataReader(_athleteTopic, qos);
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
            // Initialize
            AthleteResultTypeSupport athleteResultSupport = new AthleteResultTypeSupport();
            string athleteResultTypeName = athleteResultSupport.GetTypeName();
            ReturnCode result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a multitopic and check the properties
            string query = "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic";
            string topicName = "AthleteResultTopic";
            MultiTopic multiTopic = _participant.CreateMultiTopic(topicName, athleteResultTypeName, query);
            Assert.IsNotNull(multiTopic);
            Assert.AreEqual(topicName, multiTopic.Name);
            Assert.AreEqual(athleteResultTypeName, multiTopic.TypeName);
            Assert.AreEqual(query, multiTopic.SubscriptionExpression);
            Assert.AreEqual(_participant, multiTopic.Participant);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetExpressionParameters()
        {
            // Initialize
            AthleteResultTypeSupport athleteResultSupport = new AthleteResultTypeSupport();
            string athleteResultTypeName = athleteResultSupport.GetTypeName();
            ReturnCode result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a multitopic without expression parameters
            string query = "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic";
            string topicName = "AthleteResultTopic";
            MultiTopic multiTopic = _participant.CreateMultiTopic(topicName, athleteResultTypeName, query);
            Assert.IsNotNull(multiTopic);

            // Test null parameter
            result = multiTopic.GetExpressionParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test empty expression parameters
            List<string> parameters = new List<string>();
            result = multiTopic.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(0, parameters.Count);

            // Test with two expresion parameters
            query = "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic WHERE Id >= %0 AND Id <= %1";
            topicName = "AthleteResultTopic1";
            string parameter1 = "0";
            string parameter2 = "10";
            MultiTopic multiTopic1 = _participant.CreateMultiTopic(topicName, athleteResultTypeName, query, parameter1, parameter2);
            Assert.IsNotNull(multiTopic1);

            result = multiTopic1.GetExpressionParameters(parameters);
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
            // TODO: OpenDDS Issue: The correct number of expression parameters are not checked

            // Initialize
            AthleteResultTypeSupport athleteResultSupport = new AthleteResultTypeSupport();
            string athleteResultTypeName = athleteResultSupport.GetTypeName();
            ReturnCode result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a multitopic with two expresion parameters
            string query = "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic WHERE Id >= %0 AND Id <= %1";
            string topicName = "AthleteResultTopic";
            string parameter1 = "0";
            string parameter2 = "10";
            MultiTopic multiTopic = _participant.CreateMultiTopic(topicName, athleteResultTypeName, query, parameter1, parameter2);
            Assert.IsNotNull(multiTopic);

            List<string> parameters = new List<string>();
            result = multiTopic.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);

            // Test with null list
            result = multiTopic.SetExpressionParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Change the expression parameters
            result = multiTopic.SetExpressionParameters(parameter2, parameter1);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = multiTopic.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter2, parameters[0]);
            Assert.AreEqual(parameter1, parameters[1]);

            // Change to no expression parameters
            result = multiTopic.SetExpressionParameters();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = multiTopic.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(0, parameters.Count);
        }
        #endregion
    }
}
