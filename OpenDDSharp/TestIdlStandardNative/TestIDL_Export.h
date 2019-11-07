// -*- C++ -*-
// $Id$
// Definition for Win32 Export directives.
// This file is generated automatically by export_file_generator.exe TestIDL
// ------------------------------
#ifndef TESTIDL_EXPORT_H
#define TESTIDL_EXPORT_H

#include "ace/config-all.h"

#if defined (ACE_AS_STATIC_LIBS) && !defined (TESTIDL_HAS_DLL)
#  define TESTIDL_HAS_DLL 0
#endif /* ACE_AS_STATIC_LIBS && TESTIDL_HAS_DLL */

#if !defined (TESTIDL_HAS_DLL)
#  define TESTIDL_HAS_DLL 1
#endif /* ! TESTIDL_HAS_DLL */

#if defined (TESTIDL_HAS_DLL) && (TESTIDL_HAS_DLL == 1)
#  if defined (TESTIDL_BUILD_DLL)
#    define TestIDL_Export ACE_Proper_Export_Flag
#    define TESTIDL_SINGLETON_DECLARATION(T) ACE_EXPORT_SINGLETON_DECLARATION (T)
#    define TESTIDL_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK) ACE_EXPORT_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK)
#  else /* TESTIDL_BUILD_DLL */
#    define TestIDL_Export ACE_Proper_Import_Flag
#    define TESTIDL_SINGLETON_DECLARATION(T) ACE_IMPORT_SINGLETON_DECLARATION (T)
#    define TESTIDL_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK) ACE_IMPORT_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK)
#    endif /* TESTIDL_BUILD_DLL */
#else /* TESTIDL_HAS_DLL == 1 */
#  define TestIDL_Export
#  define TESTIDL_SINGLETON_DECLARATION(T)
#  define TESTIDL_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK)
#endif /* TESTIDL_HAS_DLL == 1 */

// Set TESTIDL_NTRACE = 0 to turn on library specific tracing even if
// tracing is turned off for ACE.
#if !defined (TESTIDL_NTRACE)
#  if (ACE_NTRACE == 1)
#    define TESTIDL_NTRACE 1
#  else /* (ACE_NTRACE == 1) */
#    define TESTIDL_NTRACE 0
#  endif /* (ACE_NTRACE == 1) */
#endif /* !TESTIDL_NTRACE */

#if (TESTIDL_NTRACE == 1)
#  define TESTIDL_TRACE(X)
#else /* (TESTIDL_NTRACE == 1) */
#  if !defined (ACE_HAS_TRACE)
#    define ACE_HAS_TRACE
#  endif /* ACE_HAS_TRACE */
#  define TESTIDL_TRACE(X) ACE_TRACE_IMPL(X)
#include "ace/Trace.h"
#endif /* (TESTIDL_NTRACE == 1) */

#endif /* TESTIDL_EXPORT_H */
// End of auto generated file.
