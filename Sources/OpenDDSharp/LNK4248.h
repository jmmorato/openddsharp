/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
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
#ifndef __LNK4248_H__
#define __LNK4248_H__

// incomplete types generate warnings similar to:
// "warning LNK4248: unresolved typeref token (0100001E) for 'CORBA.InterfaceDef'; image may not run"
// providing fake definitions for these types supresses the warnings

#define SUPPRESS_LNK4248_CORBA \
namespace CORBA { \
	class InterfaceDef {}; \
	class Context {}; \
	class NVList {}; \
	class NamedValue {}; \
	class Request {}; \
	class ExceptionList {}; \
	class ContextList {}; \
}

#endif