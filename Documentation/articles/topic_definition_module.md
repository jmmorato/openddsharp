# OpenDDSharp Topic-Definition Module

The Topic-Definition module is a central component of the Data Distribution Service (DDS) standard
(OMG DDS 1.4, §2.2.2.3). It defines the data types and communication channels used for publishing and
subscribing to data within a DDS domain. Every piece of data exchanged between DDS entities is tied to a topic,
and it is the Topic-Definition module that provides the abstractions needed to define, register, and manage those
topics.

The module is comprised of the following main classes and interfaces:

- **TypeSupport** — a per-type interface (auto-generated from IDL) that registers the type with the middleware.
- **ITopicDescription** — the base interface that ties a name to a type within a `DomainParticipant`.
- **Topic** — the concrete topic entity associated with `DataWriter` and `DataReader` objects.
- **ContentFilteredTopic** — a content-filtered view of a `Topic` that delivers only samples matching a filter.
- **MultiTopic** — a virtual topic that combines, filters, and re-arranges data from multiple topics *(optional)*.
- **TopicListener** — the listener interface for receiving status-change notifications on a `Topic`.

## Auto-Generated TypeSupport Classes

Before a type can be used in a DDS system it must be defined in an IDL file and its type information must be
communicated to the middleware. Per the DDS specification (§2.2.2.3.6), the `TypeSupport` interface is abstract
and must be specialized for each concrete type that will be used by the application. The spec requires that each DDS
implementation provides an **automatic means to generate** this type-specific class from a description of the type —
using IDL.

OpenDDSharp provides the `OpenDDSharp.IdlGenerator` tool that processes IDL files and produces the necessary
C# wrapper classes, including a `TypeSupport` class for each IDL type annotated with `@topic`.

The generated class implements the `ITypeSupport<T>` interface (and its base `ITypeSupport`), which provides
the operations needed to register the type with a `DomainParticipant` and to encode/decode instances for
out-of-band serialization.

### ITypeSupport Interface

`ITypeSupport` defines the core type registration operations mandated by the DDS spec:

- **`GetTypeName()`** — Returns the default fully-qualified type name as derived from the IDL definition.
  For example, a `struct MyType` inside IDL `module MyModule` returns `"MyModule::MyType"`. This name is used
  internally by the middleware to match publications with subscriptions.

- **`RegisterType(DomainParticipant dp, string typeName)`** — Communicates the existence of the data type to the
  `DomainParticipant`. The generated implementation of this operation embeds all the knowledge the middleware needs
  to manage the type, including its key definition (fields annotated `@key`) that allow the service to distinguish
  different instances of the same type.

  Per the spec (§2.2.2.3.6.1), the following rules apply:
  - It is a precondition error to register **two different types** under the same `typeName` with the same
    `DomainParticipant`; the operation returns `ReturnCode.PreconditionNotMet`.
  - Registering the **same TypeSupport** multiple times with the same `DomainParticipant` and `typeName` is
    allowed; subsequent calls are silently ignored and return `ReturnCode.Ok`.
  - Passing `null` as `typeName` causes the default type name (from `GetTypeName()`) to be used automatically.

- **`UnregisterType(DomainParticipant dp, string typeName)`** — Removes a previously registered type from the
  `DomainParticipant`.

### ITypeSupport\<T\> Interface

The generic `ITypeSupport<T>` interface extends `ITypeSupport` with encoding and decoding helpers that allow
applications to serialize typed data samples independently of the DDS transport:

- `EncodeToString(T sample)` / `DecodeFromString(string encoded)` — JSON encoding.
- `EncodeToBytes(T sample)` / `DecodeFromBytes(byte[] encoded)` — CDR (Common Data Representation) binary encoding.

### Using a Generated TypeSupport

Given the following IDL definition:

```
module MyModule {
    @topic
    struct MyType {
        @key string Key;
        long Value;
    };
};
```

The code generator produces a `MyTypeTypeSupport` class. The typical usage pattern to register the type and
create a topic is:

