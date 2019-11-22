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
using System.Linq;
using System.Collections.Generic;
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
                OctetField = 0x42,
                FloatField = 42.42f,
                DoubleField = 23.23d,
                LongDoubleField = 69.69m,
            };
            _dataWriter.Write(data);

            // TODO: Wait for acknowledgments
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
            Assert.AreEqual(data.FloatField, received.FloatField);
            Assert.AreEqual(data.DoubleField, received.DoubleField);
            Assert.AreEqual(data.LongDoubleField, received.LongDoubleField);

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
            Assert.AreEqual(typeof(float), data.FloatField.GetType());
            Assert.AreEqual(typeof(double), data.DoubleField.GetType());
            Assert.AreEqual(typeof(decimal), data.LongDoubleField.GetType());

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
            Assert.AreEqual(defaultStruct.FloatField, 0.0f);
            Assert.AreEqual(defaultStruct.DoubleField, 0.0);
            Assert.AreEqual(defaultStruct.LongDoubleField, 0.0m);
        }

        [TestMethod, TestCategory(TEST_CATEGORY)]
        public void TestGeneratedBasicTypeSequences()
        {
            // TODO: Bounded lists that exceed the bound throws an exception. 
            // As per documentation: Bounds checking on bounded sequences may raise an exception if necessary.
            // Check bound before toNative and throw a user friendly exception.
            // Another option is to implement an internal BoundedList that check the bound before Add/Insert.

            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {              
                BoundedBooleanSequenceField = { true, true, false, false, true },
                UnboundedBooleanSequenceField = { true, true, false, false, true, true, false },
                BoundedCharSequenceField = { 'z' },
                UnboundedCharSequenceField = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' },
                BoundedWCharSequenceField = { 'i', 'j', 'k', 'l', 'm' },
                UnboundedWCharSequenceField = { 'n', 'o', 'p' },
                BoundedOctetSequenceField = { 0x01, 0x02, 0x03 },
                UnboundedOctetSequenceField = { 0x04, 0x05, 0x06, 0x07, 0x08 },
                BoundedShortSequenceField = { -1, 2, -3 },
                UnboundedShortSequenceField = { 4, -5, 6, -7, 8 },
                BoundedUShortSequenceField = { 1, 2, 3 },
                UnboundedUShortSequenceField = { 4, 5, 6, 7, 8 },
                BoundedLongSequenceField = { -1, 2, -3, 100, -200 },
                UnboundedLongSequenceField = { 1, -2, 3, -100, 200, -300, 1000 },
                BoundedULongSequenceField = { 1, 2, 3, 100, 200 },
                UnboundedULongSequenceField = { 1, 2, 3, 100, 200, 300, 1000 },
                BoundedLongLongSequenceField = { -1, 2, -3, 100, -200 },
                UnboundedLongLongSequenceField = { 1, -2, 3, -100, 200, -300, 1000 },
                BoundedULongLongSequenceField = { 1, 2, 3, 100, 200 },
                UnboundedULongLongSequenceField = { 1, 2, 3, 100, 200, 300, 1000 },
                BoundedFloatSequenceField = { -1.0f, 2.1f, -3.2f, 100.3f, -200.4f },
                UnboundedFloatSequenceField = { 1.5f, -2.6f, 3.7f, -100.8f, 200.9f, -300.0f, 1000.1f },
                BoundedDoubleSequenceField = { -1.0d, 2.1d, -3.2d, 100.3d, -200.4d },
                UnboundedDoubleSequenceField = { 1.5d, -2.6d, 3.7d, -100.8d, 200.9d, -300.0d, 1000.1d },
                BoundedLongDoubleSequenceField = { -1.0m, 2.1m, -3.2m, 100.3m, -200.4m },
                UnboundedLongDoubleSequenceField = { 1.5m, -2.6m, 3.7m, -100.8m, 200.9m, -300.0m, 1000.1m },
            };
            _dataWriter.Write(data);

            // TODO: Wait for acknowledgments
            System.Threading.Thread.Sleep(500);

            TestStruct received = new TestStruct();
            var ret = _dataReader.ReadNextSample(received);

            Assert.AreEqual(ReturnCode.Ok, ret);           
            Assert.IsTrue(data.BoundedBooleanSequenceField.SequenceEqual(received.BoundedBooleanSequenceField));
            Assert.IsTrue(data.UnboundedBooleanSequenceField.SequenceEqual(received.UnboundedBooleanSequenceField));
            Assert.IsTrue(data.BoundedCharSequenceField.SequenceEqual(received.BoundedCharSequenceField));
            Assert.IsTrue(data.UnboundedCharSequenceField.SequenceEqual(received.UnboundedCharSequenceField));
            Assert.IsTrue(data.BoundedWCharSequenceField.SequenceEqual(received.BoundedWCharSequenceField));
            Assert.IsTrue(data.UnboundedWCharSequenceField.SequenceEqual(received.UnboundedWCharSequenceField));
            Assert.IsTrue(data.BoundedOctetSequenceField.SequenceEqual(received.BoundedOctetSequenceField));
            Assert.IsTrue(data.UnboundedOctetSequenceField.SequenceEqual(received.UnboundedOctetSequenceField));
            Assert.IsTrue(data.BoundedShortSequenceField.SequenceEqual(received.BoundedShortSequenceField));
            Assert.IsTrue(data.UnboundedShortSequenceField.SequenceEqual(received.UnboundedShortSequenceField));
            Assert.IsTrue(data.BoundedUShortSequenceField.SequenceEqual(received.BoundedUShortSequenceField));
            Assert.IsTrue(data.UnboundedUShortSequenceField.SequenceEqual(received.UnboundedUShortSequenceField));
            Assert.IsTrue(data.BoundedLongSequenceField.SequenceEqual(received.BoundedLongSequenceField));
            Assert.IsTrue(data.UnboundedLongSequenceField.SequenceEqual(received.UnboundedLongSequenceField));
            Assert.IsTrue(data.BoundedULongSequenceField.SequenceEqual(received.BoundedULongSequenceField));
            Assert.IsTrue(data.UnboundedULongSequenceField.SequenceEqual(received.UnboundedULongSequenceField));
            Assert.IsTrue(data.BoundedLongLongSequenceField.SequenceEqual(received.BoundedLongLongSequenceField));
            Assert.IsTrue(data.UnboundedLongLongSequenceField.SequenceEqual(received.UnboundedLongLongSequenceField));
            Assert.IsTrue(data.BoundedULongLongSequenceField.SequenceEqual(received.BoundedULongLongSequenceField));
            Assert.IsTrue(data.UnboundedULongLongSequenceField.SequenceEqual(received.UnboundedULongLongSequenceField));
            Assert.IsTrue(data.BoundedFloatSequenceField.SequenceEqual(received.BoundedFloatSequenceField));
            Assert.IsTrue(data.UnboundedFloatSequenceField.SequenceEqual(received.UnboundedFloatSequenceField));
            Assert.IsTrue(data.BoundedDoubleSequenceField.SequenceEqual(received.BoundedDoubleSequenceField));
            Assert.IsTrue(data.UnboundedDoubleSequenceField.SequenceEqual(received.UnboundedDoubleSequenceField));
            Assert.IsTrue(data.BoundedLongDoubleSequenceField.SequenceEqual(received.BoundedLongDoubleSequenceField));
            Assert.IsTrue(data.UnboundedLongDoubleSequenceField.SequenceEqual(received.UnboundedLongDoubleSequenceField));

            Assert.IsTrue(typeof(IList<bool>).IsAssignableFrom(data.BoundedBooleanSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<bool>).IsAssignableFrom(data.UnboundedBooleanSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<char>).IsAssignableFrom(data.BoundedCharSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<char>).IsAssignableFrom(data.UnboundedCharSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<char>).IsAssignableFrom(data.BoundedWCharSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<char>).IsAssignableFrom(data.UnboundedWCharSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<byte>).IsAssignableFrom(data.BoundedOctetSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<byte>).IsAssignableFrom(data.UnboundedOctetSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<short>).IsAssignableFrom(data.BoundedShortSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<short>).IsAssignableFrom(data.UnboundedShortSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<ushort>).IsAssignableFrom(data.BoundedUShortSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<ushort>).IsAssignableFrom(data.UnboundedUShortSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<int>).IsAssignableFrom(data.BoundedLongSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<int>).IsAssignableFrom(data.UnboundedLongSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<uint>).IsAssignableFrom(data.BoundedULongSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<uint>).IsAssignableFrom(data.UnboundedULongSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<long>).IsAssignableFrom(data.BoundedLongLongSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<long>).IsAssignableFrom(data.UnboundedLongLongSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<ulong>).IsAssignableFrom(data.BoundedULongLongSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<ulong>).IsAssignableFrom(data.UnboundedULongLongSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<float>).IsAssignableFrom(data.BoundedFloatSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<float>).IsAssignableFrom(data.UnboundedFloatSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<double>).IsAssignableFrom(data.BoundedDoubleSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<double>).IsAssignableFrom(data.UnboundedDoubleSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<decimal>).IsAssignableFrom(data.BoundedLongDoubleSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<decimal>).IsAssignableFrom(data.UnboundedLongDoubleSequenceField.GetType()));

            Assert.IsNotNull(defaultStruct.BoundedBooleanSequenceField);
            Assert.AreEqual(defaultStruct.BoundedBooleanSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedBooleanSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedBooleanSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedCharSequenceField);
            Assert.AreEqual(defaultStruct.BoundedCharSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedCharSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedCharSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedWCharSequenceField);
            Assert.AreEqual(defaultStruct.BoundedWCharSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedWCharSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedWCharSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedOctetSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedOctetSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedOctetSequenceField);
            Assert.AreEqual(defaultStruct.BoundedOctetSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedShortSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedShortSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedShortSequenceField);
            Assert.AreEqual(defaultStruct.BoundedShortSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedUShortSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedUShortSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedUShortSequenceField);
            Assert.AreEqual(defaultStruct.BoundedUShortSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedLongSequenceField);
            Assert.AreEqual(defaultStruct.BoundedLongSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedLongSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedLongSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedULongSequenceField);
            Assert.AreEqual(defaultStruct.BoundedULongSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedULongSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedULongSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedLongLongSequenceField);
            Assert.AreEqual(defaultStruct.BoundedLongLongSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedLongLongSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedLongLongSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedULongLongSequenceField);
            Assert.AreEqual(defaultStruct.BoundedULongLongSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedULongLongSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedULongLongSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedFloatSequenceField);
            Assert.AreEqual(defaultStruct.BoundedFloatSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedFloatSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedFloatSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedDoubleSequenceField);
            Assert.AreEqual(defaultStruct.BoundedDoubleSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedDoubleSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedDoubleSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedLongDoubleSequenceField);
            Assert.AreEqual(defaultStruct.BoundedLongDoubleSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedLongDoubleSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedLongDoubleSequenceField.Count, 0);
        }

        [TestMethod, TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStringTypes()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                UnboundedStringField = "Hello, I love you, won't you tell me your name?",
                UnboundedWStringField = "She's walking down the street\nBlind to every eye she meets\nDo you think you'll be the guy\nTo make the queen of the angels sigh?",
                BoundedStringField = "Hello, I love you, won't you tell me your name?",
                BoundedWStringField = "She's walking down the street\nBlind to every eye she meets\nDo you think you'll be the guy\nTo make the queen of the angels sigh?"
            };
            _dataWriter.Write(data);

            // TODO: Wait for acknowledgments
            System.Threading.Thread.Sleep(500);

            TestStruct received = new TestStruct();
            var ret = _dataReader.ReadNextSample(received);

            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.AreEqual(data.UnboundedStringField, received.UnboundedStringField);
            Assert.AreEqual(data.UnboundedWStringField, received.UnboundedWStringField);
            // TODO: I would expect ".Substring(0, 32)" is received but it seems the whole string is transported. 
            // Check with OpenDDS test and report for clarification if the behaviour is confirmed.
            Assert.AreEqual(data.BoundedStringField, received.BoundedStringField);
            Assert.AreEqual(data.BoundedWStringField, received.BoundedWStringField);

            Assert.AreEqual(typeof(string), data.UnboundedStringField.GetType());
            Assert.AreEqual(typeof(string), data.UnboundedWStringField.GetType());
            Assert.AreEqual(typeof(string), data.BoundedStringField.GetType());
            Assert.AreEqual(typeof(string), data.BoundedWStringField.GetType());

            Assert.AreEqual(defaultStruct.UnboundedStringField, string.Empty);
            Assert.AreEqual(defaultStruct.UnboundedWStringField, string.Empty);
            Assert.AreEqual(defaultStruct.BoundedStringField, string.Empty);
            Assert.AreEqual(defaultStruct.BoundedWStringField, string.Empty);
        }

        [TestMethod, TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStringSequences()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                BoundedStringSequenceField =
                {
                    "Pressure pushing down on me",
                    "Pressing down on you, no man ask for",
                    "Under pressure that burns a building down",
                    "Splits a family in two",
                    "Puts people on streets"
                },
                UnboundedStringSequenceField =
                {
                    "You've got your mother in a whirl",
                    "She's not sure if you're a boy or a girl",
                    "Hey babe! your hair's alright",
                    "Hey babe! let's go out tonight",
                    "You like me, and I like it all",
                    "We like dancing and we look divine",
                    "You love bands when they're playing hard",
                    "You want more and you want it fast",
                    "They put you down, they say I'm wrong",
                    "You tacky thing, you put them on"
                },
                BoundedWStringSequenceField =
                {
                    "Rebel Rebel, you've turn your dress",
                    "Rebel Rebel, your face is a mess",
                    "Rebel Rebel, how could they know?",
                    "Hot tramp, I love you so!"
                },
                UnboundedWStringSequenceField =
                {
                    "Well, you've got your diamonds",
                    "And you've got your pretty clothes",
                    "And the chauffer drives your car",
                    "You let everybody know",
                    "But don't play with me,",
                    "'cause you're playing with fire",
                }
            };

            _dataWriter.Write(data);

            // TODO: Wait for acknowledgments
            System.Threading.Thread.Sleep(500);

            TestStruct received = new TestStruct();
            var ret = _dataReader.ReadNextSample(received);

            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.IsTrue(data.BoundedStringSequenceField.SequenceEqual(received.BoundedStringSequenceField));
            Assert.IsTrue(data.UnboundedStringSequenceField.SequenceEqual(received.UnboundedStringSequenceField));
            Assert.IsTrue(data.BoundedWStringSequenceField.SequenceEqual(received.BoundedWStringSequenceField));
            Assert.IsTrue(data.UnboundedWStringSequenceField.SequenceEqual(received.UnboundedWStringSequenceField));

            Assert.IsTrue(typeof(IList<string>).IsAssignableFrom(data.BoundedStringSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<string>).IsAssignableFrom(data.UnboundedStringSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<string>).IsAssignableFrom(data.BoundedWStringSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<string>).IsAssignableFrom(data.UnboundedWStringSequenceField.GetType()));

            Assert.IsNotNull(defaultStruct.BoundedStringSequenceField);
            Assert.AreEqual(defaultStruct.BoundedStringSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedStringSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedStringSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.BoundedWStringSequenceField);
            Assert.AreEqual(defaultStruct.BoundedWStringSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedWStringSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedWStringSequenceField.Count, 0);
        }

        [TestMethod, TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStructuresTypes()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                NestedStructField = new NestedStruct { Id = 1, Message = "Do androids dream of electric sheep?" }
            };

            _dataWriter.Write(data);

            // TODO: Wait for acknowledgments
            System.Threading.Thread.Sleep(500);

            TestStruct received = new TestStruct();
            var ret = _dataReader.ReadNextSample(received);

            Assert.AreEqual(ReturnCode.Ok, ret);
            Assert.IsNotNull(received.NestedStructField);
            Assert.AreEqual(data.NestedStructField.Id, received.NestedStructField.Id);
            Assert.AreEqual(data.NestedStructField.Message, received.NestedStructField.Message);

            Assert.AreEqual(typeof(NestedStruct), data.NestedStructField.GetType());

            Assert.IsNotNull(defaultStruct.NestedStructField);
            Assert.AreEqual(0, defaultStruct.NestedStructField.Id);
            Assert.AreEqual(string.Empty, defaultStruct.NestedStructField.Message);
        }

        [TestMethod, TestCategory(TEST_CATEGORY)]
        public void TestGeneratedStructureSequences()
        {
            TestStruct defaultStruct = new TestStruct();

            TestStruct data = new TestStruct
            {
                UnboundedStructSequenceField =
                {
                    new NestedStruct { Id = 1, Message = "With your feet in the air and your head on the ground" },
                    new NestedStruct { Id = 2, Message = "Try this trick and spin it, yeah" },
                    new NestedStruct { Id = 3, Message = "Your head will collapse" },
                    new NestedStruct { Id = 4, Message = "But there's nothing in it" },
                    new NestedStruct { Id = 5, Message = "And you'll ask yourself" },
                    new NestedStruct { Id = 6, Message = "Where is my mind?" },
                },
                BoundedStructSequenceField =
                {
                    new NestedStruct { Id = 1, Message = "With your feet in the air and your head on the ground" },
                    new NestedStruct { Id = 2, Message = "Try this trick and spin it, yeah" },
                    new NestedStruct { Id = 3, Message = "Your head will collapse" },
                    new NestedStruct { Id = 4, Message = "But there's nothing in it" },
                    new NestedStruct { Id = 5, Message = "And you'll ask yourself" }
                }
            };

            _dataWriter.Write(data);

            // TODO: Wait for acknowledgments
            System.Threading.Thread.Sleep(500);

            TestStruct received = new TestStruct();
            var ret = _dataReader.ReadNextSample(received);

            Assert.AreEqual(ReturnCode.Ok, ret);
            for (int i = 1; i < data.BoundedStructSequenceField.Count; i++)
            {
                Assert.AreEqual(data.BoundedStructSequenceField[i].Id, received.BoundedStructSequenceField[i].Id);
                Assert.AreEqual(data.BoundedStructSequenceField[i].Message, received.BoundedStructSequenceField[i].Message);
            }
            for (int i = 1; i < data.BoundedStructSequenceField.Count; i++)
            {
                Assert.AreEqual(data.UnboundedStructSequenceField[i].Id, received.UnboundedStructSequenceField[i].Id);
                Assert.AreEqual(data.UnboundedStructSequenceField[i].Message, received.UnboundedStructSequenceField[i].Message);
            }            

            Assert.IsTrue(typeof(IList<NestedStruct>).IsAssignableFrom(data.BoundedStructSequenceField.GetType()));
            Assert.IsTrue(typeof(IList<NestedStruct>).IsAssignableFrom(data.UnboundedStructSequenceField.GetType()));

            Assert.IsNotNull(defaultStruct.BoundedStructSequenceField);
            Assert.AreEqual(defaultStruct.BoundedStructSequenceField.Count, 0);
            Assert.IsNotNull(defaultStruct.UnboundedStructSequenceField);
            Assert.AreEqual(defaultStruct.UnboundedStructSequenceField.Count, 0);
        }
        #endregion
    }
}
