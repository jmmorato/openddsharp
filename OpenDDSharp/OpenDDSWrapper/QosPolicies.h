/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "dds\DdsDcpsDomainC.h"
#include "marshal.h"

EXTERN_STRUCT_EXPORT UserDataQosPolicyWrapper
{
    void* value;

public:
    UserDataQosPolicyWrapper() {
        value = NULL;
    }

    UserDataQosPolicyWrapper(const ::DDS::UserDataQosPolicy native) {
        unbounded_sequence_to_ptr(native.value, value);
    }

    operator ::DDS::UserDataQosPolicy() const {
        ::DDS::UserDataQosPolicy native;
        if (value != NULL) {
            ptr_to_unbounded_sequence(value, native.value);
        }
        return native;
    }
};

EXTERN_STRUCT_EXPORT GroupDataQosPolicyWrapper
{
    void* value;

public:
    GroupDataQosPolicyWrapper() {
        value = NULL;
    }

    GroupDataQosPolicyWrapper(const ::DDS::GroupDataQosPolicy native) {
        unbounded_sequence_to_ptr(native.value, value);
    }

    operator ::DDS::GroupDataQosPolicy() const {
        ::DDS::GroupDataQosPolicy native;
        if (value != NULL) {
            ptr_to_unbounded_sequence(value, native.value);
        }
        return native;
    }
};

EXTERN_STRUCT_EXPORT TopicDataQosPolicyWrapper
{
    void* value;

public:
    TopicDataQosPolicyWrapper() {
        value = NULL;
    }

    TopicDataQosPolicyWrapper(const ::DDS::TopicDataQosPolicy native) {
        unbounded_sequence_to_ptr(native.value, value);
    }

    operator ::DDS::TopicDataQosPolicy() const {
        ::DDS::TopicDataQosPolicy native;
        if (value != NULL) {
            ptr_to_unbounded_sequence(value, native.value);
        }
        return native;
    }
};

EXTERN_STRUCT_EXPORT EntityFactoryQosPolicyWrapper
{
    CORBA::Boolean autoenable_created_entities;

public:
    EntityFactoryQosPolicyWrapper() {
        autoenable_created_entities = true;
    }

    EntityFactoryQosPolicyWrapper(const ::DDS::EntityFactoryQosPolicy native) {
        autoenable_created_entities = native.autoenable_created_entities;
    }

    operator ::DDS::EntityFactoryQosPolicy() const {
        ::DDS::EntityFactoryQosPolicy native;
        char buf[2048];
        sprintf(buf, "EntityFactoryQosPolicy autoenable_created_entities: %s \n", autoenable_created_entities ? "true" : "false");
        OutputDebugString(buf);
        native.autoenable_created_entities = autoenable_created_entities;
        sprintf(buf, "EntityFactoryQosPolicy native.autoenable_created_entities: %s \n", native.autoenable_created_entities ? "true" : "false");
        OutputDebugString(buf);
        return native;
    }
};

EXTERN_STRUCT_EXPORT DurabilityQosPolicyWrapper
{
    ::CORBA::Long kind;

public:
    DurabilityQosPolicyWrapper() {
        kind = 0;
    }

    DurabilityQosPolicyWrapper(const ::DDS::DurabilityQosPolicy native) {
        kind = native.kind;
    }

    operator ::DDS::DurabilityQosPolicy() const {
        ::DDS::DurabilityQosPolicy native;
        native.kind = (::DDS::DurabilityQosPolicyKind)kind;
        return native;
    }
};