```csharp
// Create an instance of the generated TypeSupport class
var support = new MyTypeTypeSupport();

// Retrieve the fully-qualified type name: "MyModule::MyType"
var typeName = support.GetTypeName();

// Register the type with the DomainParticipant
var result = support.RegisterType(participant, typeName);

// Create a Topic using the registered type name
var topic = participant.CreateTopic("MyTopic", typeName);
```

For a detailed description of the `ITypeSupport` and `ITypeSupport<T>` interfaces, please refer to the
[ITypeSupport API Reference](xref:OpenDDSharp.DDS.ITypeSupport) and
[ITypeSupport&lt;T&gt; API Reference](xref:OpenDDSharp.DDS.ITypeSupport`1) documentation.

## ITopicDescription Interface

Per the DDS specification (§2.2.2.3.1), `TopicDescription` (mapped to `ITopicDescription` in OpenDDSharp) is an
abstract base that represents the fact that both publications and subscriptions are tied to a single data type. It is
the base for `Topic`, `ContentFilteredTopic`, and `MultiTopic`.

It exposes three read-only properties:

- **`TypeName`** — The type name used to create the `ITopicDescription`. This defines a unique resulting type for
  the publication or subscription and therefore creates an implicit association with a `TypeSupport` implementation.
- **`Name`** — The unique name used to create the `ITopicDescription` within the scope of its `DomainParticipant`.
  This name must be shared between publishers and subscribers to identify the same communication channel.
- **`Participant`** — A read-only reference to the `DomainParticipant` to which the `ITopicDescription` belongs.

A topic description can be retrieved by name from a `DomainParticipant` using `LookupTopicDescription`:

```csharp
var topicDescription = participant.LookupTopicDescription("MyTopic");
Console.WriteLine($"Topic name:      {topicDescription.Name}");
Console.WriteLine($"Topic type name: {topicDescription.TypeName}");
```

> **`LookupTopicDescription` vs `FindTopic`:** Per the spec (§2.2.2.2.1.10–11), `LookupTopicDescription`
> searches only among the **locally created** `Topic`, `ContentFilteredTopic`, and `MultiTopic` objects and never
> blocks. `FindTopic` (not exposed by `ITopicDescription`) gives access to an existing or soon-to-exist enabled
> `Topic` by name and may block until the topic appears or a timeout expires. A `Topic` obtained via `FindTopic`
> must also be deleted via `DeleteTopic` to release the local reference.

`LookupTopicDescription` returns `null` if no matching description is found locally.

For a detailed description of the interface members, please refer to the
[ITopicDescription API Reference](xref:OpenDDSharp.DDS.ITopicDescription) documentation.

## Topic Class

Per the DDS specification (§2.2.2.3.2), `Topic` is the most basic description of the data to be published and
subscribed. A `Topic` is identified by its name, which must be **unique in the whole Domain**. In addition, by
virtue of implementing `ITopicDescription`, it fully specifies the type of data that can be communicated when
publishing or subscribing to the topic.

`Topic` is the **only** `ITopicDescription` that can be used for publications and therefore the only one that can
be associated with a `DataWriter`.

A `Topic` is created via the `DomainParticipant.CreateTopic()` factory method. The topic is bound to a type
described by the type name argument, and that type **must have been previously registered** with the service using
the `RegisterType` operation on a `TypeSupport` implementation:

```csharp
var support = new MyTypeTypeSupport();
var typeName = support.GetTypeName();
support.RegisterType(participant, typeName);

var topic = participant.CreateTopic("MyTopic", typeName);
```

### Topic Lifecycle and Deletion Constraints

The deletion of a `Topic` is not allowed if any existing `DataReader`, `DataWriter`, `ContentFilteredTopic`, or
`MultiTopic` objects are still using it. Calling `DeleteTopic` in this situation returns
`ReturnCode.PreconditionNotMet`. All entities using the topic must be deleted first:

```csharp
publisher.DeleteDataWriter(dataWriter);
subscriber.DeleteDataReader(dataReader);
participant.DeleteContentFilteredTopic(filteredTopic);

// Now it is safe to delete the Topic
participant.DeleteTopic(topic);
```

> Per the spec, all operations except for the base-class operations `SetQos`, `GetQos`, `SetListener`,
> `GetListener`, `Enable`, and `GetStatusCondition` may return `ReturnCode.NotEnabled` if called on a
> not-yet-enabled `Topic`.

### Inconsistent Topic Status

The `Topic` entity exposes the `INCONSISTENT_TOPIC` communication status. This status is triggered when a topic
was attempted to be used or registered that already exists in the domain with the **same name but different
characteristics** (typically a different type). It is a signal of a configuration mismatch in the DDS system.

The `InconsistentTopicStatus` structure contains:
- `TotalCount` — Cumulative count of times an inconsistent topic has been detected.
- `TotalCountChange` — The incremental change in `TotalCount` since the last time the status was read or the
  listener was called.

You can retrieve this status at any time by calling `GetInconsistentTopicStatus`:

```csharp
var status = new InconsistentTopicStatus();
var result = topic.GetInconsistentTopicStatus(ref status);

if (result == ReturnCode.Ok)
{
    Console.WriteLine($"Inconsistent topic count: {status.TotalCount}");
    Console.WriteLine($"Change since last read:   {status.TotalCountChange}");
}
```

For a detailed description of all methods and properties on the `Topic` class, please refer to the
[Topic API Reference](xref:OpenDDSharp.DDS.Topic) documentation.

### TopicQos Class

The `TopicQos` class holds all QoS policies that control the behavior of a `Topic`. These policies are propagated
as default values to any `DataWriter` or `DataReader` created on the topic (unless overridden at the entity level),
making the topic QoS the baseline for communication behavior.

The following table lists all QoS policies available on a `TopicQos`, their default values, the **RxO**
(Request/Offer compatibility) column, and whether they are **changeable** after the entity has been enabled:

| Policy | Default Value | RxO | Changeable |
|---|---|:---:|:---:|
| `TopicData` | Empty sequence | No | Yes |
| `Durability` | `Volatile` | Yes | **No** |
| `DurabilityService` | `KeepLast`, depth=1; limits=unlimited; cleanup_delay=0 | — | **No** |
| `Deadline` | Period = infinite | Yes | Yes |
| `LatencyBudget` | Duration = 0 | Yes | Yes |
| `Liveliness` | `Automatic`, lease_duration = infinite | Yes | **No** |
| `Reliability` | `BestEffort`, max_blocking_time = 100 ms | Yes | **No** |
| `DestinationOrder` | `ByReceptionTimestamp` | Yes | **No** |
| `History` | `KeepLast`, depth = 1 | No | **No** |
| `ResourceLimits` | All `LengthUnlimited` | No | **No** |
| `TransportPriority` | 0 | N/A | Yes |
| `Lifespan` | Duration = infinite | N/A | Yes |
| `Ownership` | `Shared` | Yes | **No** |

**Key semantics of each policy:**

- **`TopicData`** — Application-specific opaque data attached to the topic and distributed via the built-in topics.
  Can be used by subscribers to implement their own matching or security policies.
- **`Durability`** — Controls whether data persists beyond the lifetime of its writer. `Volatile` (default) means
  data is only delivered to currently connected subscribers. `TransientLocal` and `Transient` keep data for
  late-joining subscribers. Compatibility rule: *offered kind >= requested kind*
  (`Volatile < TransientLocal < Transient < Persistent`).
- **`DurabilityService`** — Configures the history and resource limits of the fictitious persistence service
  DataReader/DataWriter used to implement `Transient` and `Persistent` durability.
- **`Deadline`** — Establishes a contract that each data instance will be updated at least once every period.
  Compatibility rule: *offered deadline period <= requested deadline period*.
  Must be consistent with `TimeBasedFilter.MinimumSeparation` (period >= minimum_separation).
- **`LatencyBudget`** — A hint to the middleware about the maximum acceptable end-to-end latency. Not enforced by
  the service. Compatibility rule: *offered duration <= requested duration*.
- **`Liveliness`** — Determines the mechanism and parameters by which an entity proves it is still active.
  `Automatic` (default) means the infrastructure asserts liveliness automatically. Compatibility: offered kind
  must be >= requested kind and offered lease_duration <= requested lease_duration.
- **`Reliability`** — `BestEffort` (default) allows dropped samples; `Reliable` guarantees delivery via retries.
  Compatibility: if a reader requests `Reliable`, the writer must also offer `Reliable`.
- **`DestinationOrder`** — Controls whether data ordering is based on reception timestamp (default) or source
  timestamp. `BySourceTimestamp` guarantees a consistent final value across all subscribers.
- **`History`** — `KeepLast` depth=1 (default) retains only the most recent sample per instance; `KeepAll`
  retains every sample until taken by the application. Must be consistent with `ResourceLimits`.
- **`ResourceLimits`** — Sets upper bounds on memory consumption (maximum samples, instances, and samples per
  instance). Applies to both writer-side history and reader-side cache. Default is unlimited.
- **`TransportPriority`** — A hint to the transport layer about the relative priority of this topic's data.
- **`Lifespan`** — Specifies the maximum validity duration of a written sample. Expired samples are automatically
  removed before delivery.
- **`Ownership`** — `Shared` (default) allows multiple writers to update the same instance. `Exclusive` designates
  a single owner per instance, chosen by `OwnershipStrength`. The offered and requested kind must **exactly match**.

Here is an example of creating a `Topic` with custom QoS settings:

```csharp
var qos = new TopicQos
{
    Reliability =
    {
        Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
        MaxBlockingTime = new Duration { Seconds = 1, NanoSeconds = 0 },
    },
    Durability =
    {
        Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos,
    },
    History =
    {
        Kind = HistoryQosPolicyKind.KeepLastHistoryQos,
        Depth = 10,
    },
};

var topic = participant.CreateTopic("MyTopic", typeName, qos);
```

You can also read and update the QoS of an existing `Topic` via `GetQos` and `SetQos`. Immutable policies
(those marked **No** in the Changeable column) can only be set before the `Topic` is enabled; attempting to
change them afterwards causes `SetQos` to return `ReturnCode.ImmutablePolicy`.

```csharp
var qos = new TopicQos();
topic.GetQos(qos);

// Only changeable policies can be updated after creation
qos.TopicData.Value = new List<byte> { 1, 2, 3 };
var result = topic.SetQos(qos);
```

The `DomainParticipant` also maintains default `TopicQos` values for newly created topics. These can be read and
updated via `GetDefaultTopicQos` and `SetDefaultTopicQos`. The special value `TOPIC_QOS_DEFAULT` (equivalent to
the current factory defaults) can be passed to `CreateTopic` to explicitly request the current defaults:

```csharp
// Retrieve the current default Topic QoS
var qos = new TopicQos();
participant.GetDefaultTopicQos(qos);

// Change the default reliability to Reliable for all future topics
qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
participant.SetDefaultTopicQos(qos);

// Subsequent CreateTopic calls without explicit QoS use the updated defaults
var topic = participant.CreateTopic("MyTopic", typeName);
```

For a detailed description of all QoS policies, please refer to the
[TopicQos API Reference](xref:OpenDDSharp.DDS.TopicQos) documentation.

### TopicListener Class

Per the DDS specification (§2.2.2.3.5), since `Topic` is a kind of `Entity`, it has the ability to have an
associated listener of concrete type `TopicListener`.

`TopicListener` is an abstract class with a single callback:

- **`OnInconsistentTopic(Topic topic, InconsistentTopicStatus status)`** — Invoked when the `INCONSISTENT_TOPIC`
  status changes, i.e., when a `Topic` was attempted to be used that already exists in the domain with the same
  name but different characteristics (typically a different type or incompatible QoS). The `status` parameter
  provides the `TotalCount` and `TotalCountChange` fields.

To use a `TopicListener`, create a class that extends it and implements the abstract callback:

```csharp
public class MyTopicListener : TopicListener
{
    public override void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status)
    {
        Console.WriteLine($"Inconsistent topic detected on: '{topic.Name}'");
        Console.WriteLine($"Total count: {status.TotalCount}, change: {status.TotalCountChange}");
    }
}
```

Register the listener during topic creation or at any later point via `SetListener`. The `StatusMask` parameter
controls which status changes trigger the listener:

```csharp
// Register during creation with all relevant statuses enabled
var listener = new MyTopicListener();
var topic = participant.CreateTopic("MyTopic", typeName, null, listener);

