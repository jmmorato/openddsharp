#pragma once

#include "ace/Basic_Types.h"
#include "tao/Unbounded_Value_Sequence_T.h"

class marshal {

public:
    template <typename T>
    static void ptr_to_unbounded_sequence(void* ptr, TAO::unbounded_value_sequence<T> & sequence)
    {
        char* bytes = (char*)ptr;
        // First 4 bytes are the length of the array
        ACE_UINT32 length = 0;
        ACE_OS::memcpy(&length, bytes, sizeof length);
        sequence.length(length);

        // The rest of the memory is the structures aligned one after the other
        const ACE_UINT64 structs_offset = sizeof length;
        const ACE_UINT64 struct_size = sizeof T;
        for (int i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&sequence[i], &bytes[(i * struct_size) + structs_offset], struct_size);
        }
    }

    template <typename T>
    static void unbounded_sequence_to_ptr(TAO::unbounded_value_sequence<T> sequence, void* & ptr)
    {
        ACE_UINT32 length = sequence.length();
        const ACE_UINT64 struct_size = sizeof T;
        const ACE_UINT64 buffer_size = (length * struct_size) + sizeof length;
        char* bytes = new char[buffer_size];
        ACE_OS::memcpy(bytes, &length, sizeof length);

        for (int i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &sequence[i], struct_size);
        }        

        // Alloc memory for the poninter
        ACE_NEW(ptr, char[buffer_size], 0);
        // Copy the bytes in the pointer
        ACE_OS::memcpy(ptr, bytes, buffer_size);

        // Free temporally allocated memory
        delete[] bytes;
    }
};