EXTERN_STRUCT_EXPORT DurabilityServiceQosPolicyWrapper
{
    ::DDS::Duration_t service_cleanup_delay;
    ::CORBA::Long history_kind;
    ::CORBA::Long history_depth;
    ::CORBA::Long max_samples;
    ::CORBA::Long max_instances;
    ::CORBA::Long max_samples_per_instance;

public:
    DurabilityServiceQosPolicyWrapper() {
        service_cleanup_delay.sec = ::DDS::DURATION_ZERO_SEC;
        service_cleanup_delay.nanosec = ::DDS::DURATION_ZERO_NSEC;
        history_kind = 0;
        history_depth = 1;
        max_samples = ::DDS::LENGTH_UNLIMITED;
        max_instances = ::DDS::LENGTH_UNLIMITED;
        max_samples_per_instance = ::DDS::LENGTH_UNLIMITED;
    }

    DurabilityServiceQosPolicyWrapper(const ::DDS::DurabilityServiceQosPolicy native) {
        service_cleanup_delay = native.service_cleanup_delay;
        history_kind = native.history_kind;
        history_depth = native.history_depth;
        max_samples = native.max_samples;
        max_instances = native.max_instances;
        max_samples_per_instance = native.max_samples_per_instance;
    }

    operator ::DDS::DurabilityServiceQosPolicy() const {
        ::DDS::DurabilityServiceQosPolicy native;
        native.service_cleanup_delay = service_cleanup_delay;
        native.history_kind = (::DDS::HistoryQosPolicyKind)history_kind;
        native.history_depth = history_depth;
        native.max_samples = max_samples;
        native.max_instances = max_instances;
        native.max_samples_per_instance = max_samples_per_instance;
        return native;
    }
};

EXTERN_STRUCT_EXPORT DeadlineQosPolicyWrapper
{
    DDS::Duration_t period;

public:
    DeadlineQosPolicyWrapper() {
        period.sec = ::DDS::DURATION_INFINITE_SEC;
        period.nanosec = ::DDS::DURATION_INFINITE_NSEC;
    }

    DeadlineQosPolicyWrapper(const ::DDS::DeadlineQosPolicy native) {
        period = native.period;
    }

    operator ::DDS::DeadlineQosPolicy() const {
        ::DDS::DeadlineQosPolicy native;
        native.period = period;
        return native;
    }
};

EXTERN_STRUCT_EXPORT LatencyBudgetQosPolicyWrapper
{
    DDS::Duration_t duration;

public:
    LatencyBudgetQosPolicyWrapper() {
        duration.sec = ::DDS::DURATION_ZERO_SEC;
        duration.nanosec = ::DDS::DURATION_ZERO_NSEC;
    }

    LatencyBudgetQosPolicyWrapper(const ::DDS::LatencyBudgetQosPolicy native) {
        duration = native.duration;
    }

    operator ::DDS::LatencyBudgetQosPolicy() const {
        ::DDS::LatencyBudgetQosPolicy native;
        native.duration = duration;
        return native;
    }
};

EXTERN_STRUCT_EXPORT LivelinessQosPolicyWrapper
{
    ::CORBA::Long kind;
    DDS::Duration_t lease_duration;

public:
    LivelinessQosPolicyWrapper() {
        kind = 0;
        lease_duration.sec = ::DDS::DURATION_INFINITE_SEC;
        lease_duration.nanosec = ::DDS::DURATION_INFINITE_NSEC;
    }

    LivelinessQosPolicyWrapper(const ::DDS::LivelinessQosPolicy native) {
        kind = (::CORBA::Long)native.kind;
        lease_duration = native.lease_duration;
    }

    operator ::DDS::LivelinessQosPolicy() const {
        ::DDS::LivelinessQosPolicy native;
        native.kind = (::DDS::LivelinessQosPolicyKind)kind;
        native.lease_duration = lease_duration;
        return native;
    }
};

EXTERN_STRUCT_EXPORT ReliabilityQosPolicyWrapper
{
    ::CORBA::Long kind;
    DDS::Duration_t max_blocking_time;

public:
    ReliabilityQosPolicyWrapper() {
        kind = 0;
        max_blocking_time.sec = ::DDS::DURATION_INFINITE_SEC;
        max_blocking_time.nanosec = ::DDS::DURATION_INFINITE_NSEC;
    }

    ReliabilityQosPolicyWrapper(const ::DDS::ReliabilityQosPolicy native) {
        kind = (::CORBA::Long)native.kind;
        max_blocking_time = native.max_blocking_time;
    }

    operator ::DDS::ReliabilityQosPolicy() const {
        ::DDS::ReliabilityQosPolicy native;
        native.kind = (DDS::ReliabilityQosPolicyKind)kind;
        native.max_blocking_time = max_blocking_time;
        return native;
    }
};

