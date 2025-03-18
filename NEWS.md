# OpenDDSharp vNext

### New Features

### Bug Fixes

- Copy TAO libraries required by `opendds_idl` to the DDS bin folder [#281](https://github.com/jmmorato/openddsharp/pull/281)

# OpenDDSharp v3.310.1

### New Features

- Update to OpenDDS v3.31.0 [#275](https://github.com/jmmorato/openddsharp/pull/275)
- Added ITypeSupport interface to be implemented by the IDL data-types [#212](https://github.com/jmmorato/openddsharp/pull/212)
- P/Invoke with LibraryImport [#216](https://github.com/jmmorato/openddsharp/pull/216)
- Supported macOS ARM64 runtime (osx-arm64) [#239](https://github.com/jmmorato/openddsharp/pull/239)

### Bug Fixes

- OpenDDSharp.Marshaller JsonConverters now always format decimal point symbol as '.' instead of ',' [#273](https://github.com/jmmorato/openddsharp/pull/273)

### Breaking Changes

- Implemented CDR serialization for the IDL data-types
  - CDR primitives serialization [#219](https://github.com/jmmorato/openddsharp/pull/219)
  - CDR primitive sequences serialization [#220](https://github.com/jmmorato/openddsharp/pull/220)
  - CDR primitive arrays serialization [#221](https://github.com/jmmorato/openddsharp/pull/221)
  - CDR multi-array serialization [#222](https://github.com/jmmorato/openddsharp/pull/222)
  - CDR wchar support [#223](https://github.com/jmmorato/openddsharp/pull/223)
  - CDR int8 support [#224](https://github.com/jmmorato/openddsharp/pull/224)
  - CDR strings [#248](https://github.com/jmmorato/openddsharp/pull/248)
  - CDR Constants and Enums serialization [#249](https://github.com/jmmorato/openddsharp/pull/249)
  - CDR structures serialization [#257](https://github.com/jmmorato/openddsharp/pull/257)
  - CDR include IDL files [#260](https://github.com/jmmorato/openddsharp/pull/257)
  - Typed DataWriter methods with CDR serialization [#261](https://github.com/jmmorato/openddsharp/pull/261)
  - Typed DataReader methods with CDR serialization [#264](https://github.com/jmmorato/openddsharp/pull/264)
  - Use CDR serialization as default marshaling method [#276](https://github.com/jmmorato/openddsharp/pull/276)

# OpenDDSharp v3.250.2

### New Features
- Updated to OpenDDS v3.25.0 using TAO 3
- JSON samples serialization
- Link native libraries with RPATH

### Bug Fixes
- Fixed IDL include pre-processor issues
- Fixed whitespaces issue in cmake argument call

### Breaking Changes
- Removed Standard suffix from OpenDDSharp NuGet projects
- Implemented dispose pattern in Listeners

# OpenDDSharp v0.8.23023.106

### New Features
- Update OpenDDS v3.23.0 [#141](https://github.com/jmmorato/openddsharp/pull/141)

### Bug Fixes
- Added TAO_Valuetype library dependency [#135](https://github.com/jmmorato/openddsharp/pull/135)
- Added TAO_Valuetype dependency to the code generator [#136](https://github.com/jmmorato/openddsharp/pull/136)
- Fixed LookupDataReader and LookupDataWriter [#138](https://github.com/jmmorato/openddsharp/pull/138)

# OpenDDSharp v0.5.0

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