// Or attach/replace after creation with a specific mask
topic.SetListener(listener, StatusMask.InconsistentTopic);

// Remove the listener by passing null
topic.SetListener(null);
```

Passing `StatusMask.DefaultStatusMask` enables all statuses supported by `TopicListener`.

For a detailed description, please refer to the
[TopicListener API Reference](xref:OpenDDSharp.DDS.TopicListener) documentation.

## ContentFilteredTopic Class

Per the DDS specification (§2.2.2.3.3), `ContentFilteredTopic` is a specialization of `TopicDescription` that
allows for content-based subscriptions. It describes a more sophisticated subscription that indicates the subscriber
does not want to necessarily see all values of each instance published under the `Topic`. Rather, it wants to see
only the values whose contents satisfy certain criteria.

A `ContentFilteredTopic` is always associated with an underlying **related `Topic`** from which it receives data.
It does not itself define a communication channel — it only filters data from the related topic's channel.

The content selection is controlled by two attributes:

- **`FilterExpression`** — A string specifying the criteria to select data samples of interest. It is similar to
  the `WHERE` part of an SQL clause (e.g., `"temperature > %0 AND region = %1"`). The precise syntax is defined in
  Annex B of the DDS specification.
- **Expression parameters** — A sequence of strings that supply values for the `%n` placeholder tokens in the
  filter expression. The number of supplied parameters must exactly match the number of `%n` tokens in the
  expression.

A `ContentFilteredTopic` is created via `DomainParticipant.CreateContentFilteredTopic`:

```csharp
// Create the underlying topic first
var topic = participant.CreateTopic("TemperatureTopic", typeName);