EXTERN_STRUCT_EXPORT LifespanQosPolicyWrapper
{
    DDS::Duration_t duration;

public:
    LifespanQosPolicyWrapper() {
        duration.sec = ::DDS::DURATION_INFINITE_SEC;
        duration.nanosec = ::DDS::DURATION_INFINITE_NSEC;
    }
    LifespanQosPolicyWrapper(const ::DDS::LifespanQosPolicy native) {
        duration = native.duration;
    }

    operator ::DDS::LifespanQosPolicy() const {
        ::DDS::LifespanQosPolicy native;
        native.duration = duration;
        return native;
    }
};

EXTERN_STRUCT_EXPORT HistoryQosPolicyWrapper
{
    ::CORBA::Long kind;
    ::CORBA::Long depth;

public:
    HistoryQosPolicyWrapper() {
        kind = 0;
        depth = 1;
    }

    HistoryQosPolicyWrapper(const ::DDS::HistoryQosPolicy native) {
        kind = (::CORBA::Long)native.kind;
        depth = native.depth;
    }

    operator ::DDS::HistoryQosPolicy() const {
        ::DDS::HistoryQosPolicy native;
        native.kind = (DDS::HistoryQosPolicyKind)kind;
        native.depth = depth;
        return native;
    }
};

EXTERN_STRUCT_EXPORT ResourceLimitsQosPolicyWrapper
{
    ::CORBA::Long max_samples;
    ::CORBA::Long max_instances;
    ::CORBA::Long max_samples_per_instance;

public:
    ResourceLimitsQosPolicyWrapper() {
        max_samples = ::DDS::LENGTH_UNLIMITED;
        max_instances = ::DDS::LENGTH_UNLIMITED;
        max_samples_per_instance = ::DDS::LENGTH_UNLIMITED;
    }

    ResourceLimitsQosPolicyWrapper(const ::DDS::ResourceLimitsQosPolicy native) {
        max_samples = native.max_samples;
        max_instances = native.max_instances;
        max_samples_per_instance = native.max_samples_per_instance;
    }

    operator ::DDS::ResourceLimitsQosPolicy() const {
        ::DDS::ResourceLimitsQosPolicy native;
        native.max_samples = max_samples;
        native.max_instances = max_instances;
        native.max_samples_per_instance = max_samples_per_instance;
        return native;
    }
};

EXTERN_STRUCT_EXPORT TimeBasedFilterQosPolicyWrapper
{
    DDS::Duration_t minimum_separation;

public:
    TimeBasedFilterQosPolicyWrapper() {
        minimum_separation.sec = ::DDS::DURATION_ZERO_SEC;
        minimum_separation.nanosec = ::DDS::DURATION_ZERO_NSEC;
    }

    TimeBasedFilterQosPolicyWrapper(const ::DDS::TimeBasedFilterQosPolicy native) {
        minimum_separation = native.minimum_separation;        
    }

    operator ::DDS::TimeBasedFilterQosPolicy() const {
        ::DDS::TimeBasedFilterQosPolicy native;
        native.minimum_separation = minimum_separation;        
        return native;
    }
};

EXTERN_STRUCT_EXPORT ReaderDataLifecycleQosPolicyWrapper
{
    DDS::Duration_t autopurge_nowriter_samples_delay;
    DDS::Duration_t autopurge_disposed_samples_delay;

public:
    ReaderDataLifecycleQosPolicyWrapper() {
        autopurge_nowriter_samples_delay.sec = ::DDS::DURATION_INFINITE_SEC;
        autopurge_nowriter_samples_delay.nanosec = ::DDS::DURATION_INFINITE_NSEC;

        autopurge_disposed_samples_delay.sec = ::DDS::DURATION_INFINITE_SEC;
        autopurge_disposed_samples_delay.nanosec = ::DDS::DURATION_INFINITE_NSEC;
    }

    ReaderDataLifecycleQosPolicyWrapper(const ::DDS::ReaderDataLifecycleQosPolicy native) {
        autopurge_nowriter_samples_delay = native.autopurge_nowriter_samples_delay;
        autopurge_disposed_samples_delay = native.autopurge_disposed_samples_delay;
    }

    operator ::DDS::ReaderDataLifecycleQosPolicy() const {
        ::DDS::ReaderDataLifecycleQosPolicy native;
        native.autopurge_disposed_samples_delay = autopurge_disposed_samples_delay;
        native.autopurge_nowriter_samples_delay = autopurge_nowriter_samples_delay;
        return native;
    }
};

