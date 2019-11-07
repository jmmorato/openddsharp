/* Generated by E:\PROJECTS\OPENDDS\ext\OpenDDS\bin\opendds_idl version 3.13.2 (ACE version 6.2a_p15) running on input file Test.idl */
#include "TestTypeSupportImpl.h"

#include <cstring>
#include <stdexcept>
#include "dds/DCPS/BuiltInTopicUtils.h"
#include "dds/DCPS/ContentFilteredTopicImpl.h"
#include "dds/DCPS/DataReaderImpl_T.h"
#include "dds/DCPS/DataWriterImpl_T.h"
#include "dds/DCPS/FilterEvaluator.h"
#include "dds/DCPS/MultiTopicDataReader_T.h"
#include "dds/DCPS/PoolAllocator.h"
#include "dds/DCPS/PublicationInstance.h"
#include "dds/DCPS/PublisherImpl.h"
#include "dds/DCPS/Qos_Helper.h"
#include "dds/DCPS/RakeData.h"
#include "dds/DCPS/RakeResults_T.h"
#include "dds/DCPS/ReceivedDataElementList.h"
#include "dds/DCPS/Registered_Data_Types.h"
#include "dds/DCPS/Service_Participant.h"
#include "dds/DCPS/SubscriberImpl.h"
#include "dds/DCPS/Util.h"
#include "dds/DCPS/debug.h"
#include "dds/DdsDcpsDomainC.h"


/* Begin MODULE: CORBA */


/* End MODULE: CORBA */


/* Begin MODULE: Test */



/* Begin STRUCT: TestStruct */

OPENDDS_BEGIN_VERSIONED_NAMESPACE_DECL
namespace OpenDDS { namespace DCPS {

void gen_find_size(const Test::TestStruct& stru, size_t& size, size_t& padding)
{
  ACE_UNUSED_ARG(stru);
  ACE_UNUSED_ARG(size);
  ACE_UNUSED_ARG(padding);
  if ((size + padding) % 2) {
    padding += 2 - ((size + padding) % 2);
  }
  size += gen_max_marshaled_size(stru.ShortField);
  if ((size + padding) % 2) {
    padding += 2 - ((size + padding) % 2);
  }
  size += gen_max_marshaled_size(stru.UnsignedShortField);
  if ((size + padding) % 4) {
    padding += 4 - ((size + padding) % 4);
  }
  size += gen_max_marshaled_size(stru.LongField);
  if ((size + padding) % 4) {
    padding += 4 - ((size + padding) % 4);
  }
  size += gen_max_marshaled_size(stru.UnsignedLongField);
  if ((size + padding) % 8) {
    padding += 8 - ((size + padding) % 8);
  }
  size += gen_max_marshaled_size(stru.LongLongField);
  if ((size + padding) % 8) {
    padding += 8 - ((size + padding) % 8);
  }
  size += gen_max_marshaled_size(stru.UnsignedLongLongField);
}

bool operator<<(Serializer& strm, const Test::TestStruct& stru)
{
  ACE_UNUSED_ARG(strm);
  ACE_UNUSED_ARG(stru);
  return (strm << stru.ShortField)
    && (strm << stru.UnsignedShortField)
    && (strm << stru.LongField)
    && (strm << stru.UnsignedLongField)
    && (strm << stru.LongLongField)
    && (strm << stru.UnsignedLongLongField);
}

bool operator>>(Serializer& strm, Test::TestStruct& stru)
{
  ACE_UNUSED_ARG(strm);
  ACE_UNUSED_ARG(stru);
  return (strm >> stru.ShortField)
    && (strm >> stru.UnsignedShortField)
    && (strm >> stru.LongField)
    && (strm >> stru.UnsignedLongField)
    && (strm >> stru.LongLongField)
    && (strm >> stru.UnsignedLongLongField);
}

size_t gen_max_marshaled_size(const Test::TestStruct& stru, bool align)
{
  ACE_UNUSED_ARG(stru);
  ACE_UNUSED_ARG(align);
  return align ? 32 : 28;
}

size_t gen_max_marshaled_size(KeyOnly<const Test::TestStruct> stru, bool align)
{
  ACE_UNUSED_ARG(stru);
  ACE_UNUSED_ARG(align);
  return 0;
}

void gen_find_size(KeyOnly<const Test::TestStruct> stru, size_t& size, size_t& padding)
{
  ACE_UNUSED_ARG(stru);
  ACE_UNUSED_ARG(size);
  ACE_UNUSED_ARG(padding);
}

bool operator<<(Serializer& strm, KeyOnly<const Test::TestStruct> stru)
{
  ACE_UNUSED_ARG(strm);
  ACE_UNUSED_ARG(stru);
  return true;
}

bool operator>>(Serializer& strm, KeyOnly<Test::TestStruct> stru)
{
  ACE_UNUSED_ARG(strm);
  ACE_UNUSED_ARG(stru);
  return true;
}

}  }
OPENDDS_END_VERSIONED_NAMESPACE_DECL


