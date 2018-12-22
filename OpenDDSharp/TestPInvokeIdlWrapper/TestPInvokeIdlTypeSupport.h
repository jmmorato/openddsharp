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
    CORBA::Long Id;
    CORBA::Char* Message;
    CORBA::WChar* WMessage;
    void* LongSequence;
    void* StringSequence;
    void* WStringSequence;
    CORBA::Long LongArray[5];
    CORBA::Char* StringArray[10];
    CORBA::WChar* WStringArray[4];
    NestedTestStructWrapper StructTest;
    void* StructSequence;
    NestedTestStructWrapper StructArray[5];
    void* LongMultiArray;
    void* StringMultiArray;
    void* WStringMultiArray;
    void* StructMultiArray;
    CORBA::Float FloatType;
    CORBA::Double DoubleType;
    CORBA::Double LongDoubleType;
    CORBA::Float FloatArray[5];
    CORBA::Double DoubleArray[5];
    CORBA::Double LongDoubleArray[5];
    void* FloatSequence;
    void* DoubleSequence;
    void* LongDoubleSequence;
    void* FloatMultiArray;
    void* DoubleMultiArray;
    void* LongDoubleMultiArray;
    CORBA::Char CharType;
    CORBA::WChar WCharType;
    CORBA::Char CharArray[5];
    CORBA::WChar WCharArray[5];
    void* CharSequence;
    void* WCharSequence;
    void* CharMultiArray;
    void* WCharMultiArray;
    CORBA::Short ShortType;
    CORBA::LongLong LongLongType;
    CORBA::UShort UnsignedShortType;
    CORBA::ULong UnsignedLongType;
    CORBA::ULongLong UnsignedLongLongType;
    CORBA::Short ShortArray[5];
    CORBA::LongLong LongLongArray[5];
    CORBA::UShort UnsignedShortArray[5];
    CORBA::ULong UnsignedLongArray[5];
    CORBA::ULongLong UnsignedLongLongArray[5];
    void* ShortSequence;
    void* LongLongSequence;
    void* UnsignedShortSequence;
    void* UnsignedLongSequence;
    void* UnsignedLongLongSequence;
    void* ShortMultiArray;
    void* LongLongMultiArray;
    void* UnsignedShortMultiArray;
    void* UnsignedLongMultiArray;
    void* UnsignedLongLongMultiArray;
    CORBA::Boolean BooleanType;
    CORBA::Octet OctetType;
    CORBA::Boolean BooleanArray[5];
    CORBA::Octet OctetArray[5];
    void* BooleanSequence;
    void* OctetSequence;
    void* BooleanMultiArray;
    void* OctetMultiArray;

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
            ACE_OS::memcpy(nativeData.LongArray, LongArray, sizeof(CORBA::Long) * 5);
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
            ACE_OS::memcpy(nativeData.LongMultiArray, LongMultiArray, sizeof(CORBA::Long) * 24);
        }

        // Multi-dimensional array of strings
        if (StringMultiArray != NULL)
        {
            char** arr_StringMultiArray = new char*[24];
            marshal::ptr_to_basic_string_multi_array(StringMultiArray, arr_StringMultiArray, 24);
            ACE_OS::memcpy(nativeData.StringMultiArray, arr_StringMultiArray, sizeof(CORBA::Char*) * 24);
            delete[] arr_StringMultiArray;
        }

        // Multi-dimensional array of wstrings
        if (WStringMultiArray != NULL)
        {
            wchar_t** arr_WStringMultiArray = new wchar_t*[24];
            marshal::ptr_to_wide_string_multi_array(WStringMultiArray, arr_WStringMultiArray, 24);
            ACE_OS::memcpy(nativeData.WStringMultiArray, arr_WStringMultiArray, sizeof(CORBA::WChar*) * 24);
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
        
        // Floating-point types
        nativeData.FloatType = FloatType;
        nativeData.DoubleType = DoubleType;          
        nativeData.LongDoubleType.assign(static_cast<long double>(LongDoubleType));        
        if (FloatArray != NULL)
        {
            ACE_OS::memcpy(nativeData.FloatArray, FloatArray, sizeof(CORBA::Float) * 5);
        }

        if (DoubleArray != NULL)
        {
            ACE_OS::memcpy(nativeData.DoubleArray, DoubleArray, sizeof(CORBA::Double) * 5);
        }

        if (LongDoubleArray != NULL)
        {            
            for (int i = 0; i < 5; i++)
            {
                nativeData.LongDoubleArray[i].assign(static_cast<long double>(LongDoubleArray[i]));
            }
        }

        if (FloatSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(FloatSequence, nativeData.FloatSequence);
        }

        if (DoubleSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(DoubleSequence, nativeData.DoubleSequence);
        }

        if (LongDoubleSequence != NULL)
        {
            TAO::unbounded_value_sequence<CORBA::Double> aux;
            marshal::ptr_to_unbounded_sequence(LongDoubleSequence, aux);

            int length = aux.length();
            nativeData.LongDoubleSequence.length(length);
            for (int i = 0; i < length; i++)
            {
                nativeData.LongDoubleSequence[i].assign(aux[i]);
            }
        }

        if (FloatMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.FloatMultiArray, FloatMultiArray, sizeof(CORBA::Float) * 24);
        }

        if (DoubleMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.DoubleMultiArray, DoubleMultiArray, sizeof(CORBA::Double) * 24);
        }

        if (LongDoubleMultiArray != NULL)
        {
            CORBA::Double aux[24];
            ACE_OS::memcpy(aux, LongDoubleMultiArray, sizeof(CORBA::Double) * 24);

            int i = 0;
            for (ACE_UINT32 i0 = 0; i0 < 3; ++i0) {
                for (ACE_UINT32 i1 = 0; i1 < 4; ++i1) {
                    for (ACE_UINT32 i2 = 0; i2 < 2; ++i2) {
                        nativeData.LongDoubleMultiArray[i0][i1][i2].assign(aux[i]);
                        i++;
                    }
                }
            }
        }

        // Char types
        nativeData.CharType = CharType;
        nativeData.WCharType = WCharType;

        if (CharArray != NULL)
        {
            ACE_OS::memcpy(nativeData.CharArray, CharArray, sizeof(CORBA::Char) * 5);
        }

        if (WCharArray != NULL)
        {
            ACE_OS::memcpy(nativeData.WCharArray, WCharArray, sizeof(CORBA::WChar) * 5);
        }

        if (CharSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(CharSequence, nativeData.CharSequence);
        }

        if (WCharSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(WCharSequence, nativeData.WCharSequence);
        }

        if (CharMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.CharMultiArray, CharMultiArray, sizeof(CORBA::Char) * 24);
        }

        if (WCharMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.WCharMultiArray, WCharMultiArray, sizeof(CORBA::WChar) * 24);
        }

        // Integer types
        nativeData.ShortType = ShortType;
        nativeData.LongLongType = LongLongType;
        nativeData.UnsignedShortType = UnsignedShortType;
        nativeData.UnsignedLongType = UnsignedLongType;
        nativeData.UnsignedLongLongType = UnsignedLongLongType;

        if (ShortArray != NULL)
        {
            ACE_OS::memcpy(nativeData.ShortArray, ShortArray, sizeof(CORBA::Short) * 5);
        }

        if (LongLongArray != NULL)
        {
            ACE_OS::memcpy(nativeData.LongLongArray, LongLongArray, sizeof(CORBA::LongLong) * 5);
        }

        if (UnsignedShortArray != NULL)
        {
            ACE_OS::memcpy(nativeData.UnsignedShortArray, UnsignedShortArray, sizeof(CORBA::UShort) * 5);
        }

        if (UnsignedLongArray != NULL)
        {
            ACE_OS::memcpy(nativeData.UnsignedLongArray, UnsignedLongArray, sizeof(CORBA::ULong) * 5);
        }

        if (UnsignedLongLongArray != NULL)
        {
            ACE_OS::memcpy(nativeData.UnsignedLongLongArray, UnsignedLongLongArray, sizeof(CORBA::ULongLong) * 5);
        }

        if (ShortSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(ShortSequence, nativeData.ShortSequence);
        }

        if (LongLongSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(LongLongSequence, nativeData.LongLongSequence);
        }

        if (UnsignedShortSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(UnsignedShortSequence, nativeData.UnsignedShortSequence);
        }

        if (UnsignedLongSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(UnsignedLongSequence, nativeData.UnsignedLongSequence);
        }

        if (UnsignedLongLongSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(UnsignedLongLongSequence, nativeData.UnsignedLongLongSequence);
        }

        if (ShortMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.ShortMultiArray, ShortMultiArray, sizeof(CORBA::Short) * 24);
        }

        if (LongLongMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.LongLongMultiArray, LongLongMultiArray, sizeof(CORBA::LongLong) * 24);
        }

        if (UnsignedShortMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.UnsignedShortMultiArray, UnsignedShortMultiArray, sizeof(CORBA::UShort) * 24);
        }

        if (UnsignedLongMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.UnsignedLongMultiArray, UnsignedLongMultiArray, sizeof(CORBA::ULong) * 24);
        }

        if (UnsignedLongLongMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.UnsignedLongLongMultiArray, UnsignedLongLongMultiArray, sizeof(CORBA::ULongLong) * 24);
        }

        // Other primitives types
        nativeData.BooleanType = BooleanType;
        nativeData.OctetType = OctetType;

        if (BooleanArray != NULL)
        {
            ACE_OS::memcpy(nativeData.BooleanArray, BooleanArray, sizeof(CORBA::Boolean) * 5);
        }

        if (OctetArray != NULL)
        {
            ACE_OS::memcpy(nativeData.OctetArray, OctetArray, sizeof(CORBA::Octet) * 5);
        }

        if (BooleanSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(BooleanSequence, nativeData.BooleanSequence);                             
        }

        if (OctetSequence != NULL)
        {
            marshal::ptr_to_unbounded_sequence(OctetSequence, nativeData.OctetSequence);
        }

        if (BooleanMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.BooleanMultiArray, BooleanMultiArray, sizeof(CORBA::Boolean) * 24);
        }

        if (OctetMultiArray != NULL)
        {
            ACE_OS::memcpy(nativeData.OctetMultiArray, OctetMultiArray, sizeof(CORBA::Octet) * 24);
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
            ACE_OS::memcpy(LongArray, nativeData.LongArray, sizeof(CORBA::Long) * 5);
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
            LongMultiArray = ACE_OS::malloc(sizeof(CORBA::Long) * 24);
            ACE_OS::memcpy(LongMultiArray, nativeData.LongMultiArray, sizeof(CORBA::Long) * 24);
        }

        // Multi-dimensional array of strings
        if (nativeData.StringMultiArray != NULL)
        {
            CORBA::Char** arr_StringMultiArray = new CORBA::Char*[24];
            ACE_OS::memcpy(arr_StringMultiArray, nativeData.StringMultiArray, sizeof(CORBA::Char*) * 24);
            marshal::basic_string_multi_array_to_ptr(arr_StringMultiArray, StringMultiArray, 24);
            delete[] arr_StringMultiArray;
        }

        // Multi-dimensional array of strings
        if (nativeData.WStringMultiArray != NULL)
        {
            CORBA::WChar** arr_WStringMultiArray = new CORBA::WChar*[24];
            ACE_OS::memcpy(arr_WStringMultiArray, nativeData.WStringMultiArray, sizeof(CORBA::WChar*) * 24);
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
        
        // Floating point types
        FloatType = nativeData.FloatType;
        DoubleType = nativeData.DoubleType;
        LongDoubleType = nativeData.LongDoubleType;

        if (nativeData.FloatArray != NULL)
        {
            ACE_OS::memcpy(FloatArray, nativeData.FloatArray, sizeof(CORBA::Float) * 5);
        }

        if (nativeData.DoubleArray != NULL)
        {
            ACE_OS::memcpy(DoubleArray, nativeData.DoubleArray, sizeof(CORBA::Double) * 5);
        }

        if (nativeData.LongDoubleArray != NULL)
        {
            for (int i = 0; i < 5; i++)
            {
                LongDoubleArray[i] = nativeData.LongDoubleArray[i];
            }
        }

        marshal::unbounded_sequence_to_ptr(nativeData.FloatSequence, FloatSequence);
        marshal::unbounded_sequence_to_ptr(nativeData.DoubleSequence, DoubleSequence);
        
        {
            int length = nativeData.LongDoubleSequence.length();
            TAO::unbounded_value_sequence<CORBA::Double> aux;
            aux.length(length);
            for (int i = 0; i < length; i++)
            {
                aux[i] = nativeData.LongDoubleSequence[i];
            }
            marshal::unbounded_sequence_to_ptr(aux, LongDoubleSequence);
        }

        if (nativeData.FloatMultiArray != NULL)
        {
            FloatMultiArray = ACE_OS::malloc(sizeof(CORBA::Float) * 24);
            ACE_OS::memcpy(FloatMultiArray, nativeData.FloatMultiArray, sizeof(CORBA::Float) * 24);
        }

        if (nativeData.DoubleMultiArray != NULL)
        {
            DoubleMultiArray = ACE_OS::malloc(sizeof(CORBA::Double) * 24);
            ACE_OS::memcpy(DoubleMultiArray, nativeData.DoubleMultiArray, sizeof(CORBA::Double) * 24);
        }

        if (nativeData.LongDoubleMultiArray != NULL)
        {
            CORBA::Double aux[3][4][2];
            for (ACE_UINT32 i0 = 0; i0 < 3; ++i0) {
                for (ACE_UINT32 i1 = 0; i1 < 4; ++i1) {
                    for (ACE_UINT32 i2 = 0; i2 < 2; ++i2) {
                        aux[i0][i1][i2] = nativeData.LongDoubleMultiArray[i0][i1][i2];
                    }
                }
            }

            LongDoubleMultiArray = ACE_OS::malloc(sizeof(CORBA::Double) * 24);            
            ACE_OS::memcpy(LongDoubleMultiArray, aux, sizeof(CORBA::Double) * 24);
        }

        // Char types
        CharType = nativeData.CharType;
        WCharType = nativeData.WCharType;

        if (nativeData.CharArray != NULL)
        {
            ACE_OS::memcpy(CharArray, nativeData.CharArray, sizeof(CORBA::Char) * 5);
        }

        if (nativeData.WCharArray != NULL)
        {
            ACE_OS::memcpy(WCharArray, nativeData.WCharArray, sizeof(CORBA::WChar) * 5);
        }

        marshal::unbounded_sequence_to_ptr(nativeData.CharSequence, CharSequence);
        marshal::unbounded_sequence_to_ptr(nativeData.WCharSequence, WCharSequence);

        if (nativeData.CharMultiArray != NULL)
        {
            CharMultiArray = ACE_OS::malloc(sizeof(CORBA::Char) * 24);
            ACE_OS::memcpy(CharMultiArray, nativeData.CharMultiArray, sizeof(CORBA::Char) * 24);
        }

        if (nativeData.CharMultiArray != NULL)
        {
            WCharMultiArray = ACE_OS::malloc(sizeof(CORBA::WChar) * 24);
            ACE_OS::memcpy(WCharMultiArray, nativeData.WCharMultiArray, sizeof(CORBA::WChar) * 24);
        }

        // Integer types
        ShortType = nativeData.ShortType;
        LongLongType = nativeData.LongLongType;
        UnsignedShortType = nativeData.UnsignedShortType;
        UnsignedLongType = nativeData.UnsignedLongType;
        UnsignedLongLongType = nativeData.UnsignedLongLongType;

        if (nativeData.ShortArray != NULL)
        {
            ACE_OS::memcpy(ShortArray, nativeData.ShortArray, sizeof(CORBA::Short) * 5);
        }

        if (nativeData.LongLongArray != NULL)
        {
            ACE_OS::memcpy(LongLongArray, nativeData.LongLongArray, sizeof(CORBA::LongLong) * 5);
        }

        if (nativeData.UnsignedShortArray != NULL)
        {
            ACE_OS::memcpy(UnsignedShortArray, nativeData.UnsignedShortArray, sizeof(CORBA::UShort) * 5);
        }

        if (nativeData.UnsignedLongArray != NULL)
        {
            ACE_OS::memcpy(UnsignedLongArray, nativeData.UnsignedLongArray, sizeof(CORBA::ULong) * 5);
        }

        if (nativeData.UnsignedLongLongArray != NULL)
        {
            ACE_OS::memcpy(UnsignedLongLongArray, nativeData.UnsignedLongLongArray, sizeof(CORBA::ULongLong) * 5);
        }

        marshal::unbounded_sequence_to_ptr(nativeData.ShortSequence, ShortSequence);
        marshal::unbounded_sequence_to_ptr(nativeData.LongLongSequence, LongLongSequence);
        marshal::unbounded_sequence_to_ptr(nativeData.UnsignedShortSequence, UnsignedShortSequence);
        marshal::unbounded_sequence_to_ptr(nativeData.UnsignedLongSequence, UnsignedLongSequence);
        marshal::unbounded_sequence_to_ptr(nativeData.UnsignedLongLongSequence, UnsignedLongLongSequence);

        if (nativeData.ShortMultiArray != NULL)
        {
            ShortMultiArray = ACE_OS::malloc(sizeof(CORBA::Short) * 24);
            ACE_OS::memcpy(ShortMultiArray, nativeData.ShortMultiArray, sizeof(CORBA::Short) * 24);
        }

        if (nativeData.LongLongMultiArray != NULL)
        {
            LongLongMultiArray = ACE_OS::malloc(sizeof(CORBA::LongLong) * 24);
            ACE_OS::memcpy(LongLongMultiArray, nativeData.LongLongMultiArray, sizeof(CORBA::LongLong) * 24);
        }

        if (nativeData.UnsignedShortMultiArray != NULL)
        {
            UnsignedShortMultiArray = ACE_OS::malloc(sizeof(CORBA::UShort) * 24);
            ACE_OS::memcpy(UnsignedShortMultiArray, nativeData.UnsignedShortMultiArray, sizeof(CORBA::UShort) * 24);
        }

        if (nativeData.UnsignedLongMultiArray != NULL)
        {
            UnsignedLongMultiArray = ACE_OS::malloc(sizeof(CORBA::ULong) * 24);
            ACE_OS::memcpy(UnsignedLongMultiArray, nativeData.UnsignedLongMultiArray, sizeof(CORBA::ULong) * 24);
        }

        if (nativeData.UnsignedLongLongMultiArray != NULL)
        {
            UnsignedLongLongMultiArray = ACE_OS::malloc(sizeof(CORBA::ULongLong) * 24);
            ACE_OS::memcpy(UnsignedLongLongMultiArray, nativeData.UnsignedLongLongMultiArray, sizeof(CORBA::ULongLong) * 24);
        }

        // Other primitives types
        BooleanType = nativeData.BooleanType;
        OctetType = nativeData.OctetType;

        if (nativeData.BooleanArray != NULL)
        {
            ACE_OS::memcpy(BooleanArray, nativeData.BooleanArray, sizeof(CORBA::Boolean) * 5);
        }

        if (nativeData.OctetArray != NULL)
        {
            ACE_OS::memcpy(OctetArray, nativeData.OctetArray, sizeof(CORBA::Octet) * 5);
        }

        marshal::unbounded_sequence_to_ptr(nativeData.BooleanSequence, BooleanSequence);
        marshal::unbounded_sequence_to_ptr(nativeData.OctetSequence, OctetSequence);

        if (nativeData.BooleanMultiArray != NULL)
        {
            BooleanMultiArray = ACE_OS::malloc(sizeof(CORBA::Boolean) * 24);
            ACE_OS::memcpy(BooleanMultiArray, nativeData.BooleanMultiArray, sizeof(CORBA::Boolean) * 24);
        }

        if (nativeData.OctetMultiArray != NULL)
        {
            OctetMultiArray = ACE_OS::malloc(sizeof(CORBA::Octet) * 24);
            ACE_OS::memcpy(OctetMultiArray, nativeData.OctetMultiArray, sizeof(CORBA::Octet) * 24);
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

        // Release pointer to the float sequence
        if (FloatSequence != NULL)
        {
            ACE_OS::free(FloatSequence);
        }

        // Release pointer to the double sequence
        if (DoubleSequence != NULL)
        {
            ACE_OS::free(DoubleSequence);
        }

        // Release pointer to the long double sequence
        if (LongDoubleSequence != NULL)
        {
            ACE_OS::free(LongDoubleSequence);
        }

        // Release pointer to the float multi array
        if (FloatMultiArray != NULL)
        {
            ACE_OS::free(FloatMultiArray);
        }

        // Release pointer to the double multi array
        if (DoubleMultiArray != NULL)
        {
            ACE_OS::free(DoubleMultiArray);
        }

        // Release pointer to the long double multi array
        if (LongDoubleMultiArray != NULL)
        {
            ACE_OS::free(LongDoubleMultiArray);
        }

        // Release the pointer of the char sequence
        if (CharSequence != NULL)
        {
            ACE_OS::free(CharSequence);
        }

        // Release the pointer of the wchar sequence
        if (WCharSequence != NULL)
        {
            ACE_OS::free(WCharSequence);
        }

        // Release pointer to the char multi array
        if (CharMultiArray != NULL)
        {
            ACE_OS::free(CharMultiArray);
        }

        // Release pointer to the wchar multi array
        if (WCharMultiArray != NULL)
        {
            ACE_OS::free(WCharMultiArray);
        }

        // Release pointer to the short sequence
        if (ShortSequence != NULL)
        {
            ACE_OS::free(ShortSequence);
        }

        // Release pointer to the long long sequence
        if (LongLongSequence != NULL)
        {
            ACE_OS::free(LongLongSequence);
        }

        // Release pointer to the unsigned short sequence
        if (UnsignedShortSequence != NULL)
        {
            ACE_OS::free(UnsignedShortSequence);
        }

        // Release pointer to the unsigned long sequence
        if (UnsignedLongSequence != NULL)
        {
            ACE_OS::free(UnsignedLongSequence);
        }

        // Release pointer to the unsigned long long sequence
        if (UnsignedLongLongSequence != NULL)
        {
            ACE_OS::free(UnsignedLongLongSequence);
        }

        if (ShortMultiArray != NULL)
        {
            ACE_OS::free(ShortMultiArray);
        }

        if (LongLongMultiArray != NULL)
        {
            ACE_OS::free(LongLongMultiArray);
        }

        if (UnsignedShortMultiArray != NULL)
        {
            ACE_OS::free(UnsignedShortMultiArray);
        }

        if (UnsignedLongMultiArray != NULL)
        {
            ACE_OS::free(UnsignedLongMultiArray);
        }

        if (UnsignedLongLongMultiArray != NULL)
        {
            ACE_OS::free(UnsignedLongLongMultiArray);
        }

        if (BooleanSequence != NULL)
        {
            ACE_OS::free(BooleanSequence);
        }

        if (OctetSequence != NULL)
        {
            ACE_OS::free(OctetSequence);
        }

        if (BooleanMultiArray != NULL)
        {
            ACE_OS::free(BooleanMultiArray);
        }

        if (OctetMultiArray != NULL)
        {
            ACE_OS::free(OctetMultiArray);
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