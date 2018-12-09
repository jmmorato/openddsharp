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

#ifndef _TAO_IDL_TESTPINVOKEIDLC_LC7DWC_H_
#define _TAO_IDL_TESTPINVOKEIDLC_LC7DWC_H_

#include /**/ "ace/pre.h"


#include /**/ "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */


#include /**/ "TestPInvokeIdlIDL_Export.h"
#include "tao/ORB.h"
#include "tao/Basic_Types.h"
#include "tao/String_Manager_T.h"
#include "tao/Sequence_T.h"
#include "tao/Seq_Var_T.h"
#include "tao/Seq_Out_T.h"
#include "tao/VarOut_T.h"
#include "tao/Arg_Traits_T.h"
#include "tao/Basic_Arguments.h"
#include "tao/Special_Basic_Arguments.h"
#include "tao/Any_Insert_Policy_T.h"
#include "tao/Fixed_Size_Argument_T.h"
#include "tao/Var_Size_Argument_T.h"
#include "tao/Object_Argument_T.h"
#include "tao/UB_String_Arguments.h"
#include /**/ "tao/Version.h"
#include /**/ "tao/Versioned_Namespace.h"

#include "tao/BooleanSeqC.h"
#include "tao/CharSeqC.h"
#include "tao/DoubleSeqC.h"
#include "tao/FloatSeqC.h"
#include "tao/LongDoubleSeqC.h"
#include "tao/LongSeqC.h"
#include "tao/OctetSeqC.h"
#include "tao/ShortSeqC.h"
#include "tao/StringSeqC.h"
#include "tao/ULongSeqC.h"
#include "tao/UShortSeqC.h"
#include "tao/WCharSeqC.h"
#include "tao/WStringSeqC.h"
#include "tao/LongLongSeqC.h"
#include "tao/ULongLongSeqC.h"
#include "tao/PolicyC.h"
#include "tao/Policy_ManagerC.h"
#include "tao/Policy_CurrentC.h"
#include "tao/ServicesC.h"
#include "tao/ParameterModeC.h"
#include "tao/orb_typesC.h"
#include "tao/Typecode_typesC.h"
#include "tao/WrongTransactionC.h"

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

#if !defined (_TEST_LONGLIST_CH_)
#define _TEST_LONGLIST_CH_

  class LongList;

  typedef
    ::TAO_FixedSeq_Var_T<
        LongList
      >
    LongList_var;

  typedef
    ::TAO_Seq_Out_T<
        LongList
      >
    LongList_out;

  class TestPInvokeIdlIDL_Export LongList
    : public
        ::TAO::unbounded_value_sequence<
            ::CORBA::Long
          >
  {
  public:
    LongList (void);
    LongList ( ::CORBA::ULong max);
    LongList (
      ::CORBA::ULong max,
      ::CORBA::ULong length,
      ::CORBA::Long* buffer,
      ::CORBA::Boolean release = false);
    LongList (const LongList &);
    virtual ~LongList (void);
    

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:307

    
    typedef LongList_var _var_type;
    typedef LongList_out _out_type;
  };

#endif /* end #if !defined */

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_sequence\sequence_ch.cpp:102

#if !defined (_TEST_STRINGLIST_CH_)
#define _TEST_STRINGLIST_CH_

  class StringList;

  typedef
    ::TAO_VarSeq_Var_T<
        StringList
      >
    StringList_var;

  typedef
    ::TAO_Seq_Out_T<
        StringList
      >
    StringList_out;

  class TestPInvokeIdlIDL_Export StringList
    : public
        ::TAO::unbounded_basic_string_sequence<char>
  {
  public:
    StringList (void);
    StringList ( ::CORBA::ULong max);
    StringList (
      ::CORBA::ULong max,
      ::CORBA::ULong length,
      ::CORBA::Char ** buffer,
      ::CORBA::Boolean release = false);
    StringList (const StringList &);
    virtual ~StringList (void);
    

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:307

    
    typedef StringList_var _var_type;
    typedef StringList_out _out_type;
  };

