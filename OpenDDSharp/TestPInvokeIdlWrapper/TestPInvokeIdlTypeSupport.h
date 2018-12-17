#pragma once

#include "TestPInvokeIdlTypeSupportImpl.h"
#include "marshal.h"

#ifndef EXTERN_METHOD_EXPORT
    #define EXTERN_METHOD_EXPORT extern "C" __declspec(dllexport)
#endif

#ifndef EXTERN_STRUCT_EXPORT
    #define EXTERN_STRUCT_EXPORT extern "C" struct
#endif

#ifndef EXTERN_CLASS_EXPORT
    #define EXTERN_CLASS_EXPORT extern "C" class
#endif

EXTERN_STRUCT_EXPORT NestedTestStructWrapper
{
    int Id;
    char* Message;

    Test::NestedTestStruct to_native()
    {        
        Test::NestedTestStruct nativeData;
        nativeData.Id = Id;
        if (Message != NULL)
            nativeData.Message = CORBA::string_dup(Message);

        return nativeData;
    }

    void from_native(Test::NestedTestStruct nativeData)
    {
        Id = nativeData.Id;
        Message = CORBA::string_dup(nativeData.Message);
    }

    void release()
    {
        if (Message != NULL)
            CORBA::string_free(Message);
    }
};

/////////////////////////////////////////////////
// STRUCT WRAPPER
/////////////////////////////////////////////////
EXTERN_STRUCT_EXPORT BasicTestStructWrapper
{
    int Id;
    char* Message;
    wchar_t* WMessage;
    void* LongSequence;
    void* StringSequence;
    void* WStringSequence;
    int LongArray[5];
    char* StringArray[10];
    wchar_t* WStringArray[4];
    NestedTestStructWrapper StructTest;
    void* StructSequence;
    NestedTestStructWrapper StructArray[5];
    void* LongMultiArray;
    void* StringMultiArray;
    void* WStringMultiArray;
    void* StructMultiArray;

    Test::BasicTestStruct to_native()
    {
        Test::BasicTestStruct nativeData;

        // Primitives
        nativeData.Id = Id;

        // String
        if (Message != NULL)
        {
            nativeData.Message = CORBA::string_dup(Message);
        }

        // WString
        if (WMessage)
        {
            nativeData.WMessage = CORBA::wstring_dup(WMessage);
        }

        // Sequence of primitives
        if (LongSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(LongSequence, nativeData.LongSequence);
        }

        // Sequence of strings
        if (StringSequence != NULL)
        {
            marshal::ptr_to_unbounded_basic_string_sequence(StringSequence, nativeData.StringSequence);
        }

        // Sequence of wstrings
        if (WStringSequence != NULL)
        {
            marshal::ptr_to_unbounded_wide_string_sequence(WStringSequence, nativeData.WStringSequence);
        }

        // Array of primitives
        if (LongArray != NULL)
        {
            ACE_OS::memcpy(nativeData.LongArray, LongArray, sizeof(int) * 5);
        }

        // Array of string
        if (StringArray != NULL)
        {
            for (int i = 0; i < 10; i++)
            {
                if (StringArray[i] != NULL)
                {
                    nativeData.StringArray[i] = CORBA::string_dup(StringArray[i]);
                }
            }
        }

        // Array of wstring
        if (WStringArray != NULL)
        {
            for (int i = 0; i < 4; i++)
            {
                if (WStringArray[i] != NULL)
                {
                    nativeData.WStringArray[i] = CORBA::wstring_dup(WStringArray[i]);
                }
            }
        }

        // Structure        
        nativeData.StructTest = StructTest.to_native();

        // Sequence of structures need to be retrieved with the wrapper struct and then casted to the native struct.
        if (StructSequence != NULL)
        {
            TAO::unbounded_value_sequence<NestedTestStructWrapper> aux;
            marshal::ptr_to_unbounded_sequence(StructSequence, aux);
            ACE_UINT32 length = aux.length();
            nativeData.StructSequence.length(length);
            for (ACE_UINT32 i = 0; i < length; i++)
            {
                nativeData.StructSequence[i] = aux[i].to_native();
            }
        }

        // Arrays of structs
        if (StructArray != NULL)
        {
            for (int i = 0; i < 5; i++)
            {
                nativeData.StructArray[i] = StructArray[i].to_native();
            }
        }

        // Multi-dimensional array of primitives 
        if (LongMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.LongMultiArray, LongMultiArray, sizeof(int) * 24);
        }

        // Multi-dimensional array of strings
        if (StringMultiArray != NULL)
        {
            char** arr_StringMultiArray = new char*[24];
            marshal::ptr_to_basic_string_multi_array(StringMultiArray, arr_StringMultiArray, 24);
            ACE_OS::memcpy(nativeData.StringMultiArray, arr_StringMultiArray, sizeof(char*) * 24);
            delete[] arr_StringMultiArray;
        }

        // Multi-dimensional array of wstrings
        if (WStringMultiArray != NULL)
        {
            wchar_t** arr_WStringMultiArray = new wchar_t*[24];
            marshal::ptr_to_wide_string_multi_array(WStringMultiArray, arr_WStringMultiArray, 24);
            ACE_OS::memcpy(nativeData.WStringMultiArray, arr_WStringMultiArray, sizeof(wchar_t*) * 24);
            delete[] arr_WStringMultiArray;
        }

        // Multi-dimensional array of structs
        if (StructMultiArray != NULL)
        {
            NestedTestStructWrapper arr_StructMultiArray[24];
            ACE_OS::memcpy(arr_StructMultiArray, StructMultiArray, sizeof(NestedTestStructWrapper) * 24);

            int i = 0;
            for (ACE_UINT32 i0 = 0; i0 < 3; ++i0) {
                for (ACE_UINT32 i1 = 0; i1 < 4; ++i1) {
                    for (ACE_UINT32 i2 = 0; i2 < 2; ++i2) {
                        nativeData.StructMultiArray[i0][i1][i2] = arr_StructMultiArray[i].to_native();
                        i++;
                    }
                }
            }
        }

        return nativeData;
    }

    void from_native(Test::BasicTestStruct nativeData)
    {
        // Primitives
        Id = nativeData.Id;

        // String
        if (nativeData.Message != NULL)
        {
            Message = CORBA::string_dup(nativeData.Message);
        }

        // WString
        if (nativeData.WMessage != NULL)
        {
            WMessage = CORBA::wstring_dup(nativeData.WMessage);
        }

        // Sequence of primitives
        marshal::unbounded_sequence_to_ptr(nativeData.LongSequence, LongSequence);

        // Sequence of string        
        marshal::unbounded_basic_string_sequence_to_ptr(nativeData.StringSequence, StringSequence);

        // Sequence of wstring
        marshal::unbounded_wide_string_sequence_to_ptr(nativeData.WStringSequence, WStringSequence);

        // Arrays of primitives
        if (nativeData.LongArray != NULL)
        {
            ACE_OS::memcpy(LongArray, nativeData.LongArray, sizeof(int) * 5);
        }

        // Arrays of strings
        if (nativeData.StringArray != NULL)
        {
            for (int i = 0; i < 10; i++)
            {
                if (nativeData.StringArray[i] != NULL)
                {
                    StringArray[i] = CORBA::string_dup(nativeData.StringArray[i]);
                }
            }
        }

        // Arrays of wstrings
        if (nativeData.WStringArray != NULL)
        {
            for (int i = 0; i < 4; i++)
            {
                if (nativeData.WStringArray[i] != NULL)
                {
                    WStringArray[i] = CORBA::wstring_dup(nativeData.WStringArray[i]);
                }
            }
        }

        // Structures        
        StructTest.from_native(nativeData.StructTest);

        // Sequence of structures need to be casted to the wrapper struct and then used to marshal the pointer.
        {
            TAO::unbounded_value_sequence<NestedTestStructWrapper> aux;
            ACE_UINT32 length = nativeData.StructSequence.length();
            aux.length(length);
            for (ACE_UINT32 i = 0; i < length; i++)
            {
                aux[i].from_native(nativeData.StructSequence[i]);
            }
            marshal::unbounded_sequence_to_ptr(aux, StructSequence);
        }

        // Arrays of structructures
        if (nativeData.StructArray != NULL)
        {
            for (ACE_UINT32 i = 0; i < 5; i++)
            {
                StructArray[i].from_native(nativeData.StructArray[i]);
            }
        }

        // Multi-dimensional array of primitives
        if (nativeData.LongMultiArray != NULL)
        {
            LongMultiArray = ACE_OS::malloc(sizeof(int) * 24);
            ACE_OS::memcpy(LongMultiArray, nativeData.LongMultiArray, sizeof(int) * 24);
        }

        // Multi-dimensional array of strings
        if (nativeData.StringMultiArray != NULL)
        {
            char** arr_StringMultiArray = new char*[24];
            ACE_OS::memcpy(arr_StringMultiArray, nativeData.StringMultiArray, sizeof(char*) * 24);
            marshal::basic_string_multi_array_to_ptr(arr_StringMultiArray, StringMultiArray, 24);
            delete[] arr_StringMultiArray;
        }

        // Multi-dimensional array of strings
        if (nativeData.WStringMultiArray != NULL)
        {
            wchar_t** arr_WStringMultiArray = new wchar_t*[24];
            ACE_OS::memcpy(arr_WStringMultiArray, nativeData.WStringMultiArray, sizeof(wchar_t*) * 24);
            marshal::wide_string_multi_array_to_ptr(arr_WStringMultiArray, WStringMultiArray, 24);
            delete[] arr_WStringMultiArray;
        }

        // Multi-dimensional array of structs
        if (nativeData.StructMultiArray != NULL)
        {
            NestedTestStructWrapper arr_StructMultiArray[24];
            int i = 0;
            for (ACE_UINT32 i0 = 0; i0 < 3; ++i0) {
                for (ACE_UINT32 i1 = 0; i1 < 4; ++i1) {
                    for (ACE_UINT32 i2 = 0; i2 < 2; ++i2) {
                        arr_StructMultiArray[i].from_native(nativeData.StructMultiArray[i0][i1][i2]);
                        i++;
                    }
                }
            }

            StructMultiArray = ACE_OS::malloc(sizeof(NestedTestStructWrapper) * 24);
            ACE_OS::memcpy(StructMultiArray, arr_StructMultiArray, sizeof(NestedTestStructWrapper) * 24);
        }
    }

    void release() 
    {
        // Strings need to be released
        if (Message != NULL)
        {
            CORBA::string_free(Message);
        }

        // WStrings need to be released
        if (WMessage != NULL)
        {
            CORBA::wstring_free(WMessage);
        }

        // Release the pointer of the sequences
        if (LongSequence != NULL)
        {
            ACE_OS::free(LongSequence);
        }

        // Release the strings in the sequence
        if (StringSequence != NULL)
        {
            marshal::release_unbounded_basic_string_sequence_ptr(StringSequence);
        }

        // Release the wstrings in the sequence
        if (WStringSequence != NULL)
        {
            marshal::release_unbounded_wide_string_sequence_ptr(WStringSequence);
        }

        // Release the strings in the array 
        if (StringArray != NULL)
        {
            for (int i = 0; i < 10; i++)
            {
                if (StringArray[i] != NULL)
                {
                    CORBA::string_free(StringArray[i]);
                }
            }
        }

        if (WStringArray != NULL)
        {
            // Release the wstrings in the array 
            for (int i = 0; i < 4; i++)
            {
                if (WStringArray[i] != NULL)
                {
                    CORBA::wstring_free(WStringArray[i]);
                }
            }
        }

        // Release the structures to ensure the needed fields are released
        StructTest.release();     

        // Release the pointer of the sequences
        if (StructSequence != NULL)
        {
            ACE_OS::free(StructSequence);
        }

        // Release the structures in the array
        if (StructArray != NULL)
        {
            for (int i = 0; i < 5; i++)
            {
                StructArray[i].release();
            }
        }

        // Release pointer to the multi-dimensional array of primitives
        if (LongMultiArray != NULL)
        {
            ACE_OS::free(LongMultiArray);
        }

        // Release pointer to the multi-dimensional array of strings
        if (StringMultiArray != NULL)
        {
            marshal::release_basic_string_multi_array_ptr(StringMultiArray, 24);
        }

        // Release pointer to the multi-dimensional array of wstrings
        if (WStringMultiArray != NULL)
        {
            marshal::release_wide_string_multi_array_ptr(WStringMultiArray, 24);
        }

        // Release pointer to the multi-dimensional array of structs
        if (StructMultiArray != NULL)
        {
            NestedTestStructWrapper arr_StructMultiArray[24];
            ACE_OS::memcpy(arr_StructMultiArray, StructMultiArray, sizeof(NestedTestStructWrapper) * 24);
            for (int i = 0; i < 24; i++)
            {
                arr_StructMultiArray[i].release();
            }
            ACE_OS::free(StructMultiArray);
        }
    }
};

