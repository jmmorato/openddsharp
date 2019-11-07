// -*- C++ -*-
// $Id$

/**
 * Code generated by the The ACE ORB (TAO) IDL Compiler v2.2a_p15
 * TAO and the TAO IDL Compiler have been developed by:
 *       Center for Distributed Object Computing
 *       Washington University
 *       St. Louis, MO
 *       USA
 *       http://www.cs.wustl.edu/~schmidt/doc-center.html
 * and
 *       Distributed Object Computing Laboratory
 *       University of California at Irvine
 *       Irvine, CA
 *       USA
 * and
 *       Institute for Software Integrated Systems
 *       Vanderbilt University
 *       Nashville, TN
 *       USA
 *       http://www.isis.vanderbilt.edu/
 *
 * Information about TAO is available at:
 *     http://www.cs.wustl.edu/~schmidt/TAO.html
 **/

// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_codegen.cpp:152

#ifndef _TAO_IDL_TESTPINVOKEIDLTYPESUPPORTC_DXIMWW_H_
#define _TAO_IDL_TESTPINVOKEIDLTYPESUPPORTC_DXIMWW_H_

#include /**/ "ace/pre.h"


#include /**/ "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */


#include /**/ "TestPInvokeIdlIDL_Export.h"
#include "tao/ORB.h"
#include "tao/SystemException.h"
#include "tao/Basic_Types.h"
#include "tao/ORB_Constants.h"
#include "dds/DCPS/ZeroCopyInfoSeq_T.h"
#include "dds/DCPS/ZeroCopySeq_T.h"
#include "tao/Object.h"
#include "tao/String_Manager_T.h"
#include "tao/Sequence_T.h"
#include "tao/Objref_VarOut_T.h"
#include "tao/Seq_Var_T.h"
#include "tao/Seq_Out_T.h"
#include "tao/Arg_Traits_T.h"
#include "tao/Basic_Arguments.h"
#include "tao/Special_Basic_Arguments.h"
#include "tao/Any_Insert_Policy_T.h"
#include "tao/Fixed_Size_Argument_T.h"
#include "tao/Var_Size_Argument_T.h"
#include "tao/Object_Argument_T.h"
#include /**/ "tao/Version.h"
#include /**/ "tao/Versioned_Namespace.h"

#include "TestPInvokeIdlC.h"
#include "dds/DdsDcpsInfrastructureC.h"
#include "dds/DdsDcpsPublicationC.h"
#include "dds/DdsDcpsSubscriptionExtC.h"
#include "dds/DdsDcpsTopicC.h"
#include "dds/DdsDcpsTypeSupportExtC.h"

#if TAO_MAJOR_VERSION != 2 || TAO_MINOR_VERSION != 2 || TAO_BETA_VERSION != 0
#error This file should be regenerated with TAO_IDL
#endif

#if defined (TAO_EXPORT_MACRO)
#undef TAO_EXPORT_MACRO
#endif
#define TAO_EXPORT_MACRO TestPInvokeIdlIDL_Export

// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_module\module_ch.cpp:38

namespace Test
{

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_sequence\sequence_ch.cpp:102

  typedef ::TAO::DCPS::ZeroCopyDataSeq< Test::NestedTestStruct, DCPS_ZERO_COPY_SEQ_DEFAULT_SIZE> NestedTestStructSeq;
  

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_interface.cpp:751

#if !defined (_TEST_NESTEDTESTSTRUCTTYPESUPPORT__VAR_OUT_CH_)
#define _TEST_NESTEDTESTSTRUCTTYPESUPPORT__VAR_OUT_CH_

  class NestedTestStructTypeSupport;
  typedef NestedTestStructTypeSupport *NestedTestStructTypeSupport_ptr;

  typedef
    TAO_Objref_Var_T<
        NestedTestStructTypeSupport
      >
    NestedTestStructTypeSupport_var;
  
  typedef
    TAO_Objref_Out_T<
        NestedTestStructTypeSupport
      >
    NestedTestStructTypeSupport_out;

#endif /* end #if !defined */

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:43

  class TestPInvokeIdlIDL_Export NestedTestStructTypeSupport
    : public virtual ::OpenDDS::DCPS::TypeSupport
  
