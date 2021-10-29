# Getting Started

The full code of this example can be found in this [GitHub repository](https://github.com/jmmorato/openddsharp_standard_helloworld).

## Requirements

In order to follow this tutorial, you will need to install in your computer:
- Microsoft Visual Studio 2019 (v16.7 or greater)
- Microsoft [dotnet 3.1 SDK](https://dotnet.microsoft.com/download/dotnet/3.1) or greater

## Install the OpenDDSharp Templates

OpenDDSharp provides .NET Core project templates in order to ease the process of creating OpenDDSharp applications.
Run the following command to install OpenDDSharp templates in your computer from the [NuGet.org feed](https://www.nuget.org/packages/OpenDDSharp.Templates/0.8.21289.1-beta):

`dotnet new --install OpenDDSharp.Templates::0.8.21289.1-beta`

For more information on how manage .NET Core project and item templates visit the [microsoft documentation](https://docs.microsoft.com/en-us/dotnet/core/install/templates).

## Create the IDL project

From your solution folder, execute the following command:

`dotnet new openddsharp-idl-project --name TestMessage --output TestMessage`

A new C# IDL project will be created in the `TestMessage` folder; add it to your solution and modify the `IDL/Test.idl` file content:

```idl
module HelloWorld{
  @topic
  struct Message {
    string Content;
  };
};
```

Build the project and the C# code will be auto-generated and compiled in the `TestTypeSupport.cs` file.

## HelloWorld Publisher

### Create the publisher project

// TODO

### Create the DDS DomainParticipant

The first entity to be created in a DDS application is the domain participant. 
The domain participant is created with the domain participant factory and it is the factory for the DDS topics, 
publishers and subscribers. The following code shows how to create a domain participant in the domain 42:

```csharp
Ace.Init();
DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps.ini");
DomainParticipant participant = dpf.CreateParticipant(42);
if (participant == null)
{
    throw new Exception("Could not create the participant");
}
```

### Create the DDS Topic

Before create the topic, it is mandatory to register the type that is going to be used to share the data in that topic. 
We are going to use the `Message` structure defined in the IDL project. 
The following code shows how to register the `Message` type in the previously created domain participant:

```csharp
MessageTypeSupport support = new MessageTypeSupport();
ReturnCode result = support.RegisterType(participant, support.GetTypeName());
if (result != ReturnCode.Ok)
{
    throw new Exception("Could not register type: " + result.ToString());
}
```

Now, we can create the DDS topic entity where the messages will be shared with the subscribers:

```csharp
Topic topic = participant.CreateTopic("MessageTopic", support.GetTypeName());
if (topic == null)
{
    throw new Exception("Could not create the message topic");
}
```

### Create the DDS Publisher and the DDS DataWriter

The publisher entity is the factory for the datawriter entities. 
The datawriter entity is in charge of writting the data in the related topic. 
You can group several datawriter entities in the same publisher entity. 
In this example we only need one publisher and one datawriter related with the `MessageTopic` created previously. 
The following code shows how to create the publisher entity:

```csharp
Publisher publisher = participant.CreatePublisher();
if (publisher == null)
{
    throw new Exception("Could not create the publisher");
}
```

To create the datawriter first we need to create a generic datawriter related with the topic and after that a specific datawriter for the registered type of the topic. 
The following code shows how to create a datawriter for the `MessageTopic`:

```csharp
DataWriter writer = publisher.CreateDataWriter(topic);
if (writer == null)
{
    throw new Exception("Could not create the data writer");
}
MessageDataWriter messageWriter = new MessageDataWriter(writer);
```

### Wait for a subscription before write the message

The default QoS configuration for the datawriter use a volatile durability. 
Only the datareaders matched with the datawriters when the data is written will receive the message. 
The following code shows how to ensure that at least one datareader is present before we write the data:

```csharp
Console.WriteLine("Waiting for a subscriber...");
PublicationMatchedStatus status = new PublicationMatchedStatus();
do
{
    writer.GetPublicationMatchedStatus(ref status);
    System.Threading.Thread.Sleep(500);
}
while (status.CurrentCount < 1);
```

Now that we are sure that at least one datareader is present in the system we can write the message to the topic:

```csharp
Console.WriteLine("Subscriber found, writting data....");
messageWriter.Write(new Message
{
    Content = "Hello, I love you, won't you tell me your name?"
});

Console.WriteLine("Press a key to exit...");
Console.ReadKey();
```

### Shutdown the application

Before exit the application you should release all the resources used by DDS. The following code shows how to release all the entities previously created:

```csharp
participant.DeleteContainedEntities();
dpf.DeleteParticipant(participant);
ParticipantService.Instance.Shutdown();
Ace.Fini();
```

## HelloWorld Subscriber

### Create the subscriber project

// TODO

### Create the domain participant, register the type and create the topic

You need to create the same enities as in the publisher project. Pay attention to use the same domain id (42) and the same topic name (`MessageTopic`) used in the publisher application.

### Create the DDS Subscriber and the DDS DataReader

The subscriber entity is the factory for the datareader entities. 
The datareader entity is in charge of reading the data in the related topic. 
You can group several datareader entities in the same subscriber entity. 
In this example we only need one subscriber and one datareader related with the `MessageTopic` created previously. 
The following code shows how to create the subscriber entity:

```csharp
Subscriber subscriber = participant.CreateSubscriber();
if (subscriber == null)
{
    throw new Exception("Could not create the subscriber");
}
```

To create the datareader first we need to create a generic datareader related with the topic and after that a specific datareader for the registered type of the topic. 
The following code shows how to create a datareader for the `MessageTopic`:

```csharp
DataReader reader = subscriber.CreateDataReader(topic);
if (reader == null)
{
    throw new Exception("Could not create the message data reader");
}
MessageDataReader messageReader = new MessageDataReader(reader);
```

### Receive the message

There are different ways to read the information from the topic, listeners, waitsets or simply polling the information from the datareader. 
For this first application we are going to poll the information from the datareader. The following code shows how to do it:

```csharp
while (true)
{
    StatusMask mask = messageReader.StatusChanges;
    if ((mask & StatusKind.DataAvailableStatus) != 0)
    {
        List<Message> receivedData = new List();
        List<SampleInfo> receivedInfo = new List();
        result = messageReader.Take(receivedData, receivedInfo);

        if (result == ReturnCode.Ok)
        {
            bool messageReceived = false;
            for (int i = 0; i < receivedData.Count; i++)
            {
                if (receivedInfo[i].ValidData)
                {
                    Console.WriteLine(receivedData[i].Content);
                    messageReceived = true;
                }
            }

            if (messageReceived)
                break;
        }
    }

    System.Threading.Thread.Sleep(100);
}
```

### Shutdown the application

Same as with the publisher application, you should release all the resources used by DDS. You can use the same code as before:

```csharp
participant.DeleteContainedEntities();
dpf.DeleteParticipant(participant);
ParticipantService.Instance.Shutdown();
Ace.Fini();
```