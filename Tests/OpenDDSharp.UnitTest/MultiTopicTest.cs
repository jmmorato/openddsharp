/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

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
using System.Diagnostics.CodeAnalysis;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="MultiTopic"/> unit test class.
    /// </summary>
    [TestClass]
    public class MultiTopicTest
    {
        #region Constants
        private const string TEST_CATEGORY = "MultiTopic";
        #endregion

        #region Fields
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
        /// <summary>
        /// Gets or sets test context object.
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

            AthleteTypeSupport athleteSupport = new AthleteTypeSupport();
            string athleteTypeName = athleteSupport.GetTypeName();
            ReturnCode result = athleteSupport.RegisterType(_participant, athleteTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _athleteTopic = _participant.CreateTopic("AthleteTopic", athleteTypeName);
            Assert.IsNotNull(_athleteTopic);
            Assert.IsNull(_athleteTopic.Listener);
            Assert.AreEqual("AthleteTopic", _athleteTopic.Name);
            Assert.AreEqual(athleteTypeName, _athleteTopic.TypeName);

            ResultTypeSupport resultSupport = new ResultTypeSupport();
            string resultTypeName = resultSupport.GetTypeName();
            result = resultSupport.RegisterType(_participant, resultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _resultTopic = _participant.CreateTopic("ResultTopic", resultTypeName);
            Assert.IsNotNull(_resultTopic);
            Assert.IsNull(_resultTopic.Listener);
            Assert.AreEqual("ResultTopic", _resultTopic.Name);
            Assert.AreEqual(resultTypeName, _resultTopic.TypeName);

            _subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(_subscriber);

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);

            _athleteWriter = _publisher.CreateDataWriter(_athleteTopic);
            Assert.IsNotNull(_athleteWriter);
            _athleteDataWriter = new AthleteDataWriter(_athleteWriter);
            Assert.IsNotNull(_athleteDataWriter);

            _resultWriter = _publisher.CreateDataWriter(_resultTopic);
            Assert.IsNotNull(_resultWriter);
            _resultDataWriter = new ResultDataWriter(_resultWriter);
            Assert.IsNotNull(_resultDataWriter);

            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            _reader = _subscriber.CreateDataReader(_athleteTopic, qos);
            Assert.IsNotNull(_reader);
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _publisher?.DeleteDataWriter(_resultWriter);
            _publisher?.DeleteDataWriter(_athleteWriter);
            _reader?.DeleteContainedEntities();
            _publisher?.DeleteContainedEntities();
            _subscriber?.DeleteDataReader(_reader);
            _subscriber?.DeleteContainedEntities();
            _participant?.DeletePublisher(_publisher);
            _participant?.DeleteSubscriber(_subscriber);
            _participant?.DeleteTopic(_resultTopic);
            _participant?.DeleteContainedEntities();
            AssemblyInitializer.Factory?.DeleteParticipant(_participant);

            _participant = null;
            _publisher = null;
            _subscriber = null;
            _resultTopic = null;
            _resultWriter = null;
            _athleteWriter = null;
            _reader = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="MultiTopic"/> properties values.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestProperties()
        {
            // Initialize
            var athleteResultSupport = new AthleteResultTypeSupport();
            var athleteResultTypeName = athleteResultSupport.GetTypeName();
            var result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a MultiTopic and check the properties
            const string query = "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic";
            const string topicName = "AthleteResultTopic";
            var multiTopic = _participant.CreateMultiTopic(topicName, athleteResultTypeName, query);
            Assert.IsNotNull(multiTopic);
            Assert.AreEqual(topicName, multiTopic.Name);
            Assert.AreEqual(athleteResultTypeName, multiTopic.TypeName);
            Assert.AreEqual(query, multiTopic.SubscriptionExpression);
            Assert.AreEqual(_participant, multiTopic.Participant);
        }

        /// <summary>
        /// Test the <see cref="MultiTopic.GetExpressionParameters(IList{string})"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetExpressionParameters()
        {
            // Initialize
            var athleteResultSupport = new AthleteResultTypeSupport();
            var athleteResultTypeName = athleteResultSupport.GetTypeName();
            var result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a MultiTopic without expression parameters
            var query = "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic";
            var topicName = "AthleteResultTopic";
            var multiTopic = _participant.CreateMultiTopic(topicName, athleteResultTypeName, query);
            Assert.IsNotNull(multiTopic);

            // Test null parameter
            result = multiTopic.GetExpressionParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test empty expression parameters
            var parameters = new List<string>();
            result = multiTopic.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(0, parameters.Count);

            // Test with two expression parameters
            query = "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic WHERE Id >= %0 AND Id <= %1";
            topicName = "AthleteResultTopic1";
            const string parameter1 = "0";
            const string parameter2 = "10";
            var multiTopic1 = _participant.CreateMultiTopic(topicName, athleteResultTypeName, query, parameter1, parameter2);
            Assert.IsNotNull(multiTopic1);

            result = multiTopic1.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);
        }

        /// <summary>
        /// Test the <see cref="MultiTopic.SetExpressionParameters(string[])"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetExpressionParameters()
        {
            // TODO: OpenDDS Issue: The correct number of expression parameters are not checked

            // Initialize
            var athleteResultSupport = new AthleteResultTypeSupport();
            var athleteResultTypeName = athleteResultSupport.GetTypeName();
            var result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a MultiTopic with two expression parameters
            const string query = "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic WHERE Id >= %0 AND Id <= %1";
            const string topicName = "AthleteResultTopic";
            const string parameter1 = "0";
            const string parameter2 = "10";
            var multiTopic = _participant.CreateMultiTopic(topicName, athleteResultTypeName, query, parameter1, parameter2);
            Assert.IsNotNull(multiTopic);

            var parameters = new List<string>();
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