/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "marshal.h"

void release_native_ptr(void *ptr) {
  ACE_OS::free(ptr);
}

void release_basic_string_ptr(char *ptr) {
  CORBA::string_free(ptr);
}

void release_wide_string_ptr(wchar_t *ptr) {
  CORBA::wstring_free(ptr);
}

void release_basic_string_sequence_ptr(void *&ptr) {
  if (ptr == NULL) {
    return;
  }

  char *bytes = (char *) ptr;

  // First 4 bytes are the length of the array
  ACE_UINT32 length = 0;
  ACE_OS::memcpy(&length, bytes, sizeof length);

  const size_t structs_offset = sizeof length;
  const size_t struct_size = sizeof(char *);
  char **pointers = new char *[length];
  for (ACE_UINT32 i = 0; i < length; i++) {
    ACE_OS::memcpy(&pointers[i], &bytes[(i * struct_size) + structs_offset], struct_size);

    CORBA::string_free(pointers[i]);
  }

  delete[] pointers;

  ACE_OS::free(ptr);
}

void release_wide_string_sequence_ptr(void *&ptr) {
  if (ptr == NULL) {
    return;
  }

  char *bytes = (char *) ptr;

  // First 4 bytes are the length of the array
  ACE_UINT32 length = 0;
  ACE_OS::memcpy(&length, bytes, sizeof length);

  const size_t structs_offset = sizeof length;
  const size_t struct_size = sizeof(wchar_t *);
  wchar_t **pointers = new wchar_t *[length];
  for (ACE_UINT32 i = 0; i < length; i++) {
    ACE_OS::memcpy(&pointers[i], &bytes[(i * struct_size) + structs_offset], struct_size);

    CORBA::wstring_free(pointers[i]);
  }

  delete[] pointers;

  ACE_OS::free(ptr);
}