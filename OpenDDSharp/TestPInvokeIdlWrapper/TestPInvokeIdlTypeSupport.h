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

EXTERN_STRUCT_EXPORT NestedTestStruct
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
    int LongArray[5];
    char* StringArray[10];
    wchar_t* WStringArray[4];
    NestedTestStruct StructTest;

    Test::BasicTestStruct to_native()
    {
        Test::BasicTestStruct nativeData;
        nativeData.Id = Id;
        if (Message != NULL)
            nativeData.Message = CORBA::string_dup(Message);
        if (WMessage)
            nativeData.WMessage = CORBA::wstring_dup(WMessage);

        marshal::ptr_to_unbounded_sequence(LongSequence, nativeData.LongSequence);
        marshal::ptr_to_unbounded_basic_string_sequence(StringSequence, nativeData.StringSequence);

        for (int i = 0; i < 5; i++)
            nativeData.LongArray[i] = LongArray[i];

        for (int i = 0; i < 10; i++)
            nativeData.StringArray[i] = CORBA::string_dup(StringArray[i]);

        for (int i = 0; i < 4; i++)
            nativeData.WStringArray[i] = CORBA::wstring_dup(WStringArray[i]);
        
        nativeData.StructTest = StructTest.to_native();

        return nativeData;
    }

    void from_native(Test::BasicTestStruct nativeData)
    {
        Id = nativeData.Id;
        Message = CORBA::string_dup(nativeData.Message);
        WMessage = CORBA::wstring_dup(nativeData.WMessage);

        marshal::unbounded_sequence_to_ptr(nativeData.LongSequence, LongSequence);
        marshal::unbounded_basic_string_sequence_to_ptr(nativeData.StringSequence, StringSequence);

        for (int i = 0; i < 5; i++)
        {
            LongArray[i] = nativeData.LongArray[i];
        }

        for (int i = 0; i < 10; i++)
        {
            if (nativeData.StringArray[i] != NULL)
            {
                StringArray[i] = CORBA::string_dup(nativeData.StringArray[i]);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (nativeData.WStringArray[i] != NULL)
            {
                WStringArray[i] = CORBA::wstring_dup(nativeData.WStringArray[i]);
            }
        }

        StructTest.from_native(nativeData.StructTest);
    }

    void release() 
    {
        if (Message != NULL)
            CORBA::string_free(Message);
        if (WMessage)
            CORBA::wstring_free(WMessage);
        delete LongSequence;
        marshal::release_unbounded_basic_string_sequence_ptr(StringSequence);
        for (int i = 0; i < 10; i++)
        {
            if (StringArray[i] != NULL)
                CORBA::string_free(StringArray[i]);
        }

        for (int i = 0; i < 4; i++)
        {
            if (WStringArray[i] != NULL)
                CORBA::wstring_free(WStringArray[i]);
        }

        //StructTest.release();
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