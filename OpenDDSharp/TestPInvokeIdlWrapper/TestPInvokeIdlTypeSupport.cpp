#include "TestPInvokeIdlTypeSupport.h"

void BasicTestStructWrapper_release(BasicTestStructWrapper* obj)
{
    obj->release();
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
    Test::BasicTestStruct nativeData = data->to_native();

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
        data->from_native(nativeData);
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