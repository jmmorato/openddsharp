﻿/*********************************************************************
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
    /// <see cref="ContentFilteredTopic"/> unit test class.
    /// </summary>
    [TestClass]
    public class ContentFilteredTopicTest
    {
        #region Constants
        private const string TEST_CATEGORY = "ContentFilteredTopic";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Topic _topic;
        private Subscriber _subscriber;
        private Publisher _publisher;
        private DataWriter _writer;
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

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);
            Assert.IsNull(_topic.Listener);
            Assert.AreEqual(TestContext.TestName, _topic.Name);
            Assert.AreEqual(typeName, _topic.TypeName);

            _subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(_subscriber);

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);

            _writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_writer);

            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            _reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(_reader);
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _publisher?.DeleteDataWriter(_writer);
            _reader?.DeleteContainedEntities();
            _publisher?.DeleteContainedEntities();
            _subscriber?.DeleteDataReader(_reader);
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
            _writer = null;
            _reader = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="ContentFilteredTopic"/> properties values.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestProperties()
        {
            // Create a content filtered topic and check the properties
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            const string query = "Id <= 1";
            const string topicName = "FilteredTopic";
            var filteredTopic = _participant.CreateContentFilteredTopic(topicName, _topic, query);
            Assert.IsNotNull(filteredTopic);
            Assert.AreEqual(topicName, filteredTopic.Name);
            Assert.AreEqual(typeName, filteredTopic.TypeName);
            Assert.AreEqual(query, filteredTopic.FilterExpression);
            Assert.AreEqual(_participant, filteredTopic.Participant);
            Assert.AreEqual(_topic, filteredTopic.RelatedTopic);
        }

        /// <summary>
        /// Test the <see cref="ContentFilteredTopic.GetExpressionParameters(IList{string})"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetExpressionParameters()
        {
            // Create a content filtered topic without expression parameters
            var query = "Id <= 1";
            var topicName = "FilteredTopic";
            var filteredTopic = _participant.CreateContentFilteredTopic(topicName, _topic, query);
            Assert.IsNotNull(filteredTopic);

            // Test with null parameter
            var result = filteredTopic.GetExpressionParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Check empty expression parameters
            var parameters = new List<string> { "1" };
            result = filteredTopic.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(0, parameters.Count);

            // Create a content filtered topic with one expression parameter
            query = "Id <= %0";
            topicName = "FilteredTopic1";
            var parameter1 = "10";
            var filteredTopic1 = _participant.CreateContentFilteredTopic(topicName, _topic, query, parameter1);
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
            const string parameter2 = "10";
            var filteredTopic2 = _participant.CreateContentFilteredTopic(topicName, _topic, query, parameter1, parameter2);
            Assert.IsNotNull(filteredTopic2);

            parameters = new List<string> { "1" };
            result = filteredTopic2.GetExpressionParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);
        }

        /// <summary>
        /// Test the <see cref="ContentFilteredTopic.SetExpressionParameters(string[])"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetExpressionParameters()
        {
            // Create a content filtered topic with two expression parameter
            const string query = "Id <= %0 AND Id <= %1";
            const string topicName = "FilteredTopic";
            const string parameter1 = "0";
            const string parameter2 = "10";

            // Test with incorrect number of parameters
            var filteredTopic = _participant.CreateContentFilteredTopic(topicName, _topic, query, parameter1);
            Assert.IsNull(filteredTopic);

            // Test with correct number of parameters
            filteredTopic = _participant.CreateContentFilteredTopic(topicName, _topic, query, parameter1, parameter2);
            Assert.IsNotNull(filteredTopic);

            // Test with null parameter
            var result = filteredTopic.SetExpressionParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test with incorrect number of expression parameters
            result = filteredTopic.SetExpressionParameters(parameter1);
            Assert.AreEqual(ReturnCode.Error, result);

            result = filteredTopic.SetExpressionParameters();
            Assert.AreEqual(ReturnCode.Error, result);

            // Test with correct number of expression parameters
            result = filteredTopic.SetExpressionParameters(parameter2, parameter1);
            Assert.AreEqual(ReturnCode.Ok, result);

            var parameters = new List<string> { "1" };
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