namespace Test {
::DDS::DataWriter_ptr TestStructTypeSupportImpl::create_datawriter()
{
  typedef OpenDDS::DCPS::DataWriterImpl_T<TestStruct> DataWriterImplType;
  ::DDS::DataWriter_ptr writer_impl = ::DDS::DataWriter::_nil();
  ACE_NEW_NORETURN(writer_impl,
                   DataWriterImplType());
  return writer_impl;
}

::DDS::DataReader_ptr TestStructTypeSupportImpl::create_datareader()
{
  typedef OpenDDS::DCPS::DataReaderImpl_T<TestStruct> DataReaderImplType;
  ::DDS::DataReader_ptr reader_impl = ::DDS::DataReader::_nil();
  ACE_NEW_NORETURN(reader_impl,
                   DataReaderImplType());
  return reader_impl;
}

#ifndef OPENDDS_NO_MULTI_TOPIC
::DDS::DataReader_ptr TestStructTypeSupportImpl::create_multitopic_datareader()
{
  typedef OpenDDS::DCPS::DataReaderImpl_T<TestStruct> DataReaderImplType;
  typedef OpenDDS::DCPS::MultiTopicDataReader_T<TestStruct, DataReaderImplType> MultiTopicDataReaderImplType;
  ::DDS::DataReader_ptr multitopic_reader_impl = ::DDS::DataReader::_nil();
  ACE_NEW_NORETURN(multitopic_reader_impl,
                   MultiTopicDataReaderImplType());
  return multitopic_reader_impl;
}
#endif /* !OPENDDS_NO_MULTI_TOPIC */

#ifndef OPENDDS_NO_CONTENT_SUBSCRIPTION_PROFILE
const OpenDDS::DCPS::MetaStruct& TestStructTypeSupportImpl::getMetaStructForType()
{
  return OpenDDS::DCPS::getMetaStruct<TestStruct>();
}
#endif /* !OPENDDS_NO_CONTENT_SUBSCRIPTION_PROFILE */

bool TestStructTypeSupportImpl::has_dcps_key()
{
  return TraitsType::gen_has_key ();
}

const char* TestStructTypeSupportImpl::default_type_name() const
{
  return TraitsType::type_name();
}

TestStructTypeSupport::_ptr_type TestStructTypeSupportImpl::_narrow(CORBA::Object_ptr obj)
{
  return TypeSupportType::_narrow(obj);
}
}

#ifndef OPENDDS_NO_CONTENT_SUBSCRIPTION_PROFILE
OPENDDS_BEGIN_VERSIONED_NAMESPACE_DECL
namespace OpenDDS { namespace DCPS {

template<>
struct MetaStructImpl<Test::TestStruct> : MetaStruct {
  typedef Test::TestStruct T;

#ifndef OPENDDS_NO_MULTI_TOPIC
  void* allocate() const { return new T; }

  void deallocate(void* stru) const { delete static_cast<T*>(stru); }

  size_t numDcpsKeys() const { return 0; }
#endif /* OPENDDS_NO_MULTI_TOPIC */

  Value getValue(const void* stru, const char* field) const
  {
    const Test::TestStruct& typed = *static_cast<const Test::TestStruct*>(stru);
    if (std::strcmp(field, "ShortField") == 0) {
      return typed.ShortField;
    }
    if (std::strcmp(field, "UnsignedShortField") == 0) {
      return typed.UnsignedShortField;
    }
    if (std::strcmp(field, "LongField") == 0) {
      return typed.LongField;
    }
    if (std::strcmp(field, "UnsignedLongField") == 0) {
      return typed.UnsignedLongField;
    }
    if (std::strcmp(field, "LongLongField") == 0) {
      return typed.LongLongField;
    }
    if (std::strcmp(field, "UnsignedLongLongField") == 0) {
      return typed.UnsignedLongLongField;
    }
    ACE_UNUSED_ARG(typed);
    throw std::runtime_error("Field " + OPENDDS_STRING(field) + " not found or its type is not supported (in struct Test::TestStruct)");
  }

