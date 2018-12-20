#pragma once

#include "ace/Basic_Types.h"
#include "tao/Unbounded_Value_Sequence_T.h"

class marshal {

public:
    template <typename T>
    static void ptr_to_unbounded_sequence(void* ptr, TAO::unbounded_value_sequence<T> & sequence)
    {
        if (ptr == NULL)
        {
            return;
        }

        char* bytes = (char*)ptr;

        // First 4 bytes are the length of the array
        ACE_UINT32 length = 0;
        ACE_OS::memcpy(&length, bytes, sizeof length);
        sequence.length(length);

        // The rest of the memory is the structures aligned one after the other
        const ACE_UINT64 structs_offset = sizeof length;
        const ACE_UINT64 struct_size = sizeof T;
        for (ACE_UINT32 i = 0; i < length; i++)
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

        for (ACE_UINT32 i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &sequence[i], struct_size);
        }        

        // Alloc memory for the poninter
        ptr = ACE_OS::malloc(buffer_size);        
        // Copy the bytes in the pointer
        ACE_OS::memcpy(ptr, bytes, buffer_size);

        // Free temporally allocated memory
        delete[] bytes;
    }

    static void ptr_to_unbounded_basic_string_sequence(void* ptr, TAO::unbounded_basic_string_sequence<char> & sequence)
    {
        if (ptr == NULL)
        {
            return;
        }

        char* bytes = (char*)ptr;

        // First 4 bytes are the length of the array
        ACE_UINT32 length = 0;
        ACE_OS::memcpy(&length, bytes, sizeof length);
        sequence.length(length);

        const ACE_UINT64 structs_offset = sizeof length;
        const ACE_UINT64 struct_size = sizeof(char*);
        char** pointers = new char*[length];
        for (ACE_UINT32 i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&pointers[i], &bytes[(i * struct_size) + structs_offset], struct_size);

            sequence[i] = CORBA::string_dup(pointers[i]);
        }

        delete[] pointers;        
    }

    static void ptr_to_unbounded_wide_string_sequence(void* ptr, TAO::unbounded_basic_string_sequence<wchar_t> & sequence)
    {
        if (ptr == NULL)
        {
            return;
        }

        char* bytes = (char*)ptr;

        // First 4 bytes are the length of the array
        ACE_UINT32 length = 0;
        ACE_OS::memcpy(&length, bytes, sizeof length);
        sequence.length(length);

        const ACE_UINT64 structs_offset = sizeof length;
        const ACE_UINT64 struct_size = sizeof(wchar_t*);
        wchar_t** pointers = new wchar_t*[length];
        for (ACE_UINT32 i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&pointers[i], &bytes[(i * struct_size) + structs_offset], struct_size);

            sequence[i] = CORBA::wstring_dup(pointers[i]);
        }

        delete[] pointers;
    }

    static void unbounded_basic_string_sequence_to_ptr(TAO::unbounded_basic_string_sequence<char> & sequence, void* & ptr)
    {
        ACE_UINT32 length = sequence.length();
        const ACE_UINT64 struct_size = sizeof(char*);
        const ACE_UINT64 buffer_size = (length * struct_size) + sizeof length;
        char* bytes = new char[buffer_size];
        ACE_OS::memcpy(bytes, &length, sizeof length);

        for (ACE_UINT32 i = 0; i < length; i++)
        {
            char* str = CORBA::string_dup(sequence[i]);
            ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &str, struct_size);
        }

        // Alloc memory for the poninter
        ptr = ACE_OS::malloc(buffer_size);        
        // Copy the bytes in the pointer
        ACE_OS::memcpy(ptr, bytes, buffer_size);

        // Free temporally allocated memory
        delete[] bytes;
    }

    static void unbounded_wide_string_sequence_to_ptr(TAO::unbounded_basic_string_sequence<wchar_t> & sequence, void* & ptr)
    {
        ACE_UINT32 length = sequence.length();
        const ACE_UINT64 struct_size = sizeof(wchar_t*);
        const ACE_UINT64 buffer_size = (length * struct_size) + sizeof length;
        char* bytes = new char[buffer_size];
        ACE_OS::memcpy(bytes, &length, sizeof length);

        for (ACE_UINT32 i = 0; i < length; i++)
        {
            wchar_t* str = CORBA::wstring_dup(sequence[i]);
            ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &str, struct_size);
        }

        // Alloc memory for the poninter
        ptr = ACE_OS::malloc(buffer_size);        
        // Copy the bytes in the pointer
        ACE_OS::memcpy(ptr, bytes, buffer_size);

        // Free temporally allocated memory
        delete[] bytes;
    }

    static void release_unbounded_basic_string_sequence_ptr(void* & ptr)
    {
        if (ptr == NULL)
        {
            return;
        }

        char* bytes = (char*)ptr;

        // First 4 bytes are the length of the array
        ACE_UINT32 length = 0;
        ACE_OS::memcpy(&length, bytes, sizeof length);

        const ACE_UINT64 structs_offset = sizeof length;
        const ACE_UINT64 struct_size = sizeof(char*);
        char** pointers = new char*[length];
        for (ACE_UINT32 i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&pointers[i], &bytes[(i * struct_size) + structs_offset], struct_size);

            CORBA::string_free(pointers[i]);
        }

        delete[] pointers;

        ACE_OS::free(ptr);
    }

    static void release_unbounded_wide_string_sequence_ptr(void* & ptr)
    {
        if (ptr == NULL)
        {
            return;
        }

        char* bytes = (char*)ptr;

        // First 4 bytes are the length of the array
        ACE_UINT32 length = 0;
        ACE_OS::memcpy(&length, bytes, sizeof length);

        const ACE_UINT64 structs_offset = sizeof length;
        const ACE_UINT64 struct_size = sizeof(wchar_t*);
        wchar_t** pointers = new wchar_t*[length];
        for (ACE_UINT32 i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&pointers[i], &bytes[(i * struct_size) + structs_offset], struct_size);

            CORBA::wstring_free(pointers[i]);
        }

        delete[] pointers;

        ACE_OS::free(ptr);
    }

    static void ptr_to_basic_string_multi_array(void* ptr, char** & arr, int length)
    {
        if (ptr == NULL)
        {
            return;
        }

        char* bytes = (char*)ptr;

        const ACE_UINT64 struct_size = sizeof(char*);
        char** pointers = new char*[length];
        for (ACE_INT32 i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&pointers[i], &bytes[i * struct_size], struct_size);

            arr[i] = CORBA::string_dup(pointers[i]);
        }

        delete[] pointers;
    }

    static void ptr_to_wide_string_multi_array(void* ptr, wchar_t** & arr, int length)
    {
        if (ptr == NULL)
        {
            return;
        }

        char* bytes = (char*)ptr;

        const ACE_UINT64 struct_size = sizeof(wchar_t*);
        wchar_t** pointers = new wchar_t*[length];
        for (ACE_INT32 i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&pointers[i], &bytes[i * struct_size], struct_size);

            arr[i] = CORBA::wstring_dup(pointers[i]);
        }

        delete[] pointers;
    }

    static void basic_string_multi_array_to_ptr(char** & arr, void* & ptr, int length)
    {
        const ACE_UINT64 struct_size = sizeof(char*);
        const ACE_UINT64 buffer_size = length * struct_size;
        char* bytes = new char[buffer_size];

        for (ACE_INT32 i = 0; i < length; i++)
        {
            char* str = CORBA::string_dup(arr[i]);
            ACE_OS::memcpy(&bytes[i * struct_size], &str, struct_size);
        }

        // Alloc memory for the poninter
        ptr = ACE_OS::malloc(buffer_size);        
        // Copy the bytes in the pointer
        ACE_OS::memcpy(ptr, bytes, buffer_size);

        // Free temporally allocated memory
        delete[] bytes;
    }

    static void wide_string_multi_array_to_ptr(wchar_t** & arr, void* & ptr, int length)
    {
        const ACE_UINT64 struct_size = sizeof(wchar_t*);
        const ACE_UINT64 buffer_size = length * struct_size;
        char* bytes = new char[buffer_size];

        for (ACE_INT32 i = 0; i < length; i++)
        {
            wchar_t* str = CORBA::wstring_dup(arr[i]);
            ACE_OS::memcpy(&bytes[i * struct_size], &str, struct_size);
        }

        // Alloc memory for the poninter
        ptr = ACE_OS::malloc(buffer_size);        
        // Copy the bytes in the pointer
        ACE_OS::memcpy(ptr, bytes, buffer_size);

        // Free temporally allocated memory
        delete[] bytes;
    }

    static void release_basic_string_multi_array_ptr(void* & ptr, int length)
    {
        if (ptr == NULL)
        {
            return;
        }

        char* bytes = (char*)ptr;
       
        const ACE_UINT64 struct_size = sizeof(char*);
        char** pointers = new char*[length];
        for (ACE_INT32 i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&pointers[i], &bytes[i * struct_size], struct_size);

            CORBA::string_free(pointers[i]);
        }

        delete[] pointers;

        delete ptr;
    }

    static void release_wide_string_multi_array_ptr(void* & ptr, int length)
    {
        if (ptr == NULL)
        {
            return;
        }

        char* bytes = (char*)ptr;

        const ACE_UINT64 struct_size = sizeof(wchar_t*);
        wchar_t** pointers = new wchar_t*[length];
        for (ACE_INT32 i = 0; i < length; i++)
        {
            ACE_OS::memcpy(&pointers[i], &bytes[i * struct_size], struct_size);

            CORBA::wstring_free(pointers[i]);
        }

        delete[] pointers;

        delete ptr;
    }
};