EXTERN_STRUCT_EXPORT DestinationOrderQosPolicyWrapper
{
    ::CORBA::Long kind;    

public:
    DestinationOrderQosPolicyWrapper() {
        kind = 0;
    }

    DestinationOrderQosPolicyWrapper(const ::DDS::DestinationOrderQosPolicy native) {
        kind = (::CORBA::Long)native.kind;
    }

    operator ::DDS::DestinationOrderQosPolicy() const {
        ::DDS::DestinationOrderQosPolicy native;
        native.kind = (DDS::DestinationOrderQosPolicyKind)kind;
        return native;
    }
};

EXTERN_STRUCT_EXPORT OwnershipQosPolicyWrapper
{
    ::CORBA::Long kind;

public:
    OwnershipQosPolicyWrapper() {
        kind = 0;
    }

    OwnershipQosPolicyWrapper(const ::DDS::OwnershipQosPolicy native) {
        kind = (::CORBA::Long)native.kind;
    }

    operator ::DDS::OwnershipQosPolicy() const {
        ::DDS::OwnershipQosPolicy native;
        native.kind = (DDS::OwnershipQosPolicyKind)kind;
        return native;
    }
};

EXTERN_STRUCT_EXPORT OwnershipStrengthQosPolicyWrapper
{
    ::CORBA::Long value;

public:
    OwnershipStrengthQosPolicyWrapper() {
        value = 0;
    }

    OwnershipStrengthQosPolicyWrapper(const ::DDS::OwnershipStrengthQosPolicy native) {
        value = native.value;
    }

    operator ::DDS::OwnershipStrengthQosPolicy() const {
        ::DDS::OwnershipStrengthQosPolicy native;
        native.value = value;
        return native;
    }
};

EXTERN_STRUCT_EXPORT PresentationQosPolicyWrapper
{
    ::CORBA::Long access_scope;
    ::CORBA::Boolean coherent_access;
    ::CORBA::Boolean ordered_access;

public:
    PresentationQosPolicyWrapper() {
        access_scope = 0;
        coherent_access = false;
        ordered_access = false;
    }

    PresentationQosPolicyWrapper(const ::DDS::PresentationQosPolicy native) {
        access_scope = (::CORBA::Long)native.access_scope;
        coherent_access = native.coherent_access;
        ordered_access = native.ordered_access;
    }

    operator ::DDS::PresentationQosPolicy() const {
        ::DDS::PresentationQosPolicy native;
        native.access_scope = (::DDS::PresentationQosPolicyAccessScopeKind)access_scope;
        native.coherent_access = coherent_access;
        native.ordered_access = ordered_access;
        return native;
    }
};

EXTERN_STRUCT_EXPORT PartitionQosPolicyWrapper
{
    void* name;

public:
    PartitionQosPolicyWrapper() {
        name = NULL;
    }

    PartitionQosPolicyWrapper(const ::DDS::PartitionQosPolicy native) {
        ::TAO::unbounded_basic_string_sequence<char> list = native.name;
        unbounded_basic_string_sequence_to_ptr(list, name);
    }

    operator ::DDS::PartitionQosPolicy() const {
        ::DDS::PartitionQosPolicy native;
        if (name != NULL) {
            ptr_to_unbounded_basic_string_sequence(name, native.name);
        }
        return native;
    }
};

EXTERN_STRUCT_EXPORT TransportPriorityQosPolicyWrapper
{
    CORBA::Long value;

public:
    TransportPriorityQosPolicyWrapper() {
        value = 0;
    }

    TransportPriorityQosPolicyWrapper(const ::DDS::TransportPriorityQosPolicy native) {
        value = native.value;
    }

    operator ::DDS::TransportPriorityQosPolicy() const {
        ::DDS::TransportPriorityQosPolicy native;
        native.value = value;
        return native;
    }
};

EXTERN_STRUCT_EXPORT WriterDataLifecycleQosPolicyWrapper
{
    CORBA::Boolean autodispose_unregistered_instances;

public:
    WriterDataLifecycleQosPolicyWrapper() {
        autodispose_unregistered_instances = true;
    }

    WriterDataLifecycleQosPolicyWrapper(const ::DDS::WriterDataLifecycleQosPolicy native) {
        autodispose_unregistered_instances = native.autodispose_unregistered_instances;
    }

    operator ::DDS::WriterDataLifecycleQosPolicy() const {
        ::DDS::WriterDataLifecycleQosPolicy native;
        native.autodispose_unregistered_instances = autodispose_unregistered_instances;
        return native;
    }
};