EXTERN_METHOD_EXPORT void BasicTestStructWrapper_release(BasicTestStructWrapper* obj);

/////////////////////////////////////////////////
// TYPE SUPPORT METHODS
/////////////////////////////////////////////////
EXTERN_METHOD_EXPORT Test::BasicTestStructTypeSupport_ptr BasicTestStructTypeSupport_new();

EXTERN_METHOD_EXPORT char* BasicTestStructTypeSupport_GetTypeName(Test::BasicTestStructTypeSupport_ptr native);

EXTERN_METHOD_EXPORT int BasicTestStructTypeSupport_RegisterType(Test::BasicTestStructTypeSupport_ptr native, ::DDS::DomainParticipant_ptr dp, const char* typeName);

EXTERN_METHOD_EXPORT int BasicTestStructTypeSupport_UnregisterType(Test::BasicTestStructTypeSupport_ptr native, ::DDS::DomainParticipant_ptr dp, const char* typeName);

/////////////////////////////////////////////////
// DATAWRITER METHODS
/////////////////////////////////////////////////
EXTERN_METHOD_EXPORT Test::BasicTestStructDataWriter_ptr BasicTestStructDataWriter_Narrow(DDS::DataWriter_ptr dw);

EXTERN_METHOD_EXPORT int BasicTestStructDataWriter_Write(Test::BasicTestStructDataWriter_ptr dw, BasicTestStructWrapper* data, int handle);

/////////////////////////////////////////////////
// DATAREADER METHODS
/////////////////////////////////////////////////
EXTERN_METHOD_EXPORT Test::BasicTestStructDataReader_ptr BasicTestStructDataReader_Narrow(DDS::DataReader_ptr dr);

EXTERN_METHOD_EXPORT int BasicTestStructDataReader_ReadNextSample(Test::BasicTestStructDataReader_ptr dr, BasicTestStructWrapper* data);

EXTERN_METHOD_EXPORT int BasicTestStructDataReader_Read(Test::BasicTestStructDataReader_ptr dr);