  Value getValue(Serializer& ser, const char* field) const
  {
    if (std::strcmp(field, "ShortField") == 0) {
      ACE_CDR::Short val;
      if (!(ser >> val)) {
        throw std::runtime_error("Field 'ShortField' could not be deserialized");
      }
      return val;
    } else {
      if (!ser.skip(1, 2)) {
        throw std::runtime_error("Field 'ShortField' could not be skipped");
      }
    }
    if (std::strcmp(field, "UnsignedShortField") == 0) {
      ACE_CDR::UShort val;
      if (!(ser >> val)) {
        throw std::runtime_error("Field 'UnsignedShortField' could not be deserialized");
      }
      return val;
    } else {
      if (!ser.skip(1, 2)) {
        throw std::runtime_error("Field 'UnsignedShortField' could not be skipped");
      }
    }
    if (std::strcmp(field, "LongField") == 0) {
      ACE_CDR::Long val;
      if (!(ser >> val)) {
        throw std::runtime_error("Field 'LongField' could not be deserialized");
      }
      return val;
    } else {
      if (!ser.skip(1, 4)) {
        throw std::runtime_error("Field 'LongField' could not be skipped");
      }
    }
    if (std::strcmp(field, "UnsignedLongField") == 0) {
      ACE_CDR::ULong val;
      if (!(ser >> val)) {
        throw std::runtime_error("Field 'UnsignedLongField' could not be deserialized");
      }
      return val;
    } else {
      if (!ser.skip(1, 4)) {
        throw std::runtime_error("Field 'UnsignedLongField' could not be skipped");
      }
    }
    if (std::strcmp(field, "LongLongField") == 0) {
      ACE_CDR::LongLong val;
      if (!(ser >> val)) {
        throw std::runtime_error("Field 'LongLongField' could not be deserialized");
      }
      return val;
    } else {
      if (!ser.skip(1, 8)) {
        throw std::runtime_error("Field 'LongLongField' could not be skipped");
      }
    }
    if (std::strcmp(field, "UnsignedLongLongField") == 0) {
      ACE_CDR::ULongLong val;
      if (!(ser >> val)) {
        throw std::runtime_error("Field 'UnsignedLongLongField' could not be deserialized");
      }
      return val;
    } else {
      if (!ser.skip(1, 8)) {
        throw std::runtime_error("Field 'UnsignedLongLongField' could not be skipped");
      }
    }
    if (!field[0]) {
      return 0;
    }
    throw std::runtime_error("Field " + OPENDDS_STRING(field) + " not valid for struct Test::TestStruct");
  }

  ComparatorBase::Ptr create_qc_comparator(const char* field, ComparatorBase::Ptr next) const
  {
    ACE_UNUSED_ARG(next);
    if (std::strcmp(field, "ShortField") == 0) {
      return make_field_cmp(&T::ShortField, next);
    }
    if (std::strcmp(field, "UnsignedShortField") == 0) {
      return make_field_cmp(&T::UnsignedShortField, next);
    }
    if (std::strcmp(field, "LongField") == 0) {
      return make_field_cmp(&T::LongField, next);
    }
    if (std::strcmp(field, "UnsignedLongField") == 0) {
      return make_field_cmp(&T::UnsignedLongField, next);
    }
    if (std::strcmp(field, "LongLongField") == 0) {
      return make_field_cmp(&T::LongLongField, next);
    }
    if (std::strcmp(field, "UnsignedLongLongField") == 0) {
      return make_field_cmp(&T::UnsignedLongLongField, next);
    }
    throw std::runtime_error("Field " + OPENDDS_STRING(field) + " not found or its type is not supported (in struct Test::TestStruct)");
  }

#ifndef OPENDDS_NO_MULTI_TOPIC
  const char** getFieldNames() const
  {
    static const char* names[] = {"ShortField", "UnsignedShortField", "LongField", "UnsignedLongField", "LongLongField", "UnsignedLongLongField", 0};
    return names;
  }

  const void* getRawField(const void* stru, const char* field) const
  {
    if (std::strcmp(field, "ShortField") == 0) {
      return &static_cast<const T*>(stru)->ShortField;
    }
    if (std::strcmp(field, "UnsignedShortField") == 0) {
      return &static_cast<const T*>(stru)->UnsignedShortField;
    }
    if (std::strcmp(field, "LongField") == 0) {
      return &static_cast<const T*>(stru)->LongField;
    }
    if (std::strcmp(field, "UnsignedLongField") == 0) {
      return &static_cast<const T*>(stru)->UnsignedLongField;
    }
    if (std::strcmp(field, "LongLongField") == 0) {
      return &static_cast<const T*>(stru)->LongLongField;
    }
    if (std::strcmp(field, "UnsignedLongLongField") == 0) {
      return &static_cast<const T*>(stru)->UnsignedLongLongField;
    }
    throw std::runtime_error("Field " + OPENDDS_STRING(field) + " not found or its type is not supported (in struct Test::TestStruct)");
  }

