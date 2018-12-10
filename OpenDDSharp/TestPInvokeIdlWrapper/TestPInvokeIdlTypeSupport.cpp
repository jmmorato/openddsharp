#include "TestPInvokeIdlTypeSupport.h"

void BasicTestStructWrapper_release(BasicTestStructWrapper* obj)
{
    if (obj->Message != NULL)
        CORBA::string_free(obj->Message);
    if (obj->WMessage)
        CORBA::wstring_free(obj->WMessage);
    delete obj->LongSequence;
    marshal::release_unbounded_basic_string_sequence_ptr(obj->StringSequence);    
    for (int i = 0; i < 10; i++)
    {
        if (obj->StringArray[i] != NULL)
            CORBA::string_free(obj->StringArray[i]);
    }

    for (int i = 0; i < 4; i++)
    {
        if (obj->WStringArray[i] != NULL)
            CORBA::wstring_free(obj->WStringArray[i]);
    }
}

Test::BasicTestStructTypeSupport_ptr BasicTestStructTypeSupport_new()
{
    return new Test::BasicTestStructTypeSupportImpl();
}

char* BasicTestStructTypeSupport_GetTypeName(Test::BasicTestStructTypeSupport_ptr native)
{
    return native->get_type_name();
}

int BasicTestStructTypeSupport_RegisterType(Test::BasicTestStructTypeSupport_ptr native, ::DDS::DomainParticipant_ptr dp, const char* typeName)
{
    return native->register_type(dp, typeName);
}

int BasicTestStructTypeSupport_UnregisterType(Test::BasicTestStructTypeSupport_ptr native, ::DDS::DomainParticipant_ptr dp, const char* typeName)
{
    return native->unregister_type(dp, typeName);
}

Test::BasicTestStructDataWriter_ptr BasicTestStructDataWriter_Narrow(DDS::DataWriter_ptr dw)
{
    return Test::BasicTestStructDataWriter::_narrow(dw);
}

int BasicTestStructDataWriter_Write(Test::BasicTestStructDataWriter_ptr dw, BasicTestStructWrapper* data, int handle)
{
    Test::BasicTestStruct nativeData;
    nativeData.Id = data->Id;
    if (data->Message != NULL)
        nativeData.Message = CORBA::string_dup(data->Message);
    if (data->WMessage)
        nativeData.WMessage = CORBA::wstring_dup(data->WMessage);

    marshal::ptr_to_unbounded_sequence(data->LongSequence, nativeData.LongSequence);
    marshal::ptr_to_unbounded_basic_string_sequence(data->StringSequence, nativeData.StringSequence);

    for(int i = 0; i < 5; i++)
        nativeData.LongArray[i] = data->LongArray[i];

    for (int i = 0; i < 10; i++)
        nativeData.StringArray[i] = CORBA::string_dup(data->StringArray[i]);

    for (int i = 0; i < 4; i++)
        nativeData.WStringArray[i] = CORBA::wstring_dup(data->WStringArray[i]);

    return dw->write(nativeData, DDS::HANDLE_NIL);    
}

Test::BasicTestStructDataReader_ptr BasicTestStructDataReader_Narrow(DDS::DataReader_ptr dr)
{
    return Test::BasicTestStructDataReader::_narrow(dr);
}

int BasicTestStructDataReader_ReadNextSample(Test::BasicTestStructDataReader_ptr dr, BasicTestStructWrapper* data)
{
    Test::BasicTestStruct nativeData;
    ::DDS::SampleInfo sampleInfo;
    ::DDS::ReturnCode_t ret = dr->read_next_sample(nativeData, sampleInfo);
    if (ret == ::DDS::RETCODE_OK)
    {
        data->Id = nativeData.Id;
        data->Message = CORBA::string_dup(nativeData.Message);
        data->WMessage = CORBA::wstring_dup(nativeData.WMessage);

        marshal::unbounded_sequence_to_ptr(nativeData.LongSequence, data->LongSequence);
        marshal::unbounded_basic_string_sequence_to_ptr(nativeData.StringSequence, data->StringSequence);

        for (int i = 0; i < 5; i++)
        {
            data->LongArray[i] = nativeData.LongArray[i];
        }

        for (int i = 0; i < 10; i++)
        {
            if (nativeData.StringArray[i] != NULL)
            {
                data->StringArray[i] = CORBA::string_dup(nativeData.StringArray[i]);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (nativeData.WStringArray[i] != NULL)
            {
                data->WStringArray[i] = CORBA::wstring_dup(nativeData.WStringArray[i]);
            }
        }
    }

    return (int)ret;
}

int BasicTestStructDataReader_Read(Test::BasicTestStructDataReader_ptr dr)
{
    Test::BasicTestStructSeq received_data;
    ::DDS::SampleInfoSeq info_seq;    
    ::DDS::ReturnCode_t ret = dr->read(received_data, info_seq, ::DDS::LENGTH_UNLIMITED, ::DDS::ANY_SAMPLE_STATE, ::DDS::ANY_VIEW_STATE, ::DDS::ANY_INSTANCE_STATE);
    if (ret == ::DDS::RETCODE_OK)
    {
        // TODO
    }

    dr->return_loan(received_data, info_seq);

    return (int)ret;
}