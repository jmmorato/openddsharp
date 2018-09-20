/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System;
using System.Linq;
using System.Text;

namespace OpenDDSharp.ExportFileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("ERROR: No library name specified.");
                return;
            }

            string flags = string.Join(" ", args);
            string name = args.Last();
            string ucName = name.ToUpperInvariant();

            StringBuilder prologue = new StringBuilder();
            prologue.Append("// -*- C++ -*-\n");
            prologue.Append("// $Id$\n");
            prologue.Append("// Definition for Win32 Export directives.\n");
            prologue.AppendFormat("// This file is generated automatically by export_file_generator.exe {0}\n", flags);
            prologue.Append("// ------------------------------\n");
            prologue.Append("#ifndef {1}_EXPORT_H\n");
            prologue.Append("#define {1}_EXPORT_H\n");
            prologue.Append("\n");
            prologue.Append("#include \"ace/config-all.h\"\n");
            prologue.Append("\n");
            
            StringBuilder staticStuff = new StringBuilder();            
            staticStuff.Append("#if defined (ACE_AS_STATIC_LIBS) && !defined ({1}_HAS_DLL)\n");
            staticStuff.Append("#  define {1}_HAS_DLL 0\n");
            staticStuff.Append("#endif /* ACE_AS_STATIC_LIBS && {1}_HAS_DLL */\n");
            staticStuff.Append("\n");

            StringBuilder hasDll = new StringBuilder();
            hasDll.Append("#if !defined ({1}_HAS_DLL)\n");
            hasDll.Append("#  define {1}_HAS_DLL 1\n");
            hasDll.Append("#endif /* ! {1}_HAS_DLL */\n");
            hasDll.Append("\n");

            StringBuilder epilogue = new StringBuilder();
            epilogue.Append("#if defined ({1}_HAS_DLL) && ({1}_HAS_DLL == 1)\n");
            epilogue.Append("#  if defined ({1}_BUILD_DLL)\n");
            epilogue.Append("#    define {0}_Export ACE_Proper_Export_Flag\n");
            epilogue.Append("#    define {1}_SINGLETON_DECLARATION(T) ACE_EXPORT_SINGLETON_DECLARATION (T)\n");
            epilogue.Append("#    define {1}_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK) ACE_EXPORT_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK)\n");
            epilogue.Append("#  else /* {1}_BUILD_DLL */\n");
            epilogue.Append("#    define {0}_Export ACE_Proper_Import_Flag\n");
            epilogue.Append("#    define {1}_SINGLETON_DECLARATION(T) ACE_IMPORT_SINGLETON_DECLARATION (T)\n");
            epilogue.Append("#    define {1}_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK) ACE_IMPORT_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK)\n");
            epilogue.Append("#    endif /* {1}_BUILD_DLL */\n");
            epilogue.Append("#else /* {1}_HAS_DLL == 1 */\n");
            epilogue.Append("#  define {0}_Export\n");
            epilogue.Append("#  define {1}_SINGLETON_DECLARATION(T)\n");
            epilogue.Append("#  define {1}_SINGLETON_DECLARE(SINGLETON_TYPE, CLASS, LOCK)\n");
            epilogue.Append("#endif /* {1}_HAS_DLL == 1 */\n");
            epilogue.Append("\n");
            epilogue.Append("// Set {1}_NTRACE = 0 to turn on library specific tracing even if\n");
            epilogue.Append("// tracing is turned off for ACE.\n");
            epilogue.Append("#if !defined ({1}_NTRACE)\n");
            epilogue.Append("#  if (ACE_NTRACE == 1)\n");
            epilogue.Append("#    define {1}_NTRACE 1\n");
            epilogue.Append("#  else /* (ACE_NTRACE == 1) */\n");
            epilogue.Append("#    define {1}_NTRACE 0\n");
            epilogue.Append("#  endif /* (ACE_NTRACE == 1) */\n");
            epilogue.Append("#endif /* !{1}_NTRACE */\n");
            epilogue.Append("\n");
            epilogue.Append("#if ({1}_NTRACE == 1)\n");
            epilogue.Append("#  define {1}_TRACE(X)\n");
            epilogue.Append("#else /* ({1}_NTRACE == 1) */\n");
            epilogue.Append("#  if !defined (ACE_HAS_TRACE)\n");
            epilogue.Append("#    define ACE_HAS_TRACE\n");
            epilogue.Append("#  endif /* ACE_HAS_TRACE */\n");
            epilogue.Append("#  define {1}_TRACE(X) ACE_TRACE_IMPL(X)\n");
            epilogue.Append("#include \"ace/Trace.h\"\n");
            epilogue.Append("#endif /* ({1}_NTRACE == 1) */\n");
            epilogue.Append("\n");
            epilogue.Append("#endif /* {1}_EXPORT_H */\n");
            epilogue.Append("// End of auto generated file.\n");

            StringBuilder export = new StringBuilder();
            export.Append(string.Format(prologue.ToString(), name, ucName));
            export.Append(string.Format(staticStuff.ToString(), name, ucName));
            export.Append(string.Format(hasDll.ToString(), name, ucName));
            export.Append(string.Format(epilogue.ToString(), name, ucName));

            Console.Write(export);            
        }
    }
}
