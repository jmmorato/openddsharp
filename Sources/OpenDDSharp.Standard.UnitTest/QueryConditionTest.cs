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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using Test;

namespace OpenDDSharp.Standard.UnitTest
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
        private TestStructDataWriter _dataWriter;
        private DataReader _reader;
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
            _dataWriter = new TestStructDataWriter(_writer);

            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            _reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(_reader);
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
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

        #region Test Method
        /// <summary>
        /// Test the <see cref="QueryCondition.QueryExpression" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQueryExpression()
        {
            // Create a QueryCondition and check the expression
            string expression = "Id >= %0 AND Id <= %1";
            string parameter1 = "0";
            string parameter2 = "10";
            QueryCondition condition = _reader.CreateQueryCondition(expression, parameter1, parameter2);
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
            string expression = "Id >= %0 AND Id <= %1";
            string parameter1 = "0";
            string parameter2 = "10";
            QueryCondition condition = _reader.CreateQueryCondition(expression, parameter1, parameter2);
            Assert.IsNotNull(condition);

            // Test with null parameter
            ReturnCode result = condition.GetQueryParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test with correct parameter
            List<string> parameters = new List<string>();
            result = condition.GetQueryParameters(parameters);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual(parameter1, parameters[0]);
            Assert.AreEqual(parameter2, parameters[1]);

            // Test without query parameters
            expression = "Id >= 0 AND Id <= 10";
            QueryCondition otherCondition = _reader.CreateQueryCondition(expression);
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
            string expression = "Id >= %0 AND Id <= %1";
            string parameter1 = "0";
            string parameter2 = "10";
            QueryCondition condition = _reader.CreateQueryCondition(expression, parameter1, parameter2);
            Assert.IsNotNull(condition);

            // Test with null parameter
            ReturnCode result = condition.SetQueryParameters(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test with incorrect number of parameters
            result = condition.SetQueryParameters(parameter1);
            Assert.AreEqual(ReturnCode.Error, result);

            // Test with new parameters
            parameter1 = "20";
            parameter2 = "30";
            result = condition.SetQueryParameters(parameter1, parameter2);
            Assert.AreEqual(ReturnCode.Ok, result);

            List<string> parameters = new List<string>();
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