// Create a filtered view: only samples where temperature > 50.0 in region "North"
var filteredTopic = participant.CreateContentFilteredTopic(
    name: "HotNorthTopic",
    relatedTopic: topic,
    filterExpression: "temperature > %0 AND region = %1",
    expressionParameters: new[] { "50.0", "North" });
```

A `DataReader` subscribes to a `ContentFilteredTopic` the same way as to a regular `Topic`:

```csharp
var reader = subscriber.CreateDataReader(filteredTopic);
```

The filter expression parameters can be changed at runtime **without** recreating the `ContentFilteredTopic` or
the `DataReader`. `GetExpressionParameters` returns the parameters from the last successful call to
`SetExpressionParameters`, or the parameters specified at creation if `SetExpressionParameters` was never called:

```csharp
// Raise the temperature threshold at runtime
var result = filteredTopic.SetExpressionParameters("75.0", "North");

// Read back the current parameters
var parameters = new List<string>();
filteredTopic.GetExpressionParameters(parameters);
Console.WriteLine($"Current threshold: {parameters[0]}");
```

**Deletion constraints:** A `ContentFilteredTopic` cannot be deleted while any `DataReader` is using it.
All such `DataReader` objects must be deleted first. In addition, the `DeleteContentFilteredTopic` operation
must be called on the same `DomainParticipant` used to create it:

```csharp
subscriber.DeleteDataReader(reader);
participant.DeleteContentFilteredTopic(filteredTopic);