  void assign(void* lhs, const char* field, const void* rhs,
    const char* rhsFieldSpec, const MetaStruct& rhsMeta) const
  {
    ACE_UNUSED_ARG(lhs);
    ACE_UNUSED_ARG(field);
    ACE_UNUSED_ARG(rhs);
    ACE_UNUSED_ARG(rhsFieldSpec);
    ACE_UNUSED_ARG(rhsMeta);
    if (std::strcmp(field, "ShortField") == 0) {
      static_cast<T*>(lhs)->ShortField = *static_cast<const CORBA::Short*>(rhsMeta.getRawField(rhs, rhsFieldSpec));
      return;
    }
    if (std::strcmp(field, "UnsignedShortField") == 0) {
      static_cast<T*>(lhs)->UnsignedShortField = *static_cast<const CORBA::UShort*>(rhsMeta.getRawField(rhs, rhsFieldSpec));
      return;
    }
    if (std::strcmp(field, "LongField") == 0) {
      static_cast<T*>(lhs)->LongField = *static_cast<const CORBA::Long*>(rhsMeta.getRawField(rhs, rhsFieldSpec));
      return;
    }
    if (std::strcmp(field, "UnsignedLongField") == 0) {
      static_cast<T*>(lhs)->UnsignedLongField = *static_cast<const CORBA::ULong*>(rhsMeta.getRawField(rhs, rhsFieldSpec));
      return;
    }
    if (std::strcmp(field, "LongLongField") == 0) {
      static_cast<T*>(lhs)->LongLongField = *static_cast<const CORBA::LongLong*>(rhsMeta.getRawField(rhs, rhsFieldSpec));
      return;
    }
    if (std::strcmp(field, "UnsignedLongLongField") == 0) {
      static_cast<T*>(lhs)->UnsignedLongLongField = *static_cast<const CORBA::ULongLong*>(rhsMeta.getRawField(rhs, rhsFieldSpec));
      return;
    }
    throw std::runtime_error("Field " + OPENDDS_STRING(field) + " not found or its type is not supported (in struct Test::TestStruct)");
  }
#endif /* OPENDDS_NO_MULTI_TOPIC */

  bool compare(const void* lhs, const void* rhs, const char* field) const
  {
    ACE_UNUSED_ARG(lhs);
    ACE_UNUSED_ARG(field);
    ACE_UNUSED_ARG(rhs);
    if (std::strcmp(field, "ShortField") == 0) {
      return static_cast<const T*>(lhs)->ShortField == static_cast<const T*>(rhs)->ShortField;
    }
    if (std::strcmp(field, "UnsignedShortField") == 0) {
      return static_cast<const T*>(lhs)->UnsignedShortField == static_cast<const T*>(rhs)->UnsignedShortField;
    }
    if (std::strcmp(field, "LongField") == 0) {
      return static_cast<const T*>(lhs)->LongField == static_cast<const T*>(rhs)->LongField;
    }
    if (std::strcmp(field, "UnsignedLongField") == 0) {
      return static_cast<const T*>(lhs)->UnsignedLongField == static_cast<const T*>(rhs)->UnsignedLongField;
    }
    if (std::strcmp(field, "LongLongField") == 0) {
      return static_cast<const T*>(lhs)->LongLongField == static_cast<const T*>(rhs)->LongLongField;
    }
    if (std::strcmp(field, "UnsignedLongLongField") == 0) {
      return static_cast<const T*>(lhs)->UnsignedLongLongField == static_cast<const T*>(rhs)->UnsignedLongLongField;
    }
    throw std::runtime_error("Field " + OPENDDS_STRING(field) + " not found or its type is not supported (in struct Test::TestStruct)");
  }
};

template<>
const MetaStruct& getMetaStruct<Test::TestStruct>()
{
  static MetaStructImpl<Test::TestStruct> msi;
  return msi;
}

bool gen_skip_over(Serializer& ser, Test::TestStruct*)
{
  ACE_UNUSED_ARG(ser);
  MetaStructImpl<Test::TestStruct>().getValue(ser, "");
  return true;
}

}  }
OPENDDS_END_VERSIONED_NAMESPACE_DECL

#endif

/* End STRUCT: TestStruct */

/* End MODULE: Test */