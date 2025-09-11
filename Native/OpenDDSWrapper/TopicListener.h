/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "TopicListenerImpl.h"
#include "ListenerDelegates.h"

EXTERN_METHOD_EXPORT
OpenDDSharp::OpenDDS::DDS::TopicListenerImpl_ptr TopicListener_New(void *onInconsistentTopic);

EXTERN_METHOD_EXPORT
void TopicListener_Dispose(OpenDDSharp::OpenDDS::DDS::TopicListenerImpl_ptr ptr);