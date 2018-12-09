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
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace ConsoleDemoCore
{
    class Program
    {
        static void Main(string[] args)
        {
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

            BasicTestStructTypeSupport typeSupport = new BasicTestStructTypeSupport();
            string typeName = typeSupport.GetTypeName();
            ReturnCode ret = typeSupport.RegisterType(participant, typeName);
            if (ret != ReturnCode.Ok)
            {
                throw new ApplicationException("BasicTestStructTypeSupport could not be registered: " + ret.ToString());
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
            BasicTestStructDataReader dataReader = new BasicTestStructDataReader(dr);

            DataWriter dw = publisher.CreateDataWriter(topic);
            if (dw == null)
            {
                throw new ApplicationException("DataWriter could not be created.");
            }
            BasicTestStructDataWriter dataWriter = new BasicTestStructDataWriter(dw);

            System.Threading.Thread.Sleep(500);

            BasicTestStruct data = new BasicTestStruct
            {
                Id = 1,
                Message = "Hello, I love you, won't you tell me your name?",
                WMessage = "She's walking down the street\nBlind to every eye she meets\nDo you think you'll be the guy\nTo make the queen of the angels sigh?",
                LongSequence = new List<int> { 1, 2, 3, 100 },
                StringSequence = new List<string>
                {
                    "In every life we have some trouble, ",
                    "but when you worry you make it double. ",
                    "Don't worry. ",
                    "Be happy. ",
                    "Be happy now."
                }
            };
            dataWriter.Write(data);

            System.Threading.Thread.Sleep(500);

            //ret = dataReader.Read();
            //if (ret == ReturnCode.Ok)
            //{
            //    Console.WriteLine("Ok");
            //}
            //else
            //{
            //    Console.WriteLine("Data not received. Read call returned: " + ret);
            //}
            
            BasicTestStruct received = new BasicTestStruct();
            ret = dataReader.ReadNextSample(received);
            if (ret == ReturnCode.Ok)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Id:");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(received.Id);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Message:");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(received.Message);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("WMessage:");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(received.WMessage);
                Console.WriteLine();

                if (received.LongSequence.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(nameof(received.LongSequence) + ":");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    for (int i = 0; i < received.LongSequence.Count; i++)
                    {
                        Console.Write(received.LongSequence[i]);
                        if (i < received.LongSequence.Count - 1)
                        {
                            Console.Write(", ");
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                }

                if (received.StringSequence.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(nameof(received.StringSequence) + ":");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    for (int i = 0; i < received.StringSequence.Count; i++)
                    {
                        Console.WriteLine(received.StringSequence[i]);
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Data not received. ReadNextSample call returned: " + ret.ToString());
            }

            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
