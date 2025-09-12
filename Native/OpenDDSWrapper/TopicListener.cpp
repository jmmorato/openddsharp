/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "TopicListener.h"

OpenDDSharp::OpenDDS::DDS::TopicListenerImpl_ptr TopicListener_New(void *onInconsistentTopic) {
  return new OpenDDSharp::OpenDDS::DDS::TopicListenerImpl(onInconsistentTopic);;
}

void TopicListener_Dispose(OpenDDSharp::OpenDDS::DDS::TopicListenerImpl_ptr ptr) {
  ptr->dispose();
}