/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using Test;

namespace OpenDDSharp.Standard.UnitTest
{
    [TestClass]
    public class CodeGeneratorTest
    {
        #region Constants
        private const string TEST_CATEGORY = "Standard.CodeGenerator";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Publisher _publisher;
        private Subscriber _subscriber;
        private Topic _topic;
        private DataReader _dr;
        private TestStructDataReader _dataReader;
        private DataWriter _dw;
        private TestStructDataWriter _dataWriter;
        #endregion

        #region Initialization/Cleanup
        [TestInitialize]
        public void TestInitialize()
        {
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            
            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);

            _subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(_publisher);

            TestStructTypeSupport typeSupport = new TestStructTypeSupport();
            string typeName = typeSupport.GetTypeName();
            ReturnCode ret = typeSupport.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, ret);

            _topic = _participant.CreateTopic("TestTopic", typeName);
            Assert.IsNotNull(_topic);

            _dr = _subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(_dr);            
            _dataReader = new TestStructDataReader(_dr);

            _dw = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_dw);            
            _dataWriter = new TestStructDataWriter(_dw);

            Assert.IsTrue(_dataWriter.WaitForSubscriptions(1, 1000));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //if (_participant != null)
            //{
            //    ReturnCode result = _participant.DeleteContainedEntities();
            //    Assert.AreEqual(ReturnCode.Ok, result);
            //}

            //if (AssemblyInitializer.Factory != null)
            //{
            //    ReturnCode result = AssemblyInitializer.Factory.DeleteParticipant(_participant);
            //    Assert.AreEqual(ReturnCode.Ok, result);
            //}
        }
        #endregion

        #region Test Methods
        [TestMethod, TestCategory(TEST_CATEGORY)]
        public void TestGeneratedBasicTypes()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                ShortField = -1,
                LongField = -2,
                LongLongField = -3,
                UnsignedShortField = 1,
                UnsignedLongField = 2,
                UnsignedLongLongField = 3,
                CharField = 'a',
                WCharField = 'b',
                BooleanField = true,
                OctetField = 0x42
            };
            _dataWriter.Write(data);

            System.Threading.Thread.Sleep(500);

            TestStruct received = new TestStruct();
            var ret = _dataReader.ReadNextSample(received);
            
            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(data.ShortField, received.ShortField);
            Assert.AreEqual(data.LongField, received.LongField);
            Assert.AreEqual(data.LongLongField, received.LongLongField);
            Assert.AreEqual(data.UnsignedShortField, received.UnsignedShortField);
            Assert.AreEqual(data.UnsignedLongField, received.UnsignedLongField);
            Assert.AreEqual(data.UnsignedLongLongField, received.UnsignedLongLongField);
            Assert.AreEqual(data.CharField, received.CharField);
            Assert.AreEqual(data.WCharField, received.WCharField);
            Assert.AreEqual(data.BooleanField, received.BooleanField);
            Assert.AreEqual(data.OctetField, received.OctetField);

            Assert.AreEqual(typeof(short), data.ShortField.GetType());
            Assert.AreEqual(typeof(int), data.LongField.GetType());
            Assert.AreEqual(typeof(long), data.LongLongField.GetType());
            Assert.AreEqual(typeof(ushort), data.UnsignedShortField.GetType());
            Assert.AreEqual(typeof(uint), data.UnsignedLongField.GetType());
            Assert.AreEqual(typeof(ulong), data.UnsignedLongLongField.GetType());
            Assert.AreEqual(typeof(char), data.CharField.GetType());
            Assert.AreEqual(typeof(char), data.WCharField.GetType());
            Assert.AreEqual(typeof(bool), data.BooleanField.GetType());
            Assert.AreEqual(typeof(byte), data.OctetField.GetType());

            Assert.AreEqual(defaultStruct.ShortField, 0);
            Assert.AreEqual(defaultStruct.LongField, 0);
            Assert.AreEqual(defaultStruct.LongLongField, 0);
            Assert.AreEqual(defaultStruct.UnsignedShortField, (ushort)0);
            Assert.AreEqual(defaultStruct.UnsignedLongField, 0U);
            Assert.AreEqual(defaultStruct.UnsignedLongLongField, 0UL);
            Assert.AreEqual(defaultStruct.CharField, '\0');
            Assert.AreEqual(defaultStruct.WCharField, '\0');
            Assert.AreEqual(defaultStruct.BooleanField, false);
            Assert.AreEqual(defaultStruct.OctetField, 0);
        }
        #endregion
    }
}
