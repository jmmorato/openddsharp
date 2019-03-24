using System;
using OpenDDSharp.DDS;
using OpenDDSharp.HelloWorld;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.HelloWorldPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Ace.Init();

            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps.ini");
            DomainParticipant participant = dpf.CreateParticipant(42);
            if (participant == null)
            {
                throw new Exception("Could not create the participant");
            }

            MessageTypeSupport support = new MessageTypeSupport();
            ReturnCode result = support.RegisterType(participant, support.GetTypeName());
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Could not register type: " + result.ToString());
            }

            Topic topic = participant.CreateTopic("MessageTopic", support.GetTypeName());
            if (topic == null)
            {
                throw new Exception("Could not create the message topic");
            }

            Publisher publisher = participant.CreatePublisher();
            if (publisher == null)
            {
                throw new Exception("Could not create the publisher");
            }

            DataWriter writer = publisher.CreateDataWriter(topic);
            if (writer == null)
            {
                throw new Exception("Could not create the data writer");
            }
            MessageDataWriter messageWriter = new MessageDataWriter(writer);

            Console.WriteLine("Waiting for a subscriber...");
            PublicationMatchedStatus status = new PublicationMatchedStatus();
            do
            {
                result = messageWriter.GetPublicationMatchedStatus(ref status);
                System.Threading.Thread.Sleep(500);
            }
            while (status.CurrentCount < 1);

            Console.WriteLine("Subscriber found, writting data....");
            messageWriter.Write(new Message
            {
                Content = "Hello, I love you, won't you tell me your name?"
            });

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();

            participant.DeleteContainedEntities();
            dpf.DeleteParticipant(participant);
            ParticipantService.Instance.Shutdown();

            Ace.Fini();
        }
    }
}
