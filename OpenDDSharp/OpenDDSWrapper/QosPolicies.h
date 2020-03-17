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
    UserDataQosPolicyWrapper(::DDS::UserDataQosPolicy native) {
        unbounded_sequence_to_ptr(native.value, value);
    }

    operator ::DDS::UserDataQosPolicy() const {
        ::DDS::UserDataQosPolicy native;
        ptr_to_unbounded_sequence(value, native.value);                
        return native;
    }
};

EXTERN_STRUCT_EXPORT EntityFactoryQosPolicyWrapper
{
    bool autoenable_created_entities;
};

EXTERN_STRUCT_EXPORT DurabilityQosPolicyWrapper
{
    ::CORBA::Long kind;

public:
    DurabilityQosPolicyWrapper(::DDS::DurabilityQosPolicy native) {
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
    DurabilityServiceQosPolicyWrapper(::DDS::DurabilityServiceQosPolicy native) {
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
    DeadlineQosPolicyWrapper(::DDS::DeadlineQosPolicy native) {
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
    LatencyBudgetQosPolicyWrapper(::DDS::LatencyBudgetQosPolicy native) {
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
    LivelinessQosPolicyWrapper(::DDS::LivelinessQosPolicy native) {
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
    ReliabilityQosPolicyWrapper(::DDS::ReliabilityQosPolicy native) {
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
    LifespanQosPolicyWrapper(::DDS::LifespanQosPolicy native) {
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
    HistoryQosPolicyWrapper(::DDS::HistoryQosPolicy native) {
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
    ResourceLimitsQosPolicyWrapper(::DDS::ResourceLimitsQosPolicy native) {
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
    TimeBasedFilterQosPolicyWrapper(::DDS::TimeBasedFilterQosPolicy native) {
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
    ReaderDataLifecycleQosPolicyWrapper(::DDS::ReaderDataLifecycleQosPolicy native) {
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
    DestinationOrderQosPolicyWrapper(::DDS::DestinationOrderQosPolicy native) {
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
    OwnershipQosPolicyWrapper(::DDS::OwnershipQosPolicy native) {
        kind = (::CORBA::Long)native.kind;
    }

    operator ::DDS::OwnershipQosPolicy() const {
        ::DDS::OwnershipQosPolicy native;
        native.kind = (DDS::OwnershipQosPolicyKind)kind;
        return native;
    }
};

EXTERN_STRUCT_EXPORT DomainParticipantQosWrapper
{
    UserDataQosPolicyWrapper user_data;
    EntityFactoryQosPolicyWrapper entity_factory;
};

EXTERN_STRUCT_EXPORT PublisherQosWrapper 
{

};

EXTERN_STRUCT_EXPORT SubscriberQosWrapper
{

};

EXTERN_STRUCT_EXPORT TopicQosWrapper
{

};

EXTERN_STRUCT_EXPORT DataWriterQosWrapper
{

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
     TimeBasedFilterQosPolicyWrapper timeBasedFilter;
     ReaderDataLifecycleQosPolicyWrapper reader_data_lifecycle;
};