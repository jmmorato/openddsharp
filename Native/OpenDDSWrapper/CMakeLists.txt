cmake_minimum_required(VERSION 3.8.2)

project(OpenDDSWrapper CXX)

set(CMAKE_BUILD_TYPE "Release")
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

if(MSVC)
    set(CMAKE_CXX_FLAGS "/O2")
endif()

if (APPLE)
    set(CMAKE_CXX_STANDARD 17)
    set(CMAKE_CXX_STANDARD_REQUIRED ON)
    set(CMAKE_CXX_EXTENSIONS OFF)
    set(CMAKE_CXX_FLAGS "-Wall -Wextra -Wpointer-arith")
    set(CMAKE_CXX_FLAGS_RELEASE "-O3 -DNDEBUG")
elseif (UNIX)
    set(CMAKE_SKIP_BUILD_RPATH FALSE)
    set(CMAKE_BUILD_WITH_INSTALL_RPATH TRUE)
    set(CMAKE_INSTALL_RPATH $ORIGIN)
endif ()

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
        WaitSet.h WaitSet.cpp
        TimeValueWrapper.h
        MulticastInst.h MulticastInst.cpp
        TransportInst.h TransportInst.cpp
        TcpInst.h TcpInst.cpp
        RtpsUdpInst.h RtpsUdpInst.cpp
        ShmemInst.h ShmemInst.cpp
        UdpInst.h UdpInst.cpp
        ParticipantBuiltinTopicDataDataReader.h ParticipantBuiltinTopicDataDataReader.cpp
        PublicationBuiltinTopicDataDataReader.h PublicationBuiltinTopicDataDataReader.cpp
        SubscriptionBuiltinTopicDataDataReader.h SubscriptionBuiltinTopicDataDataReader.cpp
        TopicBuiltinTopicDataDataReader.h TopicBuiltinTopicDataDataReader.cpp
        MultiTopic.h MultiTopic.cpp)

if (MSVC)
    add_definitions(-D_CRT_SECURE_NO_WARNINGS)
    add_definitions(-D_WINSOCK_DEPRECATED_NO_WARNINGS)
    target_compile_options(OpenDDSWrapper PUBLIC /wd4190)
elseif (APPLE)
    target_compile_options(OpenDDSWrapper PUBLIC -Wno-switch -Wno-return-type-c-linkage -Wno-tautological-pointer-compare)
    set_target_properties(OpenDDSWrapper PROPERTIES LINK_FLAGS "-Wl,-rpath,@executable_path -Wl,-rpath,@loader_path")
elseif (UNIX)
    target_compile_options(OpenDDSWrapper PUBLIC -Wno-switch -Wno-return-type-c-linkage -Wno-tautological-pointer-compare)
    set_target_properties(OpenDDSWrapper PROPERTIES LINK_FLAGS "-Wl,-rpath,$ORIGIN")
endif ()

target_link_libraries(OpenDDSWrapper OpenDDS::OpenDDS)
