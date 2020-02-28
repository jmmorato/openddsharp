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
using Test;
using System;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp;

namespace ConsoleDemoCore
{
    class Program
    {
        static void Main()
        {
            Ace.Init();

            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps.ini");
            if (dpf == null)
            {
                throw new ApplicationException("Domain participant factory could not be created.");
            }

            DomainParticipant participant = dpf.CreateParticipant(42);
            if (dpf == null)
            {
                throw new ApplicationException("Domain participant could not be created.");
            }

            Publisher publisher = participant.CreatePublisher();
            if (publisher == null)
            {
                throw new ApplicationException("Publisher could not be created.");
            }

            Subscriber subscriber = participant.CreateSubscriber();
            if (subscriber == null)
            {
                throw new ApplicationException("Subscriber could not be created.");
            }

            TestStandard(participant, publisher, subscriber);

            ParticipantService.Instance.Shutdown();
            Ace.Fini();
        }

        private static void TestStandard(DomainParticipant participant, Publisher publisher, Subscriber subscriber)
        {
            TestStructTypeSupport typeSupport = new TestStructTypeSupport();
            string typeName = typeSupport.GetTypeName();
            ReturnCode ret = typeSupport.RegisterType(participant, typeName);
            if (ret != ReturnCode.Ok)
            {
                throw new ApplicationException("TestStructTypeSupport could not be registered: " + ret.ToString());
            }

            Topic topic = participant.CreateTopic("TestTopic", typeName);
            if (topic == null)
            {
                throw new ApplicationException("Topic could not be created.");
            }

            DataReader dr = subscriber.CreateDataReader(topic);
            if (dr == null)
            {
                throw new ApplicationException("DataReader could not be created.");
            }
            TestStructDataReader dataReader = new TestStructDataReader(dr);

            DataWriter dw = publisher.CreateDataWriter(topic);
            if (dw == null)
            {
                throw new ApplicationException("DataWriter could not be created.");
            }
            TestStructDataWriter dataWriter = new TestStructDataWriter(dw);

            System.Threading.Thread.Sleep(500);

            TestStruct data = new TestStruct
            {
                ShortField = -1,
                LongField = -2,
                LongLongField = -3,
                UnsignedShortField = 1,
                UnsignedLongField = 2,
                UnsignedLongLongField = 3,
            };

            dataWriter.Write(data);

            System.Threading.Thread.Sleep(500);

            TestStruct received = new TestStruct();
            ret = dataReader.ReadNextSample(received);
            if (ret == ReturnCode.Ok)
            {
                #region Integer Types
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(nameof(received.ShortField) + ":");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(received.ShortField);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(nameof(received.LongField) + ":");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(received.LongField);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(nameof(received.LongLongField) + ":");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(received.LongLongField);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(nameof(received.UnsignedShortField) + ":");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(received.UnsignedShortField);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(nameof(received.UnsignedLongField) + ":");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(received.UnsignedLongField);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(nameof(received.UnsignedLongLongField) + ":");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(received.UnsignedLongLongField);
                Console.WriteLine();
                #endregion
            }

            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
