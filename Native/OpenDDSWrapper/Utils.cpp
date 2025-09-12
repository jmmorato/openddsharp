/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "Utils.h"

void Utils_CreateOctetSeq(unsigned char bytes[], ::DDS::OctetSeq *seq) {
  if (bytes == NULL) {
    seq->length(0);
    return;
  }

  int size = static_cast<int>(*(&bytes + 1) - bytes);
  seq->length(size);
  for (int i = 0; i < size; i++) {
    seq[i] = bytes[i];
  }
}