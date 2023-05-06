# Getting Started

The full code of this example can be found in this [GitHub repository](https://github.com/jmmorato/openddsharp_standard_helloworld).

## Requirements

In order to follow this tutorial, you need to install in your computer:
- Microsoft [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or greater
- CMake [Version 3.8.2](https://cmake.org/download/) or greater
- Perl, on Windows system is recommended to use [Strawberry Perl](https://strawberryperl.com/)
- For Apple ARM64 systems, you must install Rosetta 2 (`softwareupdate --install-rosetta`)

## Install the OpenDDSharp Templates

OpenDDSharp provides .NET project templates in order to ease the process of creating OpenDDSharp applications.

Run the following command to install OpenDDSharp templates in your computer from the [NuGet.org feed](https://www.nuget.org/packages/OpenDDSharp.Templates/0.8.23023.106):

`dotnet new --install OpenDDSharp.Templates::0.8.23023.106`

Two new templates will be added to the dotnet templates system, `openddsharp-idl-project` and `openddsharp-console-app`,
that can be used to create the projects for your OpenDDSharp solution.

For more information on how manage .NET project and item templates visit the [microsoft documentation](https://docs.microsoft.com/en-us/dotnet/core/install/templates).

## Create the IDL project

Applications that use the Data Distribution Service (DDS) define the data types in a way that is independent of
the programming language or operating system/processor platform by using the Interface Definition Language (IDL).

OpenDDSharp generates the C# code based on the [IDL4 to C# Language Mapping specification](https://www.omg.org/spec/IDL4-CSHARP/)
using his owns code generation tool. Check this [link](idl.md) to get more information about the IDL language and the current implementation of the specification.

Execute the following command in order to create your OpenDDSharp IDL project:

`dotnet new openddsharp-idl-project --name TestMessage --output TestMessage`

A new C# IDL project will be created in the `TestMessage` folder, where you can define your own data types using the Interface Definition Language.

For the current example, modify the `IDL/TestMessage.idl` file content as follow:

```idl
module HelloWorld{
  @topic
  struct Message {
    string Content;
  };
};
```

Build the project to auto-generate and compile the C# code for the defined structure:

`dotnet build TestMessage/TestMessage.csproj --runtime <runtime_identifier> --configuration <Release|Debug>`

> [!NOTE]
> The implemented runtime identifiers are:
> - win-x64
> - win-x86
> - linux-x64
> - osx-x64

## HelloWorld Publisher

### Create the publisher project

Once you have defined your data transport types in the IDL project, it's time to create your first publisher application.

Using the following command, you will create a console application that includes the OpenDDSharp reference, some boilerplate code
and the configuration required to initialize your domain participant:

`dotnet new openddsharp-console-app --name HelloWorldPublisher --output HelloWorldPublisher`

You should add a reference to the previously created data transport types project to be able to use them in your publisher application.
You can do it with the following command:

`dotnet add HelloWorldPublisher/HelloWorldPublisher.csproj reference TestMessage/TestMessage.csproj`

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

> [!NOTE]
> This code snippet is already included in the boilerplate if you created the application with the template.
> Feel free to change the domain id or the configuration file in order to fit the requirements of your application.

### Create the DDS Topic

To allow data to flow around the system first you need to define your topics. You can define multiple topics in the same domain,
each topic consists in a topic name and the topic type that will be used to share information.

Before creating a topic, you must to register the type that is going to be used to share the data in that topic.

We are going to use the `Message` structure defined in the IDL project. The following code shows how to register the `Message` type
in the previously created domain participant:

```csharp
MessageTypeSupport support = new MessageTypeSupport();
ReturnCode result = support.RegisterType(participant, support.GetTypeName());
if (result != ReturnCode.Ok)
{
    throw new Exception("Could not register type: " + result.ToString());
}
```

After the type is registered, we can create the DDS topic entity where the messages will be shared with the subscribers:

```csharp
Topic topic = participant.CreateTopic("MessageTopic", support.GetTypeName());
if (topic == null)
{
    throw new Exception("Could not create the message topic");
}
```

### Create the DDS Publisher and the DDS DataWriter

The publisher entity is the factory for the datawriter entities. 
The datawriter entity is in charge of writing the data in the related topic. 
You can group several datawriter entities in the same publisher entity. 
In this example we only need one publisher and one datawriter related with the `MessageTopic` created previously. 
The following code shows how to create the publisher entity:

```csharp
OpenDDSharp.DDS.Publisher publisher = participant.CreatePublisher();
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
The following code shows how to ensure that at least one datareader is present before writing the data:

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
Console.WriteLine("Subscriber found, writing data....");
messageWriter.Write(new Message
{
    Content = "Hello, I love you, won't you tell me your name?"
});

Console.WriteLine("Press a key to exit...");
Console.ReadKey();
```

### Shutdown the application

Before exiting the application you should release all the resources used by DDS. The following code shows how to release all the entities previously created:

```csharp
participant.DeleteContainedEntities();
dpf.DeleteParticipant(participant);
ParticipantService.Instance.Shutdown();
Ace.Fini();
```

> [!NOTE]
> This code snippet is already included in the boilerplate if you created the application with the template.

## HelloWorld Subscriber

### Create the subscriber project

Same than with the publisher application, you can create the subscriber application and reference the data transport types project using the OpenDDSharp console template by using the following commands:

`dotnet new openddsharp-console-app --name HelloWorldSubscriber --output HelloWorldSubscriber`

`dotnet add HelloWorldSubscriber/HelloWorldSubscriber.csproj reference TestMessage/TestMessage.csproj`

### Create the domain participant, register the type and create the topic

You need to create the same entities as in the publisher project. Pay attention to use the same domain id (42) and
the same topic name(`MessageTopic`) used in the publisher application.

### Create the DDS Subscriber and the DDS DataReader

The subscriber entity is the factory for the datareader entities. The datareader entity is in charge of reading the data in the related topic. 
You can group several datareader entities in the same subscriber entity. In this example we only need one subscriber and one datareader 
related with the `MessageTopic` created previously. The following code shows how to create the subscriber entity:

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
        List<Message> receivedData = new List<Message>();
        List<SampleInfo> receivedInfo = new List<SampleInfo>();
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

Same as with the publisher application, you should release all the resources used by DDS.

```csharp
participant.DeleteContainedEntities();
dpf.DeleteParticipant(participant);
ParticipantService.Instance.Shutdown();
Ace.Fini();
```

## Build and run the projects

Build the publisher and subscriber project with the following commands:

`dotnet build HelloPublisher.csproj --configuration <Release|Debug> --runtime <runtime_identifier> --no-self-contained`

`dotnet build HelloSubscriber.csproj --configuration <Release|Debug> --runtime <runtime_identifier> --no-self-contained`

> [!NOTE]
> The implemented runtime identifiers are:
> - win-x64
> - win-x86
> - linux-x64
> - osx-x64

> [!NOTE]
> To run the program on Linux systems, you should set the `LD_LIBRARY_PATH` pointing to the directory where the executable is created.
> Similar, to run the program on MacOS systems, you should set the `DYLD_FALLBACK_LIBRARY_PATH` pointing to the directory where the executable is created. 

