#include "TestPInvokeIdlTypeSupport.h"

void BasicTestStructWrapper_release(BasicTestStructWrapper* obj)
{
    CORBA::string_free(obj->Message);    
    delete[] obj->LongSequence;
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
    nativeData.Message = CORBA::string_dup(data->Message);

    //char* bytes = (char*)data->LongSequence;
    //// First 4 bytes are the length of the array
    //ACE_UINT32 length = 0;
    //std::memcpy(&length, bytes, sizeof length);
    //nativeData.LongSequence.length(length);
    //   
    //// The rest of the memory is the structures aligned one after the other
    //const ACE_UINT64 structs_offset = sizeof length;
    //const ACE_UINT64 struct_size = sizeof int32_t;
    //for (int i = 0; i < length; i++)
    //{
    //    std::memcpy(&nativeData.LongSequence[i], &bytes[(i * struct_size) + structs_offset], struct_size);
    //}    

    marshal::ptr_to_unbounded_sequence(data->LongSequence, nativeData.LongSequence);

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

        marshal::unbounded_sequence_to_ptr<CORBA::Long>(nativeData.LongSequence, data->LongSequence);
        //ACE_UINT32 length = nativeData.LongSequence.length();        
        //const ACE_UINT64 buffer_size = (length * sizeof(int)) + sizeof length;
        //char* bytes = new char[buffer_size];
        //std::memcpy(bytes, &length, sizeof int32_t);
        //
        //if (length > 0)
        //{
        //    const ACE_UINT64 struct_size = sizeof nativeData.LongSequence[0];
        //    for (int i = 0; i < length; i++)
        //    {
        //        std::memcpy(&bytes[(i * struct_size) + sizeof length], &nativeData.LongSequence[i], struct_size);
        //    }
        //}

        //// Alloc memory for the poninter
        //ACE_NEW_RETURN(data->LongSequence, char[buffer_size], 0);
        //// Copy the bytes in the pointer
        //ACE_OS::memcpy(data->LongSequence, bytes, buffer_size);
        //// Free the temporal allocated memory
        //delete[] bytes;
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