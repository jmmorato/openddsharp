#ifndef _MARSHAL_H_
#define _MARSHAL_H_

#include "ace/Basic_Types.h"
#include "tao/Unbounded_Value_Sequence_T.h"
#include "dds/DCPS/Serializer.h"
#include "dds/DdsDcpsCoreC.h"

class marshal {

public:
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

    static void
    ptr_to_unbounded_basic_string_sequence(void *ptr, TAO::unbounded_basic_string_sequence<char> &sequence) {
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

    static void
    ptr_to_unbounded_wide_string_sequence(void *ptr, TAO::unbounded_basic_string_sequence<wchar_t> &sequence) {
      if (ptr == NULL) {
        return;
      }

      char *bytes = (char *) ptr;

      // First 4 bytes are the length of the array
      ACE_UINT32 length = 0;
      ACE_OS::memcpy(&length, bytes, sizeof length);
      sequence.length(length);

      const size_t structs_offset = sizeof length;
      const size_t struct_size = sizeof(wchar_t *);
      wchar_t **pointers = new wchar_t *[length];
      for (ACE_UINT32 i = 0; i < length; i++) {
        ACE_OS::memcpy(&pointers[i], &bytes[(i * struct_size) + structs_offset], struct_size);

        sequence[i] = CORBA::wstring_dup(pointers[i]);
      }

      delete[] pointers;
    }

    static void
    unbounded_basic_string_sequence_to_ptr(TAO::unbounded_basic_string_sequence<char> &sequence, void *&ptr) {
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

    static void
    unbounded_wide_string_sequence_to_ptr(TAO::unbounded_basic_string_sequence<wchar_t> &sequence, void *&ptr) {
      ACE_UINT32 length = sequence.length();
      const size_t struct_size = sizeof(wchar_t *);
      const size_t buffer_size = (length * struct_size) + sizeof length;
      char *bytes = new char[buffer_size];
      ACE_OS::memcpy(bytes, &length, sizeof length);

      for (ACE_UINT32 i = 0; i < length; i++) {
        wchar_t *str = CORBA::wstring_dup(sequence[i]);
        ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &str, struct_size);
      }

      // Alloc memory for the poninter
      ptr = ACE_OS::malloc(buffer_size);

      // Copy the bytes in the pointer
      ACE_OS::memcpy(ptr, bytes, buffer_size);

      // Free temporally allocated memory
      delete[] bytes;
    }

    static void release_basic_string_sequence_ptr(void *&ptr) {
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

    static void release_wide_string_sequence_ptr(void *&ptr) {
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

    template<typename T>
    static void release_structure_sequence_ptr(void *&ptr) {
      if (ptr == NULL) {
        return;
      }

      char *bytes = (char *) ptr;

      // First 4 bytes are the length of the array
      ACE_UINT32 length = 0;
      ACE_OS::memcpy(&length, bytes, sizeof length);

      const size_t structs_offset = sizeof length;
      const size_t struct_size = sizeof(T);
      T *structures = new T[length];
      for (ACE_UINT32 i = 0; i < length; i++) {
        ACE_OS::memcpy(&structures[i], &bytes[(i * struct_size) + structs_offset], struct_size);

        structures[i].release();
      }

      delete[] structures;

      ACE_OS::free(ptr);
    }

    static void ptr_to_basic_string_multi_array(void *ptr, char **&arr, int length) {
      if (ptr == NULL) {
        return;
      }

      char *bytes = (char *) ptr;

      const size_t struct_size = sizeof(char *);
      char **pointers = new char *[length];
      for (ACE_INT32 i = 0; i < length; i++) {
        ACE_OS::memcpy(&pointers[i], &bytes[i * struct_size], struct_size);

        arr[i] = CORBA::string_dup(pointers[i]);
      }

      delete[] pointers;
    }

    static void ptr_to_wide_string_multi_array(void *ptr, wchar_t **&arr, int length) {
      if (ptr == NULL) {
        return;
      }

      char *bytes = (char *) ptr;

      const size_t struct_size = sizeof(wchar_t *);
      wchar_t **pointers = new wchar_t *[length];
      for (ACE_INT32 i = 0; i < length; i++) {
        ACE_OS::memcpy(&pointers[i], &bytes[i * struct_size], struct_size);

        arr[i] = CORBA::wstring_dup(pointers[i]);
      }

      delete[] pointers;
    }

    static void basic_string_multi_array_to_ptr(char **&arr, void *&ptr, int length) {
      const size_t struct_size = sizeof(char *);
      const size_t buffer_size = length * struct_size;
      char *bytes = new char[buffer_size];

      for (ACE_INT32 i = 0; i < length; i++) {
        char *str = CORBA::string_dup(arr[i]);
        ACE_OS::memcpy(&bytes[i * struct_size], &str, struct_size);
      }

      // Alloc memory for the poninter
      ptr = ACE_OS::malloc(buffer_size);

      // Copy the bytes in the pointer
      ACE_OS::memcpy(ptr, bytes, buffer_size);

      // Free temporally allocated memory
      delete[] bytes;
    }

    static void wide_string_multi_array_to_ptr(wchar_t **&arr, void *&ptr, int length) {
      const size_t struct_size = sizeof(wchar_t *);
      const size_t buffer_size = length * struct_size;
      char *bytes = new char[buffer_size];

      for (ACE_INT32 i = 0; i < length; i++) {
        wchar_t *str = CORBA::wstring_dup(arr[i]);
        ACE_OS::memcpy(&bytes[i * struct_size], &str, struct_size);
      }

      // Alloc memory for the poninter
      ptr = ACE_OS::malloc(buffer_size);

      // Copy the bytes in the pointer
      ACE_OS::memcpy(ptr, bytes, buffer_size);

      // Free temporally allocated memory
      delete[] bytes;
    }

    static void release_basic_string_multi_array_ptr(void *&ptr, int length) {
      if (ptr == NULL) {
        return;
      }

      char *bytes = (char *) ptr;

      const size_t struct_size = sizeof(char *);
      char **pointers = new char *[length];
      for (ACE_INT32 i = 0; i < length; i++) {
        ACE_OS::memcpy(&pointers[i], &bytes[i * struct_size], struct_size);

        CORBA::string_free(pointers[i]);
      }

      delete[] pointers;

      free(ptr);
    }

    static void release_wide_string_multi_array_ptr(void *&ptr, int length) {
      if (ptr == NULL) {
        return;
      }

      char *bytes = (char *) ptr;

      const size_t struct_size = sizeof(wchar_t *);
      wchar_t **pointers = new wchar_t *[length];
      for (ACE_INT32 i = 0; i < length; i++) {
        ACE_OS::memcpy(&pointers[i], &bytes[i * struct_size], struct_size);

        CORBA::wstring_free(pointers[i]);
      }

      delete[] pointers;

      free(ptr);
    }

    template<typename T, CORBA::ULong MAX>
    static void ptr_to_bounded_sequence(void *ptr, TAO::bounded_value_sequence<T, MAX> &sequence) {
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

    template<typename T, CORBA::ULong MAX>
    static void bounded_sequence_to_ptr(TAO::bounded_value_sequence<T, MAX> sequence, void *&ptr) {
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

    template<CORBA::ULong MAX>
    static void
    ptr_to_bounded_basic_string_sequence(void *ptr, TAO::bounded_basic_string_sequence<char, MAX> &sequence) {
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

    template<CORBA::ULong MAX>
    static void
    ptr_to_bounded_wide_string_sequence(void *ptr, TAO::bounded_basic_string_sequence<wchar_t, MAX> &sequence) {
      if (ptr == NULL) {
        return;
      }

      char *bytes = (char *) ptr;

      // First 4 bytes are the length of the array
      ACE_UINT32 length = 0;
      ACE_OS::memcpy(&length, bytes, sizeof length);
      sequence.length(length);

      const size_t structs_offset = sizeof length;
      const size_t struct_size = sizeof(wchar_t *);
      wchar_t **pointers = new wchar_t *[length];
      for (ACE_UINT32 i = 0; i < length; i++) {
        ACE_OS::memcpy(&pointers[i], &bytes[(i * struct_size) + structs_offset], struct_size);

        sequence[i] = CORBA::wstring_dup(pointers[i]);
      }

      delete[] pointers;
    }

    template<CORBA::ULong MAX>
    static void
    bounded_basic_string_sequence_to_ptr(TAO::bounded_basic_string_sequence<char, MAX> &sequence, void *&ptr) {
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

    template<CORBA::ULong MAX>
    static void
    bounded_wide_string_sequence_to_ptr(TAO::bounded_basic_string_sequence<wchar_t, MAX> &sequence, void *&ptr) {
      ACE_UINT32 length = sequence.length();
      const size_t struct_size = sizeof(wchar_t *);
      const size_t buffer_size = (length * struct_size) + sizeof length;
      char *bytes = new char[buffer_size];
      ACE_OS::memcpy(bytes, &length, sizeof length);

      for (ACE_UINT32 i = 0; i < length; i++) {
        wchar_t *str = CORBA::wstring_dup(sequence[i]);
        ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &str, struct_size);
      }

      // Alloc memory for the poninter
      ptr = ACE_OS::malloc(buffer_size);

      // Copy the bytes in the pointer
      ACE_OS::memcpy(ptr, bytes, buffer_size);

      // Free temporally allocated memory
      delete[] bytes;
    }

    static wchar_t ptr_to_wchar(void *ptr) {
      wchar_t wchar = '\0';

      if (ptr == NULL) {
        return wchar;
      }

      const size_t size = sizeof(wchar_t);

      ACE_OS::memcpy(&wchar, ptr, sizeof(wchar_t));

      return wchar;
    }

    static void *wchar_to_ptr(wchar_t wchar) {
      const size_t size = sizeof(wchar_t);

      // Alloc memory for the pointer
      void *ptr = ACE_OS::malloc(size);

      // Copy the bytes in the pointer
      ACE_OS::memcpy(ptr, &wchar, size);

      return ptr;
    }

    static DDS::Time_t dds_time_deserialize_from_bytes(const char *bytes, size_t size)
    {
        const OpenDDS::DCPS::Encoding encoding(OpenDDS::DCPS::Encoding::KIND_XCDR1, OpenDDS::DCPS::ENDIAN_LITTLE);

        ACE_Message_Block mb(size);
        mb.copy(bytes, size);

        OpenDDS::DCPS::Serializer serializer(&mb, encoding);

        DDS::Time_t time_value;
        if (!(serializer >> time_value.sec)) {
          throw std::runtime_error("Failed to deserialize DDS::Time_t seconds from bytes");
        }

        if (!(serializer >> time_value.nanosec)) {
          throw std::runtime_error("Failed to deserialize DDS::Time_t nanosec from bytes");
        }
        return time_value;
    }

    static void dds_sample_info_serialize_to_bytes(DDS::SampleInfo& sample_info, char* &data, size_t &size)
    {
      const OpenDDS::DCPS::Encoding encoding(OpenDDS::DCPS::Encoding::KIND_XCDR1, OpenDDS::DCPS::ENDIAN_LITTLE);

      size_t xcdr_size = 0;
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.valid_data);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.sample_state, 1);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.view_state);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.instance_state);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.source_timestamp.sec);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.source_timestamp.nanosec);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.instance_handle);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.publication_handle);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.disposed_generation_count);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.no_writers_generation_count);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.sample_rank);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.generation_rank);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, sample_info.absolute_generation_rank);

      ACE_Message_Block mb(xcdr_size);

      OpenDDS::DCPS::Serializer serializer(&mb, encoding);

      if (!(serializer << sample_info.valid_data)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo valid_data to bytes");
      }

      if (!(serializer << sample_info.sample_state)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo sample_state to bytes");
      }

      if (!(serializer << sample_info.view_state)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo view_state to bytes");
      }

      if (!(serializer << sample_info.instance_state)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo instance_state to bytes");
      }

      if (!(serializer << sample_info.source_timestamp.sec)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo source_timestamp.sec to bytes");
      }

      if (!(serializer << sample_info.source_timestamp.nanosec)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo source_timestamp.nanosec to bytes");
      }

      if (!(serializer << sample_info.instance_handle)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo instance_handle to bytes");
      }

      if (!(serializer << sample_info.publication_handle)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo publication_handle to bytes");
      }

      if (!(serializer << sample_info.disposed_generation_count)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo disposed_generation_count to bytes");
      }

      if (!(serializer << sample_info.no_writers_generation_count)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo no_writers_generation_count to bytes");
      }

      if (!(serializer << sample_info.sample_rank)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo sample_rank to bytes");
      }

      if (!(serializer << sample_info.generation_rank)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo generation_rank to bytes");
      }

      if (!(serializer << sample_info.absolute_generation_rank)) {
        throw std::runtime_error("Failed to serialize DDS::SampleInfo absolute_generation_rank to bytes");
      }

      data = (char*)malloc(xcdr_size);
      memcpy(data, mb.base(), xcdr_size);
      size = xcdr_size;
    }

    static void dds_sample_info_seq_serialize_to_bytes(::DDS::SampleInfoSeq& seq_info, char* &data, size_t &size)
    {
      const OpenDDS::DCPS::Encoding encoding(OpenDDS::DCPS::Encoding::KIND_XCDR1, OpenDDS::DCPS::ENDIAN_LITTLE);

      size_t xcdr_size = 0;
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].valid_data);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].sample_state, 1);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].view_state);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].instance_state);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].source_timestamp.sec);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].source_timestamp.nanosec);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].instance_handle);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].publication_handle);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].disposed_generation_count);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].no_writers_generation_count);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].sample_rank);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].generation_rank);
      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info[0].absolute_generation_rank);

      xcdr_size*=seq_info.length();

      OpenDDS::DCPS::primitive_serialized_size(encoding, xcdr_size, seq_info.length());

      ACE_Message_Block mb(xcdr_size);

      OpenDDS::DCPS::Serializer serializer(&mb, encoding);

      if (!(serializer << ACE_CDR::ULong(seq_info.length()))) {
        throw std::runtime_error("Failed to serialize sequence length.");
      }

      for (int i = 0; i < seq_info.length(); i++) {
        if (!(serializer << seq_info[i].valid_data)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo valid_data to bytes");
        }

        if (!(serializer << seq_info[i].sample_state)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo sample_state to bytes");
        }

        if (!(serializer << seq_info[i].view_state)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo view_state to bytes");
        }

        if (!(serializer << seq_info[i].instance_state)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo instance_state to bytes");
        }

        if (!(serializer << seq_info[i].source_timestamp.sec)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo source_timestamp.sec to bytes");
        }

        if (!(serializer << seq_info[i].source_timestamp.nanosec)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo source_timestamp.nanosec to bytes");
        }

        if (!(serializer << seq_info[i].instance_handle)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo instance_handle to bytes");
        }

        if (!(serializer << seq_info[i].publication_handle)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo publication_handle to bytes");
        }

        if (!(serializer << seq_info[i].disposed_generation_count)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo disposed_generation_count to bytes");
        }

        if (!(serializer << seq_info[i].no_writers_generation_count)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo no_writers_generation_count to bytes");
        }

        if (!(serializer << seq_info[i].sample_rank)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo sample_rank to bytes");
        }

        if (!(serializer << seq_info[i].generation_rank)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo generation_rank to bytes");
        }

        if (!(serializer << seq_info[i].absolute_generation_rank)) {
          throw std::runtime_error("Failed to serialize DDS::SampleInfo absolute_generation_rank to bytes");
        }
      }

      data = (char*)malloc(xcdr_size);
      memcpy(data, mb.base(), xcdr_size);
      size = xcdr_size;
    }
};

#endif