EXTERN_STRUCT_EXPORT DomainParticipantQosWrapper
{
    UserDataQosPolicyWrapper user_data;
    EntityFactoryQosPolicyWrapper entity_factory;

public:
    DomainParticipantQosWrapper();

    DomainParticipantQosWrapper(const ::DDS::DomainParticipantQos native) {
        user_data = native.user_data;
        entity_factory = native.entity_factory;
    }

    operator ::DDS::DomainParticipantQos() const {
        ::DDS::DomainParticipantQos native;
        native.user_data = user_data;
        native.entity_factory = entity_factory;
        return native;
    }
};

EXTERN_STRUCT_EXPORT PublisherQosWrapper 
{
    PresentationQosPolicyWrapper presentation;
    PartitionQosPolicyWrapper partition;
    GroupDataQosPolicyWrapper group_data;
    EntityFactoryQosPolicyWrapper entity_factory;

public:
    PublisherQosWrapper();

    PublisherQosWrapper(const ::DDS::PublisherQos native) {
        presentation = native.presentation;
        partition = native.partition;
        group_data = native.group_data;
        entity_factory = native.entity_factory;
    }

    operator ::DDS::PublisherQos() const {
        ::DDS::PublisherQos native;
        native.presentation = presentation;
        native.partition = partition;
        native.group_data = group_data;
        native.entity_factory = entity_factory;
        return native;
    }
};

EXTERN_STRUCT_EXPORT SubscriberQosWrapper
{
    PresentationQosPolicyWrapper presentation;
    PartitionQosPolicyWrapper partition;
    GroupDataQosPolicyWrapper group_data;
    EntityFactoryQosPolicyWrapper entity_factory;

public:
    SubscriberQosWrapper();

    SubscriberQosWrapper(const ::DDS::SubscriberQos native) {
        presentation = native.presentation;
        partition = native.partition;
        group_data = native.group_data;
        entity_factory = native.entity_factory;
    }

    operator ::DDS::SubscriberQos() const {
        ::DDS::SubscriberQos native;
        native.presentation = presentation;
        native.partition = partition;
        native.group_data = group_data;
        native.entity_factory = entity_factory;
        return native;
    }
};

EXTERN_STRUCT_EXPORT TopicQosWrapper
{
    TopicDataQosPolicyWrapper topic_data;
    DurabilityQosPolicyWrapper durability;
    DurabilityServiceQosPolicyWrapper durability_service;
    DeadlineQosPolicyWrapper deadline;
    LatencyBudgetQosPolicyWrapper latency_budget;
    LivelinessQosPolicyWrapper liveliness;
    ReliabilityQosPolicyWrapper reliability;
    DestinationOrderQosPolicyWrapper destination_order;
    HistoryQosPolicyWrapper history;
    ResourceLimitsQosPolicyWrapper resource_limits;
    TransportPriorityQosPolicyWrapper transport_priority;
    LifespanQosPolicyWrapper lifespan;
    OwnershipQosPolicyWrapper ownership;

public:
    TopicQosWrapper();

    TopicQosWrapper(const ::DDS::TopicQos native) {
        topic_data = native.topic_data;
        durability = native.durability;
        durability_service = native.durability_service;
        deadline = native.deadline;
        latency_budget = native.latency_budget;
        liveliness = native.liveliness;
        reliability = native.reliability;
        destination_order = native.destination_order;
        history = native.history;
        resource_limits = native.resource_limits;
        transport_priority = native.transport_priority;
        lifespan = native.lifespan;
        ownership = native.ownership;
    }

    operator ::DDS::TopicQos() const {
        ::DDS::TopicQos native;
        native.topic_data = topic_data;
        native.durability = durability;
        native.durability_service = durability_service;
        native.deadline = deadline;
        native.latency_budget = latency_budget;
        native.liveliness = liveliness;
        native.reliability = reliability;
        native.destination_order = destination_order;
        native.history = history;
        native.resource_limits = resource_limits;
        native.transport_priority = transport_priority;
        native.lifespan = lifespan;
        native.ownership = ownership;        
        return native;
    }
};

