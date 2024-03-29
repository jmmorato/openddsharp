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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="QueryCondition"/> unit test class.
    /// </summary>
    [TestClass]
    public class QueryConditionTest
    {
        #region Constants
        private const string TEST_CATEGORY = "QueryCondition";
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

        #region Test Method
        /// <summary>
        /// Test the <see cref="QueryCondition.QueryExpression" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQueryExpression()
        {
            // Create a QueryCondition and check the expression
            const string expression = "Id >= %0 AND Id <= %1";
            const string parameter1 = "0";
            const string parameter2 = "10";
            var condition = _reader.CreateQueryCondition(expression, parameter1, parameter2);
            Assert.IsNotNull(condition);
            Assert.AreEqual(expression, condition.QueryExpression);
        }

        /// <summary>
        /// Test the <see cref="QueryCondition.GetQueryParameters(IList{string})" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQueryParameters()
        {
            // Create a QueryCondition
            var expression = "Id >= %0 AND Id <= %1";
            const string parameter1 = "0";
            const string parameter2 = "10";
            var condition = _reader.CreateQueryCondition(expression, parameter1, parameter2);
            Assert.IsNotNull(condition);

            // Test with null parameter
            var result = condition.GetQueryParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test with correct parameter
            var parameters = new List<string>();
            result = condition.GetQueryParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);

            // Test without query parameters
            expression = "Id >= 0 AND Id <= 10";
            var otherCondition = _reader.CreateQueryCondition(expression);
            Assert.IsNotNull(otherCondition);

            result = otherCondition.GetQueryParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(0, parameters.Count);
        }

        /// <summary>
        /// Test the <see cref="QueryCondition.SetQueryParameters(string[])" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQueryParameters()
        {
            // Initialize
            const string expression = "Id >= %0 AND Id <= %1";
            var parameter1 = "0";
            var parameter2 = "10";
            var condition = _reader.CreateQueryCondition(expression, parameter1, parameter2);
            Assert.IsNotNull(condition);

            // Test with null parameter
            var result = condition.SetQueryParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test with incorrect number of parameters
            result = condition.SetQueryParameters(parameter1);
            Assert.AreEqual(ReturnCode.Error, result);

            // Test with new parameters
            parameter1 = "20";
            parameter2 = "30";
            result = condition.SetQueryParameters(parameter1, parameter2);
            Assert.AreEqual(ReturnCode.Ok, result);

            var parameters = new List<string>();
            result = condition.GetQueryParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);
        }
        #endregion
    }
}