// The related Topic can now be deleted after all its consumers are removed
participant.DeleteTopic(topic);
```

For a detailed description, please refer to the
[ContentFilteredTopic API Reference](xref:OpenDDSharp.DDS.ContentFilteredTopic) documentation.

## MultiTopic Class

Per the DDS specification (§2.2.2.3.4), `MultiTopic` is an **optional** specialization of `TopicDescription`
that allows subscriptions to combine, filter, and re-arrange data coming from **several topics**. `MultiTopic`
allows a more sophisticated subscription that can select and combine data received from multiple topics into a
single resulting type (specified by the inherited `TypeName`). The data is then filtered (selection) and possibly
re-arranged (aggregation/projection) according to a subscription expression with expression parameters.

The `MultiTopic` uses a **subscription expression** that is structured like an SQL query with three logical parts:

- **`SELECT`** — the fields from the contributing topics to include in the result type.
- **`FROM`** — the names of the topics to draw data from (the source for data may span multiple topics).
- **`WHERE`** — an optional filter condition on the combined data.

The topics combined in a `MultiTopic` may have different types, but the fields used for the implicit
**`NATURAL JOIN`** operation must have matching names and compatible types across the contributing topics.
The precise syntax of the subscription expression is defined in Annex B of the DDS specification.

A `MultiTopic` is created via `DomainParticipant.CreateMultiTopic`. The resulting type must have been
previously registered with the service:

```csharp
// Assume "PriceTopic" (fields: symbol, price) and "VolumeTopic" (fields: symbol, volume) exist
// Register the combined result type "MarketData" first
var support = new MarketDataTypeSupport();
support.RegisterType(participant, support.GetTypeName());

