/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;

namespace OpenDDSharp.UnitTest
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
        private TransportConfig _transportConfig;
        private TransportInst _transportInst;
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
            (_transportConfig, _transportInst) = _participant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _listener = new MyTopicListener();
            _topic = _participant.CreateTopic(TestContext.TestName, typeName, null, _listener);
            Assert.IsNotNull(_topic);
            Assert.IsNotNull(_topic.Listener);
            Assert.AreEqual(TestContext.TestName, _topic.Name);
            Assert.AreEqual(typeName, _topic.TypeName);

            var pQos = new PublisherQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
                Presentation =
                {
                    OrderedAccess = true,
                    CoherentAccess = true,
                    AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos,
                },
            };
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

            _listener.Dispose();

            TransportRegistry.Instance.RemoveConfig(_transportConfig);
            TransportRegistry.Instance.RemoveInst(_transportInst);

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
            using var evt = new ManualResetEventSlim(false);

            Topic topic = null;
            var totalCount = 0;
            var totalCountChange = 0;

            // Attach to the event
            var count = 0;
            _listener.InconsistentTopic += (t, s) =>
            {
                topic = t;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;

                count++;

                evt.Set();
            };

            // Enable entities
            var result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            var supportProcess = new SupportProcessHelper(TestContext);
            var process = supportProcess.SpawnSupportProcess(SupportTestKind.InconsistentTopicTest);

            // Wait the signal
            Assert.IsTrue(evt.Wait(5_000));
            Assert.AreSame(_topic, topic);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);

            // Kill the process
            supportProcess.KillProcess(process);

            Assert.AreEqual(1, count);

            // Remove listener to avoid extra messages
            foreach (var d in _listener.InconsistentTopic.GetInvocationList())
            {
                var del = (Action<Topic, InconsistentTopicStatus>)d;
                _listener.InconsistentTopic -= del;
            }
            result = _topic.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion
    }
}
