# Version 0.5.0

### New Features
- Updated to OpenDDS v3.14
- Visual Studio 2019 migration
- Implemented `ParticipantBuiltinTopicDataDataReader` class
- Implemented `TopicBuiltinTopicDataDataReader` class
- Implemented `PublicationBuiltinTopicDataDataReader` class
- Implemented `SubsriptionBuiltinTopicDataDataReader` class

# Version 0.4.1

### New Features
- Build scripts and patches adapted to compile with OpenDDS v3.13.1
- WpfShapeDemo: Fixed RTI shape coordinates compatibility and prepared to work with other vendors

### Breaking Changes
- The calls to the `ACE::init/fini` are not called anymore by the `ParticipantService` and must be called via `OpenDDSharp.Ace.Init/Finit` method

### Bug Fixes
- Fixed memory leak in the `Write` method when using sequences

# Version 0.3.0

### New Features
- Changed license to LGPL
- Added finalizers to allow the garbage collector to free the native pointers
- Overridden `Equals` and `GetHashCode` method and implemented equality operators in the *InstanceHandle* class
- Wrapped *TransportRegistry* and *TransportConfig* classes
- Wrapped classes for the discovery configuration
- Wrapped classes for the specific transport implementation
- Include new patches for OpenDDS v3.13
- Added API unit tests

### Breaking Changes
- Renamed `Delete` methods to `Dispose` in the *DataWriter* code generated as defined in the DDS standard
- Removed default RTPS configuration. RTPS should be configured via the transport/discovery wrapped classes or the configuration .ini file
- Removed `AttachToWaitSet`, `DetachFromWaitSet` and `SignalAll` from *GuardCondition*
- Removed `OnPublicationDisconnected`, `OnPublicationReconnected` and `OnPublicationLost` from the *PublisherListener* because OpenDDS does not implement them natively
- Removed `OnBudgetExceeded`, `OnSubscriptionDisconnected`, `OnSubscriptionLost` and `OnSubscriptionReconnected` from the *SubscriberListener* because OpenDDS does not implemet them natively
- Removed `OnSubscriptionDisconnected`, `OnSubscriptionDisconnected`, `OnSubscriptionLost`, `OnBudgetExceeded`, `OnPublicationDisconnected`, `OnPublicationReconnected`, and `OnPublicationLost` from the *DomainParticipantListener* because OpendDDS does not implement them natively
- Removed `OnConnectionDeleted` callback because OpenDDS not have it anymore in version 3.13

### Bug Fixes
- Copy the code generated templates, if changed, to $(DDS_ROOT)\dds\idl
- Read method overloads that doesn't contain `SampleStateMask` parameter now use `SampleStateMask.AnySampleState` as default value
- Removed *AnyCPU* configuration from the solution/project files
- Fixed `statusCondition` null exception in the *ConsoleDemo* project
- Ensure the native pointer is *NULL* when deleting an entity
- Deletes the *DataReader* conditions during the `delete_contained_entities` call
- Fixed infinite loop in the `GetDiscoveredParticipants` and `GetDiscoveredTopics` methods
- Added transient fault handling code in the `GenerateSolutionFile` method
- Fixed default reliability properties in the *DataWriter*
- Fixed infinite loop in the `GetMatchedSubscriptions`
- Fixed `UnregisterInstance` overload without handle parameter
- Fixed `GetKeyValue method` implementation
- Fixed infinite loop in the `GetMatchedPublications`
- Fixed `Read`/`Take` overloads with *QueryConditions*
- Fixed `InvalidOperationException` in *DetachConditions*
- Fixed code generated enumerations access in .Net framework applications
- Splitted `AddProjectTemplate` method in two methods to handle transient fault separately
  
# Version 0.2.1

- Initial version