var multiTopic = participant.CreateMultiTopic(
    name: "MarketDataTopic",
    typeName: support.GetTypeName(),
    subscriptionExpression: "SELECT price, volume FROM PriceTopic, VolumeTopic WHERE symbol = %0",
    expressionParameters: new[] { "MSFT" });
```

A `DataReader` for a `MultiTopic` is created the same way as for a regular `Topic`:

```csharp
var reader = subscriber.CreateDataReader(multiTopic);
```

### Instance Lifecycle in MultiTopic

`DataReader` entities associated with a `MultiTopic` are alerted of data modifications by the usual listener or
condition mechanisms whenever modifications occur to **any** of the topics relevant to the `MultiTopic`.

`DataReader` entities access instances that are **constructed at the DataReader side** from instances written by
multiple `DataWriter` entities. A `MultiTopic` access instance begins to exist only once all constituting topic
instances are in existence.

The `view_state` and `instance_state` of a `MultiTopic` instance are derived from the corresponding states of
the constituting instances per the DDS spec (§2.2.2.3.4):

- The `view_state` is `NewViewState` if **at least one** constituting instance has `NewViewState`; otherwise
  `NotNewViewState`.
- The `instance_state` is `AliveInstanceState` if **all** constituting topic instances are alive. It is
  `NotAliveDisposedInstanceState` if **at least one** is disposed. Otherwise it is
  `NotAliveNoWritersInstanceState`.

The subscription expression parameters can be updated at runtime without recreating the `MultiTopic`:

```csharp
// Change the symbol filter at runtime
var result = multiTopic.SetExpressionParameters("AAPL");

// Read back the current parameters
var parameters = new List<string>();
multiTopic.GetExpressionParameters(parameters);
Console.WriteLine($"Current symbol filter: {parameters[0]}");
```

**Deletion constraints:** A `MultiTopic` cannot be deleted while any `DataReader` is using it. All such
`DataReader` objects must be deleted first. The `DeleteMultiTopic` operation must be called on the same
`DomainParticipant` used to create it:

```csharp
subscriber.DeleteDataReader(reader);
participant.DeleteMultiTopic(multiTopic);
```

For a detailed description, please refer to the
[MultiTopic API Reference](xref:OpenDDSharp.DDS.MultiTopic) documentation.

## Topic-Definition Module Diagram

The following diagram illustrates the class model of the Topic-Definition module as defined in the DDS
specification (§2.2.2.3, Figure 2.7), showing the relationships between the main classes and their interactions
with the surrounding modules:

```mermaid
graph TB
    subgraph A[Topic-Definition Module]
        direction TB
        TS["&lt;&lt;interface&gt;&gt;\nITypeSupport / TypeSupport\nregister_type()\nget_type_name()"] -->|register type in| DP[DomainParticipant]
        DP -->|create| T[Topic\nget_inconsistent_topic_status()]
        DP -->|create| CFT[ContentFilteredTopic\nfilter_expression\nget/set_expression_parameters()]
        DP -->|create| MT["MultiTopic [optional]\nsubscription_expression\nget/set_expression_parameters()"]
        T o-.-o|1| TQ[TopicQos]
        T o-.-o|0..1| TL["&lt;&lt;interface&gt;&gt;\nTopicListener\non_inconsistent_topic()"]
        T -.->|implements| ITD["&lt;&lt;interface&gt;&gt;\nITopicDescription\nname\ntype_name\nget_participant()"]
        CFT -.->|implements| ITD
        MT -.->|implements| ITD
        CFT -->|related to| T
    end
    A ==> B[/\nPublication\nModule\]
    A ==> C[/\nSubscription\nModule\]
```
