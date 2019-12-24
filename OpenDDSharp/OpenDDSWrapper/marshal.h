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

template <typename T>
static void unbounded_sequence_to_ptr(TAO::unbounded_value_sequence<T> sequence, void* & ptr)
{
	ACE_UINT32 length = sequence.length();
	const ACE_UINT64 struct_size = sizeof T;
	const ACE_UINT64 buffer_size = (length * struct_size) + sizeof length;
	char* bytes = new char[buffer_size];
	ACE_OS::memcpy(bytes, &length, sizeof length);

	for (ACE_UINT32 i = 0; i < length; i++)
	{
		ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &sequence[i], struct_size);
	}

	// Alloc memory for the pointer
	ptr = ACE_OS::malloc(buffer_size);
	// Copy the bytes in the pointer
	ACE_OS::memcpy(ptr, bytes, buffer_size);

	// Free temporally allocated memory
	delete[] bytes;
}

EXTERN_METHOD_EXPORT
void release_native_ptr(void* ptr) {
	ACE_OS::free(ptr);
}
