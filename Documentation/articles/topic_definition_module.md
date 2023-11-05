# OpenDDSharp Topic-Definition Module

The Data Distribution Service (DDS) Topic-Definition module is the component in the DDS middleware that handles
the definition and management of topics. Topics in DDS represent the data that is being published and subscribed to
within the DDS system.

The following sections describe the interfaces and classes that are part of the Topic-Definition module.

## ITopicDescription Interface

The `ITopicDescription` interface is the contract between publications and subscriptions that ensures both are linked
to the same data-type. It allows applications to discover the type associated with a topic, enabling them to work
with data in a generic way. The `ITopicDescription` is the base interface for the `Topic`, `ContentFilteredTopic`
and `MultiTopic` classes.

Before a type can be used in a DDS system, it must be registered within the middleware. This is done by creating
a specific `TypeSupport` object for the type and registering it to the `DomainParticipant`. You will learn more
about `TypeSupport` classes in the next section.

A new `ITopicDescription` is created locally when calling the `CreateTopic`, `CreateContentFilteredTopic` or
`CreateMultiTopic` method from a `DomainParticipant` object.

Here is an example of how to register a type and create a new topic with a `DomainParticipant`:

```csharp
var support = new MyTypeTypeSupport();
var typeName = support.GetTypeName();
var result = support.RegisterType(participant, typeName);

var myTopic = participant.CreateTopic("MyTopic", typeName);
```

The `TypeName` property in the `ITopicDescription` object gets type name used to register the type in the
`DomainParticipant`. This `TypeName` creates a unique resulting type for the publication or the subscription and
therefore creates an implicit association with a specific `TypeSupport` class.

During the registration of the type, you can use the type name you want to use in the `DomainParticipant` to refer to the
type. This type name must be unique within the scope of the `DomainParticipant` and cannot be used to register another
type. It is a good practice to use the type name returned by the `GetTypeName` method of the `TypeSupport` class as
shown in the previous example. It gives you a full-qualified name of the type as per the type definition in the IDL
file. For example, the following IDL code snippet will generate a type name of `MyModule::MyType` that can be retrieved
calling the `GetTypeName` method of the `MyTypeTypeSupport` class:

```
module MyModule {
    @topic
    struct MyType {
        @key string Key; 
    };
};
```

The `Participant` property in the `ITopicDescription` object holds a read-only reference to the `DomainParticipant`
that created it. All `ITopicDescription` created with the same `DomainParticipant` object points to the same object
reference in memory and therefore can be compared using the `==` operator.

The property `Name` gets the name used to create the `ITopicDescription`. This name must be unique within the scope of
the `DomainParticipant` that creates the `ITopicDescription` and must be shared by the publication and subscription
that want to communicate with each other.

The topic description name can be used to retrieve the `ITopicDescription` locally. For example, the following code
snippet shows how to retrieve a topic description from a `DomainParticipant` object using its name:

```csharp
var topicDescription = participant.LookupTopicDescription("MyTopic");
Console.WriteLine($"Topic name: {topicDescription.Name}");
Console.WriteLine($"Topic type name: {topicDescription.TypeName}");
```

The `LookupTopicDescription` method returns a `ITopicDescription` object that matches the name provided as a parameter.
If no `ITopicDescription` is found, the method returns `null`.

## TypeSupport Classes

`TypeSupport` is a class that provides the necessary operations for working with the data types associated with topics.
It includes functions for serializing and deserializing data, as well as for managing the type information.

## Topic Class

`Topic` is a class that defines a specific data type that will be exchanged between DDS participants.
It associates a data type (e.g., a structure or a class) with a name that identifies the topic.
It also encapsulates information about the data type, including its name and a unique identifier.

## ContentFilteredTopic Class

`ContentFilteredTopic` is a specialized type of topic that allows a subscriber to filter the data it receives
based on specific criteria. It provides a way to subscribe to a subset of the data that a publisher is offering,
based on a defined filter expression.

## MultiTopic Class
`MultiTopic` is a class that enables a subscriber to express interest in multiple related topics with a single
subscription. It allows a subscriber to consolidate its subscriptions to various related topics, reducing the overhead
associated with managing multiple subscriptions.

## TopicListener Class

`TopicListener` is an interface that allows applications to define callback methods that are triggered when certain
events related to a topic occur. For example, a listener can be notified when a new topic is discovered or when
a topic is removed.

Now, let's visualize the relationships between these classes in a diagram:

```
          +----------------+
          | TopicDescription|
          +----------------+
                 /           \
                /             \
               /               \
              /                 \
    +--------+                   +------+
    |   Topic  |                   | TypeSupport |
    +--------+                   +------+
           |                            |
           |                            |
           |                            |
           |                            |
           |                            |
 +-----------------+          +------------------+
 |ContentFilteredTopic|          |    MultiTopic    |
 +-----------------+          +------------------+
                                     |
                                     |
                              +------------+
                              |TopicListener|
                              +------------+
```

This diagram represents the relationships between the classes within the Topic-Definition module.
Note that `Topic` and `TypeSupport` are central to the module, with other classes building on or utilizing their
functionalities. `TopicDescription` provides a level of abstraction, while specialized topics like
`ContentFilteredTopic` and `MultiTopic` offer additional features for filtering and managing subscriptions.
The `TopicListener` interface allows applications to respond to events related to topics.