EXTERN_STRUCT_EXPORT DataWriterQosWrapper
{
    DurabilityQosPolicyWrapper durability;
    DurabilityServiceQosPolicyWrapper durability_service;
    DeadlineQosPolicyWrapper deadline;
    LatencyBudgetQosPolicyWrapper latency_budget;
    LivelinessQosPolicyWrapper liveliness;
    ReliabilityQosPolicyWrapper reliability;
    DestinationOrderQosPolicyWrapper destination_order;
    HistoryQosPolicyWrapper history;
    ResourceLimitsQosPolicyWrapper resource_limits;
    TransportPriorityQosPolicyWrapper transport_priority;
    LifespanQosPolicyWrapper lifespan;
    OwnershipQosPolicyWrapper ownership;
    UserDataQosPolicyWrapper user_data;
    OwnershipStrengthQosPolicyWrapper ownership_strength;
    WriterDataLifecycleQosPolicyWrapper writer_data_lifecycle;

public:
    DataWriterQosWrapper();

    DataWriterQosWrapper(const ::DDS::DataWriterQos native) {
        durability = native.durability;
        durability_service = native.durability_service;
        deadline = native.deadline;
        latency_budget = native.latency_budget;
        liveliness = native.liveliness;
        reliability = native.reliability;
        destination_order = native.destination_order;
        history = native.history;
        resource_limits = native.resource_limits;
        transport_priority = native.transport_priority;
        lifespan = native.lifespan;
        ownership = native.ownership;
        user_data = native.user_data;
        ownership_strength = native.ownership_strength;
        writer_data_lifecycle = native.writer_data_lifecycle;
    }

    operator ::DDS::DataWriterQos() const {
        ::DDS::DataWriterQos native;
        native.durability = durability;
        native.durability_service = durability_service;
        native.deadline = deadline;
        native.latency_budget = latency_budget;
        native.liveliness = liveliness;
        native.reliability = reliability;
        native.destination_order = destination_order;
        native.history = history;
        native.resource_limits = resource_limits;
        native.transport_priority = transport_priority;
        native.lifespan = lifespan;
        native.ownership = ownership;
        native.user_data = user_data;
        native.ownership_strength = ownership_strength;
        native.writer_data_lifecycle = writer_data_lifecycle;
        return native;
    }
};

EXTERN_STRUCT_EXPORT DataReaderQosWrapper
{
     DurabilityQosPolicyWrapper durability;
     DeadlineQosPolicyWrapper deadline;
     LatencyBudgetQosPolicyWrapper latency_budget;
     LivelinessQosPolicyWrapper liveliness;
     ReliabilityQosPolicyWrapper reliability;
     DestinationOrderQosPolicyWrapper destination_order;
     HistoryQosPolicyWrapper history;
     ResourceLimitsQosPolicyWrapper resource_limits;
     UserDataQosPolicyWrapper user_data;
     OwnershipQosPolicyWrapper ownership;
     TimeBasedFilterQosPolicyWrapper time_based_filter;
     ReaderDataLifecycleQosPolicyWrapper reader_data_lifecycle;

public:
    DataReaderQosWrapper();

    DataReaderQosWrapper(const ::DDS::DataReaderQos native) {
        durability = native.durability;
        deadline = native.deadline;
        latency_budget = native.latency_budget;
        liveliness = native.liveliness;
        reliability = native.reliability;
        destination_order = native.destination_order;
        history = native.history;
        resource_limits = native.resource_limits;
        user_data = native.user_data;
        ownership = native.ownership;
        time_based_filter = native.time_based_filter;
        reader_data_lifecycle = native.reader_data_lifecycle;
    }

    operator ::DDS::DataReaderQos() const {
        ::DDS::DataReaderQos native;
        native.durability = durability;
        native.deadline = deadline;
        native.latency_budget = latency_budget;
        native.liveliness = liveliness;
        native.reliability = reliability;
        native.destination_order = destination_order;
        native.history = history;
        native.resource_limits = resource_limits;
        native.user_data = user_data;
        native.ownership = ownership;
        native.time_based_filter = time_based_filter;
        native.reader_data_lifecycle = reader_data_lifecycle;
        return native;
    }
};