  {
  public:

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:307

    typedef NestedTestStructTypeSupport_ptr _ptr_type;
    typedef NestedTestStructTypeSupport_var _var_type;
    typedef NestedTestStructTypeSupport_out _out_type;

    // The static operations.
    static NestedTestStructTypeSupport_ptr _duplicate (NestedTestStructTypeSupport_ptr obj);

    static void _tao_release (NestedTestStructTypeSupport_ptr obj);

    static NestedTestStructTypeSupport_ptr _narrow (::CORBA::Object_ptr obj);
    static NestedTestStructTypeSupport_ptr _unchecked_narrow (::CORBA::Object_ptr obj);
    static NestedTestStructTypeSupport_ptr _nil (void);

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:140

    virtual ::CORBA::Boolean _is_a (const char *type_id);
    virtual const char* _interface_repository_id (void) const;
    virtual ::CORBA::Boolean marshal (TAO_OutputCDR &cdr);
  
  protected:
    // Abstract or local interface only.
    NestedTestStructTypeSupport (void);

    

    virtual ~NestedTestStructTypeSupport (void);
  
  private:
    // Private and unimplemented for concrete interfaces.
    NestedTestStructTypeSupport (const NestedTestStructTypeSupport &);

    void operator= (const NestedTestStructTypeSupport &);
  };

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_interface.cpp:751

#if !defined (_TEST_NESTEDTESTSTRUCTDATAWRITER__VAR_OUT_CH_)
#define _TEST_NESTEDTESTSTRUCTDATAWRITER__VAR_OUT_CH_

  class NestedTestStructDataWriter;
  typedef NestedTestStructDataWriter *NestedTestStructDataWriter_ptr;

  typedef
    TAO_Objref_Var_T<
        NestedTestStructDataWriter
      >
    NestedTestStructDataWriter_var;
  
  typedef
    TAO_Objref_Out_T<
        NestedTestStructDataWriter
      >
    NestedTestStructDataWriter_out;

#endif /* end #if !defined */

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:43

  class TestPInvokeIdlIDL_Export NestedTestStructDataWriter
    : public virtual ::DDS::DataWriter
  
  {
  public:

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:307

    typedef NestedTestStructDataWriter_ptr _ptr_type;
    typedef NestedTestStructDataWriter_var _var_type;
    typedef NestedTestStructDataWriter_out _out_type;

    // The static operations.
    static NestedTestStructDataWriter_ptr _duplicate (NestedTestStructDataWriter_ptr obj);

    static void _tao_release (NestedTestStructDataWriter_ptr obj);

    static NestedTestStructDataWriter_ptr _narrow (::CORBA::Object_ptr obj);
    static NestedTestStructDataWriter_ptr _unchecked_narrow (::CORBA::Object_ptr obj);
    static NestedTestStructDataWriter_ptr _nil (void);

    virtual ::DDS::InstanceHandle_t register_instance (
      const ::Test::NestedTestStruct & instance) = 0;

    virtual ::DDS::InstanceHandle_t register_instance_w_timestamp (
      const ::Test::NestedTestStruct & instance,
      const ::DDS::Time_t & timestamp) = 0;

    virtual ::DDS::ReturnCode_t unregister_instance (
      const ::Test::NestedTestStruct & instance,
      ::DDS::InstanceHandle_t handle) = 0;

    virtual ::DDS::ReturnCode_t unregister_instance_w_timestamp (
      const ::Test::NestedTestStruct & instance,
      ::DDS::InstanceHandle_t handle,
      const ::DDS::Time_t & timestamp) = 0;

    virtual ::DDS::ReturnCode_t write (
      const ::Test::NestedTestStruct & instance_data,
      ::DDS::InstanceHandle_t handle) = 0;

    virtual ::DDS::ReturnCode_t write_w_timestamp (
      const ::Test::NestedTestStruct & instance_data,
      ::DDS::InstanceHandle_t handle,
      const ::DDS::Time_t & source_timestamp) = 0;

    virtual ::DDS::ReturnCode_t dispose (
      const ::Test::NestedTestStruct & instance_data,
      ::DDS::InstanceHandle_t instance_handle) = 0;

    virtual ::DDS::ReturnCode_t dispose_w_timestamp (
      const ::Test::NestedTestStruct & instance_data,
      ::DDS::InstanceHandle_t instance_handle,
      const ::DDS::Time_t & source_timestamp) = 0;

    virtual ::DDS::ReturnCode_t get_key_value (
      ::Test::NestedTestStruct & key_holder,
      ::DDS::InstanceHandle_t handle) = 0;

    virtual ::DDS::InstanceHandle_t lookup_instance (
      const ::Test::NestedTestStruct & instance_data) = 0;

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:140

    virtual ::CORBA::Boolean _is_a (const char *type_id);
    virtual const char* _interface_repository_id (void) const;
    virtual ::CORBA::Boolean marshal (TAO_OutputCDR &cdr);
  
  protected:
    // Abstract or local interface only.
    NestedTestStructDataWriter (void);

    

    virtual ~NestedTestStructDataWriter (void);
  
  private:
    // Private and unimplemented for concrete interfaces.
    NestedTestStructDataWriter (const NestedTestStructDataWriter &);

    void operator= (const NestedTestStructDataWriter &);
  };

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_interface.cpp:751

#if !defined (_TEST_NESTEDTESTSTRUCTDATAREADER__VAR_OUT_CH_)
#define _TEST_NESTEDTESTSTRUCTDATAREADER__VAR_OUT_CH_