#endif /* end #if !defined */

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:261

  struct BasicTestStruct;

  typedef
    ::TAO_Var_Var_T<
        BasicTestStruct
      >
    BasicTestStruct_var;

  typedef
    ::TAO_Out_T<
        BasicTestStruct
      >
    BasicTestStruct_out;

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_structure\structure_ch.cpp:51

  struct TestPInvokeIdlIDL_Export BasicTestStruct
  {

    // TAO_IDL - Generated from
    // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_type.cpp:307

    
    typedef BasicTestStruct_var _var_type;
    typedef BasicTestStruct_out _out_type;
    
    ::CORBA::Long Id;
    ::TAO::String_Manager Message;
    Test::LongList LongSequence;
    Test::StringList StringSequence;
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

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_arg_traits.cpp:685

  template<>
  class Arg_Traits< ::Test::LongList>
    : public
        Var_Size_Arg_Traits_T<
            ::Test::LongList,
            TAO::Any_Insert_Policy_Noop
          >
  {
  };

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_arg_traits.cpp:685

  template<>
  class Arg_Traits< ::Test::StringList>
    : public
        Var_Size_Arg_Traits_T<
            ::Test::StringList,
            TAO::Any_Insert_Policy_Noop
          >
  {
  };

  // TAO_IDL - Generated from
  // e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_arg_traits.cpp:947

  template<>
  class Arg_Traits< ::Test::BasicTestStruct>
    : public
        Var_Size_Arg_Traits_T<
            ::Test::BasicTestStruct,
            TAO::Any_Insert_Policy_Noop
          >
  {
  };
}

TAO_END_VERSIONED_NAMESPACE_DECL



// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_traits.cpp:62

TAO_BEGIN_VERSIONED_NAMESPACE_DECL

// Traits specializations.
namespace TAO
{
}
TAO_END_VERSIONED_NAMESPACE_DECL



// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_sequence\cdr_op_ch.cpp:68

#if !defined _TAO_CDR_OP_Test_LongList_H_
#define _TAO_CDR_OP_Test_LongList_H_
TAO_BEGIN_VERSIONED_NAMESPACE_DECL


TestPInvokeIdlIDL_Export ::CORBA::Boolean operator<< (
    TAO_OutputCDR &strm,
    const Test::LongList &_tao_sequence);
TestPInvokeIdlIDL_Export ::CORBA::Boolean operator>> (
    TAO_InputCDR &strm,
    Test::LongList &_tao_sequence);

TAO_END_VERSIONED_NAMESPACE_DECL



#endif /* _TAO_CDR_OP_Test_LongList_H_ */

// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_sequence\cdr_op_ch.cpp:68

#if !defined _TAO_CDR_OP_Test_StringList_H_
#define _TAO_CDR_OP_Test_StringList_H_
TAO_BEGIN_VERSIONED_NAMESPACE_DECL


TestPInvokeIdlIDL_Export ::CORBA::Boolean operator<< (
    TAO_OutputCDR &strm,
    const Test::StringList &_tao_sequence);
TestPInvokeIdlIDL_Export ::CORBA::Boolean operator>> (
    TAO_InputCDR &strm,
    Test::StringList &_tao_sequence);

TAO_END_VERSIONED_NAMESPACE_DECL



#endif /* _TAO_CDR_OP_Test_StringList_H_ */

// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_visitor_structure\cdr_op_ch.cpp:46


TAO_BEGIN_VERSIONED_NAMESPACE_DECL

TestPInvokeIdlIDL_Export ::CORBA::Boolean operator<< (TAO_OutputCDR &, const Test::BasicTestStruct &);
TestPInvokeIdlIDL_Export ::CORBA::Boolean operator>> (TAO_InputCDR &, Test::BasicTestStruct &);

TAO_END_VERSIONED_NAMESPACE_DECL



// TAO_IDL - Generated from
// e:\projects\opendds\ext\opendds\ace_wrappers\tao\tao_idl\be\be_codegen.cpp:1703
#if defined (__ACE_INLINE__)
#include "TestPInvokeIdlC.inl"
#endif /* defined INLINE */

#include /**/ "ace/post.h"

#endif /* ifndef */

