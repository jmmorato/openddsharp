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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using StructWrapper;

namespace OpenDDSharp.Standard.UnitTest
{
    /// <summary>
    /// <see cref="WaitSet"/> unit test class.
    /// </summary>
    [TestClass]
    public class WaitSetTest
    {
        #region Constants
        private const string TEST_CATEGORY = "WaitSet";
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
        /// Test the <see cref="WaitSet.Wait(ICollection{Condition}, Duration)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestWait()
        {
            // Initialize
            WaitSet waitSet = new WaitSet();
            GuardCondition guardCondition = new GuardCondition();
            ReturnCode result = waitSet.AttachCondition(guardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with null conditions
            result = waitSet.Wait(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Attach again should return OK but not actually add another condition to the WaitSet
            result = waitSet.AttachCondition(guardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);

            List<Condition> conditions = new List<Condition>();
            result = waitSet.GetConditions(conditions);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(conditions);
            Assert.AreEqual(1, conditions.Count);
            Assert.AreSame(guardCondition, conditions[0]);

            // Test thread wait infinite
            int count = 0;
            Thread thread = new Thread(() =>
            {
                result = waitSet.Wait(conditions);
                Assert.AreEqual(ReturnCode.Ok, result);
                Assert.IsNotNull(conditions);
                Assert.AreEqual(1, conditions.Count);
                Assert.AreEqual(guardCondition, conditions[0]);
                guardCondition.TriggerValue = false;
                count++;
            });
            thread.Start();

            guardCondition.TriggerValue = true;
            thread.Join();
            Assert.AreEqual(1, count);

            // Test timeout
            count = 0;
            thread = new Thread(() =>
            {
                result = waitSet.Wait(conditions, new Duration { Seconds = 0, NanoSeconds = 100000000});
                Assert.AreEqual(ReturnCode.Timeout, result);
                Assert.IsNotNull(conditions);
                Assert.AreEqual(0, conditions.Count);
                count++;
            });
            thread.Start();
            thread.Join();
            Assert.AreEqual(1, count);

            // Test exit before timeout
            count = 0;
            thread = new Thread(() =>
            {
                result = waitSet.Wait(conditions, new Duration { Seconds = 5});
                Assert.AreEqual(ReturnCode.Ok, result);
                Assert.IsNotNull(conditions);
                Assert.AreEqual(1, conditions.Count);
                Assert.AreEqual(guardCondition, conditions[0]);
                Assert.IsTrue(guardCondition.TriggerValue);
                guardCondition.TriggerValue = false;
                count++;
            });
            thread.Start();

            guardCondition.TriggerValue = true;
            thread.Join();
            Assert.AreEqual(1, count);
        }

        /// <summary>
        /// Test the <see cref="WaitSet.AttachCondition(Condition)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestAttachCondition()
        {
            // Initialize
            WaitSet waitSet = new WaitSet();
            GuardCondition guardCondition = new GuardCondition();

            // Test with null parameter
            ReturnCode result = waitSet.AttachCondition(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test with correct parameter
            result = waitSet.AttachCondition(guardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Attach again should return OK but not actually add another condition to the WaitSet
            result = waitSet.AttachCondition(guardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);

            List<Condition> conditions = new List<Condition>();
            result = waitSet.GetConditions(conditions);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(conditions);
            Assert.AreEqual(1, conditions.Count);
            Assert.AreEqual(guardCondition, conditions[0]);
        }

        /// <summary>
        /// Test the <see cref="WaitSet.DetachCondition(Condition)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDetachCondition()
        {
            // Initialize
            WaitSet waitSet = new WaitSet();
            GuardCondition guardCondition = new GuardCondition();
            ReturnCode result = waitSet.AttachCondition(guardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with null parameter
            result = waitSet.DetachCondition(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test with correct parameter
            result = waitSet.DetachCondition(guardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test again the same condition
            result = waitSet.DetachCondition(guardCondition);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Detach a not attached condition
            GuardCondition notAttachedCondition = new GuardCondition();
            result = waitSet.DetachCondition(notAttachedCondition);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);
        }

        /// <summary>
        /// Test the <see cref="WaitSet.GetConditions(ICollection{Condition})" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetConditions()
        {
            // Initialize
            WaitSet waitSet = new WaitSet();
            GuardCondition guardCondition = new GuardCondition();
            GuardCondition otherGuardCondition = new GuardCondition();
            ReturnCode result = waitSet.AttachCondition(guardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);
            result = waitSet.AttachCondition(otherGuardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with null parameter
            result = waitSet.GetConditions(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Test with correct parameter
            List<Condition> conditions = new List<Condition>() { null };
            result = waitSet.GetConditions(conditions);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(conditions);
            Assert.AreEqual(2, conditions.Count);
            Assert.IsNotNull(conditions[0]);
            Assert.IsNotNull(conditions[1]);
            Assert.IsTrue(conditions[0] != conditions[1]);
            Assert.IsTrue(guardCondition == conditions[0] || guardCondition == conditions[1]);
            Assert.IsTrue(otherGuardCondition == conditions[0] || otherGuardCondition == conditions[1]);
        }

        /// <summary>
        /// Test the <see cref="WaitSet.DetachConditions(ICollection{Condition})" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDetachConditions()
        {
            // Initialize
            WaitSet waitSet = new WaitSet();
            GuardCondition guardCondition = new GuardCondition();
            GuardCondition otherGuardCondition = new GuardCondition();
            GuardCondition anotherGuardCondition = new GuardCondition();
            ReturnCode result = waitSet.AttachCondition(guardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);
            result = waitSet.AttachCondition(otherGuardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);
            result = waitSet.AttachCondition(anotherGuardCondition);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with null parameter
            result = waitSet.DetachConditions(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with empty list parameter
            List<Condition> conditions = new List<Condition>();
            result = waitSet.DetachConditions(conditions);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test with single element list parameter
            conditions = new List<Condition>() { guardCondition };
            result = waitSet.DetachConditions(conditions);
            Assert.AreEqual(ReturnCode.Ok, result);

            conditions = new List<Condition>() { null };
            result = waitSet.GetConditions(conditions);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(conditions);
            Assert.AreEqual(2, conditions.Count);
            Assert.IsNotNull(conditions[0]);
            Assert.IsNotNull(conditions[1]);
            Assert.IsTrue(conditions[0] != conditions[1]);
            Assert.IsTrue(anotherGuardCondition == conditions[0] || anotherGuardCondition == conditions[1]);
            Assert.IsTrue(otherGuardCondition == conditions[0] || otherGuardCondition == conditions[1]);

            // Detach the other conditions
            conditions = new List<Condition>() { otherGuardCondition, anotherGuardCondition };
            result = waitSet.DetachConditions(conditions);
            Assert.AreEqual(ReturnCode.Ok, result);

            conditions = new List<Condition>() { null };
            result = waitSet.GetConditions(conditions);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(conditions);
            Assert.AreEqual(0, conditions.Count);

            // Detach a not attached conditions
            GuardCondition notAttachedCondition = new GuardCondition();
            conditions = new List<Condition>() { notAttachedCondition };
            result = waitSet.DetachConditions(conditions);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);
        }
        #endregion
    }
}
