using System;
using System.Text;
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using System.Collections.Generic;

namespace ConsoleDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            bool useListener = true;

            ParticipantService participantService = ParticipantService.Instance;
            DomainParticipantFactory domainFactory = participantService.GetDomainParticipantFactory(args);
            DomainParticipantQos qos = new DomainParticipantQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.UserData.Value = Encoding.UTF8.GetBytes("sometext");
            DomainParticipant participant = domainFactory.CreateParticipant(42, qos);
            if (participant == null)
            {
                throw new Exception("Could not create the participant");
            }

            DomainParticipantQos aux = new DomainParticipantQos();
            ReturnCode ret = participant.GetQos(aux);
            aux.EntityFactory.AutoenableCreatedEntities = true;
            ret = participant.SetQos(aux);
            
            if (participant != null)
            {                
                TestStructTypeSupport support = new TestStructTypeSupport();
                string typeName = support.GetTypeName();
                ReturnCode result = support.RegisterType(participant, typeName);
                if (result != ReturnCode.Ok)
                {
                    throw new Exception("Could not register the type");
                }

                Topic topic = participant.CreateTopic("TopicName", typeName);
                Publisher publisher = participant.CreatePublisher();
                if (publisher == null)
                {
                    throw new Exception("Could not create the publisher");
                }

                DataWriter dw = publisher.CreateDataWriter(topic);
                if (dw == null)
                {
                    throw new Exception("Could not create the datawriter");
                }
                TestStructDataWriter dataWriter = new TestStructDataWriter(dw);                    
                Subscriber subscriber = participant.CreateSubscriber();
                if (subscriber == null)
                {
                    throw new Exception("Could not create the subscribre");
                }

                MyDataListener listener = null;
                if (useListener)
                    listener = new MyDataListener();
                DataReader dataReader = subscriber.CreateDataReader(topic, listener, StatusKind.DataAvailableStatus);
                if (dataReader == null)
                {
                    throw new Exception("Could not create the datareader");
                }

                WaitSet waitSet = null;
                StatusCondition statusCondition = null;
                if (!useListener)
                {
                    waitSet = new WaitSet();
                    
                    waitSet.AttachCondition(dataReader.StatusCondition);
                    statusCondition.SetEnabledStatuses(StatusKind.DataAvailableStatus);

                    new System.Threading.Thread(delegate ()
                    {
                        ICollection<Condition> conditions = new List<Condition>();
                        Duration duration = new Duration
                        {
                            Seconds = Duration.DurationInfiniteSec
                        };
                        waitSet.Wait(conditions, duration);

                        foreach (Condition cond in conditions)
                        {
                            if (cond == statusCondition && cond.TriggerValue)
                            {
                                StatusCondition sCond = (StatusCondition)cond;
                                StatusMask mask = sCond.GetEnabledStatuses();
                                if ((mask & StatusKind.DataAvailableStatus) != 0)
                                    DataAvailable(dataReader);
                            }
                        }

                    }).Start();
                }

                TestStruct test = new TestStruct
                {
                    RawData = "Hello, I love you, won't you tell me your name?"
                };

                test.LongSequence.Add(20);
                test.LongSequence.Add(10);
                test.LongSequence.Add(0);

                test.StringSequence.Add("Hello,");
                test.StringSequence.Add("I love you");
                test.StringSequence.Add("won't you tell me your name?");

                test.LongDoubleType = 1.1;

                test.LongDoubleSequence.Add(1.1);
                test.LongDoubleSequence.Add(2.2);
                test.LongDoubleSequence.Add(3.3);

                test.LongArray[0, 0] = 1;
                test.LongArray[0, 1] = 2;
                test.LongArray[0, 2] = 3;
                test.LongArray[0, 3] = 4;
                test.LongArray[1, 0] = 1;
                test.LongArray[1, 1] = 2;
                test.LongArray[1, 2] = 3;
                test.LongArray[1, 3] = 4;
                test.LongArray[2, 0] = 1;
                test.LongArray[2, 1] = 2;
                test.LongArray[2, 2] = 3;
                test.LongArray[2, 3] = 4;

                test.StringArray[0, 0] = "Hello,";
                test.StringArray[0, 1] = "I love you,";
                test.StringArray[1, 0] = "won't you tell me";
                test.StringArray[1, 1] = "your name?";

                test.StructArray[0, 0] = new BasicTestStruct() { Id = 0 };
                test.StructArray[0, 1] = new BasicTestStruct() { Id = 1 };
                test.StructArray[1, 0] = new BasicTestStruct() { Id = 2 };
                test.StructArray[1, 1] = new BasicTestStruct() { Id = 3 };

                test.LongDoubleArray[0, 0] = 1.1;
                test.LongDoubleArray[0, 1] = 2.2;
                test.LongDoubleArray[1, 0] = 3.3;
                test.LongDoubleArray[1, 1] = 4.4;

                test.StructSequence.Add(new BasicTestStruct()
                {
                    Id = 1
                });

                test.StructSequence.Add(new BasicTestStruct()
                {
                    Id = 2
                });

                test.StructSequence.Add(new BasicTestStruct()
                {
                    Id = 3
                });

                result = dataWriter.Write(test);

                System.Threading.Thread.Sleep(1000);

                if (!useListener)
                    waitSet.DetachCondition(statusCondition);                    
                

                
                participant.DeleteContainedEntities();
                domainFactory.DeleteParticipant(participant);
            }            
            
            participantService.Shutdown();

            Console.WriteLine("Press ENTER to finish the test.");
            Console.ReadLine();
        }

        public static void DataAvailable(DataReader reader)
        {
            try
            {
                TestStructDataReader dr = new TestStructDataReader(reader);
                
                //SampleInfo info = new SampleInfo();
                //TestStruct sample = new TestStruct();
                //ReturnCode error = dr.TakeNextSample(sample, info);

                List<TestStruct> samples = new List<TestStruct>();
                List<SampleInfo> infos = new List<SampleInfo>();
                ReturnCode error = dr.Read(samples, infos);
                if ((error == ReturnCode.Ok))
                {
                    for (int i = 0; i < samples.Count; i++)
                    {
                        SampleInfo info = infos[i];
                        TestStruct sample = samples[i];
                        if (info.ValidData)
                        {
                            Console.WriteLine();

                            Console.WriteLine("RawData:");
                            Console.WriteLine(sample.RawData);
                            Console.WriteLine();

                            Console.WriteLine("LongDoubleType:");
                            Console.WriteLine(sample.LongDoubleType);
                            Console.WriteLine();

                            Console.WriteLine("LongSequence:");
                            foreach (int l in sample.LongSequence)
                            {
                                Console.WriteLine(l);
                            }
                            Console.WriteLine();

                            Console.WriteLine("StringSequence:");
                            foreach (string s in sample.StringSequence)
                            {
                                Console.WriteLine(s);
                            }
                            Console.WriteLine();

                            Console.WriteLine("StructSequence:");
                            foreach (BasicTestStruct s in sample.StructSequence)
                            {
                                Console.WriteLine(s.Id);
                            }
                            Console.WriteLine();

                            Console.WriteLine("LongDoubleSequence:");
                            foreach (double ld in sample.LongDoubleSequence)
                            {
                                Console.WriteLine(ld);
                            }
                            Console.WriteLine();

                            Console.WriteLine("LongArray:");
                            foreach (int l in sample.LongArray)
                            {
                                Console.WriteLine(l);
                            }
                            Console.WriteLine();

                            Console.WriteLine("StringArray:");
                            foreach (string s in sample.StringArray)
                            {
                                Console.WriteLine(s);
                            }
                            Console.WriteLine();

                            Console.WriteLine("StructArray:");
                            foreach (BasicTestStruct s in sample.StructArray)
                            {
                                Console.WriteLine(s.Id);
                            }
                            Console.WriteLine();

                            Console.WriteLine("LongDoubleArray:");
                            foreach (double d in sample.LongDoubleArray)
                            {
                                Console.WriteLine(d);
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.Write(ex);
            }
        }
    }

    

    public class MyDataListener : DataReaderListener
    {        
        public override void OnDataAvailable(DataReader reader)
        {
            Program.DataAvailable(reader);            
        }

        public override void OnLivelinessChanged(DataReader reader, LivelinessChangedStatus status)
        {
            Console.WriteLine(nameof(OnLivelinessChanged));
        }

        public override void OnRequestedDeadlineMissed(DataReader reader, RequestedDeadlineMissedStatus status)
        {
            Console.WriteLine(nameof(OnRequestedDeadlineMissed));
        }

        public override void OnRequestedIncompatibleQos(DataReader reader, RequestedIncompatibleQosStatus status)
        {
            Console.WriteLine(nameof(OnRequestedIncompatibleQos));
        }

        public override void OnSampleLost(DataReader reader, SampleLostStatus status)
        {
            Console.WriteLine(nameof(OnSampleLost));
        }

        public override void OnSampleRejected(DataReader reader, SampleRejectedStatus status)
        {
            Console.WriteLine(nameof(OnSampleRejected));
        }

        public override void OnSubscriptionDisconnected(DataReader reader, SubscriptionDisconnectedStatus status)
        {
            Console.WriteLine(nameof(OnSubscriptionDisconnected));
        }

        public override void OnSubscriptionLost(DataReader reader, SubscriptionLostStatus status)
        {
            Console.WriteLine(nameof(OnSubscriptionLost));
        }

        public override void OnSubscriptionMatched(DataReader reader, SubscriptionMatchedStatus status)
        {
            Console.WriteLine(nameof(OnSubscriptionMatched));
        }

        public override void OnSubscriptionReconnected(DataReader reader, SubscriptionReconnectedStatus status)
        {
            Console.WriteLine(nameof(OnSubscriptionReconnected));
        }

        public override void OnBudgetExceeded(DataReader reader, BudgetExceededStatus status)
        {
            Console.WriteLine(nameof(OnBudgetExceeded));
        }

        public override void OnConnectionDeleted(DataReader reader)
        {
            Console.WriteLine(nameof(OnConnectionDeleted));
        }
    }
}
