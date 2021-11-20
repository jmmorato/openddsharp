project(OpenDDSWrapper)
cmake_minimum_required(VERSION 3.8.2)

find_package(OpenDDS REQUIRED)

add_library(OpenDDSWrapper SHARED
Ace.h Ace.cpp 
BuiltinTopicData.h
Condition.h Condition.cpp
ContentFilteredTopic.h ContentFilteredTopic.cpp
DataReader.h DataReader.cpp
DataReaderListener.h DataReaderListener.cpp 
DataReaderListenerImpl.h DataReaderListenerImpl.cpp
DataWriter.h DataWriter.cpp
DataWriterListener.h DataWriterListener.cpp
DataWriterListenerImpl.h DataWriterListenerImpl.cpp
Discovery.h Discovery.cpp
DomainParticipant.h DomainParticipant.cpp
DomainParticipantFactory.h DomainParticipantFactory.cpp
DomainParticipantListener.h DomainParticipantListener.cpp
DomainParticipantListenerImpl.h DomainParticipantListenerImpl.cpp
Entity.h Entity.cpp
GuardCondition.h GuardCondition.cpp
InfoRepoDiscovery.h InfoRepoDiscovery.cpp
ListenerDelegates.h
marshal.h marshal.cpp
ParticipantService.h ParticipantService.cpp
Publisher.h Publisher.cpp
PublisherListener.h PublisherListener.cpp
PublisherListenerImpl.h PublisherListenerImpl.cpp
QosPolicies.h
QueryCondition.h QueryCondition.cpp
ReadCondition.h ReadCondition.cpp
RtpsDiscovery.h RtpsDiscovery.cpp
StatusCondition.h StatusCondition.cpp
Statuses.h
Subscriber.h Subscriber.cpp
SubscriberListener.h SubscriberListener.cpp
SubscriberListenerImpl.h SubscriberListenerImpl.cpp
Topic.h Topic.cpp
TopicListener.h TopicListener.cpp
TopicListenerImpl.h TopicListenerImpl.cpp
TransportConfig.h TransportConfig.cpp
TransportRegistry.h TransportRegistry.cpp
Utils.h Utils.cpp
WaitSet.h WaitSet.cpp)

target_link_libraries(OpenDDSWrapper OpenDDS::OpenDDS)