  class NestedTestStructDataReader;
  typedef NestedTestStructDataReader *NestedTestStructDataReader_ptr;

  typedef
    TAO_Objref_Var_T<
        NestedTestStructDataReader
      >
    NestedTestStructDataReader_var;
  
  typedef
    TAO_Objref_Out_T<
        NestedTestStructDataReader
      >
    NestedTestStructDataReader_out;

#endif /* end #if !defined */

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:43

  class TestPInvokeIdlIDL_Export NestedTestStructDataReader
    : public virtual ::OpenDDS::DCPS::DataReaderEx
  
  {
  public:

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:307

    typedef NestedTestStructDataReader_ptr _ptr_type;
    typedef NestedTestStructDataReader_var _var_type;
    typedef NestedTestStructDataReader_out _out_type;

    // The static operations.
    static NestedTestStructDataReader_ptr _duplicate (NestedTestStructDataReader_ptr obj);

    static void _tao_release (NestedTestStructDataReader_ptr obj);

    static NestedTestStructDataReader_ptr _narrow (::CORBA::Object_ptr obj);
    static NestedTestStructDataReader_ptr _unchecked_narrow (::CORBA::Object_ptr obj);
    static NestedTestStructDataReader_ptr _nil (void);

    virtual ::DDS::ReturnCode_t read (
      ::Test::NestedTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t take (
      ::Test::NestedTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t read_w_condition (
      ::Test::NestedTestStructSeq & data_values,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t take_w_condition (
      ::Test::NestedTestStructSeq & data_values,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t read_next_sample (
      ::Test::NestedTestStruct & received_data,
      ::DDS::SampleInfo & sample_info) = 0;

    virtual ::DDS::ReturnCode_t take_next_sample (
      ::Test::NestedTestStruct & received_data,
      ::DDS::SampleInfo & sample_info) = 0;

    virtual ::DDS::ReturnCode_t read_instance (
      ::Test::NestedTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t take_instance (
      ::Test::NestedTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t read_instance_w_condition (
      ::Test::NestedTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t take_instance_w_condition (
      ::Test::NestedTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t read_next_instance (
      ::Test::NestedTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t take_next_instance (
      ::Test::NestedTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t read_next_instance_w_condition (
      ::Test::NestedTestStructSeq & data_values,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t previous_handle,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t take_next_instance_w_condition (
      ::Test::NestedTestStructSeq & data_values,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t previous_handle,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t return_loan (
      ::Test::NestedTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq) = 0;

    virtual ::DDS::ReturnCode_t get_key_value (
      ::Test::NestedTestStruct & key_holder,
      ::DDS::InstanceHandle_t handle) = 0;

    virtual ::DDS::InstanceHandle_t lookup_instance (
      const ::Test::NestedTestStruct & instance_data) = 0;

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:140

    virtual ::CORBA::Boolean _is_a (const char *type_id);
    virtual const char* _interface_repository_id (void) const;
    virtual ::CORBA::Boolean marshal (TAO_OutputCDR &cdr);
  
  protected:
    // Abstract or local interface only.
    NestedTestStructDataReader (void);

    

    virtual ~NestedTestStructDataReader (void);
  
  private:
    // Private and unimplemented for concrete interfaces.
    NestedTestStructDataReader (const NestedTestStructDataReader &);

    void operator= (const NestedTestStructDataReader &);
  };

// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_module\module_ch.cpp:67

} // module Test

// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_module\module_ch.cpp:38

namespace Test
{

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_sequence\sequence_ch.cpp:102

  typedef ::TAO::DCPS::ZeroCopyDataSeq< Test::BasicTestStruct, DCPS_ZERO_COPY_SEQ_DEFAULT_SIZE> BasicTestStructSeq;
  

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_interface.cpp:751

#if !defined (_TEST_BASICTESTSTRUCTTYPESUPPORT__VAR_OUT_CH_)
#define _TEST_BASICTESTSTRUCTTYPESUPPORT__VAR_OUT_CH_

  class BasicTestStructTypeSupport;
  typedef BasicTestStructTypeSupport *BasicTestStructTypeSupport_ptr;

  typedef
    TAO_Objref_Var_T<
        BasicTestStructTypeSupport
      >
    BasicTestStructTypeSupport_var;
  
  typedef
    TAO_Objref_Out_T<
        BasicTestStructTypeSupport
      >
    BasicTestStructTypeSupport_out;

#endif /* end #if !defined */

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:43

  class TestPInvokeIdlIDL_Export BasicTestStructTypeSupport
    : public virtual ::OpenDDS::DCPS::TypeSupport
  
  {
  public:

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:307

    typedef BasicTestStructTypeSupport_ptr _ptr_type;
    typedef BasicTestStructTypeSupport_var _var_type;
    typedef BasicTestStructTypeSupport_out _out_type;

    // The static operations.
    static BasicTestStructTypeSupport_ptr _duplicate (BasicTestStructTypeSupport_ptr obj);

    static void _tao_release (BasicTestStructTypeSupport_ptr obj);

    static BasicTestStructTypeSupport_ptr _narrow (::CORBA::Object_ptr obj);
    static BasicTestStructTypeSupport_ptr _unchecked_narrow (::CORBA::Object_ptr obj);
    static BasicTestStructTypeSupport_ptr _nil (void);

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:140

    virtual ::CORBA::Boolean _is_a (const char *type_id);
    virtual const char* _interface_repository_id (void) const;
    virtual ::CORBA::Boolean marshal (TAO_OutputCDR &cdr);
  
  protected:
    // Abstract or local interface only.
    BasicTestStructTypeSupport (void);

    

    virtual ~BasicTestStructTypeSupport (void);
  
  private:
    // Private and unimplemented for concrete interfaces.
    BasicTestStructTypeSupport (const BasicTestStructTypeSupport &);

    void operator= (const BasicTestStructTypeSupport &);
  };

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_interface.cpp:751

#if !defined (_TEST_BASICTESTSTRUCTDATAWRITER__VAR_OUT_CH_)
#define _TEST_BASICTESTSTRUCTDATAWRITER__VAR_OUT_CH_

  class BasicTestStructDataWriter;
  typedef BasicTestStructDataWriter *BasicTestStructDataWriter_ptr;

  typedef
    TAO_Objref_Var_T<
        BasicTestStructDataWriter
      >
    BasicTestStructDataWriter_var;
  
  typedef
    TAO_Objref_Out_T<
        BasicTestStructDataWriter
      >
    BasicTestStructDataWriter_out;

#endif /* end #if !defined */

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:43

  class TestPInvokeIdlIDL_Export BasicTestStructDataWriter
    : public virtual ::DDS::DataWriter
  
  {
  public:

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:307

    typedef BasicTestStructDataWriter_ptr _ptr_type;
    typedef BasicTestStructDataWriter_var _var_type;
    typedef BasicTestStructDataWriter_out _out_type;

    // The static operations.
    static BasicTestStructDataWriter_ptr _duplicate (BasicTestStructDataWriter_ptr obj);

    static void _tao_release (BasicTestStructDataWriter_ptr obj);

    static BasicTestStructDataWriter_ptr _narrow (::CORBA::Object_ptr obj);
    static BasicTestStructDataWriter_ptr _unchecked_narrow (::CORBA::Object_ptr obj);
    static BasicTestStructDataWriter_ptr _nil (void);

    virtual ::DDS::InstanceHandle_t register_instance (
      const ::Test::BasicTestStruct & instance) = 0;

    virtual ::DDS::InstanceHandle_t register_instance_w_timestamp (
      const ::Test::BasicTestStruct & instance,
      const ::DDS::Time_t & timestamp) = 0;

    virtual ::DDS::ReturnCode_t unregister_instance (
      const ::Test::BasicTestStruct & instance,
      ::DDS::InstanceHandle_t handle) = 0;

    virtual ::DDS::ReturnCode_t unregister_instance_w_timestamp (
      const ::Test::BasicTestStruct & instance,
      ::DDS::InstanceHandle_t handle,
      const ::DDS::Time_t & timestamp) = 0;

    virtual ::DDS::ReturnCode_t write (
      const ::Test::BasicTestStruct & instance_data,
      ::DDS::InstanceHandle_t handle) = 0;

    virtual ::DDS::ReturnCode_t write_w_timestamp (
      const ::Test::BasicTestStruct & instance_data,
      ::DDS::InstanceHandle_t handle,
      const ::DDS::Time_t & source_timestamp) = 0;

    virtual ::DDS::ReturnCode_t dispose (
      const ::Test::BasicTestStruct & instance_data,
      ::DDS::InstanceHandle_t instance_handle) = 0;

    virtual ::DDS::ReturnCode_t dispose_w_timestamp (
      const ::Test::BasicTestStruct & instance_data,
      ::DDS::InstanceHandle_t instance_handle,
      const ::DDS::Time_t & source_timestamp) = 0;

    virtual ::DDS::ReturnCode_t get_key_value (
      ::Test::BasicTestStruct & key_holder,
      ::DDS::InstanceHandle_t handle) = 0;

    virtual ::DDS::InstanceHandle_t lookup_instance (
      const ::Test::BasicTestStruct & instance_data) = 0;

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:140

    virtual ::CORBA::Boolean _is_a (const char *type_id);
    virtual const char* _interface_repository_id (void) const;
    virtual ::CORBA::Boolean marshal (TAO_OutputCDR &cdr);
  
  protected:
    // Abstract or local interface only.
    BasicTestStructDataWriter (void);

    

    virtual ~BasicTestStructDataWriter (void);
  
  private:
    // Private and unimplemented for concrete interfaces.
    BasicTestStructDataWriter (const BasicTestStructDataWriter &);

    void operator= (const BasicTestStructDataWriter &);
  };

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_interface.cpp:751

#if !defined (_TEST_BASICTESTSTRUCTDATAREADER__VAR_OUT_CH_)
#define _TEST_BASICTESTSTRUCTDATAREADER__VAR_OUT_CH_

  class BasicTestStructDataReader;
  typedef BasicTestStructDataReader *BasicTestStructDataReader_ptr;

  typedef
    TAO_Objref_Var_T<
        BasicTestStructDataReader
      >
    BasicTestStructDataReader_var;
  
  typedef
    TAO_Objref_Out_T<
        BasicTestStructDataReader
      >
    BasicTestStructDataReader_out;

#endif /* end #if !defined */

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:43

  class TestPInvokeIdlIDL_Export BasicTestStructDataReader
    : public virtual ::OpenDDS::DCPS::DataReaderEx
  
  {
  public:

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:307

    typedef BasicTestStructDataReader_ptr _ptr_type;
    typedef BasicTestStructDataReader_var _var_type;
    typedef BasicTestStructDataReader_out _out_type;

    // The static operations.
    static BasicTestStructDataReader_ptr _duplicate (BasicTestStructDataReader_ptr obj);

    static void _tao_release (BasicTestStructDataReader_ptr obj);

    static BasicTestStructDataReader_ptr _narrow (::CORBA::Object_ptr obj);
    static BasicTestStructDataReader_ptr _unchecked_narrow (::CORBA::Object_ptr obj);
    static BasicTestStructDataReader_ptr _nil (void);

    virtual ::DDS::ReturnCode_t read (
      ::Test::BasicTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t take (
      ::Test::BasicTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t read_w_condition (
      ::Test::BasicTestStructSeq & data_values,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t take_w_condition (
      ::Test::BasicTestStructSeq & data_values,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t read_next_sample (
      ::Test::BasicTestStruct & received_data,
      ::DDS::SampleInfo & sample_info) = 0;

    virtual ::DDS::ReturnCode_t take_next_sample (
      ::Test::BasicTestStruct & received_data,
      ::DDS::SampleInfo & sample_info) = 0;

    virtual ::DDS::ReturnCode_t read_instance (
      ::Test::BasicTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t take_instance (
      ::Test::BasicTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t read_instance_w_condition (
      ::Test::BasicTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t take_instance_w_condition (
      ::Test::BasicTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t read_next_instance (
      ::Test::BasicTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t take_next_instance (
      ::Test::BasicTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t a_handle,
      ::DDS::SampleStateMask sample_states,
      ::DDS::ViewStateMask view_states,
      ::DDS::InstanceStateMask instance_states) = 0;

    virtual ::DDS::ReturnCode_t read_next_instance_w_condition (
      ::Test::BasicTestStructSeq & data_values,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t previous_handle,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t take_next_instance_w_condition (
      ::Test::BasicTestStructSeq & data_values,
      ::DDS::SampleInfoSeq & sample_infos,
      ::CORBA::Long max_samples,
      ::DDS::InstanceHandle_t previous_handle,
      ::DDS::ReadCondition_ptr a_condition) = 0;

    virtual ::DDS::ReturnCode_t return_loan (
      ::Test::BasicTestStructSeq & received_data,
      ::DDS::SampleInfoSeq & info_seq) = 0;

    virtual ::DDS::ReturnCode_t get_key_value (
      ::Test::BasicTestStruct & key_holder,
      ::DDS::InstanceHandle_t handle) = 0;

    virtual ::DDS::InstanceHandle_t lookup_instance (
      const ::Test::BasicTestStruct & instance_data) = 0;

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_interface\interface_ch.cpp:140

    virtual ::CORBA::Boolean _is_a (const char *type_id);
    virtual const char* _interface_repository_id (void) const;
    virtual ::CORBA::Boolean marshal (TAO_OutputCDR &cdr);
  
  protected:
    // Abstract or local interface only.
    BasicTestStructDataReader (void);

    

    virtual ~BasicTestStructDataReader (void);
  
  private:
    // Private and unimplemented for concrete interfaces.
    BasicTestStructDataReader (const BasicTestStructDataReader &);

    void operator= (const BasicTestStructDataReader &);
  };

// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_module\module_ch.cpp:67

} // module Test

// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_arg_traits.cpp:68

TAO_BEGIN_VERSIONED_NAMESPACE_DECL


// Arg traits specializations.
namespace TAO
{
}

TAO_END_VERSIONED_NAMESPACE_DECL



// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_traits.cpp:62

TAO_BEGIN_VERSIONED_NAMESPACE_DECL

// Traits specializations.
namespace TAO
{

#if !defined (_TEST_NESTEDTESTSTRUCTTYPESUPPORT__TRAITS_)
#define _TEST_NESTEDTESTSTRUCTTYPESUPPORT__TRAITS_

  template<>
  struct TestPInvokeIdlIDL_Export Objref_Traits< ::Test::NestedTestStructTypeSupport>
  {
    static ::Test::NestedTestStructTypeSupport_ptr duplicate (
        ::Test::NestedTestStructTypeSupport_ptr p);
    static void release (
        ::Test::NestedTestStructTypeSupport_ptr p);
    static ::Test::NestedTestStructTypeSupport_ptr nil (void);
    static ::CORBA::Boolean marshal (
        const ::Test::NestedTestStructTypeSupport_ptr p,
        TAO_OutputCDR & cdr);
  };

#endif /* end #if !defined */

#if !defined (_TEST_NESTEDTESTSTRUCTDATAWRITER__TRAITS_)
#define _TEST_NESTEDTESTSTRUCTDATAWRITER__TRAITS_

  template<>
  struct TestPInvokeIdlIDL_Export Objref_Traits< ::Test::NestedTestStructDataWriter>
  {
    static ::Test::NestedTestStructDataWriter_ptr duplicate (
        ::Test::NestedTestStructDataWriter_ptr p);
    static void release (
        ::Test::NestedTestStructDataWriter_ptr p);
    static ::Test::NestedTestStructDataWriter_ptr nil (void);
    static ::CORBA::Boolean marshal (
        const ::Test::NestedTestStructDataWriter_ptr p,
        TAO_OutputCDR & cdr);
  };

#endif /* end #if !defined */

#if !defined (_TEST_NESTEDTESTSTRUCTDATAREADER__TRAITS_)
#define _TEST_NESTEDTESTSTRUCTDATAREADER__TRAITS_

  template<>
  struct TestPInvokeIdlIDL_Export Objref_Traits< ::Test::NestedTestStructDataReader>
  {
    static ::Test::NestedTestStructDataReader_ptr duplicate (
        ::Test::NestedTestStructDataReader_ptr p);
    static void release (
        ::Test::NestedTestStructDataReader_ptr p);
    static ::Test::NestedTestStructDataReader_ptr nil (void);
    static ::CORBA::Boolean marshal (
        const ::Test::NestedTestStructDataReader_ptr p,
        TAO_OutputCDR & cdr);
  };

#endif /* end #if !defined */

#if !defined (_TEST_BASICTESTSTRUCTTYPESUPPORT__TRAITS_)
#define _TEST_BASICTESTSTRUCTTYPESUPPORT__TRAITS_

  template<>
  struct TestPInvokeIdlIDL_Export Objref_Traits< ::Test::BasicTestStructTypeSupport>
  {
    static ::Test::BasicTestStructTypeSupport_ptr duplicate (
        ::Test::BasicTestStructTypeSupport_ptr p);
    static void release (
        ::Test::BasicTestStructTypeSupport_ptr p);
    static ::Test::BasicTestStructTypeSupport_ptr nil (void);
    static ::CORBA::Boolean marshal (
        const ::Test::BasicTestStructTypeSupport_ptr p,
        TAO_OutputCDR & cdr);
  };

#endif /* end #if !defined */

#if !defined (_TEST_BASICTESTSTRUCTDATAWRITER__TRAITS_)
#define _TEST_BASICTESTSTRUCTDATAWRITER__TRAITS_

  template<>
  struct TestPInvokeIdlIDL_Export Objref_Traits< ::Test::BasicTestStructDataWriter>
  {
    static ::Test::BasicTestStructDataWriter_ptr duplicate (
        ::Test::BasicTestStructDataWriter_ptr p);
    static void release (
        ::Test::BasicTestStructDataWriter_ptr p);
    static ::Test::BasicTestStructDataWriter_ptr nil (void);
    static ::CORBA::Boolean marshal (
        const ::Test::BasicTestStructDataWriter_ptr p,
        TAO_OutputCDR & cdr);
  };

#endif /* end #if !defined */

#if !defined (_TEST_BASICTESTSTRUCTDATAREADER__TRAITS_)
#define _TEST_BASICTESTSTRUCTDATAREADER__TRAITS_

  template<>
  struct TestPInvokeIdlIDL_Export Objref_Traits< ::Test::BasicTestStructDataReader>
  {
    static ::Test::BasicTestStructDataReader_ptr duplicate (
        ::Test::BasicTestStructDataReader_ptr p);
    static void release (
        ::Test::BasicTestStructDataReader_ptr p);
    static ::Test::BasicTestStructDataReader_ptr nil (void);
    static ::CORBA::Boolean marshal (
        const ::Test::BasicTestStructDataReader_ptr p,
        TAO_OutputCDR & cdr);
  };

#endif /* end #if !defined */
}
TAO_END_VERSIONED_NAMESPACE_DECL



// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_codegen.cpp:1703
#if defined (__ACE_INLINE__)
#include "TestPInvokeIdlTypeSupportC.inl"
#endif /* defined INLINE */

#include /**/ "ace/post.h"

#endif /* ifndef */

