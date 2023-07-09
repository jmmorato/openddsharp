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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#include "Utils.h"

#include "ace/Basic_Types.h"
#include "tao/Unbounded_Value_Sequence_T.h"
#include "tao/Unbounded_Basic_String_Sequence_T.h"

template<typename T>
static void unbounded_sequence_to_ptr(TAO::unbounded_value_sequence<T> sequence, void *&ptr) {
  ACE_UINT32 length = sequence.length();
  const size_t struct_size = sizeof(T);
  const size_t buffer_size = (length * struct_size) + sizeof length;
  char *bytes = new char[buffer_size];
  ACE_OS::memcpy(bytes, &length, sizeof length);

  for (ACE_UINT32 i = 0; i < length; i++) {
    ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &sequence[i], struct_size);
  }

  // Alloc memory for the pointer
  ptr = ACE_OS::malloc(buffer_size);

  // Copy the bytes in the pointer
  ACE_OS::memcpy(ptr, bytes, buffer_size);

  // Free temporally allocated memory
  delete[] bytes;
}

template<typename T>
static void ptr_to_unbounded_sequence(void *ptr, TAO::unbounded_value_sequence<T> &sequence) {
  if (ptr == NULL) {
    return;
  }

  char *bytes = (char *) ptr;

  // First 4 bytes are the length of the array
  ACE_UINT32 length = 0;
  ACE_OS::memcpy(&length, bytes, sizeof length);
  sequence.length(length);

  // The rest of the memory is the structures aligned one after the other
  const size_t structs_offset = sizeof length;
  const size_t struct_size = sizeof(T);
  for (ACE_UINT32 i = 0; i < length; i++) {
    ACE_OS::memcpy(&sequence[i], &bytes[(i * struct_size) + structs_offset], struct_size);
  }
}

static void unbounded_basic_string_sequence_to_ptr(TAO::unbounded_basic_string_sequence<char> &sequence, void *&ptr) {
  ACE_UINT32 length = sequence.length();
  const size_t struct_size = sizeof(char *);
  const size_t buffer_size = (length * struct_size) + sizeof length;
  char *bytes = new char[buffer_size];
  ACE_OS::memcpy(bytes, &length, sizeof length);

  for (ACE_UINT32 i = 0; i < length; i++) {
    char *str = CORBA::string_dup(sequence[i]);
    ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &str, struct_size);
  }

  // Alloc memory for the poninter
  ptr = ACE_OS::malloc(buffer_size);
  // Copy the bytes in the pointer
  ACE_OS::memcpy(ptr, bytes, buffer_size);

  // Free temporally allocated memory
  delete[] bytes;
}

static void ptr_to_unbounded_basic_string_sequence(void *ptr, TAO::unbounded_basic_string_sequence<char> &sequence) {
  if (ptr == NULL) {
    return;
  }

  char *bytes = (char *) ptr;

  // First 4 bytes are the length of the array
  ACE_UINT32 length = 0;
  ACE_OS::memcpy(&length, bytes, sizeof length);
  sequence.length(length);

  const size_t structs_offset = sizeof length;
  const size_t struct_size = sizeof(char *);
  char **pointers = new char *[length];
  for (ACE_UINT32 i = 0; i < length; i++) {
    ACE_OS::memcpy(&pointers[i], &bytes[(i * struct_size) + structs_offset], struct_size);

    sequence[i] = CORBA::string_dup(pointers[i]);
  }

  delete[] pointers;
}

EXTERN_METHOD_EXPORT void release_native_ptr(void *ptr);

EXTERN_METHOD_EXPORT void release_basic_string_ptr(char *ptr);

EXTERN_METHOD_EXPORT void release_wide_string_ptr(wchar_t *ptr);

EXTERN_METHOD_EXPORT void release_basic_string_sequence_ptr(void *&ptr);

EXTERN_METHOD_EXPORT void release_wide_string_sequence_ptr(void *&ptr);